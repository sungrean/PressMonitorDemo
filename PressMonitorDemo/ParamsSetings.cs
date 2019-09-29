using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PressMonitorDemo
{
    public partial class ParamsSetings : Form
    {
        demo hParent;
        tagCFG cFG;
        public ParamsSetings(demo hParent)
        {
            InitializeComponent();
            this.hParent = hParent;
            cFG = hParent.cFG;
            cFG.errAdj = 12f;
        }

        private void btkSubmit_Click(object sender, EventArgs e)
        {
            #region //注释下面的代码因为，不用等到提交时才读取修改的参数，而是在编辑的时候修改。
            
            //cFG.code = new tagCode();       //密码有效设置
            //cFG.code.learneEn = comboBoxPswValidLearn.SelectedIndex == 0 ? SYS_CFG_TYPE.CFG_ENABLE : SYS_CFG_TYPE.CFG_DISABLE;
            //cFG.code.modifySettingEn = comboBoxPswValidParam.SelectedIndex == 0 ? SYS_CFG_TYPE.CFG_ENABLE : SYS_CFG_TYPE.CFG_DISABLE;
            //cFG.code.almCLrEn = comboBoxPswValidAlarm.SelectedIndex == 0 ? SYS_CFG_TYPE.CFG_ENABLE : SYS_CFG_TYPE.CFG_DISABLE;
            //cFG.code.modifyToleranceEn = comboBoxPswValidTolerance.SelectedIndex == 0 ? SYS_CFG_TYPE.CFG_ENABLE : SYS_CFG_TYPE.CFG_DISABLE;
            //cFG.code.clrCntEn = comboBoxPswValidClearCount.SelectedIndex == 0 ? SYS_CFG_TYPE.CFG_ENABLE : SYS_CFG_TYPE.CFG_DISABLE;
            //cFG.code.clrAlmRecEn = comboBoxPswValidClearnError.SelectedIndex == 0 ? SYS_CFG_TYPE.CFG_ENABLE : SYS_CFG_TYPE.CFG_DISABLE;
            //switch(comboBoxUnit.SelectedIndex)              //单位
            //{
            //    case 0:
            //        cFG.unit = SYS_CFG_TYPE.UNIT_KG;
            //        break;
            //    case 1:
            //        cFG.unit = SYS_CFG_TYPE.UNIT_KN;
            //        break;
            //    case 2:
            //        cFG.unit = SYS_CFG_TYPE.UNIT_LB;
            //        break;
            //    default:
            //        cFG.unit = SYS_CFG_TYPE.UNIT_KG;
            //        break;
            //}
            //cFG.learnNum = int.Parse(comboBoxNumLearn.Text);
            //cFG.stopAfterLearn = false;
            //cFG.touch = new tagTpAdj();
            //cFG.touch.xfac = 0;
            //cFG.touch.xoff = 0;
            //cFG.touch.yfac = 0;
            //cFG.touch.yoff = 0;
            //cFG.touch.isAdj = DefineConstants.CLR_TOUCH_BY_PC;        
            #endregion

            hParent.cProtocol.SetCfg(cFG);
            hParent.cFG = cFG;          //值回传给父窗口。
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void ParamsSetings_Load(object sender, EventArgs e)
        {

        }

        private void comboBoxTolerance_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxToleranceIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxTolerance_SelectedIndexChanged(sender, e);
        }

        private void ParamsSetings_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnCancel_Click(sender, e);
        }
    }
}
