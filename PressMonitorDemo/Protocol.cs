using PressMonitorDemo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;


///////////////////////////////////////////////
//		协议处理类，实现与管理装置的通讯功能
///////////////////////////////////////////////
// 
//模式定义
public enum tagMode
{
    STOP = 0,
    WORK = 1,
    LEARN = 2,
    LOG_IN = 3,
    LOG_OFF = 4,
}
//传感器连接状态
public enum tagSensorState
{
    SENSOR_CONNECTED = 0,
    SENSOR_DISCONNECT = 1,
}

//最大缓存记录条数

//报警记录状态
public enum tagAlmJudge
{
	ALM_OK = 0, //表示过判定
	ALM_BAD = 1, //表示实际不良
}

////串口指令定义
//public enum tagProCmd
//{
//	PRO_CMD_LOG_IN = 0,
//	PRO_CMD_LOG_OFF,
//	PRO_CMD_SWITtagMode,
//	PRO_CMD_GET_CFG,
//	PRO_CMD_SET_CFG,
//	PRO_CMD_GET_REC,
//	PRO_CMD_CLR_REC,
//	PRO_CMD_GET_STATE,
//	PRO_CMD_RST_ALM,
//	PRO_CMD_REPORT,
//	PRO_CMD_ALARM,
//	PRO_CMD_BUSY,
//	PRO_CMD_RESET_CNT, //20180902    //清除计数器
//	PRO_CMD_SYNC_TIME, //20180902    //同步系统时间
//	PRO_CMD_MODEL_SEL, //20190618 选择基准波形
//}

public class tagPM_chState
{
	public tagMode mode = new tagMode(); //模式
	public int tolIdx; //公差编号
	public int total; //总数
	public int alarm; //报警数
	public int bad; //实际不良数
	public tagSensorState sensor = new tagSensorState(); //传感器接入状态
}

//通用串口Tx数据帧结构体
[StructLayout(LayoutKind.Sequential, Pack = 1)] //Pack =4保持与STM32内存结构一致
public struct COMM_TX_GEN_T
{
    public byte frameHeader;
   // public byte rsvU8;  //保留，占位，保证与下位机数据结构一致
    public UInt16 len;
    public UInt16 type;
    public byte sum;
    public byte frameTail;
};
//通用串口Tx数据帧结构体
[StructLayout(LayoutKind.Sequential, Pack = 1)] //Pack =4保持与STM32内存结构一致
public struct COMM_TX_GEN_HEADER_T
{
    public byte frameHeader;        //帧头一字节
    //public byte rsvU8;  //保留，占位，保证与下位机数据结构一致
    public UInt16 len;
    public UInt16 type;
};
//串口Rx Tx数据帧结构体
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct COMM_FRAME_T
{
    public byte frameHeader;
    //public byte rsvu8_1;		//占位，用于调整字节对齐
    public UInt16 len;
    public UInt16 type;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1536)]
    public byte[] data; //
    public byte sum;
    public byte frameTail;
};

public class CProtocol 
{
    public CProtocol(demo hParent)
	{ 
		this.rxThreadRunning = DefineConstants.FALSE;
		this.m_RcbBufCnt = 0;
		int i; 
		m_hParent = hParent; //接收消息的父窗体 
  //      for (i = 0; i < m_cmd.Length; i++) //20180902
		//{
		//	m_cmd[i][ 1] = (char)DefineConstants.DEFAULT_PRO_CMD_LEN; //default length
		//	m_cmd[i][ 2] = (char)0;
             
		//	int j;
		//	byte sum;
		//	for (sum = 0, j = 0; j < 5; j++)
		//	{
		//		sum +=(byte) m_cmd[i][ j];
		//	}
		//	m_cmd[i][ 5] = (char)sum; 
		//	m_cmd[i][ 5] = (char)DefineConstants.DEFAULT_SUM; 
		//}
		//m_cmd[(int)tagProCmd.PRO_CMD_SWITtagMode][ 1] = (char)DefineConstants.FRM_LEN_SM;
		//m_cmd[(int)tagProCmd.PRO_CMD_RST_ALM][ 1] = (char)DefineConstants.FRM_LEN_RA;
         
        m_rec[0].m_model = new tagRecItem(); 

		string fileName = new string(new char[DefineConstants.MAX_PROFILE_ITEM_LEN]); 
		m_recCnt = 0;
    }

