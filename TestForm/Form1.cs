using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestForm
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
            //dateTimeRangeSelector12. = false;
     // this.dateTimeRangeSelector11.SelectedDateChanged += this.OnDateChanged;
      //this.dateTimeRangeSelector11.SelectedDateChanged += this.OnDateChanged1;
    }

    private void button1_Click(object sender, EventArgs e)
    {

    }

    private void OnDateChanged(DateTime startDate, DateTime endDate, bool startCheckBoxChecked, bool endCheckBoxChecked)
    {
      //this.checkBox1.Checked = !this.checkBox1.Checked;
    }

    private void OnDateChanged1(DateTime startDate, DateTime endDate, bool startCheckBoxChecked, bool endCheckBoxChecked)
    {
      MessageBox.Show("hello!");
         
    }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hello");
        }

        //private void checkBox1_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (checkBox1.Checked)
        //    {
        //        dateTimeRangeSelector11.Enabled = dateTimeRangeSelector11.Visible = true;
        //    }
        //    else
        //    {
        //        dateTimeRangeSelector11.Enabled = dateTimeRangeSelector11.Visible = false;
        //    }
        //}
    }
}
