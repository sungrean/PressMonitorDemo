using System;
using System.Runtime.InteropServices;


/////////////////////////////////////////
//		��¼��
//		����ʵ�ּ���¼�ͱ�����¼�Ĺ���
//		ע�⣡�����еĽṹ����ڴ�ṹ��enum���岻�������κα䶯,
//		����ᵼ�����豸ͨѶʧ�ܣ�
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

//ʱ��/���ڽṹ��
public class tagRTC_DateTimeTypeDef
{
	public tagRtcDateType date = new tagRtcDateType();
	public tagRtcTimeType time = new tagRtcTimeType();
}

//�������Ͷ���
public enum tagAlmType
{
	ALM_AREA = 0, //����ж��쳣
	ALM_PEAK, //��ֵ�ж��쳣
	ALM_SHIFT, //SHIFT�ж��쳣
	ALM_SC, //SC�ж��쳣
	ALM_NONE //��������
}

//����״̬����
public enum tagJudgeType
{
	JUDGE_NONE = 0,
	JUDGE_BAD,
	JUDGE_OK
}

//��¼��Ŀ�ṹ�嶨�壬������¼�е�һ�����Σ���׼���λ򱻲Ⲩ��
public struct  tagRecItem
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = DefineConstants.WAVE_LEN)]
    public ushort[] wave ; //��ǰ����
	public tagRtcDateType RTC_DateStruct  ;
	public tagRtcTimeType RTC_TimeStruct  ;

	public ushort ch; //ͨ����            //20190618
	public ushort modelIdx; //��׼�������        //20190618
	public int totalCnt; //�ܼƸ���
	public int alarmCnt; //��Ʒ����
	public int badCnt; //ʵ�ʲ�������
	public float cpk; //CPK        ANAL_CalcCPK()
	public float stability; //������        ANAL_CalcStb()
	public float load; //Load settings in configuration, to comvert peakVal to Kg
	public ushort peakVal; //ѹ���ķ�ֵ(ADֵ����ʹ��ǰ������Ҫת����λ) ANAL_Align()
	public ushort sampTime; //�������ڿ��

	public int tolIdx; //���������                        ANAL_IsAlm()

	public float areaMax; //����ж����(������/������/DATA)    ANAL_IsAlm()
	public float areaMin;
	public float areaErr;
	public float peakMax; //��ֵ�ж����(������/������/DATA)    ANAL_IsAlm()
	public float peakMin;
	public float peakErr;
	public float shiftMax; //ƫ��ֵ(������/������/DATA)        ANAL_IsAlm()
	public float shiftMin;
	public float shiftErr;
	public float SCMax; //SC(����ƫ��)������/DATA��            ANAL_IsAlm()
	public float SCErr;
	public int almArea; //����״̬                ANAL_IsAlm()
	public int almPeak;
	public int almShift;
	public int almSC;
	public int isAlm;

	public ushort triggerThresh; //������ֵ ��ͨ�� model��¼�Ĵ�����ֵΪ�Զ�����ģʽ�Ĵ�����ֵ        ANAL_FirstLearnAutoMode()

	//���²����Ƿ�������ʱ���õ�
	public int peakIdx; //��ֵ��λ��        ANAL_Align()
	public int alignIdx; //�����λ��        ANAL_Align()    20170611
	public float peakAvr; //���������ѹ�����ε����ֵ��90%���ϵ�����ѹ���ļ��㡢����������ǵ�ƽ��ֵ��Ȼ�����ƽ��ֵ��Ϊѹ���ķ�ֵ�����ж�    ANAL_GetPeakAvr()
	public float peakAvrRef; //peakAvr�ı��ݡ�����׼������������Чʱ��peakAvr��ÿ�β�����ᱻ���¼��㣬peakAvrRef�б���δ�����Ļ�׼���ε�peakAvrֵ

	public int idxAreaStart; //����ж�����㣬����"����ж��ķ�Χ"areaJudgeZone=0ʱ������  areaJudgeZoneָ������ж��Ŀ�ʼλ�á�����ֵΪ0����ʾ��
										//���ж�����Ŀ�ʼλ���Զ��������������Ϊ������ֵ��ʱ�򡢱�ʾ�ǴӲ��η�ֵ���90%��λ�ÿ�ʼ����ָ���ĵ�����Ϊ����ж�����Ŀ�ʼλ�á�
										//ANAL_GetAreaSum(REC_ITEM *)�м���,  ANAL_GetAreaSum(FRAME_T *)��ʹ��
	public int idxAreaEnd; //����ж����յ㡣
	public float areaSum; //����ж�ֵ        ANAL_GetAreaSum(REC_ITEM *)�м���

	public int idxSCStart; //SC�ж���㡣ָ������ƫ�������㿪ʼλ�á����ڂ�Ϊ0����ʾ����Ŀ�ʼλ���Զ������������������ֵ��ʾ�ǴӲ��η�ֵ�Ҳ�80%��λ������ָ���ĵ�����Ϊƫ�����Ŀ�ʼλ�á�
										//ANAL_CalcSCStart()�и�ֵ���ڻ�׼���βɼ��ɹ�ʱ���øú���
	public int idxSCEnd; //SC�ж��յ㡣
	public float SCSum; //SC�ж�������

	public int isModelAdp; //�Ƿ���¹���׼���Ρ���adaptive=CFG_ENABLE�������µ�10 ���Ի�׼���ν�����������������õ�ʱ�����ڲ�����ѹ����ҵ�����Ի�׼���ν��еĲ�����λ���ָ�������Ļ�׼���Ρ�

	public int isJudge; //�Ƿ񾭹��˹��ж�    0������δ����1��ȷ����
	public tagJudgeType judge ; //�˹��ж����    0���ޣ�1�������Ļ���2���ھ���

	//��¼��Ч�Ա�ʶ
	public uint valid; //��¼��Ч�Ա�ʶ
}

//��¼�ṹ�嶨�壬����һ�������ļ�¼
public struct tagRec
{
	public tagRecItem m_model;
	public tagRecItem m_wave;
}

//��¼��
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

	public tagRecItem[] m_model  =new tagRecItem[DefineConstants.CH_NUM]; //����ͨ���Ļ�׼����
	public tagRecItem[] m_wave =new tagRecItem[DefineConstants.CH_NUM]; //����ͨ���ĵ�ǰ����
}