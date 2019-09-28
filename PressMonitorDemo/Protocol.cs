using PressMonitorDemo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
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

//串口指令定义
public enum tagProCmd
{
	PRO_CMD_LOG_IN = 0,
	PRO_CMD_LOG_OFF,
	PRO_CMD_SWITtagMode,
	PRO_CMD_GET_CFG,
	PRO_CMD_SET_CFG,
	PRO_CMD_GET_REC,
	PRO_CMD_CLR_REC,
	PRO_CMD_GET_STATE,
	PRO_CMD_RST_ALM,
	PRO_CMD_REPORT,
	PRO_CMD_ALARM,
	PRO_CMD_BUSY,
	PRO_CMD_RESET_CNT, //20180902    //清除计数器
	PRO_CMD_SYNC_TIME, //20180902    //同步系统时间
	PRO_CMD_MODEL_SEL, //20190618 选择基准波形
}


public class tagPM_chState
{
	public tagMode mode = new tagMode(); //模式
	public int tolIdx; //公差编号
	public int total; //总数
	public int alarm; //报警数
	public int bad; //实际不良数
	public tagSensorState sensor = new tagSensorState(); //传感器接入状态
}


public class CProtocol 
{
////#if ! CHECKSUM_EN
////#define DEFAULT_SUM
//#endif
	public CProtocol(demo hParent)
	{
		this.m_isLogin = false;
		this.rxThreadRunning = DefineConstants.FALSE;
		this.m_RcbBufCnt = 0;
		int i;

		//Debug.Assert(null != hParent);
		m_hParent = hParent; //接收消息的父窗体

        //size_t
        //m_cmd[(int)tagProCmd.PRO_CMD_LOG_IN] = "[  LI ]".ToCharArray();
        //m_cmd[(int)tagProCmd.PRO_CMD_LOG_OFF] = "[  LO ]";
        //m_cmd[(int)tagProCmd.PRO_CMD_SWITtagMode] = "[  SMm ]"; //SwitchMode set m=Mode
        //m_cmd[(int)tagProCmd.PRO_CMD_GET_CFG] = "[  GC ]";
        //m_cmd[(int)tagProCmd.PRO_CMD_SET_CFG] = "[  SC ]";
        //m_cmd[(int)tagProCmd.PRO_CMD_GET_REC] = "[  GR ]";
        //m_cmd[(int)tagProCmd.PRO_CMD_CLR_REC] = "[  CR ]";
        //m_cmd[(int)tagProCmd.PRO_CMD_GET_STATE] = "[  GS ]";
        //m_cmd[(int)tagProCmd.PRO_CMD_RST_ALM] = "[  RAa ]";
        //m_cmd[(int)tagProCmd.PRO_CMD_RESET_CNT] = "[  RC ]"; //20180902    //清除计数器
        //m_cmd[(int)tagProCmd.PRO_CMD_SYNC_TIME] = "[  ST ]"; //20180902    //同步系统时间
        //m_cmd[(int)tagProCmd.PRO_CMD_MODEL_SEL] = "[  MS   ]"; //20180902    //ModelSel

        //for(i = PRO_CMD_LOG_IN; i <= PRO_CMD_RST_ALM; i++)
        for (i = 0; i < m_cmd.Length; i++) //20180902
		{
			m_cmd[i][ 1] = (char)DefineConstants.DEFAULT_PRO_CMD_LEN; //default length
			m_cmd[i][ 2] = (char)0;

//#if CHECKSUM_EN
			int j;
			byte sum;
			for (sum = 0, j = 0; j < 5; j++)
			{
				sum +=(byte) m_cmd[i][ j];
			}
			m_cmd[i][ 5] = (char)sum;
//#else
			m_cmd[i][ 5] = (char)DefineConstants.DEFAULT_SUM;
//#endif
		}
		m_cmd[(int)tagProCmd.PRO_CMD_SWITtagMode][ 1] = (char)DefineConstants.FRM_LEN_SM;
		m_cmd[(int)tagProCmd.PRO_CMD_RST_ALM][ 1] = (char)DefineConstants.FRM_LEN_RA;

        //C++ TO C//# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The memory management function 'memset' has no equivalent in C//#:
        //memset(m_rec, 0, Marshal.SizeOf(new tagRec()));//清空基准波形
        m_rec[0].m_model = new tagRecItem();
        //
        // 本程序配置文件和设备配置文件
        //
        //FILE pFile;
        //errno_t err = new errno_t();

        //检查是否有配置文件
  //      err = fopen_s(pFile, (".\\config.cfg"), "r");
  //      if (0 != err)
		//{
  //          WritePrivateProfileString("RUN", "PORT", "COM1", (".\\config.cfg"));
  //          WritePrivateProfileString("RUN", "DEV_CFG_FILE", (".\\defcfg.dev"), (".\\config.cfg"));
  //          WritePrivateProfileString("RUN", "EXT_CSV_FILE_NAME", "0", (".\\config.cfg")); //0：不使用扩展CSV文件名，CSV文件名仅包含时间信息。1：使用扩展CSV文件名。CSV文件名包含用户自定义信息及时间信息
  //          WritePrivateProfileString("RUN", "LOGIN_AUTO_STOP", "0", (".\\config.cfg")); //0：不使用该功能。1：登录时自动切换到停机模式
  //          WritePrivateProfileString("RUN", "LOGOFF_AUTO_STOP", "0", (".\\config.cfg")); //0：不使用该功能。1：脱机时自动切换到停机模式
  //      }
		//else
		//{
		//	fclose(pFile);
		//}

		string fileName = new string(new char[DefineConstants.MAX_PROFILE_ITEM_LEN]);
		//GetPrivateProfileString("RUN", "DEV_CFG_FILE", (".\\defcfg.dev"), fileName, DefineConstants.MAX_PROFILE_ITEM_LEN, (".\\config.cfg"));
		//err = fopen_s(pFile, fileName, "r");
         
		//
		//  Record buffer
		//
		m_recCnt = 0; 

	}
//	public void Dispose()
//	{
//		CloseHandle(m_hThreadExitEvent);
//		if (null != m_pRecBuf)
//		{
////C++ TO C//# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The memory management function 'free' has no equivalent in C//#:
//			free(m_pRecBuf);
//		}

//		CloseHandle(m_hWorkEvent);
//		CloseHandle(m_hLearnEvent);
//		CloseHandle(m_hStopEvent);
//		CloseHandle(m_hAlarmEvent);
//		CloseHandle(m_hEvtDelRecFinish);
//	}

