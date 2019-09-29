internal static class DefineConstants
{
	public const int _SECURE_ATL = 1;
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The following #define constant was defined in different ways:
	public const int CH_NUM = 2; //通道数量
	//public const int CH_NUM = 1; //通道数量
	public const int TRUE = 1;
	public const int FALSE = 0;
	public const int MF_WIDTH = 1024; //Main window width
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The following #define constant was defined in different ways:
	public const int MF_HEIGHT = 768 - 15; //Main window hight
	//public const int MF_HEIGHT = 600; //Main window hight
	public const int MF_BOARDER = 2; //Main window 边框厚度
	public const int BOARDER_GROUPBOX = 2; //Main frame GroupBox boarder size
	public const int FONT_HEIGHT_GROUP_BOX = 14;
	public const int FONT_HEIGHT_WAVE = 14;
	public const int FONT_HEIGHT_TREND = 14;
	public const int FONT_HEIGHT_BT = 24;
	public const int MODE_NUM = 6;
	public const int CFG_VALID = 0x55AA55AA;
	public const int TOL_NUM = 48; //公差组数量
	public const int MAX_MODEL_NUM = 10; //每个通道最多10组基准波形 20190618
	public const int SAMP_RATE = 20000; //20KHz采样率
	public const int MAX_SAMP_TIME_SCALE = 100; //采样窗口倍数 以20K采样率，每n个点取一个采样点
	public const int DEF_SAMP_TIME_SCALE = 1;
	public const int MIN_SAMP_TIME_SCALE = 1;
	public const int MAX_DELAY = 15000; //20171225    150->15000ms
	public const int DEF_DELAY = 0;
	public const int MIN_DELAY = 0;
	public const int MAX_TG_LEARN = 5000;
	public const int DEF_TG_LEARN = 150;
	public const int MIN_TG_LEARN = 20;
	public const int MAX_TG_WORK = 80;
	public const int DEF_TG_WORK = 60;
	public const int MIN_TG_WORK = 10;
	public const int MAX_NOISE = 2500;
	public const int DEF_NOISE = 50;
	public const int MIN_NOISE = 10;
	public const int MAX_POS_ADJ = 100;
	public const int DEF_POS_ADJ = 40;
	public const int MIN_POS_ADJ = 10;
	public const int MAX_SC = 150;
	public const int DEF_SC = 0;
	public const int MIN_SC = 0;
	public const int MAX_RST_TIMER = 600;
	public const int DEF_RST_TIMER = 600;
	public const int MIN_RST_TIMER = 60;
	public const int MAX_AREA = 150;
	public const int DEF_AREA = 0;
	public const int MIN_AREA = 0;
	public const int MAX_LOAD = 4409;
	public const int DEF_LOAD = 600;
	public const double MIN_LOAD = 0.1;
	public const int MAX_SENSOR = 2000;
	public const int DEF_SENSOR = 1000;
	public const int MIN_SENSOR = 20;
	public const int MAX_LEARN_NUM = 10;
	public const int DEF_LEARN_NUM = 4;
	public const int MIN_LEARN_NUM = 2;
	public const int MAX_AREA_LV = 99;
	public const int DEF_AREA_LV = 90;
	public const int MIN_AREA_LV = 10;
	public const int MAX_SC_LV = 99;
	public const int DEF_SC_LV = 80;
	public const int MIN_SC_LV = 10;
	public const int DEF_TOL_IDX = 18;
	public const int MIN_TOL_IDX = 0;
	public const int MAX_OK_WIDTH = 1000; //OK信号输出宽度 ms
	public const int DEF_OK_WIDTH = 150; //OK信号输出宽度 ms
	public const int MIN_OK_WIDTH = 10; //OK信号输出宽度 ms
	public const int GOOD_LOW_MODE = 0; //OK信号电平输出，良品低电平，报警高电平    20181102
	public const int GOOD_HIGH_MODE = 1; //OK信号电平输出，良品高电平，报警低电平    20181102
	public const int SOL_IO_MODE = 0x00; //第0位为0 SOL mode
	public const int OES_IO_MODE = 0x01; //Last bit 1: OES mode
	public const int GOOD_IO = 0x00; //second bit 0 :output good signal    //20171124
	public const int REJECT_IO = 0x02; //second bit 1 :output reject signal    //20171124
	public const int MAX_WRITE_BUFFER = 20480;
	public const int MAX_READ_BUFFER = 20480;
	public const int READ_TIMEOUT = 500;
	public const int WRITE_CHECK_TIMEOUT = 500;
	public const int WAVE_LEN = 300; //波形长度
	public const int REC_VALID = 0x55AA55AA; //记录有效性标识
	public const int MAX_REC_BUFF = 100000;
	public const int PRO_CMD_HEADER_LEN = 10;
	public const int PRO_CMD_NUM = 15; //20190618
	public const char FRM_HEADER = '['; //帧头
	public const char FRM_TAIL = ']'; //帧尾
	public const int HEADER_LEN = 3; //STX + LNG 字节数
	public const int TAIL_LEN = 1; //CR 1 byte
	public const int LNG_NUM = 2;
	public const int MAX_TRAN_ITEM = 1000; //翻译最大条数
	public const int CLR_TOUCH_BY_PC = 0x55AA;
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The following #define constant was defined in different ways:
	public const string VERSION_THIS = "2C V1.03-0000";
	//public const string VERSION_THIS = "1C V1.03-0000";
	public const int MAX_VERSION_NUM = 30;
	public const int INVALID_VERSION = 99999999;
	public const string DEF_PASSWORD = "123"; //通用密码定义
	public const int TREND_NUM = 80;
	public const int CTRL_BT_NUM = 4; // button 此窗口控件数量
	public const int DEFAULT_PRO_CMD_LEN = 7; //默认串口指令长度（不带数据的指令）
	public const int FRM_LEN_SM = 8;
	public const int FRM_LEN_RA = 8;
	public const int FRM_LEN_AL = 8; //alarm
    public const int FRM_LEN_SC = 1484+7;       //set config 设置配置参数
    public const int MAX_CMD_LEN = 4096; //最大帧长度
	public const int DEFAULT_SUM = 0x55; //备用校验和
	public const int MAX_PROFILE_ITEM_LEN = 1000;
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The following #define constant was defined in different ways:
	public const int CALIB_WIDTH = 30; //y轴刻度宽度
	//public const int CALIB_WIDTH = 36; //座标值宽度
	public const int H_SIDE = 5; //水平方向边界空间
	public const int CALIB_Y_WIDTH = 50; //y轴刻度宽度
	public const int CALIB_X_HEIGHT = 20; //x轴刻度高度
	public const int LEGEND_NUM = 5;
	public const int BIG_BAR_WIDTH = 3; //座标上大刻度宽度
	public const int SMALL_BAR_WIDTH = 2; //座标上小刻度宽度
	public const int Y_CALIB = 4; //Y轴刻度数    Calibration
	public const int X_CALIB = 6; //X轴刻度数
	public const int X_SUB_CALIB = 5; //X轴小刻度数
	public const int BT_BOARDER = 5;
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The following #define constant was defined in different ways:
	public const int BOARDER_SIZE = 3;
	//public const int BOARDER_SIZE = 2;
	public const int IDX_CH = 0;
	public const int IDX_SENSOR = 1;
	public const int IDX_PEAK_ERR = 2;
	public const int IDX_AREA_ERR = 3;
	public const int IDX_SHIFT_ERR = 4;
	public const int IDX_SC_ERR = 5;
	public const int IDX_PEAK = 6;
	public const int IDX_CPK = 7;
	public const int IDX_STB = 8;
	public const int IDX_TOTAL = 9;
	public const int IDX_GOOD = 10;
	public const int IDX_ALM = 11;
	public const int IDX_BAD = 12;
	public const int IDX_TOL = 13;
	public const int COL_NUM = 6;
	public const int IDX_STD_TIM = 0;
	public const int IDX_SAMP_TIM = 1;
	public const int IDX_CNT = 2;
	public const int IDC_WAVE_NUM = 3;
	public const int IDC_TREND_NUM = 4;
	public const int IDC_ST_MS_CH = 4;
	public const int IDC_STATE_NUM = 14;
	public const int IDD_ABOUTBOX = 100;
	public const int IDP_OLE_INIT_FAILED = 100;
	public const int IDR_POPUP_EDIT = 119;
	public const int ID_STATUSBAR_PANE1 = 120;
	public const int ID_STATUSBAR_PANE2 = 121;
	public const int IDS_STATUS_PANE1 = 122;
	public const int IDS_STATUS_PANE2 = 123;
	public const int IDS_TOOLBAR_STANDARD = 124;
	public const int IDS_TOOLBAR_CUSTOMIZE = 125;
	public const int ID_VIEW_CUSTOMIZE = 126;
	public const int IDR_MAINFRAME = 128;
	public const int IDR_MAINFRAME_256 = 129;
	public const int IDR_MAINFRAME1 = 129;
	public const int IDR_PressMonitorTYPE = 130;
	public const int IDS_EDIT_MENU = 306;
	public const int IDI_ICON1 = 310;
	public const int IDD_DEV_CFG = 312;
	public const int IDD_COMM_SET = 313;
	public const int IDD_WAITING_DEL_REC = 314;
	public const int IDD_ALARM = 315;
	public const int IDD_VERSION = 316;
	public const int IDD_PASSWORD = 317;
	public const int IDD_EDIT_PASSWORD = 318;
	public const int IDD_INPUT = 319;
	public const int IDD_MODEL_SEL = 320;
	public const int IDC_GROUP_CH = 1000;
	public const int IDC_ST_COM = 1001;
	public const int IDC_COMBO1 = 1001;
	public const int IDC_COMBO_CH = 1001;
	public const int IDC_COMBO_MS_CH = 1001;
	public const int IDC_COMBO_COMM = 1002;
	public const int IDC_COMBO2 = 1002;
	public const int IDC_COMBO_TG_MODE = 1002;
	public const int IDC_COMBO_MS_IDX = 1002;
	public const int IDC_BT_OK = 1003;
	public const int IDC_ST_TG_MODE = 1003;
	public const int IDC_ST_SAMP_WIDTH = 1004;
	public const int IDC_ST_DELAY = 1005;
	public const int IDC_ST_TG_THRESH_LEARN = 1006;
	public const int IDC_ST_TG_THRESH_WORK = 1007;
	public const int IDC_ST_NOIS = 1008;
	public const int IDC_ST_POS_ADJ_EN = 1009;
	public const int IDC_ST_POS_ADJ_DIR = 1010;
	public const int IDC_ST_POS_ADJ_VAL = 1011;
	public const int IDC_ST_SC_LEFT = 1012;
	public const int IDC_ST_SC_RIGHT = 1013;
	public const int IDC_ST_AREA_ZONE = 1014;
	public const int IDC_ST_SC_ZONE = 1015;
	public const int IDC_ST_SC_RIGHT2 = 1016;
	public const int IDC_ST_CH = 1016;
	public const int IDC_ST_LOAD = 1017;
	public const int IDC_ST_VOLTAGE = 1018;
	public const int IDC_EDIT_PASSWORD = 1018;
	public const int IDC_ST_TG_THRESH_WORK2 = 1019;
	public const int IDC_ST_FILTER = 1019;
	public const int IDC_BT_PW_OK = 1019;
	public const int IDC_ST_AREA_LEVEL = 1020;
	public const int IDC_BT_PW_CANCEL = 1020;
	public const int IDC_EDIT_PWE1 = 1020;
	public const int IDC_ST_POS_ADJ_EN2 = 1021;
	public const int IDC_ST_SC_LEVEL = 1021;
	public const int IDC_EDIT_PWE2 = 1021;
	public const int IDC_ST_POS_ADJ_VAL2 = 1022;
	public const int IDC_GROUP_CODE = 1022;
	public const int IDC_BT_PWE_OK = 1022;
	public const int IDC_ST_POS_ADJ_DIR2 = 1023;
	public const int IDC_ST_TOL_IDX = 1023;
	public const int IDC_BT_PWE_CANCEL = 1023;
	public const int IDC_ST_SC_LEFT2 = 1024;
	public const int IDC_GROUP_TOL = 1024;
	public const int IDC_ST_SAMP_WIDTH3 = 1025;
	public const int IDC_ST_ADAPTIVE = 1025;
	public const int IDC_ST_DELAY3 = 1026;
	public const int IDC_ST_RESET_TIMER = 1026;
	public const int IDC_COMBO_POS_ADJ_EN = 1027;
	public const int IDC_COMBO_SC_LEFT = 1028;
	public const int IDC_COMBO_SC_RIGHT = 1029;
	public const int IDC_COMBO_ADAPTIVE = 1030;
	public const int IDC_COMBO_TOL_IDX = 1031;
	public const int IDC_ST_CODE_SETTING = 1032;
	public const int IDC_ST_CODE_LEARN = 1033;
	public const int IDC_ST_CODE_ALM_CLR = 1034;
	public const int IDC_ST_CODE_TOL = 1035;
	public const int IDC_ST_CODE_CLR_CNT = 1036;
	public const int IDC_ST_CODE_CLR_REC = 1037;
	public const int IDC_COMBO_CODE_LEARN = 1038;
	public const int IDC_COMBO_CODE_SETTING = 1039;
	public const int IDC_COMBO_CODE_ALM_CLR = 1040;
	public const int IDC_COMBO_CODE_TOL = 1041;
	public const int IDC_COMBO_CODE_CLR_CNT = 1042;
	public const int IDC_COMBO_CODE_CLR_REC = 1043;
	public const int IDC_ST_UNIT = 1044;
	public const int IDC_COMBO_UNIT = 1045;
	public const int IDC_ST_LEARN_NUM = 1046;
	public const int IDC_COMBO_LEARN_NUM = 1047;
	public const int IDC_ST_LANGUAGE = 1048;
	public const int IDC_ST_IO_MODE = 1048;
	public const int IDC_COMBO_LANGUAGE = 1049;
	public const int IDC_ST_S_ADJ_T = 1049;
	public const int IDC_COMBO_TOL = 1050;
	public const int IDC_ST_TOL = 1051;
	public const int IDC_ST_PEAK_N = 1052;
	public const int IDC_ST_PEAK_P = 1053;
	public const int IDC_ST_AREA_P = 1054;
	public const int IDC_ST_AREA_N = 1055;
	public const int IDC_ST_SHIFT = 1056;
	public const int IDC_ST_SC = 1057;
	public const int IDC_EDIT_SAMP_WIDTH = 1058;
	public const int IDC_EDIT_DELAY = 1059;
	public const int IDC_EDIT_TG_THRESH_LEARN = 1060;
	public const int IDC_EDIT_TG_THRESH_WORK = 1061;
	public const int IDC_EDIT_NOIS = 1062;
	public const int IDC_COMBO_POS_ADJ_DIR = 1063;
	public const int IDC_EDIT_POS_ADJ_VAL = 1064;
	public const int IDC_EDIT_SC_ZONE = 1065;
	public const int IDC_EDIT_VOLTAGE = 1066;
	public const int IDC_COMBO_FILTER = 1067;
	public const int IDC_EDIT_AREA_ZONE = 1068;
	public const int IDC_EDIT_AREA_LEVEL = 1069;
	public const int IDC_EDIT_SC_LEVEL = 1070;
	public const int IDC_EDIT_LOAD = 1071;
	public const int IDC_EDIT_RESET_TIMER = 1072;
	public const int IDC_EDIT_PEAK_P = 1073;
	public const int IDC_EDIT_PEAK_N = 1074;
	public const int IDC_EDIT_AREA_P = 1075;
	public const int IDC_EDIT_AREA_N = 1076;
	public const int IDC_EDIT_SHIFT = 1077;
	public const int IDC_EDIT_SC = 1078;
	public const int IDC_BT_CANCLE = 1079;
	public const int IDC_BT_CFG_OK = 1080;
	public const int IDC_PROGRESS_DEL_REC = 1080;
	public const int IDC_BT_DEL_REC_OK = 1081;
	public const int IDC_EDIT_S_ADJ_E = 1081;
	public const int IDC_BT_DEL_REC_CANCLE = 1082;
	public const int IDC_EDIT_S_ADJ_T = 1082;
	public const int IDC_BT_ALM_BAD = 1083;
	public const int IDC_EDIT_S_ADJ_C = 1083;
	public const int IDC_BT_ALM_OK = 1084;
	public const int IDC_ST_VERSION = 1084;
	public const int IDC_ST_S_ADJ_E = 1084;
	public const int IDC_ST_DEV_VERSION = 1085;
	public const int IDC_ST_S_ADJ_C = 1085;
	public const int IDC_BT_VERSION_OK = 1086;
	public const int IDC_EDIT_OK_WIDTH = 1086;
	public const int IDC_COMBO_IO_MODE = 1087;
	public const int IDC_CK_CLR_TOUCH = 1088;
	public const int IDC_ST_OK_WIDTH = 1089;
	public const int IDC_EDIT_S_ADJ_E2 = 1090;
	public const int IDC_EDIT_ERR_ADJ = 1090;
	public const int IDC_ST_ERR_ADJ = 1091;
	public const int IDC_EDIT_DOWN_CNT = 1092;
	public const int IDC_ST_DOWN_CNT = 1093;
	public const int IDC_EDIT_MODELNUM = 1094;
	public const int IDC_ST_POS_ADJ_VAL3 = 1095;
	public const int IDC_ST_POS_MODEL_NUM = 1095;
	public const int IDC_COMBO_MODEL_AS = 1096;
	public const int IDC_ST_CODE_LEARN2 = 1097;
	public const int IDC_ST_MODEL_SW = 1097;
	public const int IDC_LBL_FILE = 10000;
	public const int IDC_LBL_NO = 10001;
	public const int IDC_ST_FILE = 10002;
	public const int IDC_ST_NO = 10003;
	public const int IDC_ED_FIND = 10004;
	public const int IDC_BT_LAST_REC = 10005;
	public const int IDC_BT_NEXT_REC = 10006;
	public const int IDC_BT_FIND = 10007;
	public const int IDC_BT_LAST_ALM = 10008;
	public const int IDC_BT_NEXT_ALM = 10009;
	public const int IDC_LBL_ALM = 10010;
	public const int IDC_LBL_STATE = 10100;
	public const int IDC_ST_STATE = 10120;
	public const int IDC_LBL_WAVE = 10200;
	public const int IDC_ST_WAVE = 10210;
	public const int IDC_WAVE_WAVE = 10220;
	public const int IDC_WAVE_LEGEND = 10230;
	public const int IDC_LBL_TREND = 10300;
	public const int IDC_TREND_TREND = 10310;
	public const int IDC_BT_WORK = 10400;
	public const int IDC_BT_LEARN = 10401;
	public const int IDC_BT_CLR = 10402;
	public const int IDC_ST_CFG_FILE = 10403;
	public const int IDC_LBL_CFG_FILE = 10404;
	public const int ID_32771 = 32771;
	public const int ID_32772 = 32772;
	public const int ID_32773 = 32773;
	public const int ID_32774 = 32774;
	public const int ID_32775 = 32775;
	public const int ID_32776 = 32776;
	public const int ID_32777 = 32777;
	public const int ID_32778 = 32778;
	public const int ID_32779 = 32779;
	public const int ID_32780 = 32780;
	public const int ID_MN_LOGIN = 32781;
	public const int ID_MN_LOGOUT = 32782;
	public const int ID_MN_OPEN = 32783;
	public const int ID_MN_SAVE = 32784;
	public const int ID_MN_PAR_SET = 32785;
	public const int ID_MN_TOL_SET = 32786;
	public const int ID_MN_LOAD_SET = 32787;
	public const int ID_MN_SAVE_SET = 32788;
	public const int ID_MN_MANAGE_SET = 32789;
	public const int ID_MN_SWICH_CH = 32790;
	public const int ID_MN_LOAD_REC = 32791;
	public const int ID_MN_LANGUAGE = 32792;
	public const int ID_MN_PORT = 32793;
	public const int ID_MN_VERSION = 32794;
	public const int ID_32795 = 32795;
	public const int ID_MN_CLR_REC = 32796;
	public const int ID_32797 = 32797;
	public const int ID_32798 = 32798;
	public const int ID_MN_CHINESE = 32799;
	public const int ID_MN_ENGLISH = 32800;
	public const int ID_32801 = 32801;
	public const int ID_MODIFY_PASSWORD = 32802;
	public const int ID_SYCRN_DEV_TIME = 32803;
	public const int ID_32804 = 32804;
	public const int ID_CLR_CNT = 32805;
	public const int ID_32806 = 32806;
	public const int ID_MN_CLR_ALM = 32807;
	public const int ID_32808 = 32808;
	public const int ID_MODEL_SEL = 32809;
	public const int _APS_NEXT_RESOURCE_VALUE = 321;
	public const int _APS_NEXT_COMMAND_VALUE = 32810;
	public const int _APS_NEXT_CONTROL_VALUE = 1089;
	public const int _APS_NEXT_SYMED_VALUE = 310;     
    //常量
    public const float KG_N = (float)9.80665;	//KG->N
    public const float KG_KN      = (KG_N / 1000);		//KG->KN
    public const float KG_LB  = (float)2.2046;  //KG->lb


    public const bool CHECKSUM_EN = false;  //默认不使用校验和；



    public static readonly tagTolItem[] _tol =
    {
        new tagTolItem(2.5f, 2.5f, 2, 2, 12, 3),
        new tagTolItem(3, 3, 3, 3, 12, 3.5f),
        new tagTolItem(3.5f, 3.5f, 4, 4, 12, 4),
        new tagTolItem(4, 4, 5, 5, 12, 4.5f),
        new tagTolItem(4.5f, 4.5f, 6, 6, 12, 5),
        new tagTolItem(5, 5, 7, 7, 12, 5.5f),
        new tagTolItem(5.5f, 5.5f, 8, 8, 12, 6),
        new tagTolItem(6, 6, 9, 9, 12, 6),
        new tagTolItem(3, 3, 2.5f, 2.5f, 12, 3),
        new tagTolItem(3.5f, 3.5f, 3.5f, 3.5f, 12, 3.5f),
        new tagTolItem(4, 4, 4.5f, 4.5f, 12, 4),
        new tagTolItem(4.5f, 4.5f, 5.5f, 5.5f, 12, 4.5f),
        new tagTolItem(5, 5, 6.5f, 6.5f, 12, 5),
        new tagTolItem(5.5f, 5.5f, 7.5f, 7.5f, 12, 5.5f),
        new tagTolItem(6, 6, 8.5f, 8.5f, 12, 6),
        new tagTolItem(6.5f, 6.5f, 9.5f, 9.5f, 12, 6),
        new tagTolItem(3, 3, 4, 4, 10, 3),
        new tagTolItem(3.5f, 3.5f, 5, 5, 10, 3.5f),
        new tagTolItem(4, 4, 6, 6, 10, 4),
        new tagTolItem(4.5f, 4.5f, 7, 7, 10, 4.5f),
        new tagTolItem(5, 5, 8, 8, 10, 5),
        new tagTolItem(5.5f, 5.5f, 9, 9, 10, 5.5f),
        new tagTolItem(6, 6, 10, 10, 10, 6),
        new tagTolItem(6.5f, 6.5f, 11, 11, 10, 6),
        new tagTolItem(3.5f, 3.5f, 6, 6, 10, 3),
        new tagTolItem(4, 4, 7, 7, 10, 3.5f),
        new tagTolItem(4.5f, 4.5f, 8, 8, 10, 4),
        new tagTolItem(5, 5, 9, 9, 10, 4.5f),
        new tagTolItem(5.5f, 5.5f, 10, 10, 10, 5),
        new tagTolItem(6, 6, 11, 11, 10, 5.5f),
        new tagTolItem(6.5f, 6.5f, 12, 12, 10, 6),
        new tagTolItem(7, 7, 13, 13, 10, 6),
        new tagTolItem(4, 4, 5, 5, 10, 3.5f),
        new tagTolItem(4.5f, 4.5f, 6, 6, 10, 3.5f),
        new tagTolItem(5, 5, 7, 7, 10, 4),
        new tagTolItem(5.5f, 5.5f, 8, 8, 10, 4.5f),
        new tagTolItem(6, 6, 9, 9, 10, 5),
        new tagTolItem(6.5f, 6.5f, 10, 10, 10, 5.5f),
        new tagTolItem(7, 7, 11, 11, 10, 6),
        new tagTolItem(7.5f, 7.5f, 12, 12, 10, 6.5f),
        new tagTolItem(4, 4, 7, 7, 10, 3.5f),
        new tagTolItem(4.5f, 4.5f, 8, 8, 10, 3.5f),
        new tagTolItem(5, 5, 9, 9, 10, 4),
        new tagTolItem(5.5f, 5.5f, 10, 10, 10, 4.5f),
        new tagTolItem(6, 6, 11, 11, 10, 5),
        new tagTolItem(6.5f, 6.5f, 12, 12, 10, 5.5f),
        new tagTolItem(7, 7, 13, 13, 10, 6),
        new tagTolItem(7.5f, 7.5f, 14, 14, 10, 6.5f)
    };


}