    public bool RxBuffAdd(byte[] rx, int len)
    {
        int i;
        bool ret = true;
        COMM_FRAME_T frame = new COMM_FRAME_T();

        if (len + rxCnt >= RX_BUFF_SIZE)
            return false;

        //添加新收到的数据到末端
        for (i = 0; i < len; i++)
            buffRX[i + rxCnt] = rx[i];
        rxCnt += len; 
        while (sBufGetFrame(ref frame))
        {
            queueRX.Enqueue(frame);         //先进先出的队列
        }
        return ret;
    }

    public bool GetRxFrame(ref COMM_FRAME_T frame)
    {
        bool ret = false;

        if (queueRX.Count >= 1)
        {
            frame = queueRX.Dequeue();
            ret = true;
        }
        return ret;
    }

    public byte[] frame = new byte[DefineConstants.MAX_CMD_LEN];
    public const byte FRM_HEADER = (byte)'[';
    public const byte FRM_TAIL = (byte)']';
    public const byte DEF_CHECKSUM = 0x55; //default frm checksum
    public Queue<COMM_FRAME_T> queueRX = new Queue<COMM_FRAME_T>(); 
//	public SerialPort m_comm = new SerialPort();


	
	//public void LogIn()
	//{  
	//		m_RcbBufCnt = 0;
	//		SwitchMode(tagMode.STOP); 
	//}

	
	public void LogOff()
	{ 
	}

	
	public byte[] SwitchMode(tagMode mode)
	{
        byte[] data = new byte[1];   
        data[0] = (byte)((int)mode); 
        byte[] txBuf = GetCmdFrm(FRAME_TYPE_SM,data, 1);
        return txBuf;
    }
    public byte[] GetCmdFrm(ushort type)
    {
        COMM_TX_GEN_T frm = new COMM_TX_GEN_T();
        frm.frameHeader = FRM_HEADER;
        frm.len = DEF_FRM_LEN;
        frm.type = type;
        frm.sum = DEF_CHECKSUM;
        frm.frameTail = FRM_TAIL;
        byte[] txBuf = StructToBytes(frm);
        return txBuf;
    }
    //Cmd frame with data
    /// <summary>
    /// 获取带数据的命令帧
    /// </summary>
    /// <param name="type">命令</param>
    /// <param name="data">数据</param>
    /// <param name="len">数据长度</param>
    /// <returns></returns>
    public byte[] GetCmdFrm(ushort type, byte[] data, UInt16 len)
    {
        COMM_TX_GEN_HEADER_T frm = new COMM_TX_GEN_HEADER_T();
        frm.frameHeader = FRM_HEADER;
        frm.len = (UInt16)(DEF_FRM_LEN + len);
        frm.type = type;
        byte[] header = StructToBytes(frm);

        byte[] tail = { DEF_CHECKSUM, (byte)FRM_TAIL };
        byte[] txBuf = header.Concat(data).ToArray();
        txBuf = txBuf.Concat(tail).ToArray();
        //txBuf.Concat(tail);
        return txBuf;
    } 
    
    public byte[] GetCfg()
	{ 
        return  GetCmdFrm(FRAME_TYPE_GC);        
    }
	
	public byte[] SetCfg(tagCFG cfg)            //重新写一遍就好了
    {
        byte[] data = StructToBytes(cfg);
        return GetCmdFrm(FRAME_TYPE_SC, data, (ushort)data.Length);
    }
     
	/// <summary>
    /// 读取报警记录
    /// </summary>
    /// <returns></returns>
	public byte[] GetRecord()
	{
        return GetCmdFrm(FRAME_TYPE_GR);
	}