	/////////////////////////////////////////////////////////////////////////////
	//  接收线程
//C++ TO C//# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: This was formerly a static local variable declaration (not allowed in C//#):
	private static byte[] rxThread_buff = new byte[DefineConstants.MAX_CMD_LEN];
	byte[] frame = new byte[DefineConstants.MAX_CMD_LEN];
	public void rxThread(object arg)
	{
		CProtocol p = (CProtocol)arg;
		sbyte c;
//C++ TO C//# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: This static local variable declaration (not allowed in C//#) has been moved just prior to the method:
//		static byte buff[DefineConstants.MAX_CMD_LEN], frame[DefineConstants.MAX_CMD_LEN];

		p.rxThreadRunning = DefineConstants.TRUE;

		while (true)
		{
			//uint dwRes = WaitForSingleObject(p.m_hThreadExitEvent, 100);
			//switch (dwRes)
			//{
			//case WAIT_OBJECT_0:
			//	p.rxThreadRunning = DefineConstants.FALSE;
			//	break;
			//case STATUS_TIMEOUT:
			//	if (DefineConstants.FALSE == p.m_comm.m_RxThreadRunning) //comm接收线程已退出
			//	{
			//		global::PostMessageA(p.m_hParent, WM_USER + 511, 0, 0);
			//		break;
			//	}
			//default:
			//	break;
			//}
			//if (DefineConstants.FALSE == p.rxThreadRunning)
			//{
			//	break;
			//}
			//while (p.m_comm.ReadChar(ref c) != 0)
			//{
			//	rxThread_buff[p.m_RcbBufCnt++] = c;
			//	if (p.m_RcbBufCnt >= DefineConstants.MAX_CMD_LEN)
			//	{
			//		break;
			//	}
			//}

			//分析帧
			if (p.sBufGetFrame(rxThread_buff, ref p.m_RcbBufCnt, frame) != 0)
			{
				p.frameDeal(frame);
			}
		}

		//ExitThread((uint)0);
	}

	public SerialPort m_comm = new SerialPort();


