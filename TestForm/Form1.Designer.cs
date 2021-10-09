using NaitonControls;

namespace TestForm
{
  partial class Form1
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.dateTimeRangeSelector12 = new NaitonControls.DateTimeRangeSelector1();
            this.dateTimeRangeSelector11 = new NaitonControls.DateTimeRangeSelector1();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // dateTimeRangeSelector12
            // 
            this.dateTimeRangeSelector12.BackColor = System.Drawing.Color.White;
            this.dateTimeRangeSelector12.CheckBoxState = false;
            this.dateTimeRangeSelector12.CurrentControlType = NaitonControls.DateTimeRangeSelector1.ControlType.Date;
            this.dateTimeRangeSelector12.EndCheckBox = true;
            this.dateTimeRangeSelector12.EndDate = new System.DateTime(2020, 6, 15, 2, 26, 33, 330);
            this.dateTimeRangeSelector12.IsNullable = true;
            this.dateTimeRangeSelector12.IsSync = false;
            resources.ApplyResources(this.dateTimeRangeSelector12, "dateTimeRangeSelector12");
            this.dateTimeRangeSelector12.Name = "dateTimeRangeSelector12";
            this.dateTimeRangeSelector12.StartCheckBox = true;
            this.dateTimeRangeSelector12.StartDate = new System.DateTime(2020, 6, 15, 2, 26, 33, 330);
            // 
            // dateTimeRangeSelector11
            // 
            this.dateTimeRangeSelector11.BackColor = System.Drawing.SystemColors.WindowText;
            this.dateTimeRangeSelector11.CheckBoxState = false;
            this.dateTimeRangeSelector11.CurrentControlType = NaitonControls.DateTimeRangeSelector1.ControlType.DateTime;
            this.dateTimeRangeSelector11.EndCheckBox = true;
            this.dateTimeRangeSelector11.EndDate = new System.DateTime(2020, 6, 10, 20, 52, 18, 262);
            this.dateTimeRangeSelector11.IsNullable = true;
            this.dateTimeRangeSelector11.IsSync = false;
            resources.ApplyResources(this.dateTimeRangeSelector11, "dateTimeRangeSelector11");
            this.dateTimeRangeSelector11.Name = "dateTimeRangeSelector11";
            this.dateTimeRangeSelector11.StartCheckBox = true;
            this.dateTimeRangeSelector11.StartDate = new System.DateTime(2020, 6, 10, 20, 52, 18, 262);
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dateTimeRangeSelector12);
            this.Controls.Add(this.dateTimeRangeSelector11);
            this.Name = "Form1";
            this.ResumeLayout(false);

    }


        #endregion
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private DateTimeRangeSelector1 dateTimeRangeSelector11;
        private DateTimeRangeSelector1 dateTimeRangeSelector12;
    }
}