    /// <summary>
    ///清除报警记录
    /// </summary>
    /// <returns></returns>
    public byte[] ClearRecord()
	{
        return GetCmdFrm(FRAME_TYPE_CR);
	}
    /// <summary>
    /// 获取设备状态
    /// </summary>
    /// <returns></returns>
    public byte[] GetState()
	{
        return GetCmdFrm(FRAME_TYPE_GS);
		//m_comm.Write(ref m_cmd[(int)tagProCmd.PRO_CMD_GET_STATE], DefineConstants.DEFAULT_PRO_CMD_LEN);
	}
    //报警记录状态
    //almJudge  = ALM_GOOD ALM_BAD
    /// <summary>
    /// 良品判定
    /// </summary>
    /// <param name="almJudge"></param>
    /// <returns></returns>
    public byte[] ResetAlarm(tagAlmJudge almJudge)
	{
        byte[] data = {(byte)almJudge };
        return GetCmdFrm(FRAME_TYPE_RA, data, (ushort)data.Length); 
	}


    //20180902 清除计数器
    public byte[] ResetCounters()
	{
        return GetCmdFrm(FRAME_TYPE_RC);
	}
	
	//20180902 同步系数时间 
	public tagRTC_DateTimeTypeDef SyncSysTime_rtc = new tagRTC_DateTimeTypeDef();
    /// <summary>
    /// 同步系统时间
    /// </summary>
    /// <returns></returns>
    public byte[] SyncSysTime()
	{ 
        DateTime t=DateTime.Now;

        SyncSysTime_rtc.date.RTC_Year = (byte)(t.Year % 100);
		SyncSysTime_rtc.date.RTC_Month = (byte)(t.Month);
		SyncSysTime_rtc.date.RTC_Date = (byte)(t.Day);
		SyncSysTime_rtc.date.RTC_WeekDay = (byte)(t.DayOfWeek); //1 = Sunday, 2 = Monday, ..., 7 = Saturday.
		if (--SyncSysTime_rtc.date.RTC_WeekDay < 1)
		{
			SyncSysTime_rtc.date.RTC_WeekDay += 7; //7 = Sunday, 1 = Monday, ..., 6 = Saturday.
		}
		SyncSysTime_rtc.time.RTC_Hours = (byte)(t.Hour);
		SyncSysTime_rtc.time.RTC_H12 = (byte)(t.Hour % 12);
		SyncSysTime_rtc.time.RTC_Minutes = (byte)(t.Minute);
		SyncSysTime_rtc.time.RTC_Seconds = (byte)(t.Second);
        byte[] data = StructToBytes(SyncSysTime_rtc);
        return GetCmdFrm(FRAME_TYPE_ST, data, (ushort)data.Length);

	}
	
	//20190618 选择基准波形
	public void ModelSelect(int ch, int idx)//暂时用的功能
	{ 
         
	}

    //终止接收进程
    //public void ExitRxThread()
    //{
    //	if (DefineConstants.FALSE == rxThreadRunning)
    //	{
    //		return; //20171121 如果串口打开失败，不需要等待线程结束
    //	} 
    //}

    public int m_recCnt;           //记录当前收到的记录
    public int rxThreadRunning;

    //public static char[][] m_cmd= { "[  LI ]".ToCharArray(), "[  LO ]".ToCharArray(), "[  SMm ]".ToCharArray(), "[  GC ]".ToCharArray(),
    //    "[  SC ]".ToCharArray(), "[  GR ]".ToCharArray(), "[  CR ]".ToCharArray(), "[  GS ]".ToCharArray(), "[  RAa ]".ToCharArray(),
    //    "[  RC ]".ToCharArray(), "[  ST ]".ToCharArray(), "[  MS   ]".ToCharArray()   };