	/////////////////////////////////////////////////////////////////////////////
	public void LogIn()
	{
		//m_pFrame =(CMDIFrameWnd*)AfxGetApp()->m_pMainWnd;
		if (false == m_isLogin)
		{
			if (false == m_comm.IsOpen)
			{
				string port = new string(new char[100]);
				//GetPrivateProfileString("RUN", "PORT", "COM1", port, 100, (".\\config.cfg"));//
                m_comm.Open();
    //            if ()// (ref port, 115200, 10240, 10240))
				//{
				//	global::PostMessageA(m_hParent, WM_USER + 500, 0, 0);
				//	return;
				//}
			}
			if (DefineConstants.FALSE == rxThreadRunning)
			{
				uint threadId;
				//m_hRcv = CreateThread(0, 0, (LPTHREAD_START_ROUTINE) & rxThread, this, (uint) null, threadId);
                //开启接收线程
			}
			m_RcbBufCnt = 0;

			SwitchMode(tagMode.STOP);
			//m_comm.Purge();
            //发送登陆命令
			//m_comm.Write(ref m_cmd[(int)tagProCmd.PRO_CMD_LOG_IN], DefineConstants.DEFAULT_PRO_CMD_LEN);
		}
	}

	/////////////////////////////////////////////////////////////////////////////
	public void LogOff()
	{
		if (m_isLogin)
		{
            //发送脱机命令
			//m_comm.Write(ref m_cmd[(int)tagProCmd.PRO_CMD_LOG_OFF], DefineConstants.DEFAULT_PRO_CMD_LEN);
		}
	}

	/////////////////////////////////////////////////////////////////////////////
	public void SwitchMode(tagMode mode)
	{
		string cmd = new string(new char[DefineConstants.FRM_LEN_SM]);
        //C++ TO C//# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The memory management function ' Array.Copy' has no equivalent in C//#:
        char[] cmds = new char[DefineConstants.FRM_LEN_SM];
         Array.Copy( m_cmd[2],cmds, DefineConstants.FRM_LEN_SM);
        cmd = new string(cmds);     //;
		cmd = StringFunctions.ChangeCharacter(cmd, 5, (char)mode); 
        if (DefineConstants.CHECKSUM_EN)
        {
		    int j;
		    byte sum=0;
		    for (sum = 0, j = 0; j < 6; j++)
		    {
			    sum += (byte)cmd[j];
		    }
		    cmd = StringFunctions.ChangeCharacter(cmd, 6, (char)sum);
        }
        else
        {
            cmd = StringFunctions.ChangeCharacter(cmd, 6, (char)DefineConstants.DEFAULT_SUM);
        }
//#else
		//cmd = StringFunctions.ChangeCharacter(cmd, 6,(char) DefineConstants.DEFAULT_SUM);
//#endif
		//m_comm.Purge();
		m_RcbBufCnt = 0;
        m_comm.Write(cmd);
        //发送相应指令
		//m_comm.Write(ref cmd, DefineConstants.FRM_LEN_SM);
        //
	}

	/////////////////////////////////////////////////////////////////////////////
	public string GetCfg()
	{
        return new string(m_cmd[(int)tagProCmd.PRO_CMD_GET_CFG]) ;
        //m_comm.Write(ref m_cmd[(int)tagProCmd.PRO_CMD_GET_CFG], DefineConstants.DEFAULT_PRO_CMD_LEN);
    }
	/////////////////////////////////////////////////////////////////////////////
	public void SetCfg()            //重新写一遍就好了
	{ 


	}

	///////////////////////////////////////////////////////////////////////////////
	//void CProtocol::SetConfig(CFG_T * pCfg)
	//{
	//}

	/////////////////////////////////////////////////////////////////////////////
	public void GetRecord()
	{
		//m_comm.Write(ref m_cmd[(int)tagProCmd.PRO_CMD_GET_REC], DefineConstants.DEFAULT_PRO_CMD_LEN);
	}

	/////////////////////////////////////////////////////////////////////////////
	public void ClearRecord()
	{
		//m_comm.Write(ref m_cmd[(int)tagProCmd.PRO_CMD_CLR_REC], DefineConstants.DEFAULT_PRO_CMD_LEN);
	}

