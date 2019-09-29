using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
 

///////////////////////////////////////////////////
//		����ʵ���豸������Ϣ����ʹ���
//		ע�⣡�����еĽṹ����ڴ�ṹ��enum���岻�������κα䶯,
//		����ᵼ�����豸ͨѶʧ�ܣ�
///////////////////////////////////////////////////



//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: C# has no need of forward class declarations:
//class CCfg;

//���ò�����Ч���� 
//�豸���ýṹ����ʹ�õ����������Ͷ���
public enum SYS_CFG_TYPE
{ // TRIG_MODE;
 //Language
 CHN = 0, //CHN����Ϊ0
 ENG = 1, //ENG����Ϊ1
 TRIGGER_MODE_AUTO,
 TRIGGER_MODE_EXT,
 //UNIT
 UNIT_KG,
 UNIT_KN,
 UNIT_LB,
//Enable Disable Define
 CFG_ENABLE,
 CFG_DISABLE,
 CFG_ENABLE_DOWN, //��Ч���£�
 CFG_ENABLE_UP_DOWN, //��Ч�����£�
//TYPE_FLT;
 FLT_150Hz,
 FLT_200Hz,
 FLT_250Hz,
//DIRECTION;
 DIR_LEFT,
 DIR_RIGHT,
 DIR_UP,
 DIR_DOWN,
 TYPE_INVALID //��Ч����
}

//���� �ṹ�� ����һ�鹫��
public class tagTolItem
{
	public float peakP; //20160224
	public float peakN;
	public float areaP;
	public float areaN;
	public float shift;
	public float SC;

    public tagTolItem(float peakP, float peakN, float areaP, float areaN, float shift, float sC)
    {
        this.peakP = peakP;
        this.peakN = peakN;
        this.areaP = areaP;
        this.areaN = areaN;
        this.shift = shift;
        SC = sC;
    }
}

