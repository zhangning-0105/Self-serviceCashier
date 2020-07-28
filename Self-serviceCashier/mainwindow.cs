using CommUtility;
using MySqlX.XDevAPI.Relational;

using Org.BouncyCastle.Asn1.Cms;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using UHFLinkage;

namespace Self_serviceCashier
{
    public partial class mainwindow : Form
    {
        public mainwindow()
        {
            InitializeComponent();
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.AutoGenerateColumns = false;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("黑体", 10, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            //列Header的背景色
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(57, 57, 57);
            dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridView1.ColumnHeadersHeight = 30;
            dataGridView1.RowTemplate.Height = 30;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
             this.panel1.Hide();
            timer1.Interval = 3000;
            timer1.Tick += Timer1_Tick;
            timer2.Interval = 1000;
            timer2.Tick += Timer2_Tick;

        }

        // 连接模式
        static UHFConstants.RF_CONNECT_MODE sConnectMode = UHFConstants.RF_CONNECT_MODE.RF_CONNECT_MODE_LOCAL_UART;
        // 模块类型, 高8位表示RF_MODULE_TYPE的值,低8位定义如下:
        // 对于R2000,    值为RF_MODULE_TYPE_R2000
        // 对于RM8011, 20,26,30 -- RM8011,数字表示模块最大功率
        // 对于RM70xx, 值为RF_MODULE_TYPE_R2000,表示RM70xx连接了R2000模块
        //             值为RF_MODULE_TYPE_70XX, 表示RM70xx未检测到R2000/RM8011模块
        //             值为20,26,30 -- RM8011, 表示RM70xx连接了RM8011模块
        static int gRFModuleInformation = 0;
        // 选择的RF模块
        static UHFConstants.RF_MODULE_TYPE mRFModule = UHFConstants.RF_MODULE_TYPE.RF_MODULE_TYPE_R2000;
        // 回调函数,定义为static变量,防止垃圾回收
        private static UHFNative.rfidCallbackFunc sRfidCallbackFunc;
        /// <summary>
        /// 定义一个代理
        /// </summary>
        private delegate void delegateUpdataListInventoryDelegate(Structs.InventoryData invantoryData);
        // 开始盘点时间(tick)
        private long mlStartTime = 0;
        // 当前设备连接状态
        private static bool gDeviceConnectStatus = false;
        // 天线个数
        private uint sAntennaPortNum = 0;
        private Dictionary<int, string> dicInventoryData = new Dictionary<int, string> { };
        private List<string> tids = new List<string>();
        // 标签个数（不重复),总的个数(含重复)
        private long mlTagCount = 0, mlTotalCount = 0;
        private decimal totalpricie = 0;
        private  int count = 3;

        private void mainwindow_Load(object sender, EventArgs e)
        {
            new Thread(Init).Start();
        }

        private void Init()
        {
            try
            {
                var readerPort = "COM7";
                byte[] comName = new byte[readerPort.Length + 1];
                comName = StringUtility.string2bytes(readerPort);

                sConnectMode = UHFConstants.RF_CONNECT_MODE.RF_CONNECT_MODE_LOCAL_UART;

                if (InitRFID() != 0)
                {
                    //tipMsg.Text = "初始化RFID库失败！";
                    return;
                }

                if (UHFNative.openCom(comName, 115200) == 0)
                {
                    if (getModuleBaseInfo() != 0)
                    {


                        UHFNative.closeCom();
                        LogHelper.WriteLog("获取模块信息失败！");
                    }

                    else
                    {
                        //byte DR = 0, M = 0, TRext = 0, Sel = 0, Session = 0, Target = 0, Q = 7;
                        //if (UHFNative.setQuery(DR, M, TRext, Sel, Session, Target, Q) == 0)
                        //{
                        int maskFlag = 0;
                        // 清除掉过滤信息
                        UHFNative.resetInventoryFilter();
                        // 启动盘点,发送盘点指令
                        // if (UHFNative.startInventory((byte)1, (byte)maskFlag) == 0)
                        //{
                        //设置读取EPC和tid
                        UHFNative.setInventoryArea((byte)1, (byte)0, (byte)6);
                        UHFNative.startInventory((byte)1, (byte)maskFlag);

                        LogHelper.WriteLog("串口打开成功");
                        // }
                        // }



                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("初始化端口出错", ex);
            }
        }


        /// <summary>
        /// 初始化RFID库文件
        /// </summary>
        /// <returns></returns>
        private int InitRFID()
        {
            gRFModuleInformation = ((int)mRFModule << 8);

            // 初始化回调函数
            if (sRfidCallbackFunc == null)
            {
                sRfidCallbackFunc = new UHFNative.rfidCallbackFunc(rfidCallbackFunc);
            }

            // 初始化RFID库
            if (UHFNative.initRFID(sRfidCallbackFunc, null) == 0)
            {
                // 模块类型
                UHFNative.setRFModuleType(mRFModule);

                // 本地连接模式
                UHFNative.setRFConnectMode(sConnectMode);

                return 0;
            }
            return -1;
        }

        private unsafe int rfidCallbackFunc(int status, UHFConstants.OPTION_TYPES type, IntPtr data, int dataLen)
        {
            /* 盘点返回的数据信息 */
            switch (type)
            {
                case UHFConstants.OPTION_TYPES.OPTION_TYPE_INVENTORY:
                    // 收到盘点到的标签信息
                    Structs.InventoryData invantoryData = (Structs.InventoryData)Marshal.PtrToStructure(data, typeof(Structs.InventoryData));
                    detailWithInventoryData(invantoryData);
                    // 收到盘点到的标签信息
                    break;
                case UHFConstants.OPTION_TYPES.OPTION_TYPE_ALARM_REPORT:
                    {
                        // 收到报警信息
                        break;
                    }
                case UHFConstants.OPTION_TYPES.OPTION_TYPE_HEARTBEAT_REPORT:
                    {
                        break;
                    }
                case UHFConstants.OPTION_TYPES.OPTION_TYPE_NET_CONNECT:
                    {
                        Structs.NetStatusData netStatusData = (Structs.NetStatusData)Marshal.PtrToStructure(data, typeof(Structs.NetStatusData));
                        //detailNetConnect(netStatusData);
                        break;
                    }
                case UHFConstants.OPTION_TYPES.OPTION_TYPE_NET_DISCONNECT:
                    {
                        // 收到网络连接状态,更新网络信息
                        //DeinitRFID();
                        break;
                    }
                case UHFConstants.OPTION_TYPES.OPTION_TYPE_READ:
                    {
                        if (dataLen > 0)
                        {
                            Structs.RWData rwData = (Structs.RWData)Marshal.PtrToStructure(data, typeof(Structs.RWData));
                            // detailWithRWData(rwData);
                        }
                        else
                        {
                            //tipMsg.Text = String.Format(@"读取失败！错误码：{0}", status);
                        }
                        break;
                    }
                case UHFConstants.OPTION_TYPES.OPTION_TYPE_WRITE:
                case UHFConstants.OPTION_TYPES.OPTION_TYPE_KILL:
                case UHFConstants.OPTION_TYPES.OPTION_TYPE_LOCK:
                    {
                        string[] strTipTitle = { "写入", "销毁", "锁定" };
                        if (status == 0)
                        {
                            // tipMsg.Text = String.Format(@"{0}成功！", strTipTitle[type - UHFConstants.OPTION_TYPES.OPTION_TYPE_WRITE]);
                        }
                        else
                        {
                            // tipMsg.Text = String.Format(@"{0}失败！错误码：{1}", strTipTitle[type - UHFConstants.OPTION_TYPES.OPTION_TYPE_WRITE], status);
                        }
                        break;
                    }
            }
            return 0;
        }

        /// <summary>
        /// 处理盘点到的标签数据
        /// </summary>
        /// <param name="invantoryData"></param>
        private void detailWithInventoryData(Structs.InventoryData invantoryData)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new delegateUpdataListInventoryDelegate(detailWithInventoryData), invantoryData);
                }
                else
                {
                    if (mlStartTime == -1)
                    {
                        // 盘点到第一个数据之后才开始计时(对于RM70xx,盘点指令发出后,会有给模块
                        // 复位的时间,不能计算在内)
                        mlStartTime = System.Environment.TickCount;
                    }

                    string epcData = StringUtility.bytes2hexs(invantoryData.epcData, invantoryData.epcLen);
                    string externalData = "";
                    string pc = StringUtility.bytes2hexs(invantoryData.pc, 2);
                    string strFullData = epcData;
                    bool findFlag = false;
                    int itemIdx = 0;
                    if (invantoryData.externalDataLen > 0)
                    {
                        externalData = StringUtility.bytes2hexs(invantoryData.externalData, invantoryData.externalDataLen);
                        strFullData += externalData;
                    }

                    var list = DatabaseHelper.GetProductInformation(epcData);
                    if (tids.Contains(externalData))
                    {
                        return;
                    }
                    else
                    {
                        tids.Add(externalData);
                    }
                    if (list.Count > 0)
                    {
                        foreach (KeyValuePair<int, string> item in dicInventoryData)
                        {
                            if (item.Value.Contains(strFullData))
                            {
                                findFlag = true;
                                itemIdx = item.Key;
                            }
                        }

                        foreach (var item in list)
                        {
                            if (findFlag)
                            {
                                dataGridView1.Rows[itemIdx].Cells[2].Value = dataGridView1.Rows[itemIdx].Cells[2].ToString() + item[3].ToString();
                                dataGridView1.Rows[itemIdx].Cells[3].Value = dataGridView1.Rows[itemIdx].Cells[3].ToString() + item[3].ToString();
                            }
                            else
                            {
                                dataGridView1.Rows.Add(item);
                                dicInventoryData.Add((int)mlTagCount, strFullData);
                                // 标签个数加1
                                mlTagCount++;
                                dataGridView1.Rows[0].Selected = false;
                            }
                            totalpricie += Convert.ToDecimal(item[3]);
                            this.label3.Text = totalpricie.ToString();
                        }
                        this.panel1.Show();
                        this.groupBox1.Hide();
                    }

                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("", ex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            this.groupBox1.Show();
            this.label6.Hide();
            this.button2.Hide();
            timer1.Enabled = true;
          
        
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            count = 3;
            this.label5.Text = "付款完成！";
            timer1.Enabled = false;
            this.label6.Show();
            this.button2.Show();
            timer2.Enabled = true;
   
          
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
           
            if (count == 0)
            {
                
                timer2.Enabled = false;
                this.label5.Text = "正在付款，请稍后";
                this.label6.Text = "3秒后返回";
                this.totalpricie = 0;
                this.label3.Text = "";
                this.dicInventoryData.Clear();
                this.tids.Clear();
                this.dataGridView1.Rows.Clear();
                Console.WriteLine(count+"  1   " + DateTime.Now.ToString());
                this.panel1.Hide();
            }
            else
            {
                this.label6.Text = $"{count}秒后返回";
                Console.WriteLine(count+"   2   " + DateTime.Now.ToString());
                count--;
            }
            
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex < 0 && e.RowIndex >= 0) // 绘制 自动序号
            {
                e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
                Rectangle vRect = e.CellBounds;
                vRect.Inflate(-2, 2);
                TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), e.CellStyle.Font, vRect, e.CellStyle.ForeColor, TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                e.Handled = true;
            }
            // e.CellStyle.SelectionBackColor = Color.White; // 选中单元格时，背景色
            e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //单位格内数据对齐方式
        }