	/////////////////////////////////////////////////////////////////////////////
	public void GetState()
	{
		//m_comm.Write(ref m_cmd[(int)tagProCmd.PRO_CMD_GET_STATE], DefineConstants.DEFAULT_PRO_CMD_LEN);
	}

	/////////////////////////////////////////////////////////////////////////////
	//报警记录状态
	//almJudge  = ALM_GOOD ALM_BAD
	public void ResetAlarm(tagAlmJudge almJudge)
	{
		string cmd = new string(new char[DefineConstants.FRM_LEN_SM]);
//C++ TO C//# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The memory management function ' Array.Copy' has no equivalent in C//#:
		 Array.Copy(cmd.ToCharArray(), m_cmd[(int)tagProCmd.PRO_CMD_RST_ALM], DefineConstants.FRM_LEN_RA);
		cmd = StringFunctions.ChangeCharacter(cmd, 5,(char) almJudge);

//#if CHECKSUM_EN
		int j;
		byte sum;
		for (sum = 0, j = 0; j < 6; j++)
		{
			sum += (byte)cmd[j];
		}
		cmd = StringFunctions.ChangeCharacter(cmd, 6, (char)sum);
//#else
		cmd = StringFunctions.ChangeCharacter(cmd, 6, (char)DefineConstants.DEFAULT_SUM);
//#endif
//		m_comm.Write(ref cmd, DefineConstants.FRM_LEN_RA);
	}

	/////////////////////////////////////////////////////////////////////////////
	//20180902 清除计数器
	public void ResetCounters()
	{
		//m_comm.Write(ref m_cmd[(int)tagProCmd.PRO_CMD_RESET_CNT], DefineConstants.DEFAULT_PRO_CMD_LEN);
	}
	/////////////////////////////////////////////////////////////////////////////
	//20180902 同步系数时间
//C++ TO C//# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: This was formerly a static local variable declaration (not allowed in C//#):
	private tagRTC_DateTimeTypeDef SyncSysTime_rtc = new tagRTC_DateTimeTypeDef();
	public void SyncSysTime()
	{
        //C++ TO C//# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: This static local variable declaration (not allowed in C//#) has been moved just prior to the method:
        //		static tagRTC_DateTimeTypeDef rtc;

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

		string cmd = new string(new char[(DefineConstants.DEFAULT_PRO_CMD_LEN + Marshal.SizeOf(new tagRTC_DateTimeTypeDef()))]);
//C++ TO C//# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The memory management function ' Array.Copy' has no equivalent in C//#:
		// Array.Copy(cmd, m_cmd[(int)tagProCmd.PRO_CMD_SYNC_TIME], DefineConstants.DEFAULT_PRO_CMD_LEN);
		//(ushort)(cmd.Substring(1)) = (DefineConstants.DEFAULT_PRO_CMD_LEN + Marshal.SizeOf(new tagRTC_DateTimeTypeDef()));

        //C++ TO C//# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The memory management function ' Array.Copy' has no equivalent in C//#:
        //cmd.Replace(cmd.Substring(5, Marshal.SizeOf(new tagRTC_DateTimeTypeDef())), SyncSysTime_rtc);
		 //Array.Copy(cmd.Substring(5), SyncSysTime_rtc, Marshal.SizeOf(new tagRTC_DateTimeTypeDef()));

//#if CHECKSUM_EN
//		int j;
//		byte sum;
//		for (sum = 0, j = 0; j < (DefineConstants.DEFAULT_PRO_CMD_LEN + Marshal.SizeOf(new tagRTC_DateTimeTypeDef())) - 2; j++)
//		{
//			sum += cmd[j];
//		}
//		cmd = StringFunctions.ChangeCharacter(cmd, (DefineConstants.DEFAULT_PRO_CMD_LEN + Marshal.SizeOf(new tagRTC_DateTimeTypeDef())) - 2,(char) sum);
////#else
//		cmd = StringFunctions.ChangeCharacter(cmd, (DefineConstants.DEFAULT_PRO_CMD_LEN + Marshal.SizeOf(new tagRTC_DateTimeTypeDef())) - 2, (char)DefineConstants.DEFAULT_SUM);
//#endif
		//cmd = StringFunctions.ChangeCharacter(cmd, (DefineConstants.DEFAULT_PRO_CMD_LEN + Marshal.SizeOf(new tagRTC_DateTimeTypeDef())) - 1, DefineConstants.FRM_TAIL);
		//m_comm.Write(ref cmd, (DefineConstants.DEFAULT_PRO_CMD_LEN + Marshal.SizeOf(new tagRTC_DateTimeTypeDef())));
	}
	/////////////////////////////////////////////////////////////////////////////
	//20190618 选择基准波形
	public void ModelSelect(int ch, int idx)
	{ 



	}

