using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace NaitonControls
{
    [ToolboxItem(false)]
    public partial class TimeRangeSelectorPopup1 : UserControl
    {
        #region Private variables
        private bool isStart = true;
        private Label endLabel;
        private Button startEndButton;
        private Button cancelButton;
        private Button okButton;
        private DCButtons button38;
        private DCButtons button37;
        private DCButtons button36;
        private DCButtons button35;
        private DCButtons button34;
        private DCButtons button33;
        private DCButtons button32;
        private DCButtons button31;
        private DCButtons button30;
        private DCButtons button29;
        private DCButtons button28;
        private DCButtons button27;
        private DCButtons button26;
        private DCButtons button25;
        private DCButtons button12;
        private DCButtons button11;
        private DCButtons button10;
        private DCButtons button9;
        private DCButtons button8;
        private DCButtons button7;
        private DCButtons button6;
        private DCButtons button5;
        private DCButtons button4;
        private DCButtons button3;
        private System.Windows.Forms.TextBox hourTextBox;
        private System.Windows.Forms.GroupBox hoursGroupBox;
        private DCButtons button24;
        private DCButtons button23;
        private DCButtons button22;
        private DCButtons button21;
        private DCButtons button20;
        private DCButtons button19;
        private DCButtons button18;
        private DCButtons button17;
        private DCButtons button16;
        private DCButtons button15;
        private DCButtons button14;
        private DCButtons button13;
        private System.Windows.Forms.TextBox minutesTextBox;
        private System.Windows.Forms.GroupBox minutesGroupBox;

        private Color selectedObjectColor = Color.SpringGreen;
        private DateTime startSelectedDateTime = DateTime.Now;
        private DateTime endSelectedDateTime = DateTime.Now;

        private Label startLabel;
        #endregion Private variables

        #region Custom events
        public Action<DateTime, DateTime, bool, bool> SelectedDateChanged = null;
        protected virtual void OnSelectedDateChanged(DateTime startDateTime, DateTime endDateTime, bool sCheckBox, bool eCheckBox)
        {
            if (this.SelectedDateChanged != null)
            {
                this.SelectedDateChanged(startDateTime, endDateTime, sCheckBox, eCheckBox);
            }
        }
        #endregion Custom events

        #region Properties

        /// <summary>
        /// The selected date.
        /// </summary>
        [Description("The selected date.")]
        [Category("TimeRangeSelector")]

        public Color SelectedObjectColor
        {
            get
            {
                return this.selectedObjectColor;
            }
            set
            {
                this.selectedObjectColor = value;
            }
        }

        /// <summary>
        /// Property - Color of the text for selected day.
        /// </summary>
        /// 
        [Description("The start selected date")]
        [Category("TimeRangeSelector")]
        public DateTime StartSelectedDateTime
        {
            get
            {
                return this.startSelectedDateTime;
            }
            set
            {
                this.startSelectedDateTime = value;
            }
        }

        [Description("The end selected date")]
        [Category("TimeRangeSelector")]
        public DateTime EndSelectedDateTime
        {
            get
            {
                return this.endSelectedDateTime;
            }
            set
            {
                this.endSelectedDateTime = value;
            }
        }
        private DateTime selectedDateTime = DateTime.Now;
        private DateTime SelectedDateTime
        {
            get
            {
                return this.selectedDateTime;
            }
            set
            {
                this.selectedDateTime = value;

                this.ShowSelectedDate();
            }
        }
        [Category("TimeRangeSelector")]
        public bool StartCheckBox
        {
            get
            {
                return this.startCheckBox.Checked;
            }
            set
            {
                this.startCheckBox.Checked = value;
            }
        }

        [Category("TimeRangeSelector")]
        public bool EndCheckBox
        {
            get
            {
                return this.endCheckBox.Checked;
            }
            set
            {
                this.endCheckBox.Checked = value;
            }
        }
        #endregion Properties

        private void ShowSelectedDate()
        {
            string hour = $"{this.SelectedDateTime.Hour}";
            if (this.SelectedDateTime.Hour / 10 == 0)
            {
                hour = $"0{this.SelectedDateTime.Hour}";
            }

            string minutes = $"{this.SelectedDateTime.Minute}";
            if (this.SelectedDateTime.Minute / 10 == 0)
            {
                minutes = $"0{this.SelectedDateTime.Minute}";
            }

            if (this.isStart == true)
            {
                this.startLabel.Text = $"{hour}:{minutes}";
            }
            else
            {
                this.endLabel.Text = $"{hour}:{minutes}";
            }
        }

        private string DateTimeToFormattedDate(DateTime dateTime)
        {
            string hour = $"{dateTime.Hour}";
            if (dateTime.Hour / 10 == 0)
            {
                hour = $"0{dateTime.Hour}";
            }

            string minutes = $"{dateTime.Minute}";
            if (dateTime.Minute / 10 == 0)
            {
                minutes = $"0{dateTime.Minute}";
            }
            return $"{hour}:{minutes}";
        }

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TimeRangeSelectorPopup1()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            AttachEventToDCButtons();
        }


        private void AttachEventToDCButtons()
        {
            this.button3.DoubleClick += this.DCButton_DoubleClick;
            this.button4.DoubleClick += this.DCButton_DoubleClick;
            this.button5.DoubleClick += this.DCButton_DoubleClick;
            this.button6.DoubleClick += this.DCButton_DoubleClick;
            this.button7.DoubleClick += this.DCButton_DoubleClick;
            this.button8.DoubleClick += this.DCButton_DoubleClick;
            this.button9.DoubleClick += this.DCButton_DoubleClick;
            this.button10.DoubleClick += this.DCButton_DoubleClick;
            this.button11.DoubleClick += this.DCButton_DoubleClick;
            this.button12.DoubleClick += this.DCButton_DoubleClick;
            this.button13.DoubleClick += this.DCButton_DoubleClick;
            this.button14.DoubleClick += this.DCButton_DoubleClick;
            this.button15.DoubleClick += this.DCButton_DoubleClick;
            this.button16.DoubleClick += this.DCButton_DoubleClick;
            this.button17.DoubleClick += this.DCButton_DoubleClick;
            this.button18.DoubleClick += this.DCButton_DoubleClick;
            this.button19.DoubleClick += this.DCButton_DoubleClick;
            this.button20.DoubleClick += this.DCButton_DoubleClick;
            this.button21.DoubleClick += this.DCButton_DoubleClick;
            this.button22.DoubleClick += this.DCButton_DoubleClick;
            this.button23.DoubleClick += this.DCButton_DoubleClick;
            this.button24.DoubleClick += this.DCButton_DoubleClick;
            this.button25.DoubleClick += this.DCButton_DoubleClick;
            this.button26.DoubleClick += this.DCButton_DoubleClick;
            this.button27.DoubleClick += this.DCButton_DoubleClick;
            this.button28.DoubleClick += this.DCButton_DoubleClick;
            this.button29.DoubleClick += this.DCButton_DoubleClick;
            this.button30.DoubleClick += this.DCButton_DoubleClick;
            this.button31.DoubleClick += this.DCButton_DoubleClick;
            this.button32.DoubleClick += this.DCButton_DoubleClick;
            this.button33.DoubleClick += this.DCButton_DoubleClick;
            this.button34.DoubleClick += this.DCButton_DoubleClick;
            this.button35.DoubleClick += this.DCButton_DoubleClick;
            this.button36.DoubleClick += this.DCButton_DoubleClick;
            this.button37.DoubleClick += this.DCButton_DoubleClick;
            this.button38.DoubleClick += this.DCButton_DoubleClick;
        }

        bool isDoubleClick = false;

        private void DCButton_DoubleClick(object sender, EventArgs e)
        {
            this.isDoubleClick = true;
            if (this.StartCheckBox == true && this.EndCheckBox == true)
            {
                if (this.isStart == true)
                {
                    this.startEndButton.PerformClick();
                }
                else
                {
                    this.okButton.PerformClick();
                }
            }
            else
            {
                this.okButton.PerformClick();
            }
        }
        #endregion Constructor

        #region Control events

        private string previousText = string.Empty;
        private void hourTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.previousText = this.hourTextBox.Text;
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void hourTextBox_TextChanged(object sender, EventArgs e)
        {
            #region paint prior selected hour button with default color
            Button priorSelectedHourButton = this.hoursGroupBox.Controls.OfType<Button>()
                                                               .Where(b => Convert.ToInt32(b.Text) == this.SelectedDateTime.Hour)
                                                               .FirstOrDefault();

            if (priorSelectedHourButton != null)
            {
                priorSelectedHourButton.BackColor = Color.Gainsboro;
            }
            #endregion paint prior selected hour button with default color

            string stringHour = this.hourTextBox.Text;
            if (stringHour.Trim().Length == 0)
            {
                stringHour = "0";
            }
            int hour = Convert.ToInt32(stringHour);

            if (hour > 23)
            {
                this.hourTextBox.Text = this.previousText;
                this.hourTextBox.Select(this.hourTextBox.Text.Length, 0);

                hour = Convert.ToInt32(this.hourTextBox.Text);
            }

            if (this.hourTextBox.Text.Trim().Length > 2)
            {
                int h = Convert.ToInt32(this.hourTextBox.Text);
                if (h >= 0 && h < 10)
                {
                    this.hourTextBox.Text = "0" + h;
                }
                else
                {
                    this.hourTextBox.Text = h.ToString();
                }
                this.hourTextBox.Select(this.hourTextBox.Text.Length, 0);
            }

            Button currentSelectedHourButton = this.hoursGroupBox.Controls.OfType<Button>()
                                                           .Where(b => Convert.ToInt32(b.Text) == hour)
                                                           .FirstOrDefault();

            if (currentSelectedHourButton != null)
            {
                currentSelectedHourButton.BackColor = this.SelectedObjectColor;
            }
            this.SelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                                                this.SelectedDateTime.Month,
                                                                this.SelectedDateTime.Day,
                                                                hour,
                                                                this.SelectedDateTime.Minute,
                                                                this.SelectedDateTime.Second);
            if (isStart == true)
            {
                if (this.SelectedDateTime > this.EndSelectedDateTime)
                {
                    this.EndSelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                                 this.SelectedDateTime.Month,
                                                 this.SelectedDateTime.Day,
                                                 hour,
                                                 this.SelectedDateTime.Minute,
                                                 59);

                }

                //this.endLabel.Text = $"{this.EndSelectedDateTime.Hour}:{this.EndSelectedDateTime.Minute}";
                this.endLabel.Text = this.DateTimeToFormattedDate(this.EndSelectedDateTime);
            }
            else
            {
                if (this.StartSelectedDateTime > this.SelectedDateTime)
                {
                    this.StartSelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                                 this.SelectedDateTime.Month,
                                                 this.SelectedDateTime.Day,
                                                 hour,
                                                 this.SelectedDateTime.Minute,
                                                 0);

                }

                //this.startLabel.Text = $"{this.StartSelectedDateTime.Hour}:{this.StartSelectedDateTime.Minute}";
                this.startLabel.Text = this.DateTimeToFormattedDate(this.StartSelectedDateTime);
            }
        }
        private void HoursButton_Click(object sender, EventArgs e)
        {
            if (this.isDoubleClick == true)
            {
                this.isDoubleClick = false;
                return;
            }

            Button selectedButton = (Button)sender;
            if (Convert.ToInt32(selectedButton.Text) / 10 == 0)
            {
                this.hourTextBox.Text = "0" + selectedButton.Text;
            }
            else
            {
                this.hourTextBox.Text = selectedButton.Text;
            }
        }

        private void minutesTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            previousText = this.minutesTextBox.Text;
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void minutesTextBox_TextChanged(object sender, EventArgs e)
        {
            #region paint prior selected minute button with default color
            Button priorSelectedMinuteButton = this.minutesGroupBox.Controls.OfType<Button>()
                                                                   .Where(b => Convert.ToInt32(b.Text) == this.SelectedDateTime.Minute)
                                                                   .FirstOrDefault();
            if (priorSelectedMinuteButton != null)
            {
                priorSelectedMinuteButton.BackColor = Color.Gainsboro;
            }
            #endregion paint prior selected minute button with default color

            string stringMinutes = this.minutesTextBox.Text;
            if (stringMinutes.Trim().Length == 0)
            {
                stringMinutes = "0";
            }

            int minutes = Convert.ToInt32(stringMinutes);

            if (minutes > 59)
            {
                this.minutesTextBox.Text = this.previousText;
                this.minutesTextBox.Select(this.minutesTextBox.Text.Length, 0);

                minutes = Convert.ToInt32(this.minutesTextBox.Text);
            }

            if (this.minutesTextBox.Text.Trim().Length > 2)
            {
                int min = Convert.ToInt32(this.minutesTextBox.Text);
                if (min >= 0 && min < 10)
                {
                    this.minutesTextBox.Text = "0" + min;
                }
                else
                {
                    this.minutesTextBox.Text = min.ToString();
                }
                this.minutesTextBox.Select(this.minutesTextBox.Text.Length, 0);
            }

            foreach (Button button in this.minutesGroupBox.Controls.OfType<Button>())
            {
                button.BackColor = Color.Gainsboro;
            }
            Button currentSelectedMinuteButton = this.minutesGroupBox.Controls.OfType<Button>()
                                                                     .Where(b => Convert.ToInt32(b.Text) == minutes)
                                                                     .FirstOrDefault();
            if (currentSelectedMinuteButton != null)
            {
                currentSelectedMinuteButton.BackColor = this.SelectedObjectColor;
            }
            this.SelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                                 this.SelectedDateTime.Month,
                                                 this.SelectedDateTime.Day,
                                                 this.SelectedDateTime.Hour,
                                                 minutes,
                                                 this.SelectedDateTime.Second);
            if (isStart == true)
            {
                if (this.SelectedDateTime > this.EndSelectedDateTime)
                {
                    this.EndSelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                                 this.SelectedDateTime.Month,
                                                 this.SelectedDateTime.Day,
                                                this.SelectedDateTime.Hour,
                                                 minutes,
                                                59);

                }
                //this.endLabel.Text = $"{this.EndSelectedDateTime.Hour}:{this.EndSelectedDateTime.Minute}";
                this.endLabel.Text = this.DateTimeToFormattedDate(this.EndSelectedDateTime);
            }
            else
            {
                if (this.StartSelectedDateTime > this.SelectedDateTime)
                {
                    this.StartSelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                                 this.SelectedDateTime.Month,
                                                 this.SelectedDateTime.Day,
                                                 this.SelectedDateTime.Hour,
                                                 minutes,
                                                 0);

                }
                //this.startLabel.Text = $"{this.StartSelectedDateTime.Hour}:{this.StartSelectedDateTime.Minute}";
                this.startLabel.Text = this.DateTimeToFormattedDate(this.StartSelectedDateTime);
            }

        }
        private void MinutesButton_Click(object sender, EventArgs e)
        {
            if (this.isDoubleClick == true)
            {
                this.isDoubleClick = false;
                return;
            }
            Button selectedButton = (Button)sender;
            this.minutesTextBox.Text = selectedButton.Text;
        }
        #endregion Control events

        #region Form events

        private void TimeSelector_Load(object sender, System.EventArgs e)
        {
            this.InitializeTimeRangeSelector();
            this.startCheckBox.CheckedChanged += startCheckBox_CheckedChanged;
            this.endCheckBox.CheckedChanged += endCheckBox_CheckedChanged;
        }


        public void InitializeTimeRangeSelector()
        {
            this.startEndButton.Enabled = true;
            this.hoursGroupBox.Enabled = true;
            this.minutesGroupBox.Enabled = true;
            if (this.StartCheckBox == true)
            {
                if (this.EndCheckBox == true)
                {
                    this.startLabel.Enabled = true;
                    this.endLabel.Enabled = true;

                    this.isStart = true;
                    this.SelectedDateTime = this.StartSelectedDateTime;
                    //changed
                    if (this.StartSelectedDateTime.Hour / 10 == 0)
                    {
                        this.hourTextBox.Text = "0" + this.StartSelectedDateTime.Hour.ToString();
                    }
                    else
                    {
                        this.hourTextBox.Text = this.StartSelectedDateTime.Hour.ToString();
                    }

                    if (this.StartSelectedDateTime.Minute / 10 == 0)
                    {
                        this.minutesTextBox.Text = "0" + this.StartSelectedDateTime.Minute.ToString();
                    }
                    else
                    {
                        this.minutesTextBox.Text = this.StartSelectedDateTime.Minute.ToString();
                    }
                    //changed

                    this.endLabel.Text = this.DateTimeToFormattedDate(this.EndSelectedDateTime);
                }
                else
                {
                    // start == true & end == false 

                    this.startLabel.Enabled = true;
                    //changed
                    //this.endLabel.Enabled = false;
                    this.endLabel.Font = new Font(this.endLabel.Font.Name, 8, FontStyle.Regular);
                    //changed
                    this.isStart = true;
                    this.SelectedDateTime = this.StartSelectedDateTime;

                    //changed
                    if (this.StartSelectedDateTime.Hour / 10 == 0)
                    {
                        this.hourTextBox.Text = "0" + this.StartSelectedDateTime.Hour.ToString();
                    }
                    else
                    {
                        this.hourTextBox.Text = this.StartSelectedDateTime.Hour.ToString();
                    }

                    if (this.StartSelectedDateTime.Minute / 10 == 0)
                    {
                        this.minutesTextBox.Text = "0" + this.StartSelectedDateTime.Minute.ToString();
                    }
                    else
                    {
                        this.minutesTextBox.Text = this.StartSelectedDateTime.Minute.ToString();
                    }
                    //changed

                    this.endLabel.Text = this.DateTimeToFormattedDate(this.EndSelectedDateTime);
                }
            }
            else
            {
                if (this.EndCheckBox == true)
                {
                    // start == false & end == true

                    //changed
                    //this.startLabel.Enabled = false;
                    this.startLabel.Font = new Font(this.startLabel.Font.Name, 8, FontStyle.Regular);
                    //changed

                    this.endLabel.Enabled = true;

                    this.isStart = false;
                    this.SelectedDateTime = this.EndSelectedDateTime;
                    //changed
                    if (this.EndSelectedDateTime.Hour / 10 == 0)
                    {
                        this.hourTextBox.Text = "0" + this.EndSelectedDateTime.Hour.ToString();
                    }
                    else
                    {
                        this.hourTextBox.Text = this.EndSelectedDateTime.Hour.ToString();
                    }

                    if (this.EndSelectedDateTime.Minute / 10 == 0)
                    {
                        this.minutesTextBox.Text = "0" + this.EndSelectedDateTime.Minute.ToString();
                    }
                    else
                    {
                        this.minutesTextBox.Text = this.EndSelectedDateTime.Minute.ToString();
                    }
                    //start

                    //changed
                    this.endLabel.Font = new Font(endLabel.Font.Name, 8, FontStyle.Bold | FontStyle.Underline);

                    this.startLabel.Text = this.DateTimeToFormattedDate(this.StartSelectedDateTime);
                }
                else
                {
                    // start == false & end == false 

                    this.startEndButton.Enabled = false;

                    this.hoursGroupBox.Enabled = false;
                    this.minutesGroupBox.Enabled = false;

                    //this.startLabel.Enabled = false;
                    //this.endLabel.Enabled = false;

                    this.isStart = true;
                    this.SelectedDateTime = this.StartSelectedDateTime;
                    //changed
                    if (this.StartSelectedDateTime.Hour / 10 == 0)
                    {
                        this.hourTextBox.Text = "0" + this.StartSelectedDateTime.Hour.ToString();
                    }
                    else
                    {
                        this.hourTextBox.Text = this.StartSelectedDateTime.Hour.ToString();
                    }

                    if (this.StartSelectedDateTime.Minute / 10 == 0)
                    {
                        this.minutesTextBox.Text = "0" + this.StartSelectedDateTime.Minute.ToString();
                    }
                    else
                    {
                        this.minutesTextBox.Text = this.StartSelectedDateTime.Minute.ToString();
                    }
                    //changed

                    this.endLabel.Text = this.DateTimeToFormattedDate(this.EndSelectedDateTime);
                }
            }

        }
        #endregion Form events

        public Action<object, EventArgs> okButtonClick = null;
        private void okButton_Click(object sender, EventArgs e)
        {
            if (this.isStart == true)
            {
                this.StartSelectedDateTime = this.SelectedDateTime;
            }
            else
            {
                this.EndSelectedDateTime = this.SelectedDateTime;
            }

            this.OnSelectedDateChanged(this.StartSelectedDateTime, this.EndSelectedDateTime, this.StartCheckBox, this.EndCheckBox);
            if (this.okButtonClick != null)
            {
                this.okButtonClick(sender, e);
            }
        }

        public Action<object, EventArgs> cancelButtonClick = null;
        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (this.cancelButtonClick != null)
            {
                this.cancelButtonClick(sender, e);
            }
        }

        private void SetButtonsToDefaultColor()
        {
            Button selectedHoursButton = this.hoursGroupBox.Controls.OfType<Button>()
                                   .Where(b => Convert.ToInt32(b.Text) == this.SelectedDateTime.Hour)
                                   .FirstOrDefault();
            if (selectedHoursButton != null)
            {
                selectedHoursButton.BackColor = Color.Gainsboro;
            }

            Button selectedMinuteButton = this.minutesGroupBox.Controls.OfType<Button>()
                               .Where(b => Convert.ToInt32(b.Text) == this.SelectedDateTime.Minute)
                               .FirstOrDefault();
            if (selectedMinuteButton != null)
            {
                selectedMinuteButton.BackColor = Color.Gainsboro;
            }
        }

        private void SetButtonsAsSelected(int hour, int minute)
        {
            //hour
            Button selectedHourButton = this.hoursGroupBox.Controls.OfType<Button>()
                                                .Where(b => Convert.ToInt32(b.Text) == hour)
                                                .FirstOrDefault();
            if (selectedHourButton != null)
            {
                selectedHourButton.BackColor = SelectedObjectColor;
            }

            //minute
            Button selectedMinuteButton = this.minutesGroupBox.Controls.OfType<Button>()
                                         .Where(b => Convert.ToInt32(b.Text) == minute)
                                         .FirstOrDefault();
            if (selectedMinuteButton != null)
            {
                selectedMinuteButton.BackColor = SelectedObjectColor;
            }
        }

        private void startEndButton_Click(object sender, EventArgs e)
        {
            this.isStart = !this.isStart;
            this.SetButtonsToDefaultColor();

            if (this.isStart == true)
            {
                this.startCheckBox.Checked = true;
                startLabel.Font = new Font(startLabel.Font.Name, 8, FontStyle.Bold | FontStyle.Underline);
                endLabel.Font = new Font(endLabel.Font.Name, 8, FontStyle.Regular | FontStyle.Regular);

                this.EndSelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                                        this.SelectedDateTime.Month,
                                                        this.SelectedDateTime.Day,
                                                        this.SelectedDateTime.Hour,
                                                        this.SelectedDateTime.Minute,
                                                        this.SelectedDateTime.Second);
                this.SelectedDateTime = this.StartSelectedDateTime;

            }
            else
            {
                this.endCheckBox.Checked = true;
                this.endLabel.Font = new Font(startLabel.Font.Name, 8, FontStyle.Bold | FontStyle.Underline);
                this.startLabel.Font = new Font(endLabel.Font.Name, 8, FontStyle.Regular | FontStyle.Regular);

                this.StartSelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                                          this.SelectedDateTime.Month,
                                                          this.SelectedDateTime.Day,
                                                          this.SelectedDateTime.Hour,
                                                          this.SelectedDateTime.Minute,
                                                          this.SelectedDateTime.Second);
                this.SelectedDateTime = this.EndSelectedDateTime;
            }
            if (this.SelectedDateTime.Hour / 10 == 0)
            {
                this.hourTextBox.Text = "0" + this.SelectedDateTime.Hour.ToString();
            }
            else
            {
                this.hourTextBox.Text = this.SelectedDateTime.Hour.ToString();
            }

            if (this.SelectedDateTime.Minute / 10 == 0)
            {
                this.minutesTextBox.Text = "0" + this.SelectedDateTime.Minute.ToString();
            }
            else
            {
                this.minutesTextBox.Text = this.SelectedDateTime.Minute.ToString();
            }


            this.SetButtonsAsSelected(this.SelectedDateTime.Hour, this.SelectedDateTime.Minute);
        }

        public void ShowDateRangeSelectorState01()
        {
            this.hoursGroupBox.Enabled = true;
            this.minutesGroupBox.Enabled = true;
            this.startEndButton.Enabled = true;

            if (this.StartCheckBox == true)
            {
                if (this.EndCheckBox == true)
                {
                    this.startLabel.Enabled = true;
                    this.startLabel.Font = new Font(startLabel.Font.Name, 8, FontStyle.Regular);

                    this.endLabel.Text = this.DateTimeToFormattedDate(this.SelectedDateTime);
                }
                else
                {
                    // start == true & end == false 

                    this.startLabel.Enabled = true;
                    //changed

                    this.isStart = true;
                    this.SelectedDateTime = this.StartSelectedDateTime;
                    //changed
                    if (this.SelectedDateTime.Hour / 10 == 0)
                    {
                        this.hourTextBox.Text = "0" + this.SelectedDateTime.Hour.ToString();
                    }
                    else
                    {
                        this.hourTextBox.Text = this.SelectedDateTime.Hour.ToString();
                    }

                    if (this.SelectedDateTime.Minute / 10 == 0)
                    {
                        this.minutesTextBox.Text = "0" + this.SelectedDateTime.Minute.ToString();
                    }
                    else
                    {
                        this.minutesTextBox.Text = this.SelectedDateTime.Minute.ToString();
                    }
                    //changed
                    this.startLabel.Font = new Font(startLabel.Font.Name, 8, FontStyle.Bold | FontStyle.Underline);
                }
            }
            else
            {
                if (this.EndCheckBox == true)
                {
                    // start == false & end == true

                    if (this.isStart == true)
                    {
                        this.startEndButton.PerformClick();
                    }
                    //changed         
                }
                else
                {
                    // start == false & end == false 
                    this.startEndButton.Enabled = false;
                    this.hoursGroupBox.Enabled = false;
                    this.minutesGroupBox.Enabled = false;

                    //this.startLabel.Enabled = false;
                    //this.endLabel.Enabled = false;

                    this.StartSelectedDateTime = this.SelectedDateTime;
                    //changed
                    if (this.SelectedDateTime.Hour / 10 == 0)
                    {
                        this.hourTextBox.Text = "0" + this.SelectedDateTime.Hour.ToString();
                    }
                    else
                    {
                        this.hourTextBox.Text = this.SelectedDateTime.Hour.ToString();
                    }

                    if (this.SelectedDateTime.Minute / 10 == 0)
                    {
                        this.minutesTextBox.Text = "0" + this.SelectedDateTime.Minute.ToString();
                    }
                    else
                    {
                        this.minutesTextBox.Text = this.SelectedDateTime.Minute.ToString();
                    }
                    //changed
                }
            }
        }

        private void startCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.ShowDateRangeSelectorState01();
        }

        public void ShowDateRangeSelectorState02()
        {
            this.hoursGroupBox.Enabled = true;
            this.minutesGroupBox.Enabled = true;
            this.startEndButton.Enabled = true;

            if (this.StartCheckBox == true)
            {
                if (this.EndCheckBox == true)
                {
                    this.endLabel.Enabled = true;
                }
                else
                {
                    // start == true & end == false 

                    if (this.isStart == false)
                    {
                        this.startEndButton.PerformClick();
                    }
                    //changed
                    //this.endLabel.Enabled = false;

                }
            }
            else
            {
                if (this.EndCheckBox == true)
                {
                    // start == false & end == true

                    this.endLabel.Enabled = true;

                    this.startLabel.Font = new Font(this.startLabel.Font.Name, 8, FontStyle.Regular);
                    this.endLabel.Font = new Font(this.endLabel.Font.Name, 8, FontStyle.Bold | FontStyle.Underline);

                    this.SetButtonsToDefaultColor();

                    this.isStart = false;
                    this.SelectedDateTime = this.EndSelectedDateTime;
                    //changed
                    if (this.SelectedDateTime.Hour / 10 == 0)
                    {
                        this.hourTextBox.Text = "0" + this.SelectedDateTime.Hour.ToString();
                    }
                    else
                    {
                        this.hourTextBox.Text = this.SelectedDateTime.Hour.ToString();
                    }

                    if (this.SelectedDateTime.Minute / 10 == 0)
                    {
                        this.minutesTextBox.Text = "0" + this.SelectedDateTime.Minute.ToString();
                    }
                    else
                    {
                        this.minutesTextBox.Text = this.SelectedDateTime.Minute.ToString();
                    }
                    //changed
                    this.SetButtonsAsSelected(this.SelectedDateTime.Hour, this.SelectedDateTime.Minute);

                }
                else
                {
                    // start == false & end == false 
                    this.startEndButton.Enabled = false;
                    this.hoursGroupBox.Enabled = false;
                    this.minutesGroupBox.Enabled = false;

                    //this.startLabel.Enabled = false;
                    //this.endLabel.Enabled = false;

                    this.EndSelectedDateTime = this.SelectedDateTime;

                    this.startLabel.Font = new Font(this.startLabel.Font.Name, 8, FontStyle.Bold | FontStyle.Underline);
                    this.endLabel.Font = new Font(this.endLabel.Font.Name, 8, FontStyle.Regular);

                    this.SetButtonsToDefaultColor();

                    this.isStart = true;
                    this.SelectedDateTime = this.StartSelectedDateTime;
                    //changed
                    if (this.SelectedDateTime.Hour / 10 == 0)
                    {
                        this.hourTextBox.Text = "0" + this.SelectedDateTime.Hour.ToString();
                    }
                    else
                    {
                        this.hourTextBox.Text = this.SelectedDateTime.Hour.ToString();
                    }

                    if (this.SelectedDateTime.Minute / 10 == 0)
                    {
                        this.minutesTextBox.Text = "0" + this.SelectedDateTime.Minute.ToString();
                    }
                    else
                    {
                        this.minutesTextBox.Text = this.SelectedDateTime.Minute.ToString();
                    }

                    //changed
                    this.SetButtonsAsSelected(this.SelectedDateTime.Hour, this.SelectedDateTime.Minute);
                }
            }
        }
        private void endCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.ShowDateRangeSelectorState02();
        }

        private void startLabel_Click(object sender, EventArgs e)
        {
            //this.startCheckBox.Checked = !startCheckBox.Checked;
            if (this.isStart)
            {
                this.startCheckBox.Checked = !this.startCheckBox.Checked;
            }
            else
            {
                this.startEndButton.PerformClick();
            }
        }

        private void endLabel_Click(object sender, EventArgs e)
        {
            //this.endCheckBox.Checked = !this.endCheckBox.Checked;
            if (this.isStart)
            {
                if (this.startCheckBox.Checked)
                {
                    this.startEndButton.PerformClick();
                }
                else
                {
                    this.startEndButton.Enabled = true;
                    this.startEndButton.PerformClick();
                }

            }
            else
            {
                this.endCheckBox.Checked = !this.endCheckBox.Checked;
            }
        }

        private void TimeRangeSelectorPopup1_Click(object sender, EventArgs e)
        {
            hoursGroupBox.Enabled = true;
            minutesGroupBox.Enabled = true;
            startCheckBox.Checked = true;
        }

        private void TimeRangeSelectorPopup1_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.hoursGroupBox.Enabled == false)
            {
                #region check the clicking of this.hoursGroupBox.Controls
                foreach (Control ctl in this.hoursGroupBox.Controls)
                {
                    if (ctl is Button)
                    {
                        Point point = new Point(e.X - this.hoursGroupBox.Location.X, e.Y - this.hoursGroupBox.Location.Y);

                        Button button = (Button)ctl;
                        if (button.Bounds.Contains(point))
                        {                            
                            this.hoursGroupBox.Enabled = true;
                            this.minutesGroupBox.Enabled = true;
                            this.startCheckBox.Checked = true;
                            button.PerformClick();
                        }
                    }
                }
                #endregion check the clicking of this.hoursGroupBox.Controls                
            }
            if (this.minutesGroupBox.Enabled == false)
            {
                #region check the clicking of this.minutesGroupBox.Controls
                foreach (Control ctl in this.minutesGroupBox.Controls)
                {
                    if (ctl is Button)
                    {
                        Point point = new Point(e.X - this.minutesGroupBox.Location.X, e.Y - this.minutesGroupBox.Location.Y);

                        Button button = (Button)ctl;
                        if (button.Bounds.Contains(point))
                        {                            
                            this.hoursGroupBox.Enabled = true;
                            this.minutesGroupBox.Enabled = true;
                            this.startCheckBox.Checked = true;
                            button.PerformClick();
                        }
                    }
                }
                #endregion check the clicking of this.minutesGroupBox.Controls
            }
        }
    }
}
