using System;
using System.ComponentModel;
namespace NaitonControls
{
  [ToolboxItem(false)]
  public partial class DateRangeSelectorSyncPopup1 : DateRangeSelectorPopup1
  {
    protected override void startCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.StartCheckBox == true)
      {
        this.EndCheckBox = true;

        //this.startEndButton.Enabled = true;

        this.monthGroupBox.Enabled = true;
        this.dayGroupBox.Enabled = true;
        this.yearGroupBox.Enabled = true;
                this.presetsGroupBox.Enabled = true;
        this.startLabel.Enabled = true;
        this.endLabel.Enabled = true;
      }
      else
      {
        this.EndCheckBox = false;
        //this.startEndButton.Enabled = false;

        this.monthGroupBox.Enabled = false;
        this.dayGroupBox.Enabled = false;
        this.yearGroupBox.Enabled = false;
                this.presetsGroupBox.Enabled = false;
                //this.startLabel.Enabled = false;
                //this.endLabel.Enabled = false;
            }
    }

    protected override void endCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.EndCheckBox == true)
      {
        this.StartCheckBox = true;
       // this.startEndButton.Enabled = true;

        this.monthGroupBox.Enabled = true;
        this.dayGroupBox.Enabled = true;
        this.yearGroupBox.Enabled = true;
                this.presetsGroupBox.Enabled = true;
                this.startLabel.Enabled = true;
        this.endLabel.Enabled = true;
      }
      else
      {
        this.StartCheckBox = false;
        //this.startEndButton.Enabled = false;

        this.monthGroupBox.Enabled = false;
        this.dayGroupBox.Enabled = false;
        this.yearGroupBox.Enabled = false;
                this.presetsGroupBox.Enabled = false;
                //this.startLabel.Enabled = false;
                //this.endLabel.Enabled = false;
            }
    }
  }
}
