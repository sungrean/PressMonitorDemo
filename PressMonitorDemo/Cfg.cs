using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
 

///////////////////////////////////////////////////
//		本类实现设备配置信息保存和处理
//		注意！该类中的结构体的内存结构和enum定义不可以做任何变动,
//		否则会导致与设备通讯失败！
///////////////////////////////////////////////////



//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: C# has no need of forward class declarations:
//class CCfg;

//配置参数有效区间 
//设备配置结构体中使用到的配置类型定义
public enum SYS_CFG_TYPE
{ // TRIG_MODE;
 //Language
 CHN = 0, //CHN必需为0
 ENG = 1, //ENG必需为1
 TRIGGER_MODE_AUTO,
 TRIGGER_MODE_EXT,
 //UNIT
 UNIT_KG,
 UNIT_KN,
 UNIT_LB,
//Enable Disable Define
 CFG_ENABLE,
 CFG_DISABLE,
 CFG_ENABLE_DOWN, //有效（下）
 CFG_ENABLE_UP_DOWN, //有效（上下）
//TYPE_FLT;
 FLT_150Hz,
 FLT_200Hz,
 FLT_250Hz,
//DIRECTION;
 DIR_LEFT,
 DIR_RIGHT,
 DIR_UP,
 DIR_DOWN,
 TYPE_INVALID //无效定义
}

//公差 结构体 描述一组公差
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

//通道配置结构体，描述一个通首的配置参数
public struct CH_SETTING_T
{
    public SYS_CFG_TYPE triggerMode;   //触发器模式  TRIGGER_MODE_AUTO TRIGGER_MODE_EXT 初值：TRIGGER_MODE_AUTO
    public int sampTimeScale;//sampTime;		//20190620 之后版本将不使用采样窗口宽度，用采样率取代。 采样窗口宽度  15 - 150ms  初值：30 ms			只有在选择了外部触发器时才有效。用于设置波形的测量时间。20171225 该参数决定AD采样率。15-150间20KHz 151-1500间 2KHz 1501-15000间200Hz
    public int sampDelay;      //延时触发时间 0～150(mSec) 初值：0mSec			只有在选择了外部触发器时才有效。可以设置从触发信号ON 到波形读取开始的延迟时间
    public int triggerThreshLearn; //学习时的触发器阈值 1～10000  (当AD 值总数为10000 时的比例) 初值：150 当选择自动触发模式时、第1 根采样时的压力检知阈值。
    public int triggerThreshWork;  //工作时的触发器阈值 1～100% (相对于基准波形中的最大值的百分比) 初值：60% 当选择自动触发模式时、从学习第2 根开始工作时的压力检知阈值。指定对应于学习第1 根压力最大值的百分比
    public int extDisturbThresh;   //干扰阈值1～10000(当AD 值总数为10000 时的比例) 初值：50 当选择自动触发模式时、用以决定波形测定时间的数值。学习时的触发器阈值＞干扰阈值
    public SYS_CFG_TYPE posAdjSwith;//位置调整 有效/无效 初值：有效 利用每次波形上的某个点进行波形的左右方向上的位置对准。当选择自动触发模式时将自动设置为ON。
    public SYS_CFG_TYPE posAdjDir; //位置调整方向 左/右 初值：右 指定波形的位置对准点是在波形峰值的右侧还是左侧。如果波形的前侧（峰值的右侧）波动较大误判定多发的情况下、可以将位置对准点设置到左侧以达到判定稳定的效果。
    public int posAdjVal;          //位置调整比例	10～100% 初值：40% 指定波形的位置对准点相对于波形峰值作为100%时的数值。位置调整方向为左时推荐设置70%。
    public SYS_CFG_TYPE SCAdjLeft; //少芯线位置调整左 有效(上下)/ 有效(下)/ 无效 上下=波形的上部(+) 下部 (-)     下=波形的下部(-)  初值：有效(上下) 当波形的位置调整方向设置为左时关于波形偏差量的计算功能。
                            //无效=不使用该功能  有效(下)=将比基准波形小的部分作为判定值。有效（上下）=将与基准波形偏差的绝对值之和作为判断值。	
    public SYS_CFG_TYPE SCAdjRight;    //有效(下)/ 无效 初值：有效(下) 当波形的位置调整方向设置为右时关于波形偏差量的计算功能。
    public int SCZone;             //少芯线判定范围 0～300 初值：0 指定波形偏差量计算开始位置。初期値为0、表示计算的开始位置自动决定。如果是其它数值表示是从波形峰值右侧80%的位置向左到指定的点数作为偏差计算的开始位置。	
    public int areaZone;           //面积判定的范围 0～150 初值：0 指定面积判定的开始位置。初期值为0、表示面积判定计算的开始位置自动决定。如果设置为其它数值的时候、
                            //表示是从波形峰值左侧90%的位置开始向左到指定的点数作为面积判定计算的开始位置。
    public float load;         //传感器负荷 存储单位kg, 显示单位由配置中unit变量决定 范围：1~2000kg 0.1~196.0kN 1~4409lb 初值：600kg 。
                        //设置值请参考下表。 
                        //传感器——压力值——电压值
                        //2.0t  ——600kg——1000mV
                        //500kg ——120kg——1000mV
                        //对于使用标准品的PSS 传感器的情况、由于是检测倾斜的传感器、因此显示的压力值不是实际压力值、只是一个参考数据，因此这个项目不需要设置。
    public int sensorVoltage;  //传感器电压  1～1000(mV) 初值：1000	请参照上述设置值参考表中的电压值进行设置
    public SYS_CFG_TYPE filter;            //帯通滤波器 150Hz/200Hz/250Hz  初值：150Hz 决定为了将传感器输入电压中的干扰部分除去而加上的低通滤波器的系数。
    public int areaLevel;      //面积LV  10～99%  90%  这是对面积判定的判定领域的终端（终了点）进行变更的参数。这个数值是相对于峰值压力的百分比的意思。从峰值压力开始向左侧搜索（峰值*面积LV）的点、然后将该点作为面积判定范围的结束点。
    public int SCLevel;        //SC LV  10～99%   80%  SC 判定的判定领域的终端（终了点）进行变更的参数。从峰值压力开始向右侧搜索（峰值*SC LV）的点、然后将该点作为SC 判定范围的结束点。