        private void button2_Click(object sender, EventArgs e)
        {
            count = 3;
            timer2.Enabled = false;
            this.label5.Text = "正在付款，请稍后";
            this.label6.Text = "3秒后返回";
            this.totalpricie = 0;
            this.label3.Text = "";
            this.dicInventoryData.Clear();
            this.tids.Clear();
            this.dataGridView1.Rows.Clear();
            this.panel1.Hide();
        }

        private int getModuleBaseInfo()
        {
            string strTmp = "";
            string strSn = "", strBoardSn = "";
            byte[] snData = new byte[56], boardSnData = new byte[64];
            int snLen = snData.Length, boardSnLen = boardSnData.Length;

            // 停止盘点
            UHFNative.stopInventory();

            if (mRFModule == UHFConstants.RF_MODULE_TYPE.RF_MODULE_TYPE_RM70XX)
            {
                // RM70xx版本，先获取板子的序列号
                if (UHFNative.getBoardSerialNumber(ref boardSnData[0], ref boardSnLen) == 0)
                {
                    strBoardSn = StringUtility.bytes2string(boardSnData, 0, boardSnLen);
                }
                else
                {
                    // tipMsg.Text = "设备连接失败！";
                    return -1;
                }
            }

            // 获取模块序列号
            if (UHFNative.getModuleSerialNumber(ref snData[0], ref snLen) == 0)
            {
                byte[] version = new byte[56];
                int versionLen = version.Length;

                // 获取软件版本号
                if (UHFNative.getModuleSoftVersion(ref version[0], ref versionLen) == 0)
                {
                    string strVersion = StringUtility.bytes2string(version, 0, versionLen);

                    strSn = StringUtility.bytes2string(snData, 0, snLen);
                    if (mRFModule == UHFConstants.RF_MODULE_TYPE.RF_MODULE_TYPE_RM70XX)
                    {
                        strTmp = String.Format(@"设备连接成功！RM70XX序列号:{0}, 设备序列号:{1},软件版本：{2}",
                                               strBoardSn, strSn, strVersion);
                    }
                    else
                    {
                        strTmp = String.Format(@"设备连接成功！设备序列号:{0},软件版本：{1}",
                                              strSn, strVersion);
                    }

                    if (UHFNative.getAntennaPortNum(out sAntennaPortNum) != 0)
                    {
                        // 多口模块
                    }

                    // 加载RM8011的功率信息
                    if (strSn.IndexOf("RM", 0) == 0)
                    {
                        // 棋连模块
                        // RM-26dBm
                        int moduleType = Int32.Parse(strSn.Substring(3, 2));
                        gRFModuleInformation |= moduleType;
                        //loadPowerLevel();
                        // cmbRM8011PowerLevelAntentSetting.Visible = true;
                    }
                    else
                    {
                        gRFModuleInformation |= (int)UHFConstants.RF_MODULE_TYPE.RF_MODULE_TYPE_R2000;
                        if (sAntennaPortNum > 1)
                        {
                            /*enum {
                                RFID_RESPONSE_MODE_COMPACT = 0x00000001,
                                RFID_RESPONSE_MODE_NORMAL = 0x00000003,
                                RFID_RESPONSE_MODE_EXTENDED = 0x00000007
                            };*/
                            UHFNative.setResponseDataMode(0x07);
                        }
                    }

                    // tipMsg.Text = strTmp;

                    gDeviceConnectStatus = true;

                    if (sConnectMode == UHFConstants.RF_CONNECT_MODE.RF_CONNECT_MODE_LOCAL_UART)
                    {
                        // 串口模式
                        //  btnComConnect.Text = "断 开";
                    }
                    else if (sConnectMode == UHFConstants.RF_CONNECT_MODE.RF_CONNECT_MODE_USB)
                    {
                        // btnConnectUSB.Text = "断开";
                    }
                    else
                    {
                        // 网络模式
                        // btnNetConnect.Text = "断 开";
                    }

                    // 加载tab页面
                    // InitTabControls();
                    return 0;
                }
            }
            else
            {
                if (mRFModule == UHFConstants.RF_MODULE_TYPE.RF_MODULE_TYPE_RM70XX)
                {

                    strTmp = String.Format(@"RM70XX连接成功，RF设备连接失败, RM70XX序列号:{0}！", strBoardSn);
                    // tipMsg.Text = strTmp;
                    gRFModuleInformation |= (int)UHFConstants.RF_MODULE_TYPE.RF_MODULE_TYPE_RM70XX;
                    // InitTabControls();
                    // 网络模式
                    gDeviceConnectStatus = true;
                    if (sConnectMode == UHFConstants.RF_CONNECT_MODE.RF_CONNECT_MODE_LOCAL_UART)
                    {
                        // 串口模式
                        // btnComConnect.Text = "断 开";
                    }
                    else if (sConnectMode == UHFConstants.RF_CONNECT_MODE.RF_CONNECT_MODE_USB)
                    {
                        //  btnConnectUSB.Text = "断开";
                    }
                    else
                    {
                        // 网络模式
                        //  btnNetConnect.Text = "断 开";
                    }
                    return 0;
                }
                else
                {
                    // tipMsg.Text = "设备连接失败！";
                }
            }
            return -1;
        }

    }
}
