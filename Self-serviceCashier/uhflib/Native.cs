using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFLinkage
{
    class UHFNative
    {
        // 回调函数定义
        public unsafe delegate int rfidCallbackFunc(int status, UHFConstants.OPTION_TYPES type, IntPtr data, int dataLen);
        public unsafe delegate int rfidRwCfgCallbackFunc(int rwFlag, IntPtr data, int dataLen);

        /* libaray functions */
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int initRFID(rfidCallbackFunc pFunc, rfidRwCfgCallbackFunc pRwCfgFunc);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int deinitRFID();
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int setRFModuleType(UHFConstants.RF_MODULE_TYPE moduleType);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int setRFConnectMode(UHFConstants.RF_CONNECT_MODE flag);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int powerOnRFModule();
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int powerOffRFModule();
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int pushRFData(Byte[] rdBuf, int rdLen);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int parseRFData();
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int openCom(Byte[] port, int baud);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int closeCom();
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int connectRemoteNetwork(Byte[] remoteAddr, int remotePort);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int listenRemoteNetwork(int listPort);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int closeNetwork();
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int scanUsbDevice(IntPtr[] pDevNamesBuf, int maxBufSize, int maxDevNum);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]	
        public static extern int openUsbDev(int idx);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
	    public static extern  int closeUsbDev();

		/*
		  RF common and don't depend on actual RF moudule(R2000 AND RM8011)
		  RM70xx is virtual RF module
		*/
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int setInventoryArea(byte area, byte startAddr, byte wordLen);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getInventoryArea(out byte area, out byte startAddr, out byte wordLen);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int setInventoryFilterThreshold(byte threshold, uint maxRepeatTimes, uint maxCacheTimeMs);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int getInventoryFilterThreshold(out byte threshold, out uint maxRepeatTimes, out uint maxCacheTimeMs);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int resetInventoryFilter();
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int setAlarmParams(byte mode, byte alarmTimes, byte ignoreSec, byte alarmSec,
									            byte matchStart, byte matchLen, Byte[] match);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getAlarmParams(out byte mode, out byte alarmTimes, out byte ignoreSec, out byte alarmSec,
                                                out byte matchStart, out byte matchLen, ref byte match);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int setAlarmStatus(byte status);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int getAlarmStatus(ref byte status);

		/* RF common functions */
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int getModuleSerialNumber(ref byte snData, ref int snLen);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int getModuleSoftVersion(ref byte version, ref int versionLen);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int startInventory(byte mode, byte maskFlag);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int stopInventory();
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int getInventoryStatus();
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int setPowerLevel(byte antennaPort, uint powerLevel);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int getPowerLevel(byte antennaPort, out uint powerLevel);

        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int readTag(byte[] accessPassword, UHFConstants.MEMORY_BANK memBank, UInt32 startAddr, UInt16 wordLen);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int readTagSync(byte[] accessPassword, UHFConstants.MEMORY_BANK memBank, UInt32 startAddr, UInt16 wordLen, UInt16 timeOutMs,
                        out Structs.RWData rwData);
       
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int writeTag(byte[] accessPassword, UHFConstants.MEMORY_BANK memBank, UInt32 startAddr, UInt16 wordLen, byte[] pWriteData);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int writeTagSync(byte[] accessPassword, UHFConstants.MEMORY_BANK memBank, UInt32 startAddr, UInt16 wordLen, byte[] pWriteData,
                                UInt16 timeOutMs, out Structs.RWData rwData);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int lockTag(byte killPasswordPermissions,
							             byte accessPasswordPermissions,
							             byte epcMemoryBankPermissions, byte tidMemoryBankPermissions,
							             byte userMemoryBankPermissions, byte[] accessPassword);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int killTag(byte[] accessPassword, byte[] killPassword);

        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int setRegion(byte range);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int getRegion(out byte range);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int setFixFreq(uint freq);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int getFixFreq(out uint freq);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int set18K6CSelectCriteria(byte selectorIdx, byte status, byte memBank,
		                                                byte target, byte action, byte enableTruncate,
											            int maskOffset, int maskCount, byte[] mask);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int get18K6CSelectCriteria(byte selectorIdx, out byte status, out byte memBank,
                                                        out byte target, out byte action, out byte enableTruncate,
                                                        out int maskOffset, out int maskCount, ref byte mask);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int getAntennaPortNum(out uint num);

		/* For R2000 module and RM70xx */
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int setResponseDataMode(byte mode);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int getResponseDataMode(ref byte mode);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int getAntennaSWR(byte antennaPort, out uint swr);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int setCurrentProfile(uint profile);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int getCurrentProfile(out uint profile);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int setAntennaPortState(byte antennaPort, byte antennaStatus);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int getAntennaPortState(byte antennaPort, ref byte antennaStatus);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int setCurrentSingulationAlgorithm(uint algorithm);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int getCurrentSingulationAlgorithm(out uint algorithm);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int prepareModuleUpdate(ref byte version, out uint versionLen, out uint maxPacketLen,
							                         out uint chunkSize, uint firmwareLen);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int updateModuleFirmware(byte[] data, uint dataLen); 
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int setProtschTxtime(uint txOn, uint txOff);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getProtschTxtime(out uint txOn, out uint txOff);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int macGetPacket(uint hstCmd, uint wantPacketType, byte[] packet,
		                        out uint packetDataLen);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int setSingulationFixedQParameters(uint qValue, uint retryCount,
													            uint toggleTarget,
													            uint repeatUntilNoTags);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getSingulationFixedQParameters(out uint qValue, out uint retryCount,
                                                                out uint toggleTarget,
                                                                out uint repeatUntilNoTags);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int setSingulationDynamicQParameters(uint startQValue, uint minQValue,
														          uint maxQValue,
														          uint thresholdMultiplier,
														          uint retryCount,
														          uint toggleTarget);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getSingulationDynamicQParameters(out uint startQValue,
                                                                  out uint minQValue,
                                                                  out uint maxQValue,
                                                                  out uint thresholdMultiplier,
                                                                  out uint retryCount, out uint toggleTarget);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int set18K6CQueryTagGroup(UHFConstants.SELECT_MODE select, byte session, byte target);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int get18K6CQueryTagGroup(out UHFConstants.SELECT_MODE selected, out byte session, out byte target);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int setPostSingulationMatchCriteria(byte status, int maskOffset,
													             int maskCount, byte[] mask);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getPostSingulationMatchCriteria(out byte status, out int maskOffset,
                                                                 out int maskCount, ref byte mask);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int setMonzaQtTagMode(byte memMap, byte maskFlag, byte[] accessPassword);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int readMonzaQtTag(byte memMap, byte[] accessPassword, UHFConstants.MEMORY_BANK memBank, 
		                                        uint startAddr, uint wordLen);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int readMonzaQtTagSync(byte memMap, byte[] accessPassword, UHFConstants.MEMORY_BANK memBank, uint startAddr, 
                                                    uint wordLen, uint timeOutMs, out Structs.RWData pRwData);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int writeMonzaQtTag(byte memMap, byte[] accessPassword, UHFConstants.MEMORY_BANK memBank, 
		                                         uint startAddr, uint wordLen, byte[] pWriteData);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int writeMonzaQtTagSync(byte memMap, byte[] accessPassword, UHFConstants.MEMORY_BANK memBank,
                                                     uint startAddr, uint wordLen, byte[] pWriteData,
                                                     uint timeOutMs, Structs.RWData pRwData);
        
		/* For multiple Channel module and RM70xx */
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int setAntennaPort(byte antennaPort, byte antennaStatus, uint powerLevel,
									            uint dwellTime, uint numberInventoryCycles);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getAntennaPort(uint antennaPort, out byte antennaStatus, out uint powerLevel,
                                                out uint dwellTime, out uint numberInventoryCycles);

		/* For RM8011 module and RM70xx*/
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int setWorkMode(byte mode);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int setQuery(byte DR, byte M, byte TRext, byte Sel, byte Session, byte Target,
								          byte Q);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getQuery(out byte DR, out byte M, out byte TRext, out byte Sel, out byte Session,
                                          out byte Target, out byte Q);

		/* Only For RM70xx */
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int boardFirmwareUpdate(int curIdx, int maxIdx, int dataLen, byte[] data);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int boardReboot();
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getBoardSerialNumber(ref byte snData, ref int snLen);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int getBoardSoftVersion(ref byte version, ref int versionLen);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int setBoardModuleType(byte moduleType);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int getBoardModuleType(out byte moduleType);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int setInventoryParams(byte mode, byte triggerDInPort, byte triggerLevel,
                                                    uint ignoreTimeMs, uint inventoryTimeMs,
                                                    uint maskFlag);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int getInventoryParams(out byte mode, out byte triggerDInPort, out byte triggerLevel,
                                                    out uint ignoreTimeMs, out uint inventoryTimeMs,
                                                    out uint maskFlag);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int setHeartBeat(byte status, uint interval);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getHeartBeat(out byte status, out uint interval);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int setNetInfo(byte[] ip, byte[] gateWay, byte[] netmask, byte[] macAddr,
								            byte[] remoteIp, uint remotePort, byte pingGateWay);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int setWifiInfo(byte[] ssid, byte ssidLen, byte[]passwd, byte passwdLen,
                                             byte[] ip, byte[] gateWay, byte[] netmask, byte[] remoteIp,
                                             UInt16 remotePort);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int getNetInfo(ref byte ip, ref byte gateWay, ref byte netmask, ref byte macAddr,
								            ref byte remoteIp, out uint remotePort, out byte pingGateWay);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getWifiInfo(ref byte ssid, out byte ssidLen, ref byte passwd, out byte passwdLen,
                                             ref byte ip, ref byte gateWay, ref byte netmask, ref byte remoteIp,
                                             out UInt16 remotePort);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int setDOutStatus(byte port, byte status);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int getDOutStatus(byte port, out byte status);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int setDoutInspectMask(byte mask);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getDoutInspectMask(out byte mask);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int setDoutDefaultVal(byte mask);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getDoutDefaultVal(out byte mask);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int setInputTriggerAlarm(byte status, byte triggerDInPort, byte triggerLevel,
                                                      byte DPort, byte DOutLevel, UInt32 ignoreMs, UInt32 alarmMs,
                                                      byte reportFlag);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getInputTriggerAlarm(out byte status, out byte triggerDInPort, out byte triggerLevel,
                                                      out byte DPort, out byte DOutLevel, out UInt32 ignoreMs, 
                                                      out UInt32 alarmMs, out byte reportFlag);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int getDInStatus(byte port, out byte status);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int setAlarmDout(byte port, byte status);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int getAlarmDout(out byte port, out byte status);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int startManualAlarm(byte port, byte level, UInt32 interval, UInt32 times);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int stopManualAlarm();
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int setHttpReportFormat(byte status, byte actionLen, byte[] actionName, byte customMsgLen,
                                                     byte[] customMsg, byte method, UInt16 contentMask);
        [DllImport("uhf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getHttpReportFormat(out byte status, out byte actionLen, ref byte actionName,
		                                             out byte customMsgLen, ref byte customMsg, out byte method, 
                                                     out UInt16 contentMask);
         
    }
}
