namespace NaitonControls
{
    partial class DateTimeRangeSelector1
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.startLabel = new System.Windows.Forms.Label();
            this.endLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // startLabel
            // 
            this.startLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.startLabel.BackColor = System.Drawing.Color.White;
            this.startLabel.Location = new System.Drawing.Point(7, 0);
            this.startLabel.Name = "startLabel";
            this.startLabel.Size = new System.Drawing.Size(62, 20);
            this.startLabel.TabIndex = 11;
            this.startLabel.Text = "21-Sep-19";
            this.startLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // endLabel
            // 
            this.endLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.endLabel.BackColor = System.Drawing.Color.White;
            this.endLabel.Location = new System.Drawing.Point(75, 0);
            this.endLabel.Name = "endLabel";
            this.endLabel.Size = new System.Drawing.Size(84, 20);
            this.endLabel.TabIndex = 12;
            this.endLabel.Text = "21-Sep-19";
            this.endLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DateTimeRangeSelector1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.startLabel);
            this.Controls.Add(this.endLabel);
            this.MinimumSize = new System.Drawing.Size(140, 20);
            this.Name = "DateTimeRangeSelector1";
            this.Size = new System.Drawing.Size(179, 20);
            this.Load += new System.EventHandler(this.DateTimeRangeSelector1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label startLabel;
        private System.Windows.Forms.Label endLabel;
    }
}