	//终止接收进程
	public void ExitRxThread()
	{
		if (DefineConstants.FALSE == rxThreadRunning)
		{
			return; //20171121 如果串口打开失败，不需要等待线程结束
		} 
	}
	public bool m_isLogin;

	//public   tagCFG REC_GetCfgStruct()
	//{
	//	if (DefineConstants.CFG_VALID == gCCfg.Get().valid)
	//	{
	//		return gCCfg.Get();
	//	}
	//	else
	//	{
	//		return new tagCFG();
	//	}
	//}

	/*********************************************************************************************************
		Function: Deal frame from Device
	*/
	public void SaveRecFile(string strFileName)             //保存记录文件
	{
		if (rec.Count==0)
		{
            MessageBox.Show("无记录");
            return;
		} 
		else
		{

			string str="";
            str += rec.Count;
			//str.Format("%d", cnt);
			str += (("条记录已保存到文件"));
			str += strFileName;
			MessageBox.Show(str, "PressMonitor", MessageBoxButtons.OK);
		}
	}
	/*********************************************************************************************************
		清除缓存的记录信息
	*/
	public void ClearBuff()
	{
		m_recCnt = 0;
		if (null != rec)
		{
            rec.Clear();
//C++ TO C//# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The memory management function 'memset' has no equivalent in C//#:
			//memset(m_pRecBuf, 0, Marshal.SizeOf(new tagRec()) * DefineConstants.MAX_REC_BUFF);
		}
	}

	//public System.IntPtr m_hWorkEvent;
	//public System.IntPtr m_hLearnEvent;
	//public System.IntPtr m_hStopEvent;
	//public System.IntPtr m_hAlarmEvent;
	//public System.IntPtr m_hEvtDelRecFinish;

	//private  tagCFG m_pCfg;
	//private System.IntPtr m_hRcv; // handle of the receive thread

	//
	//  Flags controlling thread actions
	//
	//private System.IntPtr m_hThreadExitEvent; 
	//
	//  Record buffer
	//
	//private tagRec m_pRecBuf;       //记录缓存
	private int m_recCnt;           //记录当前收到的记录
	private int rxThreadRunning;

