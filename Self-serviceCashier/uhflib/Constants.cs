using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFLinkage
{
    class UHFConstants
    {
        // 模块连接方式(本地串口,本地网络,远端-如蓝牙透传)
        public enum RF_CONNECT_MODE
        {
	        RF_CONNECT_MODE_LOCAL_UART = 0,
	        RF_CONNECT_MODE_LOCAL_NET,
	        RF_CONNECT_MODE_REMOTE,
            RF_CONNECT_MODE_USB,
	        RF_CONNECT_MODE_MAX
        };


        // RF模块
        public enum RF_MODULE_TYPE
        {
	        RF_MODULE_TYPE_R2000 = 0,
	        RF_MODULE_TYPE_RM801X,
	        RF_MODULE_TYPE_RM70XX,

	        RF_MODULE_TYPE_MAX
        };

        // 操作状态,对于Inventory/Read/Write/Kill,数据包不能直接返回,需要从PACKET中解包返回
        public enum OPTION_TYPES
        {
	        OPTION_TYPE_NORMAL = 0,
	        OPTION_TYPE_INVENTORY,
	        OPTION_TYPE_READ,
	        OPTION_TYPE_WRITE,
	        OPTION_TYPE_KILL,
	        OPTION_TYPE_LOCK,

	        // 报警信息
	        OPTION_TYPE_ALARM_REPORT = 100,
            // 设备心跳
            OPTION_TYPE_HEARTBEAT_REPORT,

	        // 透传到APP层
	        OPTION_TYPE_TRANS_TO_REMOTE = 1000,

	        // 网络连接状态(连接对list模式有效)
	        OPTION_TYPE_NET_CONNECT = 10000,
	        OPTION_TYPE_NET_DISCONNECT,
        } ;

        // bank区域
        public enum MEMORY_BANK
        {
	        MEMORY_BANK_RESERVED,
	        MEMORY_BANK_EPC,
	        MEMORY_BANK_TID,
	        MEMORY_BANK_USER
        } ;

        // 报警模式
        public enum ALARM_MODE {
	        ALARM_MODE_SPEC_TAG = 0,				 // 特定标签报名
	        ALARM_MODE_ANY_TAG, 				     // 任意标签报警
	        ALARM_MODE_NO_TAG, 				         // 无标签报警
        } ;

        //select选项，设置Query参数时使用
        public enum SELECT_MODE
        {
            SELECT_ALL = 1,
            SELECT_DEASSERTED,
            SELECT_ASSERTED
        };
    }
}
