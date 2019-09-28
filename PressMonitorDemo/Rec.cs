using System;
using System.Runtime.InteropServices;


/////////////////////////////////////////
//		记录类
//		本类实现检测记录和报警记录的管理
//		注意！该类中的结构体的内存结构和enum定义不可以做任何变动,
//		否则会导致与设备通讯失败！
/////////////////////////////////////////

//ORIGINAL LINE: #define REC_SIZE (sizeof(REC_T))
/** 
  * @brief  RTC Time structure definition  
  */
public class tagRtcTimeType
{
  public byte RTC_Hours; /*!< Specifies the RTC Time Hour.
                        This parameter must be set to a value in the 0-12 range
                        if the RTC_HourFormat_12 is selected or 0-23 range if
                        the RTC_HourFormat_24 is selected. */

  public byte RTC_Minutes; /*!< Specifies the RTC Time Minutes.
                        This parameter must be set to a value in the 0-59 range. */

  public byte RTC_Seconds; /*!< Specifies the RTC Time Seconds.
                        This parameter must be set to a value in the 0-59 range. */

  public byte RTC_H12; /*!< Specifies the RTC AM/PM Time.
                        This parameter can be a value of @ref RTC_AM_PM_Definitions */
}

/** 
  * @brief  RTC Date structure definition  
  */
public class tagRtcDateType
{
  public byte RTC_WeekDay; /*!< Specifies the RTC Date WeekDay.
                        This parameter can be a value of @ref RTC_WeekDay_Definitions */

  public byte RTC_Month; /*!< Specifies the RTC Date Month (in BCD format).
                        This parameter can be a value of @ref RTC_Month_Date_Definitions */

  public byte RTC_Date; /*!< Specifies the RTC Date.
                        This parameter must be set to a value in the 1-31 range. */

  public byte RTC_Year; /*!< Specifies the RTC Date Year.
                        This parameter must be set to a value in the 0-99 range. */
}

//时间/日期结构体
public class tagRTC_DateTimeTypeDef
{
	public tagRtcDateType date = new tagRtcDateType();
	public tagRtcTimeType time = new tagRtcTimeType();
}

//报警类型定义
public enum tagAlmType
{
	ALM_AREA = 0, //面积判定异常
	ALM_PEAK, //峰值判定异常
	ALM_SHIFT, //SHIFT判定异常
	ALM_SC, //SC判定异常
	ALM_NONE //波形正常
}

//报警状态定义
public enum tagJudgeType
{
	JUDGE_NONE = 0,
	JUDGE_BAD,
	JUDGE_OK
}

//记录项目结构体定义，描述记录中的一个波形，基准波形或被测波形
public struct  tagRecItem
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = DefineConstants.WAVE_LEN)]
    public ushort[] wave ; //当前波形
	public tagRtcDateType RTC_DateStruct  ;
	public tagRtcTimeType RTC_TimeStruct  ;

	public ushort ch; //通道号            //20190618
	public ushort modelIdx; //基准波形序号        //20190618
	public int totalCnt; //总计个数
	public int alarmCnt; //次品个数
	public int badCnt; //实际不良个数
	public float cpk; //CPK        ANAL_CalcCPK()
	public float stability; //安定性        ANAL_CalcStb()
	public float load; //Load settings in configuration, to comvert peakVal to Kg
	public ushort peakVal; //压力的峰值(AD值，在使用前跟据需要转换单位) ANAL_Align()
	public ushort sampTime; //采样窗口宽度

	public int tolIdx; //公差组序号                        ANAL_IsAlm()

	public float areaMax; //面积判定结果(＋公差/－公差/DATA)    ANAL_IsAlm()
	public float areaMin;
	public float areaErr;
	public float peakMax; //峰值判定结果(＋公差/－公差/DATA)    ANAL_IsAlm()
	public float peakMin;
	public float peakErr;
	public float shiftMax; //偏移值(＋公差/－公差/DATA)        ANAL_IsAlm()
	public float shiftMin;
	public float shiftErr;
	public float SCMax; //SC(波形偏差)（公差/DATA）            ANAL_IsAlm()
	public float SCErr;
	public int almArea; //报警状态                ANAL_IsAlm()
	public int almPeak;
	public int almShift;
	public int almSC;
	public int isAlm;

	public ushort triggerThresh; //触发阈值 本通道 model记录的触发阈值为自动触发模式的触发阈值        ANAL_FirstLearnAutoMode()

	//以下参数是分析波形时会用到
	public int peakIdx; //峰值点位置        ANAL_Align()
	public int alignIdx; //对齐点位置        ANAL_Align()    20170611
	public float peakAvr; //搜索相对于压力波形的最大值的90%以上的所有压力的检测点、并计算出它们的平均值、然后将这个平均值作为压力的峰值进行判定    ANAL_GetPeakAvr()
	public float peakAvrRef; //peakAvr的备份。当基准値补偿机能有效时，peakAvr在每次采样后会被重新计算，peakAvrRef中保存未补偿的基准波形的peakAvr值

	public int idxAreaStart; //面积判定的起点，仅在"面积判定的范围"areaJudgeZone=0时起作用  areaJudgeZone指定面积判定的开始位置。初期值为0、表示面
										//积判定计算的开始位置自动决定。如果设置为其它数值的时候、表示是从波形峰值左侧90%的位置开始向左到指定的点数作为面积判定计算的开始位置。
										//ANAL_GetAreaSum(REC_ITEM *)中计算,  ANAL_GetAreaSum(FRAME_T *)中使用
	public int idxAreaEnd; //面积判定的终点。
	public float areaSum; //面积判定值        ANAL_GetAreaSum(REC_ITEM *)中计算

	public int idxSCStart; //SC判定起点。指定波形偏差量计算开始位置。初期値为0、表示计算的开始位置自动决定。如果是其它数值表示是从波形峰值右侧80%的位置向左到指定的点数作为偏差计算的开始位置。
										//ANAL_CalcSCStart()中赋值，在基准波形采集成功时调用该函数
	public int idxSCEnd; //SC判定终点。
	public float SCSum; //SC判定面积求和

	public int isModelAdp; //是否更新过基准波形。当adaptive=CFG_ENABLE，以最新的10 根对基准波形进行修正。如果在设置的时间以内不进行压着作业、将对基准波形进行的补偿复位、恢复到最初的基准波形。

	public int isJudge; //是否经过人工判定    0：数据未定，1：确定。
	public tagJudgeType judge ; //人工判定结果    0：无，1：真正的坏，2：在决定

	//记录有效性标识
	public uint valid; //记录有效性标识
}

//记录结构体定义，描述一条完整的记录
public struct tagRec
{
	public tagRecItem m_model;
	public tagRecItem m_wave;
}

//记录类
public class CRec : System.IDisposable
{
//	public CRec()
//	{
////C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The memory management function 'memset' has no equivalent in C#:
//		memset(m_wave, 0, sizeof(tagRecItem));
////C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The memory management function 'memset' has no equivalent in C#:
//		memset(m_model, 0, sizeof(tagRecItem));
//	}
	public void Dispose()
	{
	}

	public tagRecItem[] m_model  =new tagRecItem[DefineConstants.CH_NUM]; //两个通道的基准波形
	public tagRecItem[] m_wave =new tagRecItem[DefineConstants.CH_NUM]; //两个通道的当前波形
}