    private static char[][] m_cmd= { "[  LI ]".ToCharArray(), "[  LO ]".ToCharArray(), "[  SMm ]".ToCharArray(), "[  GC ]".ToCharArray(),
        "[  SC ]".ToCharArray(), "[  GR ]".ToCharArray(), "[  CR ]".ToCharArray(), "[  GS ]".ToCharArray(), "[  RAa ]".ToCharArray(),
        "[  RC ]".ToCharArray(), "[  ST ]".ToCharArray(), "[  MS   ]".ToCharArray()   };
	/*********************************************************************************************************
		Function: Deal frame from Device
	*/
	private void cmdSM(byte[] buf)
	{
        tagMode mode = (tagMode)buf[5];
		if ((tagMode.STOP == mode) || (tagMode.WORK == mode) || (tagMode.LEARN == mode))
		{
			//global::PostMessageA(m_hParent, WM_USER + 503, 0, mode);
		}
		switch (mode)
		{
		case tagMode.WORK:
			//SetEvent(m_hWorkEvent);
			break;
		case tagMode.LEARN:
			//SetEvent(m_hLearnEvent);
			break;
		case tagMode.STOP:
			//SetEvent(m_hStopEvent);
			break;
		default:
			break; 
		}
	}
	/*********************************************************************************************************
		Function: Deal frame from Device
	*/
	private void cmdGC(byte[] buf)              //获取到配置信息
	{
		ushort len = (ushort)(buf[1]);
		if ((DefineConstants.DEFAULT_PRO_CMD_LEN + Marshal.SizeOf(new tagCFG())) == len)
		{
            //C++ TO C//# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The memory management function ' Array.Copy' has no equivalent in C//#:
            byte[] dstArray = new byte[Marshal.SizeOf(new tagCFG())];
            Buffer.BlockCopy(buf, 5, dstArray, 0, dstArray.Length);
             tagCFG temp =new tagCFG() ;
            BytesToStruct(dstArray, dstArray.Length, temp);
           //  Array.Copy(gCCfg.Get(), buf[5], Marshal.SizeOf(new new tagCFG()()));
			//gCCfg.SaveCfgFile("");
			//global::PostMessageA(m_hParent, WM_USER + 504, (System.IntPtr)gCCfg.Get(), 0);
		}
	}
	/*********************************************************************************************************
		Function: Deal frame from Device
	*/
	private void cmdGR(byte[] buf)
	{
		ushort len = (ushort)(buf[1]);
		if ((DefineConstants.DEFAULT_PRO_CMD_LEN + Marshal.SizeOf(new tagRec())) == len)
		{
            byte[] dstArray = new byte[Marshal.SizeOf(new tagRec())];
            Buffer.BlockCopy(buf, 5, dstArray, 0, dstArray.Length);
            tagRec p = new tagRec();
            BytesToStruct(dstArray, dstArray.Length, p);        //获得对象
            //tagRec p = (tagRec)(buf + 5);
            int ch = p.m_wave.ch;
			if ((ch >= 0) && (ch < DefineConstants.CH_NUM))
			{
                //C++ TO C//# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The memory management function ' Array.Copy' has no equivalent in C//#:
                m_rec[ch] = p;
				// Array.Copy(m_rec[ch], buf + 5, (Marshal.SizeOf(new tagRec())));
				//global::PostMessageA(m_hParent, WM_USER + 506, (System.IntPtr)ch, (System.IntPtr) m_rec[ch]);
				if (null != rec)
				{
					//tagRec pW = m_pRecBuf + m_recCnt; //计算当前写入位置
//C++ TO C//# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The memory management function ' Array.Copy' has no equivalent in C//#:
					// Array.Copy(pW, m_rec[ch], (Marshal.SizeOf(new tagRec())));
					if (DefineConstants.MAX_REC_BUFF == ++m_recCnt)
					{
						m_recCnt = 0;
					}
				}
			}
		}
		if (DefineConstants.DEFAULT_PRO_CMD_LEN == len) //记录发送完毕
		{
			//global::PostMessageA(m_hParent, WM_USER + 512, 0, 0);
		}
	}

