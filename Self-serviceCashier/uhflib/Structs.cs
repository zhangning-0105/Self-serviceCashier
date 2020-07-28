using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace UHFLinkage
{
    class Structs
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct InventoryData
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (20))]
            public Byte[] fromDev;       //  设备信息(对于串口为COMXX, 对于网络为xxx.xxx.xxx.xxx)
            public Byte antennaPort;       // 天线号
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public Byte[] pc;              // 2字节PC值,pc[0]为高位,pc[1]为低位
            public UInt16 epcLen;          // EPC字节长度
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (64 + 1))]
            public Byte[] epcData;         // EPC信息 
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public Byte[] epc_crc;         // ecp的2字节CRC;
            public UInt16 externalDataLen; // TID/USR字节长度
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (64 + 1))]
            public Byte[] externalData;    // TID/USR数据信息
            public Int16 rssi;             // RSSI
        };

        /* 读取返回数据信息 */
        [StructLayout(LayoutKind.Sequential)]
        public struct RWData
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (20))]
            public Byte[] fromDev;       //  设备信息(对于串口为COMXX, 对于网络为xxx.xxx.xxx.xxx)
            public Byte antennaPort;       // 天线号
            public UInt16 epcLen;          // EPC字节长度
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (64 + 1))]
            public Byte[] epcData;         // EPC信息 
            public UInt16 rwDataLen; // TID/USR字节长度
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (64 + 1))]
            public Byte[] rwData;    // TID/USR数据信息
            public Int16 rssi;             // RSSI
        };

        /* 报警返回数据信息 */
        [StructLayout(LayoutKind.Sequential)]
        public struct AlaramData
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (20))]
            public Byte[] fromDev;       //  设备信息(对于串口为COMXX, 对于网络为xxx.xxx.xxx.xxx)
            public Byte alarmMode;       // 报警模式
            public Byte isTimeOut;       // 是否是报警超时
            public Byte antennaPort;     // 天线号
            public UInt16 epcLen;        // EPC字节长度
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (64 + 1))]
            public Byte[] epcData;       // EPC信息
            public UInt16 externalDataLen; // EPC字节长度
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (64 + 1))]
            public Byte[] externalData;    // EPC信息
            public Int16 rssi;           // RSSI
        };

        /* 报警返回数据信息 */
        [StructLayout(LayoutKind.Sequential)]
        /* 设备心跳数据(仅对RM70xx有效) */
        public struct HeartbeatData
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (20))]
            public Byte[] fromDev;       // 设备信息(对于串口为COMXX, 对于网络为xxx.xxx.xxx.xxx)
            public UInt32 seq;           // 设备自身记录的序号,达到最大值时进行回环
        }

        /* 网络连接/端口数据 */
        [StructLayout(LayoutKind.Sequential)]
        public struct NetStatusData {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (20))]
            public Byte[] ipAddr;       // IP地址,形如192.168.1.16
            public UInt32 port;         // 端口
        }
    }
}