    #region 接收到的各种消息对应的处理函数
    //    /*********************************************************************************************************
    //		Function: Deal frame from Device
    //	*/
    //    public void cmdSM(byte[] buf)
    //	{
    //        tagMode mode = (tagMode)buf[5];
    //		if ((tagMode.STOP == mode) || (tagMode.WORK == mode) || (tagMode.LEARN == mode))
    //		{
    //			//global::PostMessageA(m_hParent, WM_USER + 503, 0, mode);
    //		}
    //		switch (mode)
    //		{
    //		case tagMode.WORK:
    //			//SetEvent(m_hWorkEvent);
    //			break;
    //		case tagMode.LEARN:
    //			//SetEvent(m_hLearnEvent);
    //			break;
    //		case tagMode.STOP:
    //			//SetEvent(m_hStopEvent);
    //			break;
    //		default:
    //			break; 
    //		}
    //	}
    //	/*********************************************************************************************************
    //		Function: Deal frame from Device
    //	*/
    //	public void cmdGC(byte[] buf)              //获取到配置信息
    //	{
    //		ushort len = (ushort)(buf[1]);
    //		if ((DefineConstants.DEFAULT_PRO_CMD_LEN + Marshal.SizeOf(new tagCFG())) == len)
    //		{
    //            //C++ TO C//# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The memory management function ' Array.Copy' has no equivalent in C//#:
    //            byte[] dstArray = new byte[Marshal.SizeOf(new tagCFG())];
    //            Buffer.BlockCopy(buf, 5, dstArray, 0, dstArray.Length);
    //             tagCFG temp =new tagCFG() ;
    //            BytesToStruct(dstArray, dstArray.Length, temp);
    //           //  Array.Copy(gCCfg.Get(), buf[5], Marshal.SizeOf(new new tagCFG()()));
    //			//gCCfg.SaveCfgFile("");
    //			//global::PostMessageA(m_hParent, WM_USER + 504, (System.IntPtr)gCCfg.Get(), 0);
    //		}
    //	}
    //	/*********************************************************************************************************
    //		Function: Deal frame from Device
    //	*/
    //	public void cmdGR(byte[] buf)              //读取报警记录
    //	{
    //		ushort len = (ushort)(buf[1]);
    //		if ((DefineConstants.DEFAULT_PRO_CMD_LEN + Marshal.SizeOf(new tagRec())) == len)
    //		{
    //            byte[] dstArray = new byte[Marshal.SizeOf(new tagRec())];
    //            Buffer.BlockCopy(buf, 5, dstArray, 0, dstArray.Length);
    //            tagRec p = new tagRec();
    //            BytesToStruct(dstArray, dstArray.Length, p);        //获得对象
    //            //tagRec p = (tagRec)(buf + 5);
    //            int ch = p.m_wave.ch;
    //			if ((ch >= 0) && (ch < DefineConstants.CH_NUM))
    //			{ 
    //                m_rec[ch] = p; 
    //			}
    //		}
    //		if (DefineConstants.DEFAULT_PRO_CMD_LEN == len) //记录发送完毕
    //		{
    //			//global::PostMessageA(m_hParent, WM_USER + 512, 0, 0);
    //		}
    //	}

    //	/*********************************************************************************************************
    //		Function: Deal frame from Device 接收到传感器状态
    //	*/
    //	public void cmdGS(byte[] buf)
    //	{ 
    //        ushort len = (ushort)(buf[1]);
    //        if ((DefineConstants.DEFAULT_PRO_CMD_LEN + DefineConstants.CH_NUM * Marshal.SizeOf(new tagPM_chState()) + 4) == len)
    //		{
    ////C++ TO C//# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The memory management function ' Array.Copy' has no equivalent in C//#:
    //			// Array.Copy(m_state, buf + 5, Marshal.SizeOf(new tagPM_chState()));