	/*********************************************************************************************************
		Function: Deal frame from Device 接收到传感器状态
	*/
	private void cmdGS(byte[] buf)
	{ 
        ushort len = (ushort)(buf[1]);
        if ((DefineConstants.DEFAULT_PRO_CMD_LEN + DefineConstants.CH_NUM * Marshal.SizeOf(new tagPM_chState()) + 4) == len)
		{
//C++ TO C//# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The memory management function ' Array.Copy' has no equivalent in C//#:
			// Array.Copy(m_state, buf + 5, Marshal.SizeOf(new tagPM_chState()));

            byte[] dstArray = new byte[Marshal.SizeOf(new tagRec())];
            Buffer.BlockCopy(buf, 5, dstArray, 0, dstArray.Length);
            tagPM_chState p = new tagPM_chState();
            BytesToStruct(dstArray, dstArray.Length, p);        //获得对象
                                                                //C++ TO C//# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: C//# does not have an equivalent to pointers to value types:
                                                                //ORIGINAL LINE: int *p = (int*)&buf[5+Marshal.SizeOf(m_state)];
            int version = (int) buf[5 + Marshal.SizeOf(new tagPM_chState())];
			//int version = p;
			//PostMessage(m_hParent, WM_USER + 508, (System.IntPtr)version, (System.IntPtr)m_state);
		}
	}
	/*********************************************************************************************************
		Function: Deal frame from Device
	*/
	private void cmdRP(byte[] buf)
    {
        ushort len = (ushort)(buf[1]);
        if ((DefineConstants.DEFAULT_PRO_CMD_LEN + Marshal.SizeOf(new tagRec())) == len)
		{

            byte[] dstArray = new byte[Marshal.SizeOf(new tagRec())];
            Buffer.BlockCopy(buf, 5, dstArray, 0, dstArray.Length);
            tagRec p = new tagRec();
            BytesToStruct(dstArray, dstArray.Length, p);        //获得对象

           // tagRec p = (tagRec)(buf[ 5]);

			int ch = p.m_wave.ch;
			if ((ch >= 0) && (ch < DefineConstants.CH_NUM))
			{
                //C++ TO C//# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The memory management function ' Array.Copy' has no equivalent in C//#:
                m_rec[ch] = p;
				// Array.Copy(m_rec[ch], buf[ 5], (Marshal.SizeOf(new tagRec())));
				if (DefineConstants.REC_VALID == m_rec[ch].m_wave.valid) //采集波形记录有效
				{ 
					//global::PostMessageA(m_hParent, WM_USER + 510, (System.IntPtr)ch, (System.IntPtr) m_rec[ch]);
					if (null != rec)
					{
						//tagRec pW = m_pRecBuf + m_recCnt; //计算当前写入位置
                        rec.Add(p);
//C++ TO C//# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The memory management function ' Array.Copy' has no equivalent in C//#:
						// Array.Copy(pW, m_rec[ch], (Marshal.SizeOf(new tagRec())));
						if (DefineConstants.MAX_REC_BUFF == ++m_recCnt)
						{
							m_recCnt = 0;
						}
					}
				}
			}
		}
	}
	/*********************************************************************************************************
		Function: Deal frame from Device   //报警
	*/
	private void cmdAL(byte[] buf)
    {
        ushort len = (ushort)(buf[1]);
        if (DefineConstants.FRM_LEN_AL == len)
		{
			int ch = buf[5];
			//global::PostMessageA(m_hParent, WM_USER + 513, 0, (System.IntPtr)ch);
		}
	}
	/*********************************************************************************************************
		Function: Deal frame from Device 基准波形切换
	*/
	private void cmdMS(byte[] buf)
    {
        ushort len = (ushort)(buf[1]);
        if ((DefineConstants.DEFAULT_PRO_CMD_LEN + 2) == len)
		{
			int ch = buf[5];
			int idx = buf[6];
			//string str;
			//if (SYS_CFG_TYPE.CHN == CTranslate.GetLanguage())
			//{
			//	str.Format("通道 %d 选择基准波形 %d", ch + 1, idx + 1);
			//}
			//else
			//{
			//	str.Format("CH %d select model %d", ch + 1, idx + 1);
			//}
			//global::MessageBoxA(m_hParent, str, "", MB_OK);
		}
	}

	// 计算校验和
	private bool Checksum(byte[] buf)
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

	/*********************************************************************************************************
		Function: Identify the first frame from buffer, copy the frame to _buf[] and delete the frame from _sBuf[]
		Return: If find frame, return the length of the frame, else return 0.
	*/

	private int sBufGetFrame(byte[] sBuf, ref int pCnt, byte[] buf)
	{
		int i;
		int start;
		int end;
		int len = 0;
		int lng = 0; //LNG in frame
		int frmLen;
		int sCnt = pCnt; 
		//find header
		for (i = 0; i < sCnt; i++)
		{
			if (sBuf[i] == DefineConstants.FRM_HEADER)
			{
				break;
			}
		}
		start = i; 
		//trim char before header
		if (start != 0)
		{
			for (i = 0; i < sCnt - start; i++)
			{
				sBuf[i] = sBuf[i + start];
			}
		}
		sCnt -= start; 
		//Get LNG
		if (sCnt >= DefineConstants.HEADER_LEN)
		{
			lng = (ushort)(sBuf[1]);
			 if (!isValidLen(lng)) //如果接收到的长度信息不正确，删除缓冲区第一字节
			 {
				sCnt--;
				 for (i = 0; i < sCnt; i++)
				 {
					sBuf[i] = sBuf[i + 1];
				 }
			 }
		} 
		if (lng != 0)
		{
			frmLen = lng;
			if (sCnt >= frmLen) //接收数据结束
			{
				if ((sBuf[frmLen - 1] == DefineConstants.FRM_TAIL) && (Checksum(sBuf))) //frame tail OK, checksum OK
				{
					end = frmLen;
					for (i = 0; i < end; i++)
					{
						buf[i] = sBuf[i];
					}
					buf[frmLen] = (byte)'\0';
					len = frmLen;
					for (i = end; i < sCnt; i++)
					{
						sBuf[i - end] = sBuf[i];
					}
					sCnt = sCnt - end;
				}
				else //if frame tail err or checksum err, delete frame head, search frame again
				{
					for (i = 0; i < sCnt - DefineConstants.HEADER_LEN; i++)
					{
						sBuf[i] = sBuf[i + DefineConstants.HEADER_LEN];
					}
					sCnt -= DefineConstants.HEADER_LEN;
				}
			}
		} 
		pCnt = sCnt;
		return len;
	}