    public int tolIdx;         //该通道使用的公差组 上位机下载配置时使用 取值范围：0-47，对应公差编号1-48
    public int reserv1;        //国产传感器补偿系数，百分比，表示对传感器幅值的最大修正系数。
    public int reserv2;        //国产传感器补偿系数，单位：秒。表示传感器恢复时间常数。
    public int reserv3;        //国产传感器补偿系数，无量纲，表示每次压接时传感器的输出幅值的变化量。
    public int OKWidth;        //OK信号输出宽度  100~1000ms		20170726 添加电平输出模式配置功能 20181102 GOOD_LOW_MODE 或GOOD_HIGH_MODE
    public int modelNum;       //本通道存储的基准波形的数量	20190618
    [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
    public bool isModelSW;     //是否自动切换基准波形			20190618
    [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I4)]
    public int[] reserve;		//保留 备扩展
};

//设备密码结构体
public class tagCode
{
	public SYS_CFG_TYPE learneEn; //学习模式密码使能        有效/无效  初值：有效 设置在进入学习模式时是否需要输入密码。
	public SYS_CFG_TYPE modifySettingEn; //参数更改密码使能        有效/无效  初值：有效 设置在进行参数变更时是否需要输入密码。
	public SYS_CFG_TYPE almCLrEn; //警报解除密码使能        有效/无效  初值：有效 设置当判定为NG 需要进行报警复位时是否需要输入密码。
	public SYS_CFG_TYPE modifyToleranceEn; //公差更改密码使能    有效/无效  初值：有效 设置当改变公差号码时是否需要输入密码。
	public SYS_CFG_TYPE clrCntEn; //计数器清除密码使能        有效/无效  初值：有效 设置当将计数器清零时是否需要输入密码。
	public SYS_CFG_TYPE clrAlmRecEn; //异常记录清除密码使能    有效/无效  初值：有效 设置当将报警记录清除时是否需要输入密码。
	public int code; //密码设置
	public int generalCode; //通用密码
}
//IO模式
//IO模式
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define IO_MODE_MAX (OES_IO_MODE | REJECT_IO)
//设备触摸屏配置结构体，不需要修改
public class tagTpAdj
{
	public float xfac;
	public float yfac;
	public short xoff;
	public short yoff;
	public int isAdj; //是否已校准标记
}
//管理装置设备配置结构体，描述了一份完整的设备配置信息
[StructLayout(LayoutKind.Sequential, Pack = 1)] //Pack =4保持与STM32内存结构一致
public struct tagCFG
{
	public uint valid; //If (valid != CFG_VALIC), set the config data to default
	public tagCode code  ; //密码设置
	public SYS_CFG_TYPE unit; //单位 Kg / kN / lb 初值：Kg 设置在工作模式画面中显示的峰值压力的单位。
	public int learnNum; //学习模式个数 2～10次 初值：4  设置在学习时以几根良品的波形平均值作为基准波形。
	public bool stopAfterLearn; //学习后停机
	public tagTpAdj touch  ;
    [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
    public tagTolItem[] tol ;
	public SYS_CFG_TYPE adaptive; //基准値补偿机能 有效/无效 初值：有效 以最新的10根良品的波形对基准波形进行补偿。由于存在机械的热漂移、因此基本上该功能是设置为有效的。
	public int modelResetTimer; //基准値复位时间 单位秒 60～600 秒 初值：600 当基准波形补偿机能设置为有效时该参数才有效。如果在设置的时间以内不进行压着作业、将对基准波形进行的补偿复位、恢复到最初的基准波形。
	public uint sn; //产品序列号
	public SYS_CFG_TYPE language; //语言
	public int ioMode; //IO模式    0（默认 索路模式） 1 OES模式
	public float errAdj; //分析结果修正系数。0.1~1.0之间。1.00 不进行修正 仅对质量差的端子机设计修正系数
	public int reserve1; //ADCycleScale 20190620 取消此参数，以sampRate取代  AD采样周期倍数。1（20KHz）， 10(2KHz)， 100(200Hz). 20171225
	public uint downCnt; //20180816 设备寿命计数器。默认为0，无限寿命。大于零时，每压接一条递减。递减到0时显示"设备计数异常！"
	public int[] reserve; //保留 备扩展
    [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
    public CH_SETTING_T[] ch  ; //通道参数设置
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//!!! ch成员必需在最未位，这样可以保证第二通道参数ch[1]在本结构体尾部。单双通道版本共用此结构体。对于单通道版本，在串口通讯时直接截断第二通道的成员。
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The following method format was not recognized, possibly due to an unrecognized macro:
//typedef tagCFG sizeof(tagCFG)必需为偶数
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
        //波峰值计算比例：
        //SOL: 传感器负荷(load)：600kg	波峰值284mV   .            .  屏幕输出331Kg
        //My:  传感器负荷(load)：60kg 	波峰值2840mV  . AD值  3534 .  屏幕输出331Kg
        //(AD/4096)*load*p=force(kg)  
        // =>  (3534/4096) * 60 * p = 331    
        // => p = 1.94 (取2)
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
                str = string.Format("% 7.2f KN", pPeakCvt); //1 千克力 = 0.00980665 千牛顿
                break;
            case SYS_CFG_TYPE.UNIT_LB:
                pPeakCvt = force * DefineConstants.KG_LB;
                str = string.Format("% 5.0f lb", pPeakCvt );     //1 千克力 = 2.2046 磅力
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

    public tagCFG SetDefaultCfg = new tagCFG();        //默认配置
    public tagCFG m_deviceCfg = new tagCFG();          //当前配置
    public string m_strCfgFileName;                    //当前使用的配置文件名称
} 
/*********************************************************************************************************
	检验参数有效性。如果参数无效，设为默认参数
*/

/*********************************************************************************************************
	确保参数有效
*/

/*********************************************************************************************************	
	设置默认配置
*/
/*********************************************************************************************************	
	获取指定通道峰值字符串 和转换单位的峰值
*/
/*********************************************************************************************************	
	获取指定通道峰值字符串
*/
/*********************************************************************************************************	
	峰值进行单位转换
*/
/*********************************************************************************************************	
	设置当前使用的配置文件名称
*/

/*********************************************************************************************************	
	获取当前使用的配置文件名称
*/

//将当前配置保存到文件
//strFileName 要保存的配置文件名。如果为空，则保存到当前的配置文件