    //            byte[] dstArray = new byte[Marshal.SizeOf(new tagRec())];
    //            Buffer.BlockCopy(buf, 5, dstArray, 0, dstArray.Length);
    //            tagPM_chState p = new tagPM_chState();
    //            BytesToStruct(dstArray, dstArray.Length, p);        //获得对象
    //                                                                //C++ TO C//# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: C//# does not have an equivalent to pointers to value types:
    //                                                                //ORIGINAL LINE: int *p = (int*)&buf[5+Marshal.SizeOf(m_state)];
    //            int version = (int) buf[5 + Marshal.SizeOf(new tagPM_chState())];
    //			//int version = p;
    //			//PostMessage(m_hParent, WM_USER + 508, (System.IntPtr)version, (System.IntPtr)m_state);
    //		}
    //	}
    //	/*********************************************************************************************************
    //		Function: Deal frame from Device
    //	*/
    //	public void cmdRP(byte[] buf)
    //    {
    //        ushort len = (ushort)(buf[1]);
    //        if ((DefineConstants.DEFAULT_PRO_CMD_LEN + Marshal.SizeOf(new tagRec())) == len)
    //		{

    //            byte[] dstArray = new byte[Marshal.SizeOf(new tagRec())];
    //            Buffer.BlockCopy(buf, 5, dstArray, 0, dstArray.Length);
    //            tagRec p = new tagRec();
    //            BytesToStruct(dstArray, dstArray.Length, p);        //获得对象

    //           // tagRec p = (tagRec)(buf[ 5]);

    //			int ch = p.m_wave.ch;
    //			if ((ch >= 0) && (ch < DefineConstants.CH_NUM))
    //			{
    //                //C++ TO C//# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The memory management function ' Array.Copy' has no equivalent in C//#:
    //                m_rec[ch] = p;
    //				// Array.Copy(m_rec[ch], buf[ 5], (Marshal.SizeOf(new tagRec())));
    //				if (DefineConstants.REC_VALID == m_rec[ch].m_wave.valid) //采集波形记录有效
    //				{ 
    //					//global::PostMessageA(m_hParent, WM_USER + 510, (System.IntPtr)ch, (System.IntPtr) m_rec[ch]);
    ////					if (null != rec)
    ////					{
    ////						//tagRec pW = m_pRecBuf + m_recCnt; //计算当前写入位置
    ////                        rec.Add(p);
    //////C++ TO C//# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The memory management function ' Array.Copy' has no equivalent in C//#:
    ////						// Array.Copy(pW, m_rec[ch], (Marshal.SizeOf(new tagRec())));
    ////						if (DefineConstants.MAX_REC_BUFF == ++m_recCnt)
    ////						{
    ////							m_recCnt = 0;
    ////						}
    ////					}
    //				}
    //			}
    //		}
    //	}
    //	/*********************************************************************************************************
    //		Function: Deal frame from Device   //报警 
    //	*/
    //	public void cmdAL(byte[] buf)
    //    {
    //        ushort len = (ushort)(buf[1]);
    //        if (DefineConstants.FRM_LEN_AL == len)
    //		{
    //			int ch = buf[5];
    //			//global::PostMessageA(m_hParent, WM_USER + 513, 0, (System.IntPtr)ch);
    //		}
    //	} 
    //	public void cmdMS(byte[] buf)
    //    {
    //        ushort len = (ushort)(buf[1]);
    //        if ((DefineConstants.DEFAULT_PRO_CMD_LEN + 2) == len)
    //		{
    //			int ch = buf[5];
    //			int idx = buf[6]; 
    //		}
    //	}
    #endregion
    /// 计算校验和
    /// 
    public bool Checksum(byte[] buf)
    {
        ushort len = (ushort)(buf[1]);
        if ((DefineConstants.MAX_CMD_LEN < len) || (0 == len))
		{
			return false;
		} 
//#if CHECKSUM_EN
		int i;
		byte sum = 0;
		for (i = 0; i < len - 2; i++)
		{
			sum += buf[i];
		}
		if (sum == buf[i])
		{
			return true;
		}
		else
		{
			return false;
		}
//#else
		if (DefineConstants.DEFAULT_SUM == buf[len - 2])
		{
			return true;
		}
		else
		{
			return false;
		}
//#endif
	} 
    public bool sBufGetFrame(ref COMM_FRAME_T frame)
    {
        bool ret = false;
        int i;
        int start, end;
        int len = 0;
        int lng = 0;    //LNG in frame
        int frmLen; 
        //在buff中查找帧头
        for (i = 0; i < rxCnt; i++)
        {
            if (FRM_HEADER == buffRX[i])
            {
                break; 
            } 
        }
        start = i;
        if (start != 0)
        {
            for (i = 0; i < rxCnt - start; i++)
                buffRX[i] = buffRX[i + start]; 
        }
        rxCnt -= start;
        if (rxCnt >= DEF_FRM_LEN)
        {
            //lng = frame.len;// lng = *(u16*)(_sBuf[port] + 1);//lng = _sBuf[port][1] * 256 + _sBuf[port][2];
            lng = (int)buffRX[1] + (int)buffRX[2] * 256;
            if ((FRM_LEN_MAX < lng) || (lng == 0))  //如果接收到的长度信息不正确，删除缓冲区第一字节
            {
                rxCnt--;
                for (i = 0; i < rxCnt; i++)
                    buffRX[i] = buffRX[i + 1];          //？？？？
                lng = 0;
            }
        } 
        if (lng != 0)
        {
            frmLen = lng; //frmLen = lng + HEADER_LEN + TAIL_LEN;
            if (rxCnt >= frmLen)    //接收数据结束
            {
                if ((FRM_TAIL == buffRX[frmLen - 1]) && (DEF_CHECKSUM == buffRX[frmLen - 2]))   //frame tail OK Checksum OK
                {
                    end = frmLen;
                    byte[] buf = new byte[FRM_LEN_MAX];
                    for (i = 0; i < end; i++)
                        buf[i] = buffRX[i];
                    len = frmLen;
                    for (i = end; i < rxCnt; i++)
                    {
                        buffRX[i - end] = buffRX[i];
                    }
                    rxCnt = rxCnt - end;
                    frame.data = new byte[frmLen - DEF_FRM_LEN]; 
                    frame = (COMM_FRAME_T)BytesToStruct(buf, frmLen, typeof(COMM_FRAME_T));
                    ret = true;
                }
                else    //if frame tail err, delete frame head, search frame again
                {
                    rxCnt--;
                    for (i = 0; i < rxCnt; i++)
                        buffRX[i] = buffRX[i + 1];
                }
            }
        }
        return ret;
    }