	/*********************************************************************************************************
		Function: Deal frame from Device
	*/
	private void frameDeal(byte[] buf)
	{          
		int i;
		char[][] cmdStr =
		{
			"li".ToCharArray(), "lo".ToCharArray(), "sm".ToCharArray(),
			"gc".ToCharArray(), "sc".ToCharArray(), "gr".ToCharArray(),
			"cr".ToCharArray(), "gs".ToCharArray(), "ra".ToCharArray(),
			"rp".ToCharArray(), "al".ToCharArray(), "nk".ToCharArray(),
            "rc".ToCharArray(), "st".ToCharArray(), "ms".ToCharArray()
        };
		for (i = 0; i < DefineConstants.PRO_CMD_NUM; i++)
		{
			if ((buf[3] == cmdStr[i][ 0]) && (buf[4] == cmdStr[i][ 1]))
			{
				break;
			}
		}
        tagProCmd cmd = (tagProCmd)i; //获取命令字

		if ((int)cmd < DefineConstants.PRO_CMD_NUM)
		{
			switch (cmd)
			{
				case tagProCmd.PRO_CMD_LOG_IN:
					m_isLogin = true;
                    currMode = tagMode.LOG_IN;
                    //global::PostMessageA(m_hParent, WM_USER + 501, 0, 0);
                    break;
				case tagProCmd.PRO_CMD_LOG_OFF:
                    currMode = tagMode.LOG_OFF;
                    m_isLogin = false;
					//global::PostMessageA(m_hParent, WM_USER + 502, 0, 0);
					//ExitRxThread();	//接收进程终止
					break;
				case tagProCmd.PRO_CMD_SWITtagMode:
					cmdSM(buf);
					break;
				case tagProCmd.PRO_CMD_GET_CFG:
					cmdGC(buf);
					break;
				case tagProCmd.PRO_CMD_SET_CFG:
					//global::PostMessageA(m_hParent, WM_USER + 505, 0, 0);
					break;
				case tagProCmd.PRO_CMD_GET_REC:
					cmdGR(buf);
					break;
				case tagProCmd.PRO_CMD_CLR_REC:             //清除记录
							//SetEvent(m_hEvtDelRecFinish);
							//global::PostMessageA(m_hParent, WM_USER + 507, 0, 0);
							break;
				case tagProCmd.PRO_CMD_GET_STATE:
					cmdGS(buf);
					break;
				case tagProCmd.PRO_CMD_RST_ALM:
					//global::PostMessageA(m_hParent, WM_USER + 509, 0, 0);
					break;
				case tagProCmd.PRO_CMD_REPORT:
					cmdRP(buf);
					break;
				case tagProCmd.PRO_CMD_ALARM:
					cmdAL(buf);
					break;
				case tagProCmd.PRO_CMD_BUSY:
					//global::PostMessageA(m_hParent, WM_USER + 514, 0, 0);
					break;
				case tagProCmd.PRO_CMD_RESET_CNT:
					//global::PostMessageA(m_hParent, WM_USER + 515, 0, 0);
					break;
				case tagProCmd.PRO_CMD_SYNC_TIME:
					//global::PostMessageA(m_hParent, WM_USER + 516, 0, 0);
					break;
				case tagProCmd.PRO_CMD_MODEL_SEL:
					cmdMS(buf);
					break; //20190618
//C++ TO C//# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: C//# does not allow fall-through from a non-empty 'case':
				default:
					break;
			}
		}
	}
	/*********************************************************************************************************
		检查帧长有效性
	*/ 
	private bool isValidLen(int len)
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
    IntPtr buffer = Marshal.AllocHGlobal(20000);
    public object BytesToStruct(byte[] buf, int len, Type type)
    {
        object rtn; 
        Marshal.Copy(buf, 0, buffer, len);
        rtn = Marshal.PtrToStructure(buffer, type);
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
    public List<tagRec> rec = new List<tagRec>();      //记录缓存，
    public tagMode currMode = 0;
    //CCfg gCCfg;
} 