//ͨ�����ýṹ�壬����һ��ͨ�׵����ò���
public struct CH_SETTING_T
{
    public SYS_CFG_TYPE triggerMode;   //������ģʽ  TRIGGER_MODE_AUTO TRIGGER_MODE_EXT ��ֵ��TRIGGER_MODE_AUTO
    public int sampTimeScale;//sampTime;		//20190620 ֮��汾����ʹ�ò������ڿ�ȣ��ò�����ȡ���� �������ڿ��  15 - 150ms  ��ֵ��30 ms			ֻ����ѡ�����ⲿ������ʱ����Ч���������ò��εĲ���ʱ�䡣20171225 �ò�������AD�����ʡ�15-150��20KHz 151-1500�� 2KHz 1501-15000��200Hz
    public int sampDelay;      //��ʱ����ʱ�� 0��150(mSec) ��ֵ��0mSec			ֻ����ѡ�����ⲿ������ʱ����Ч���������ôӴ����ź�ON �����ζ�ȡ��ʼ���ӳ�ʱ��
    public int triggerThreshLearn; //ѧϰʱ�Ĵ�������ֵ 1��10000  (��AD ֵ����Ϊ10000 ʱ�ı���) ��ֵ��150 ��ѡ���Զ�����ģʽʱ����1 ������ʱ��ѹ����֪��ֵ��
    public int triggerThreshWork;  //����ʱ�Ĵ�������ֵ 1��100% (����ڻ�׼�����е����ֵ�İٷֱ�) ��ֵ��60% ��ѡ���Զ�����ģʽʱ����ѧϰ��2 ����ʼ����ʱ��ѹ����֪��ֵ��ָ����Ӧ��ѧϰ��1 ��ѹ�����ֵ�İٷֱ�
    public int extDisturbThresh;   //������ֵ1��10000(��AD ֵ����Ϊ10000 ʱ�ı���) ��ֵ��50 ��ѡ���Զ�����ģʽʱ�����Ծ������βⶨʱ�����ֵ��ѧϰʱ�Ĵ�������ֵ��������ֵ
    public SYS_CFG_TYPE posAdjSwith;//λ�õ��� ��Ч/��Ч ��ֵ����Ч ����ÿ�β����ϵ�ĳ������в��ε����ҷ����ϵ�λ�ö�׼����ѡ���Զ�����ģʽʱ���Զ�����ΪON��
    public SYS_CFG_TYPE posAdjDir; //λ�õ������� ��/�� ��ֵ���� ָ�����ε�λ�ö�׼�����ڲ��η�ֵ���Ҳ໹����ࡣ������ε�ǰ�ࣨ��ֵ���Ҳࣩ�����ϴ����ж��෢������¡����Խ�λ�ö�׼�����õ�����Դﵽ�ж��ȶ���Ч����
    public int posAdjVal;          //λ�õ�������	10��100% ��ֵ��40% ָ�����ε�λ�ö�׼������ڲ��η�ֵ��Ϊ100%ʱ����ֵ��λ�õ�������Ϊ��ʱ�Ƽ�����70%��
    public SYS_CFG_TYPE SCAdjLeft; //��о��λ�õ����� ��Ч(����)/ ��Ч(��)/ ��Ч ����=���ε��ϲ�(+) �²� (-)     ��=���ε��²�(-)  ��ֵ����Ч(����) �����ε�λ�õ�����������Ϊ��ʱ���ڲ���ƫ�����ļ��㹦�ܡ�
                            //��Ч=��ʹ�øù���  ��Ч(��)=���Ȼ�׼����С�Ĳ�����Ϊ�ж�ֵ����Ч�����£�=�����׼����ƫ��ľ���ֵ֮����Ϊ�ж�ֵ��	
    public SYS_CFG_TYPE SCAdjRight;    //��Ч(��)/ ��Ч ��ֵ����Ч(��) �����ε�λ�õ�����������Ϊ��ʱ���ڲ���ƫ�����ļ��㹦�ܡ�
    public int SCZone;             //��о���ж���Χ 0��300 ��ֵ��0 ָ������ƫ�������㿪ʼλ�á����ڂ�Ϊ0����ʾ����Ŀ�ʼλ���Զ������������������ֵ��ʾ�ǴӲ��η�ֵ�Ҳ�80%��λ������ָ���ĵ�����Ϊƫ�����Ŀ�ʼλ�á�	
    public int areaZone;           //����ж��ķ�Χ 0��150 ��ֵ��0 ָ������ж��Ŀ�ʼλ�á�����ֵΪ0����ʾ����ж�����Ŀ�ʼλ���Զ��������������Ϊ������ֵ��ʱ��
                            //��ʾ�ǴӲ��η�ֵ���90%��λ�ÿ�ʼ����ָ���ĵ�����Ϊ����ж�����Ŀ�ʼλ�á�
    public float load;         //���������� �洢��λkg, ��ʾ��λ��������unit�������� ��Χ��1~2000kg 0.1~196.0kN 1~4409lb ��ֵ��600kg ��
                        //����ֵ��ο��±� 
                        //����������ѹ��ֵ������ѹֵ
                        //2.0t  ����600kg����1000mV
                        //500kg ����120kg����1000mV
                        //����ʹ�ñ�׼Ʒ��PSS ������������������Ǽ����б�Ĵ������������ʾ��ѹ��ֵ����ʵ��ѹ��ֵ��ֻ��һ���ο����ݣ���������Ŀ����Ҫ���á�
    public int sensorVoltage;  //��������ѹ  1��1000(mV) ��ֵ��1000	�������������ֵ�ο����еĵ�ѹֵ��������
    public SYS_CFG_TYPE filter;            //��ͨ�˲��� 150Hz/200Hz/250Hz  ��ֵ��150Hz ����Ϊ�˽������������ѹ�еĸ��Ų��ֳ�ȥ�����ϵĵ�ͨ�˲�����ϵ����
    public int areaLevel;      //���LV  10��99%  90%  ���Ƕ�����ж����ж�������նˣ����˵㣩���б���Ĳ����������ֵ������ڷ�ֵѹ���İٷֱȵ���˼���ӷ�ֵѹ����ʼ�������������ֵ*���LV���ĵ㡢Ȼ�󽫸õ���Ϊ����ж���Χ�Ľ����㡣
    public int SCLevel;        //SC LV  10��99%   80%  SC �ж����ж�������նˣ����˵㣩���б���Ĳ������ӷ�ֵѹ����ʼ���Ҳ���������ֵ*SC LV���ĵ㡢Ȼ�󽫸õ���ΪSC �ж���Χ�Ľ����㡣

    public int tolIdx;         //��ͨ��ʹ�õĹ����� ��λ����������ʱʹ�� ȡֵ��Χ��0-47����Ӧ������1-48
    public int reserv1;        //��������������ϵ�����ٷֱȣ���ʾ�Դ�������ֵ���������ϵ����
    public int reserv2;        //��������������ϵ������λ���롣��ʾ�������ָ�ʱ�䳣����
    public int reserv3;        //��������������ϵ���������٣���ʾÿ��ѹ��ʱ�������������ֵ�ı仯����
    public int OKWidth;        //OK�ź�������  100~1000ms		20170726 ��ӵ�ƽ���ģʽ���ù��� 20181102 GOOD_LOW_MODE ��GOOD_HIGH_MODE
    public int modelNum;       //��ͨ���洢�Ļ�׼���ε�����	20190618
    [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
    public bool isModelSW;     //�Ƿ��Զ��л���׼����			20190618
    [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I4)]
    public int[] reserve;		//���� ����չ
};