	/*********************************************************************************************************
		检查帧长有效性
	*/ 
	public bool isValidLen(int len)
	{
		if ((DefineConstants.DEFAULT_PRO_CMD_LEN == len) || ((DefineConstants.DEFAULT_PRO_CMD_LEN + Marshal.SizeOf(new tagRec())) == len) ||
            ((DefineConstants.DEFAULT_PRO_CMD_LEN + Marshal.SizeOf(new tagCFG())) == len) || ((DefineConstants.DEFAULT_PRO_CMD_LEN 
            + DefineConstants.CH_NUM * Marshal.SizeOf(new tagPM_chState()) + 4) == len) || (DefineConstants.FRM_LEN_RA == len) ||
            (DefineConstants.FRM_LEN_SM == len) || ((DefineConstants.DEFAULT_PRO_CMD_LEN + 2) == len))
		{
			return true;
		}
		else
		{
			return false;
		}
	} 
    //StructToBytes
    public byte[] StructToBytes(object obj)
    {
        int rawsize = Marshal.SizeOf(obj);
        IntPtr buffer = Marshal.AllocHGlobal(rawsize);
        Marshal.StructureToPtr(obj, buffer, false);
        byte[] rawdatas = new byte[rawsize];
        Marshal.Copy(buffer, rawdatas, 0, rawsize);
        Marshal.FreeHGlobal(buffer);
        return rawdatas;
    }
    //BytesToStruct
    public   IntPtr buffer = Marshal.AllocHGlobal(2000);
    public object BytesToStruct(byte[] buf, int len, Type type)
    {
         object rtn=null; 
        try
        {
            Marshal.Copy(buf, 0, buffer, len);
            rtn = Marshal.PtrToStructure(buffer, type);

        }catch(Exception e)
        {
            MessageBox.Show(e.ToString());
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
        //Marshal.FreeHGlobal(buffer);
        return rtn;
    }
    //BytesToStruct
    public void BytesToStruct(byte[] buf, int len, object rtn)
    {
        IntPtr buffer = Marshal.AllocHGlobal(len);
        Marshal.Copy(buf, 0, buffer, len);
        Marshal.PtrToStructure(buffer, rtn);
        Marshal.FreeHGlobal(buffer);
    }
    //BytesToStruct
    public void BytesToStruct(byte[] buf, object rtn)
    {
        BytesToStruct(buf, buf.Length, rtn);
    }
    //BytesToStruct
    public object BytesToStruct(byte[] buf, Type type)
    {
        return BytesToStruct(buf, buf.Length, type);
    }
    public demo m_hParent; //接收消息的父窗体
    public tagRec[] m_rec = new tagRec[DefineConstants.CH_NUM]; // m_rec[0]:基准波形  m_rec[1]:检测波形
    public tagPM_chState[] m_state = new tagPM_chState[(DefineConstants.CH_NUM)];
    public int m_RcbBufCnt;
    //public List<tagRec> rec = new List<tagRec>();      //记录缓存，
    public tagMode currMode = 0;
    const int RX_BUFF_SIZE = 20000;
    byte[] buffRX = new byte[RX_BUFF_SIZE];    //RX buffer
    int rxCnt = 0;
    public static UInt16 DEF_FRM_LEN = 7;//默认帧长度
    public static UInt16 FRM_LEN_RP = (UInt16)(DEF_FRM_LEN + Marshal.SizeOf(typeof(tagRecItem)));
    static UInt16 FRM_LEN_MAX = 4096;       //最大帧长度
    //cmd 
    public const UInt16 FRAME_TYPE_LI = 0x494C; //"LI" Login
    public const UInt16 FRAME_TYPE_LO = 0x4F4C; //"LO" Logout
    public const UInt16 FRAME_TYPE_SM = 0x4D53; //"SM" SetMode
    public const UInt16 FRAME_TYPE_GC = 0x4347; //"GC" GetConfig
    public const UInt16 FRAME_TYPE_SC = 0x4353; //"SC" SetConfig 
    public const UInt16 FRAME_TYPE_GR = 0x5247; //"GR" Get record
    public const UInt16 FRAME_TYPE_CR = 0x5243; //"CR" clr record
    public const UInt16 FRAME_TYPE_GS = 0x5347; //"GS" Get state
    public const UInt16 FRAME_TYPE_RA = 0x4152; //"RA" Reset Alarm
    public const UInt16 FRAME_TYPE_RP = 0x5052; //"RP" Report
    public const UInt16 FRAME_TYPE_AL = 0x4C41; //"AL" Alarm
    public const UInt16 FRAME_TYPE_CE = 0x4543; //"CE" Check ENC
    public const UInt16 FRAME_TYPE_SN = 0x4E53; //"SN" Get SN
    public const UInt16 FRAME_TYPE_RG = 0x4752; //"RG" Registration
    public const UInt16 FRAME_TYPE_RC = 0x4352; //"RC" Reset counter
    public const UInt16 FRAME_TYPE_ST = 0x5453; //"ST" Set RTC Time
    public const UInt16 FRAME_TYPE_MS = 0x534D; //"MS" Mode select

    //public const UInt16 FRAME_TYPE_FS = 0x5346; //"FS" Factory Setting
    //public const UInt16 FRAME_TYPE_DT = 0x5444; //"DT" Start Detection
    //public const UInt16 FRAME_TYPE_LN = 0x4E4C; //"LN" 传感器上传采样线数据
    public const UInt16 FRAME_TYPE_NK = 0x4E4B; //"NK" Device busy


} 