//�豸����ṹ��
public class tagCode
{
	public SYS_CFG_TYPE learneEn; //ѧϰģʽ����ʹ��        ��Ч/��Ч  ��ֵ����Ч �����ڽ���ѧϰģʽʱ�Ƿ���Ҫ�������롣
	public SYS_CFG_TYPE modifySettingEn; //������������ʹ��        ��Ч/��Ч  ��ֵ����Ч �����ڽ��в������ʱ�Ƿ���Ҫ�������롣
	public SYS_CFG_TYPE almCLrEn; //�����������ʹ��        ��Ч/��Ч  ��ֵ����Ч ���õ��ж�ΪNG ��Ҫ���б�����λʱ�Ƿ���Ҫ�������롣
	public SYS_CFG_TYPE modifyToleranceEn; //�����������ʹ��    ��Ч/��Ч  ��ֵ����Ч ���õ��ı乫�����ʱ�Ƿ���Ҫ�������롣
	public SYS_CFG_TYPE clrCntEn; //�������������ʹ��        ��Ч/��Ч  ��ֵ����Ч ���õ�������������ʱ�Ƿ���Ҫ�������롣
	public SYS_CFG_TYPE clrAlmRecEn; //�쳣��¼�������ʹ��    ��Ч/��Ч  ��ֵ����Ч ���õ���������¼���ʱ�Ƿ���Ҫ�������롣
	public int code; //��������
	public int generalCode; //ͨ������
}
//IOģʽ
//IOģʽ
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define IO_MODE_MAX (OES_IO_MODE | REJECT_IO)
//�豸���������ýṹ�壬����Ҫ�޸�
public class tagTpAdj
{
	public float xfac;
	public float yfac;
	public short xoff;
	public short yoff;
	public int isAdj; //�Ƿ���У׼���
}
//����װ���豸���ýṹ�壬������һ���������豸������Ϣ
[StructLayout(LayoutKind.Sequential, Pack = 1)] //Pack =4������STM32�ڴ�ṹһ��
public struct tagCFG
{
	public uint valid; //If (valid != CFG_VALIC), set the config data to default
	public tagCode code  ; //��������
	public SYS_CFG_TYPE unit; //��λ Kg / kN / lb ��ֵ��Kg �����ڹ���ģʽ��������ʾ�ķ�ֵѹ���ĵ�λ��
	public int learnNum; //ѧϰģʽ���� 2��10�� ��ֵ��4  ������ѧϰʱ�Լ�����Ʒ�Ĳ���ƽ��ֵ��Ϊ��׼���Ρ�
	public bool stopAfterLearn; //ѧϰ��ͣ��
	public tagTpAdj touch  ;
    [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
    public tagTolItem[] tol ;
	public SYS_CFG_TYPE adaptive; //��׼���������� ��Ч/��Ч ��ֵ����Ч �����µ�10����Ʒ�Ĳ��ζԻ�׼���ν��в��������ڴ��ڻ�е����Ư�ơ���˻����ϸù���������Ϊ��Ч�ġ�
	public int modelResetTimer; //��׼����λʱ�� ��λ�� 60��600 �� ��ֵ��600 ����׼���β�����������Ϊ��Чʱ�ò�������Ч����������õ�ʱ�����ڲ�����ѹ����ҵ�����Ի�׼���ν��еĲ�����λ���ָ�������Ļ�׼���Ρ�
	public uint sn; //��Ʒ���к�
	public SYS_CFG_TYPE language; //����
	public int ioMode; //IOģʽ    0��Ĭ�� ��·ģʽ�� 1 OESģʽ
	public float errAdj; //�����������ϵ����0.1~1.0֮�䡣1.00 ���������� ����������Ķ��ӻ��������ϵ��
	public int reserve1; //ADCycleScale 20190620 ȡ���˲�������sampRateȡ��  AD�������ڱ�����1��20KHz���� 10(2KHz)�� 100(200Hz). 20171225
	public uint downCnt; //20180816 �豸������������Ĭ��Ϊ0������������������ʱ��ÿѹ��һ���ݼ����ݼ���0ʱ��ʾ"�豸�����쳣��"
	public int[] reserve; //���� ����չ
    [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
    public CH_SETTING_T[] ch  ; //ͨ����������
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//!!! ch��Ա��������δλ���������Ա�֤�ڶ�ͨ������ch[1]�ڱ��ṹ��β������˫ͨ���汾���ô˽ṹ�塣���ڵ�ͨ���汾���ڴ���ͨѶʱֱ�ӽضϵڶ�ͨ���ĳ�Ա��
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The following method format was not recognized, possibly due to an unrecognized macro:
//typedef tagCFG sizeof(tagCFG)����Ϊż��
  class CCfg
{
    public string PeakCvtString (float val, int ch)
    {        
            float f=0;
            return PeakCvt(val, ch, ref f);        
    }
    public float PeakCvtUnit(float val, int ch)
    {
        float f = 0;
         PeakCvt(val, ch, ref f);
        return f;
    }
    public string GetCfgFileName()
    {
                return m_strCfgFileName;
    }
    public void SetCfgFileName(string fileName)
    {
        this.m_strCfgFileName = fileName;
    }
    public void SaveCfgFile(String strFileName)
    {
        if (strFileName==null|strFileName.Equals(""))
            strFileName = GetCfgFileName();
        else
            SetCfgFileName(strFileName);
        System.IO.StreamWriter f2 = new System.IO.StreamWriter(strFileName, true, System.Text.Encoding.UTF8);
        f2.Write("RUN ", "DEV_CFG_FILE ", strFileName);
        f2.Close();
        f2.Dispose();
        //FileStream pFile; 
    }
    public bool CFG_Check() {
        return false;
    }
    public tagCFG Get() {
        return this.m_deviceCfg;
    }
    public void Set(tagCFG  cfg)
    {
        this.m_deviceCfg = cfg;
    }

    public string PeakCvt(float val, int ch,ref float pPeakCvt)
    {
        string str;
        str = "";
        pPeakCvt = 0.0f;

        tagCFG pCfg = m_deviceCfg ;
        if (pCfg.Equals(null))
            return str;
        //����ֵ���������
        //SOL: ����������(load)��600kg	����ֵ284mV   .            .  ��Ļ���331Kg
        //My:  ����������(load)��60kg 	����ֵ2840mV  . ADֵ  3534 .  ��Ļ���331Kg
        //(AD/4096)*load*p=force(kg)  
        // =>  (3534/4096) * 60 * p = 331    
        // => p = 1.94 (ȡ2)
        // => AD *3.3 * 2 * load / 4096 => force
        float force = (float)(val * 6.6 * pCfg.ch[ch].load / 4096);    //KG
        switch (pCfg.unit)
        {
            case SYS_CFG_TYPE.UNIT_KG:
                pPeakCvt = force;
                str=string.Format("% 5.0f Kg", pPeakCvt );
                break;
            case SYS_CFG_TYPE.UNIT_KN:
                pPeakCvt = force * DefineConstants.KG_KN;
                str = string.Format("% 7.2f KN", pPeakCvt); //1 ǧ���� = 0.00980665 ǧţ��
                break;
            case SYS_CFG_TYPE.UNIT_LB:
                pPeakCvt = force * DefineConstants.KG_LB;
                str = string.Format("% 5.0f lb", pPeakCvt );     //1 ǧ���� = 2.2046 ����
                break;
            default:
                pPeakCvt = force;
                str = string.Format("% 5.0f Kg", pPeakCvt);
                break;
        }
        return str;
    }
    public SYS_CFG_TYPE CFG_CheckVal(SYS_CFG_TYPE val, SYS_CFG_TYPE defVal, SYS_CFG_TYPE min, SYS_CFG_TYPE max)
    {
        SYS_CFG_TYPE ret = val;
        if (val < min)
            ret = defVal;
        if (val > max)
            ret = defVal;
        return ret;
    }

    public tagCFG SetDefaultCfg = new tagCFG();        //Ĭ������
    public tagCFG m_deviceCfg = new tagCFG();          //��ǰ����
    public string m_strCfgFileName;                    //��ǰʹ�õ������ļ�����
} 
/*********************************************************************************************************
	���������Ч�ԡ����������Ч����ΪĬ�ϲ���
*/

/*********************************************************************************************************
	ȷ��������Ч
*/

/*********************************************************************************************************	
	����Ĭ������
*/
/*********************************************************************************************************	
	��ȡָ��ͨ����ֵ�ַ��� ��ת����λ�ķ�ֵ
*/
/*********************************************************************************************************	
	��ȡָ��ͨ����ֵ�ַ���
*/
/*********************************************************************************************************	
	��ֵ���е�λת��
*/
/*********************************************************************************************************	
	���õ�ǰʹ�õ������ļ�����
*/

/*********************************************************************************************************	
	��ȡ��ǰʹ�õ������ļ�����
*/

//����ǰ���ñ��浽�ļ�
//strFileName Ҫ����������ļ��������Ϊ�գ��򱣴浽��ǰ�������ļ�
