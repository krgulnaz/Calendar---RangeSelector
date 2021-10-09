using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;

namespace NaitonControls
{
    [ToolboxItem(false)]
    public partial class DateTimeRangeSelectorPopup1 : UserControl
    {
        #region Private DS variables
        private PictureBox dateSelectorPictureBox;
        private bool isStart = true;
        private bool weekendsDarker = false;
        private DateTime startSelectedDateTime = DateTime.Now;
        private DateTime endSelectedDateTime = DateTime.Now;

        private Color gridColor = System.Drawing.SystemColors.Control;
        private Color headerColor = System.Drawing.SystemColors.Control;

        private Color activeMonthColor = Color.Gainsboro;
        private Color inactiveMonthColor = SystemColors.ControlLight;

        private Color selectedDayFontColor = Color.Black;
        private Color selectedObjectColor = Color.SpringGreen;
        private Color nonselectedDayFontColor = Color.Gray;
        private Color selectedRangeColor = Color.PaleGreen;
        private Color weekendColor = Color.Red;

        private Font dayFont = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular);
        private Font headerFont = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);

        private Rectangle[,] rects;
        private Rectangle[] rectDays;

        private string[] strDays = new string[7] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        private string[] strAbbrDays = new string[7] { "mo", "tu", "we", "th", "fr", "sa", "su" };
        private string[] strMonths = new string[12] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        private bool bDesign = true;

        private SolidBrush headerBrush;
        private SolidBrush activeMonthBrush;
        private SolidBrush inactiveMonthBrush;
        private SolidBrush selectedDayBrush;
        private SolidBrush selectedDayFontBrush;
        private SolidBrush selectedMonthBrush;
        private SolidBrush nonselectedDayFontBrush;
        private SolidBrush selectedRangeBrush;
        private SolidBrush weekendBrush;
        private StringFormat stringFormat;
        protected GroupBox monthGroupBox;
        private DCButtons decButton;
        private DCButtons junButton;
        private DCButtons novButton;
        private DCButtons mayButton;
        private DCButtons octButton;
        private DCButtons aprButton;
        private DCButtons sepButton;
        private DCButtons marButton;
        private DCButtons augButton;
        private DCButtons febButton;
        private DCButtons julButton;
        private DCButtons janButton;
        protected GroupBox yearGroupBox;
        private Button prevButton;
        private Button nextButton;
        private DCButtons fourthButtonYear;
        private DCButtons thirdButtonYear;
        private DCButtons secondYearButton;
        private DCButtons firstYearButton;
        private Button okButton;
        private Button cancelButton;
        protected Label startLabel;
        private ImageList imageList1;
        protected GroupBox dayGroupBox;
        protected GroupBox minutesGroupBox;
        private TextBox minutesTextBox;
        private DCButtons minuteButton55;
        private DCButtons minuteButton50;
        private DCButtons minuteButton45;
        private DCButtons minuteButton40;
        private DCButtons minuteButton35;
        private DCButtons minuteButton30;
        private DCButtons minuteButton25;
        private DCButtons minuteButton20;
        private DCButtons minuteButton15;
        private DCButtons minuteButton10;
        private DCButtons minuteButton05;
        private DCButtons minuteButton00;
        private DCButtons dayButton;
        private DateTime[,] arrDates;

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

        [Description("The start selected date.")]
        [Category("DateTimeRangeSelector")]
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

        [Description("The end selected date.")]
        [Category("DateTimeRangeSelector")]
        public DateTime EndSelectedDateTime
        {
            get
            {
                return endSelectedDateTime;
            }
            set
            {
                endSelectedDateTime = value;
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

                FormatHourAndMinuteToShow();
            }
        }

        [Description("Selected object color")]
        [Category("DateTimeRangeSelector")]
        public Color SelectedObjectColor
        {
            get
            {
                return selectedObjectColor;
            }
            set
            {
                selectedObjectColor = value;
            }
        }
        [Category("DateRangeSelector")]
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

        [Category("DateRangeSelector")]
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
        public bool IsNullable { get; set; } = true;

        #endregion Properties
        string hour = "";
        string minutes = "";
        private void FormatHourAndMinuteToShow()
        {
            hour = $"{this.SelectedDateTime.Hour}";
            if (this.SelectedDateTime.Hour / 10 == 0)
            {
                hour = $"0{this.SelectedDateTime.Hour}";
            }

            minutes = $"{this.SelectedDateTime.Minute}";
            if (this.SelectedDateTime.Minute / 10 == 0)
            {
                minutes = $"0{this.SelectedDateTime.Minute}";
            }
            if (this.isStart == true)
            {
                this.startLabel.Text = $"{this.SelectedDateTime.DayOfWeek} {this.SelectedDateTime.Day}, {this.strMonths[this.SelectedDateTime.Month - 1]} {this.SelectedDateTime.Year} {hour}:{minutes}"; ;
            }
            else
            {
                this.endLabel.Text = $"{this.SelectedDateTime.DayOfWeek} {this.SelectedDateTime.Day}, {this.strMonths[this.SelectedDateTime.Month - 1]} {this.SelectedDateTime.Year} {hour}:{minutes}"; ;
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

        public DateTimeRangeSelectorPopup1()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            AttachEventToDCButtons();
        }

        private void AttachEventToDCButtons()
        {
            this.decButton.DoubleClick += this.DCButton_DoubleClick;

            this.novButton.DoubleClick += this.DCButton_DoubleClick;
            this.octButton.DoubleClick += this.DCButton_DoubleClick;
            this.sepButton.DoubleClick += this.DCButton_DoubleClick;
            this.augButton.DoubleClick += this.DCButton_DoubleClick;
            this.julButton.DoubleClick += this.DCButton_DoubleClick;
            this.junButton.DoubleClick += this.DCButton_DoubleClick;
            this.mayButton.DoubleClick += this.DCButton_DoubleClick;
            this.aprButton.DoubleClick += this.DCButton_DoubleClick;
            this.marButton.DoubleClick += this.DCButton_DoubleClick;
            this.febButton.DoubleClick += this.DCButton_DoubleClick;
            this.janButton.DoubleClick += this.DCButton_DoubleClick;

            this.firstYearButton.DoubleClick += this.DCButton_DoubleClick;
            this.secondYearButton.DoubleClick += this.DCButton_DoubleClick;
            this.thirdButtonYear.DoubleClick += this.DCButton_DoubleClick;
            this.fourthButtonYear.DoubleClick += this.DCButton_DoubleClick;

            this.hourButton0.DoubleClick += this.DCButton_DoubleClick;
            this.hourButton1.DoubleClick += this.DCButton_DoubleClick;
            this.hourButton2.DoubleClick += this.DCButton_DoubleClick;
            this.hourButton3.DoubleClick += this.DCButton_DoubleClick;
            this.hourButton4.DoubleClick += this.DCButton_DoubleClick;
            this.hourButton5.DoubleClick += this.DCButton_DoubleClick;
            this.hourButton6.DoubleClick += this.DCButton_DoubleClick;
            this.hourButton7.DoubleClick += this.DCButton_DoubleClick;
            this.hourButton8.DoubleClick += this.DCButton_DoubleClick;
            this.hourButton9.DoubleClick += this.DCButton_DoubleClick;
            this.hourButton10.DoubleClick += this.DCButton_DoubleClick;
            this.hourButton11.DoubleClick += this.DCButton_DoubleClick;
            this.hourButton12.DoubleClick += this.DCButton_DoubleClick;
            this.hourButton13.DoubleClick += this.DCButton_DoubleClick;
            this.hourButton14.DoubleClick += this.DCButton_DoubleClick;
            this.hourButton15.DoubleClick += this.DCButton_DoubleClick;
            this.hourButton16.DoubleClick += this.DCButton_DoubleClick;
            this.hourButton17.DoubleClick += this.DCButton_DoubleClick;
            this.hourButton18.DoubleClick += this.DCButton_DoubleClick;
            this.hourButton19.DoubleClick += this.DCButton_DoubleClick;
            this.hourButton20.DoubleClick += this.DCButton_DoubleClick;
            this.hourButton21.DoubleClick += this.DCButton_DoubleClick;
            this.hourButton22.DoubleClick += this.DCButton_DoubleClick;
            this.hourButton23.DoubleClick += this.DCButton_DoubleClick;


            this.minuteButton00.DoubleClick += this.DCButton_DoubleClick;
            this.minuteButton05.DoubleClick += this.DCButton_DoubleClick;
            this.minuteButton10.DoubleClick += this.DCButton_DoubleClick;
            this.minuteButton15.DoubleClick += this.DCButton_DoubleClick;
            this.minuteButton20.DoubleClick += this.DCButton_DoubleClick;
            this.minuteButton25.DoubleClick += this.DCButton_DoubleClick;
            this.minuteButton30.DoubleClick += this.DCButton_DoubleClick;
            this.minuteButton35.DoubleClick += this.DCButton_DoubleClick;
            this.minuteButton40.DoubleClick += this.DCButton_DoubleClick;
            this.minuteButton45.DoubleClick += this.DCButton_DoubleClick;
            this.minuteButton50.DoubleClick += this.DCButton_DoubleClick;
            this.minuteButton55.DoubleClick += this.DCButton_DoubleClick;
            this.dayButton.DoubleClick+= this.DCButton_DoubleClick1;
        }
        bool isDoubleClick = false;

        private void DCButton_DoubleClick(object sender, EventArgs e)
        {
            this.isDoubleClick = true;
            if (this.StartCheckBox == true && this.EndCheckBox == true)
            {
                if (this.isStart == true)
                {
                    //this.startEndButton.PerformClick();
                    this.StartEndButtonClick();
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
        private void DCButton_DoubleClick1(object sender, EventArgs e)
        {
            this.isDoubleClick = true;
            
                this.okButton.PerformClick();
           
        }

        #endregion Constructor

        #region Control events

        private void yearsButton_Click(object sender, EventArgs e)
        {
            if (this.isDoubleClick == true)
            {
                this.isDoubleClick = false;
                return;
            }

            Button selectedButton = (Button)sender;

            int day = this.SelectedDateTime.Day;
            int month = this.SelectedDateTime.Month;
            int year = Convert.ToInt32(selectedButton.Text);
            DateTime tempDateTime = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            if (day > tempDateTime.Day)
            {
                day = tempDateTime.Day;
            }
            this.SelectedDateTime = new DateTime(year,
                                           month,
                                           day,
                                           this.SelectedDateTime.Hour,
                                           this.SelectedDateTime.Minute,
                                           this.SelectedDateTime.Second);

            if (isStart == true)
            {
                this.StartSelectedDateTime = this.SelectedDateTime;
            }
            else
            {
                this.EndSelectedDateTime = this.SelectedDateTime;
            }
            IEnumerable<Button> buttons = this.yearGroupBox.Controls
                                                    .OfType<Button>();
            foreach (Button button in buttons)
            {
                if (button.Text == "")
                {
                    button.BackColor = Color.Gainsboro;
                    continue;
                }
                if (Convert.ToInt32(button.Text) >= this.StartSelectedDateTime.Year && Convert.ToInt32(button.Text) <= this.EndSelectedDateTime.Year)
                {
                    button.BackColor = this.selectedRangeColor;
                }
                else
                {
                    button.BackColor = Color.Gainsboro;
                }
            }

            selectedButton.BackColor = SelectedObjectColor;


            if (isStart == true)
            {
                if (this.SelectedDateTime > this.EndSelectedDateTime)
                {
                    this.EndSelectedDateTime = new DateTime(year,
                                                 month,
                                                 day,
                                                 this.EndSelectedDateTime.Hour,
                                                 this.EndSelectedDateTime.Minute,
                                                 this.EndSelectedDateTime.Second).AddDays(1);

                }
                this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year} {this.DateTimeToFormattedDate(this.EndSelectedDateTime)}";
            }
            else
            {
                if (this.StartSelectedDateTime > this.SelectedDateTime)
                {
                    this.StartSelectedDateTime = new DateTime(year,
                                                 month,
                                                 day,
                                                 this.StartSelectedDateTime.Hour,
                                                 this.StartSelectedDateTime.Minute,
                                                 this.StartSelectedDateTime.Second).AddDays(-1);

                }
                this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year} {this.DateTimeToFormattedDate(this.StartSelectedDateTime)}";
            }
            count++;
            this.dateSelectorPictureBox.Invalidate();
            this.InitializeMonthButtons();
            this.InitializePresentLabels();
            this.GetWeekNumber(this.SelectedDateTime);
        }

        private void monthsButton_Click(object sender, EventArgs e)
        {
            if (this.isDoubleClick == true)
            {
                this.isDoubleClick = false;
                return;
            }

            Button selectedButton = (Button)sender;
            int day = this.SelectedDateTime.Day;
            int year = this.SelectedDateTime.Year;
            int month = Convert.ToInt32(selectedButton.Tag);

            DateTime tempDateTime = new DateTime(year, month,
                              DateTime.DaysInMonth(year, month));
            if (day > tempDateTime.Day)
            {
                day = tempDateTime.Day;
            }
            this.SelectedDateTime = new DateTime(year,
                                                 month,
                                                 day,
                                                 this.SelectedDateTime.Hour,
                                                 this.SelectedDateTime.Minute,
                                                 this.SelectedDateTime.Second);

            if (isStart == true)
            {
                this.StartSelectedDateTime = this.SelectedDateTime;
            }
            else
            {
                this.EndSelectedDateTime = this.SelectedDateTime;
            }

            if (this.StartSelectedDateTime.Year < this.EndSelectedDateTime.Year)
            {
                IEnumerable<Button> buttons = this.monthGroupBox.Controls
                                                        .OfType<Button>();
                foreach (Button button in buttons)
                {
                    if (isStart)
                    {
                        if (Convert.ToInt32(button.Tag) >= this.StartSelectedDateTime.Month)
                        {
                            button.BackColor = this.selectedRangeColor;
                        }
                        else
                        {
                            button.BackColor = Color.Gainsboro;
                        }

                    }
                    else
                    {
                        if (Convert.ToInt32(button.Tag) <= this.EndSelectedDateTime.Month)
                        {
                            button.BackColor = this.selectedRangeColor;
                        }
                        else
                        {
                            button.BackColor = Color.Gainsboro;
                        }
                    }
                }
            }
            else
            {
                IEnumerable<Button> buttons = this.monthGroupBox.Controls
                                                        .OfType<Button>();
                foreach (Button button in buttons)
                {
                    if (Convert.ToInt32(button.Tag) >= this.StartSelectedDateTime.Month && Convert.ToInt32(button.Tag) <= this.EndSelectedDateTime.Month)
                    {
                        button.BackColor = this.selectedRangeColor;
                    }
                    else
                    {
                        button.BackColor = Color.Gainsboro;
                    }

                }
            }

            selectedButton.BackColor = this.SelectedObjectColor;

            if (isStart == true)
            {
                DateTime firstDay = new DateTime(year, month, 1);
                DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
                DateTime firstDayEndDate = new DateTime(EndSelectedDateTime.Year, EndSelectedDateTime.Month, 1);
                DateTime lastDayEndDate = firstDayEndDate.AddMonths(1).AddDays(-1);

                if (this.SelectedDateTime > this.EndSelectedDateTime)
                {
                    this.StartSelectedDateTime = new DateTime(year,
                                                            month,
                                                            firstDay.Day,
                                                            this.SelectedDateTime.Hour,
                                                            this.SelectedDateTime.Minute,
                                                            this.SelectedDateTime.Second);
                    this.SelectedDateTime = this.StartSelectedDateTime;

                    if (this.EndSelectedDateTime.Day > lastDay.Day)
                    {
                        this.EndSelectedDateTime = new DateTime(EndSelectedDateTime.Year,
                                                               month,
                                                               lastDay.Day,
                                                               this.EndSelectedDateTime.Hour,
                                                               this.EndSelectedDateTime.Minute,
                                                               this.EndSelectedDateTime.Second);

                    }
                    else
                    {
                        this.EndSelectedDateTime = new DateTime(EndSelectedDateTime.Year,
                                                               month,
                                                               this.EndSelectedDateTime.Day,
                                                               this.EndSelectedDateTime.Hour,
                                                               this.EndSelectedDateTime.Minute,
                                                               this.EndSelectedDateTime.Second);
                    }

                }
                this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year} {this.DateTimeToFormattedDate(this.EndSelectedDateTime)} ";
            }
            else
            {
                DateTime firstDay = new DateTime(year, month, 1);
                DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);

                if (this.StartSelectedDateTime > this.SelectedDateTime)
                {
                    this.EndSelectedDateTime = new DateTime(year,
                                                 month,
                                                 lastDay.Day,
                                                 this.SelectedDateTime.Hour,
                                                 this.SelectedDateTime.Minute,
                                                 this.SelectedDateTime.Second);
                    this.SelectedDateTime = this.EndSelectedDateTime;


                    if (this.StartSelectedDateTime.Day > lastDay.Day)
                    {
                        this.StartSelectedDateTime = new DateTime(this.StartSelectedDateTime.Year,
                                                                  month,
                                                                  lastDay.Day,
                                                                  this.StartSelectedDateTime.Hour,
                                                                  this.StartSelectedDateTime.Minute,
                                                                  this.StartSelectedDateTime.Second);
                    }
                    else
                    {
                        this.StartSelectedDateTime = new DateTime(this.StartSelectedDateTime.Year,
                                                                  month,
                                                                  this.StartSelectedDateTime.Day,
                                                                  this.StartSelectedDateTime.Hour,
                                                                  this.StartSelectedDateTime.Minute,
                                                                  this.StartSelectedDateTime.Second);


                    }
                }

                this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year} {this.DateTimeToFormattedDate(this.StartSelectedDateTime)}";
            }
            count++;
            this.GetWeekNumber(this.SelectedDateTime);
            this.dateSelectorPictureBox.Invalidate();
            this.InitializePresentLabels();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {

            int year = Convert.ToInt32(this.firstYearButton.Text) + 4;
            this.firstYearButton.Text = Convert.ToString(year);
            this.secondYearButton.Text = Convert.ToString(year + 1);
            this.thirdButtonYear.Text = Convert.ToString(year + 2);
            this.fourthButtonYear.Text = Convert.ToString(year + 3);
            IEnumerable<Button> buttons = this.yearGroupBox.Controls
                                                         .OfType<Button>();
            foreach (Button button in buttons)
            {
                if (button.Text == "")
                {
                    button.BackColor = Color.Gainsboro;
                    continue;
                }
                if (Convert.ToInt32(button.Text) >= this.StartSelectedDateTime.Year && Convert.ToInt32(button.Text) <= this.EndSelectedDateTime.Year)
                {
                    button.BackColor = this.selectedRangeColor;

                }
                else
                {
                    button.BackColor = Color.Gainsboro;
                }

                if (isStart == true)
                {
                    if (Convert.ToInt32(button.Text) == this.SelectedDateTime.Year)
                    {
                        button.BackColor = this.SelectedObjectColor;
                    }
                    if (Convert.ToInt32(button.Text) != this.StartSelectedDateTime.Year)
                    {
                        if (Convert.ToInt32(button.Text) == this.EndSelectedDateTime.Year)
                        {
                            button.BackColor = this.selectedRangeColor;
                        }
                    }
                }
                else
                {
                    if (Convert.ToInt32(button.Text) == this.SelectedDateTime.Year)
                    {
                        button.BackColor = this.SelectedObjectColor;
                    }
                    if (Convert.ToInt32(button.Text) != this.EndSelectedDateTime.Year)
                    {
                        if (Convert.ToInt32(button.Text) == this.StartSelectedDateTime.Year)
                        {
                            button.BackColor = this.selectedRangeColor;
                        }
                    }
                }
            }
        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            int year = Convert.ToInt32(this.firstYearButton.Text) - 4;
            this.firstYearButton.Text = Convert.ToString(year);
            this.secondYearButton.Text = Convert.ToString(year + 1);
            this.thirdButtonYear.Text = Convert.ToString(year + 2);
            this.fourthButtonYear.Text = Convert.ToString(year + 3);
            IEnumerable<Button> buttons = this.yearGroupBox.Controls
                                                   .OfType<Button>();
            foreach (Button button in buttons)
            {
                if (button.Text == "")
                {
                    button.BackColor = Color.Gainsboro;
                    continue;
                }
                if (Convert.ToInt32(button.Text) >= this.StartSelectedDateTime.Year && Convert.ToInt32(button.Text) <= this.EndSelectedDateTime.Year)
                {
                    button.BackColor = this.selectedRangeColor;

                }
                else
                {
                    button.BackColor = Color.Gainsboro;
                }

                if (isStart == true)
                {
                    if (Convert.ToInt32(button.Text) == this.SelectedDateTime.Year)
                    {
                        button.BackColor = this.SelectedObjectColor;
                    }
                    if (Convert.ToInt32(button.Text) != this.StartSelectedDateTime.Year)
                    {
                        if (Convert.ToInt32(button.Text) == this.EndSelectedDateTime.Year)
                        {
                            button.BackColor = this.selectedRangeColor;
                        }
                    }
                }
                else
                {
                    if (Convert.ToInt32(button.Text) == this.SelectedDateTime.Year)
                    {
                        button.BackColor = this.SelectedObjectColor;
                    }
                    if (Convert.ToInt32(button.Text) != this.EndSelectedDateTime.Year)
                    {
                        if (Convert.ToInt32(button.Text) == this.StartSelectedDateTime.Year)
                        {
                            button.BackColor = this.selectedRangeColor;
                        }
                    }

                }
            }
        }

        private string previousText = string.Empty;
        private void hourTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            previousText = this.hourTextBox.Text;
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
                this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year} {this.DateTimeToFormattedDate(this.EndSelectedDateTime)}";
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
                this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year} {this.DateTimeToFormattedDate(this.StartSelectedDateTime)}";
            }
            dateSelectorPictureBox.Invalidate();
            //this.InitializePresentLabels();            
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
            count++;
            //this.GetWeekNumber(this.SelectedDateTime);
            dateSelectorPictureBox.Invalidate();
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
                this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year} {this.DateTimeToFormattedDate(this.EndSelectedDateTime)}";
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
                this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year} {this.DateTimeToFormattedDate(this.StartSelectedDateTime)}";
            }
            dateSelectorPictureBox.Invalidate();
        }


        private void MinutesButton_Click(object sender, EventArgs e)
        {
            if (this.isDoubleClick == true)
            {
                this.isDoubleClick = false;
                return;
            }
            Button selectedButton = (Button)sender;
            count++;
            this.minutesTextBox.Text = selectedButton.Text;
            //this.GetWeekNumber(this.SelectedDateTime);
            dateSelectorPictureBox.Invalidate();
            //this.InitializePresentLabels();

        }

        private void dateSelector_SizeChanged(object sender, System.EventArgs e)
        {
            rects = CreateGrid(dateSelectorPictureBox.Width, dateSelectorPictureBox.Height);
            dateSelectorPictureBox.Invalidate();
        }

        private void dateSelector_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            for (int line = 0; line < 6; line++)
            {
                for (int column = 0; column < 7; column++)
                {
                    if (this.rects[column, line].Contains(e.X, e.Y))
                    {
                        if (this.arrDates[column, line] != DateTime.MinValue)
                        {
                            this.SelectedDateTime = this.arrDates[column, line];

                            if (isStart == true)
                            {
                                if (this.SelectedDateTime > this.EndSelectedDateTime)
                                {
                                    this.EndSelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                                                            this.SelectedDateTime.Month,
                                                                            this.SelectedDateTime.Day,
                                                                            this.EndSelectedDateTime.Hour,
                                                                            this.EndSelectedDateTime.Minute,
                                                                            this.EndSelectedDateTime.Second).AddDays(1);

                                }
                                this.endLabel.Text = this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year} {this.DateTimeToFormattedDate(this.EndSelectedDateTime)}";
                            }
                            else
                            {
                                if (this.StartSelectedDateTime > this.SelectedDateTime)
                                {
                                    this.StartSelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                                                 this.SelectedDateTime.Month,
                                                                 this.SelectedDateTime.Day,
                                                                 this.StartSelectedDateTime.Hour,
                                                                 this.StartSelectedDateTime.Minute,
                                                                 this.StartSelectedDateTime.Second).AddDays(-1);

                                }
                                this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year} {this.DateTimeToFormattedDate(this.StartSelectedDateTime)}";
                            }
                        }
                    }
                }
            }
            count++;
            this.GetWeekNumber(this.SelectedDateTime);
            this.MonthClickByNextPrevButtons(SelectedDateTime);
            this.InitializeMonthButtons();
            this.InitializeYearButtons();
            this.dateSelectorPictureBox.Invalidate();
            this.InitializePresentLabels();
            this.FormatHourAndMinuteToShow();
        }

        private void dateSelector_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (!bDesign)
            {
                //the calendar is created as a 7 x 6 grid drawn onto the picture box
                //the data to be displayed in the calendar is stored in a 7 x 6 array of arrays
                //update
                this.FillDates2(this.SelectedDateTime);
                this.CreateGraphicObjects();

                Graphics g = e.Graphics;
                this.DrawHeader(g);
                this.DrawCalendar(g);
            }
        }

        private void DrawHeader(Graphics g)
        {
            for (int column = 0; column < 7; column++)
            {
                //draw weekday header rectangle
                g.FillRectangle(this.headerBrush, this.rectDays[column]);
                g.DrawString(this.strAbbrDays[column], this.headerFont, Brushes.Black, this.rectDays[column], this.stringFormat);
            }
        }

        private void DrawCalendar(Graphics g)
        {
            if (this.isStart == true)
            {
                StartSelectedDateTime = SelectedDateTime;
            }
            else
            {
                EndSelectedDateTime = SelectedDateTime;
            }
            bool active_month = false;
            string str = string.Empty;
            //actual calendar 
            for (int line = 0; line < 6; line++)
            {
                for (int column = 0; column < 7; column++)
                {
                    Rectangle layoutRectangle = new Rectangle(this.rects[column, line].X, this.rects[column, line].Y, this.rects[column, line].Width, (int)(this.rects[column, line].Height * 1));

                    str = string.Empty;
                    if (this.arrDates[column, line] != DateTime.MinValue)
                    {
                        str = this.arrDates[column, line].Day.ToString();
                    }

                    if (this.arrDates[column, line].Month == this.SelectedDateTime.Month)
                    {
                        active_month = true;
                    }
                    else
                    {
                        active_month = false;
                    }

                    if (this.arrDates[column, line].Date >= this.StartSelectedDateTime.Date && this.arrDates[column, line].Date <= this.EndSelectedDateTime.Date)//selected date
                    {
                        g.FillRectangle(this.selectedRangeBrush, this.rects[column, line]);
                        g.DrawString(str, this.dayFont, this.selectedDayFontBrush, layoutRectangle, this.stringFormat);
                    }
                    else if (active_month)
                    {
                        if (str != string.Empty)
                        {
                            if (((column == 0) || (column == 6)) && this.weekendsDarker) //weekend
                            {
                                g.FillRectangle(this.weekendBrush, this.rects[column, line]);
                            }
                            else //weekday
                            {
                                g.FillRectangle(this.activeMonthBrush, this.rects[column, line]);
                            }
                            g.DrawString(str, this.dayFont, this.selectedDayFontBrush, layoutRectangle, this.stringFormat);
                        }

                    }
                    else
                    {
                        if (str != string.Empty)
                        {
                            if (((column == 0) || (column == 6)) && this.weekendsDarker) //weekend
                            {
                                g.FillRectangle(this.weekendBrush, this.rects[column, line]);
                            }
                            else //weekday
                            {
                                g.FillRectangle(this.inactiveMonthBrush, this.rects[column, line]);
                            }
                            g.DrawString(str, this.dayFont, this.nonselectedDayFontBrush, layoutRectangle, this.stringFormat);
                        }
                    }
                    if (this.arrDates[column, line].Date == this.StartSelectedDateTime.Date && isStart == true || this.arrDates[column, line].Date == this.EndSelectedDateTime.Date && isStart == false)
                    {
                        g.FillRectangle(this.selectedDayBrush, this.rects[column, line]);
                        g.DrawString(str, this.dayFont, this.selectedDayFontBrush, layoutRectangle, this.stringFormat);
                    }

                    //if (this.arrDates[column, line].Date == this.SelectedDateTime)//selected date
                    //{
                    //    g.FillRectangle(this.selectedDayBrush, this.rects[column, line]);
                    //    g.DrawString(str, this.dayFont, this.selectedDayFontBrush, layoutRectangle, this.stringFormat);
                    //}
                    //changed
                }
            }
        }
        #endregion Control events

        #region Private functions

        private void CreateGraphicObjects()
        {
            this.headerBrush = new SolidBrush(this.headerColor);
            this.activeMonthBrush = new SolidBrush(this.activeMonthColor);
            this.inactiveMonthBrush = new SolidBrush(this.inactiveMonthColor);
            this.selectedDayBrush = new SolidBrush(this.SelectedObjectColor);
            this.selectedMonthBrush = new SolidBrush(this.SelectedObjectColor);
            this.selectedRangeBrush = new SolidBrush(selectedRangeColor);
            this.selectedDayFontBrush = new SolidBrush(this.selectedDayFontColor);
            this.nonselectedDayFontBrush = new SolidBrush(this.nonselectedDayFontColor);
            this.weekendBrush = new SolidBrush(this.weekendColor);
            this.stringFormat = new StringFormat();
            this.stringFormat.Alignment = StringAlignment.Center;
            this.stringFormat.LineAlignment = StringAlignment.Center;
        }

        private Rectangle[,] CreateGrid(int intW, int intH)
        {
            //Array of rectangles representing the calendar
            Rectangle[,] rectTemp = new Rectangle[7, 6];

            //header rectangles
            //
            rectDays = new Rectangle[7];

            int deltaWidth = 3;
            int rectWidth = (int)Math.Floor((double)intW / 7) - deltaWidth;
            int deltaHeight = 3;
            int rectHeight = ((int)Math.Floor((double)(intH - 20) / 6)) - deltaHeight;

            int intXX = 3;
            int intYY = 0;

            for (int i = 0; i < 7; i++)
            {
                Rectangle r1 = new Rectangle(intXX, intYY, rectWidth, 20);
                intXX += (rectWidth + deltaWidth);
                rectDays[i] = r1;

            }

            intYY = 20;
            for (int j = 0; j < 6; j++)
            {
                intXX = 3;
                for (int i = 0; i < 7; i++)
                {
                    Rectangle r1 = new Rectangle(intXX, intYY, rectWidth, rectHeight);
                    intXX += (rectWidth + deltaWidth);
                    rectTemp[i, j] = r1;
                }
                intYY += (rectHeight + deltaHeight);
            }
            return rectTemp;
        }

        public void FillDates2(DateTime currentDateTime)
        {
            //grid column
            int intDayofWeek = 0;
            //grid row
            int intWeek = 0;

            //total day counter
            int intTotalDays = -1;

            //this is where the first day of the month falls in the grid
            int intFirstDay = 0;

            DateTime datPrevMonth = currentDateTime.AddMonths(-1);
            DateTime datNextMonth = currentDateTime.AddMonths(1);

            //number of days in active month
            int intCurrDays = DateTime.DaysInMonth(currentDateTime.Year, currentDateTime.Month);

            //number of days in prev month
            int intPrevDays = DateTime.DaysInMonth(datPrevMonth.Year, datPrevMonth.Month);

            //number of days in active month
            int intNextDays = DateTime.DaysInMonth(datNextMonth.Year, datNextMonth.Month);


            DateTime[] datesCurr = new DateTime[intCurrDays];
            DateTime[] datesPrev = new DateTime[intPrevDays];
            DateTime[] datesNext = new DateTime[intNextDays];

            for (int i = 0; i < intCurrDays; i++)
            {
                datesCurr[i] = new DateTime(currentDateTime.Year, currentDateTime.Month, i + 1, currentDateTime.Hour, currentDateTime.Minute, currentDateTime.Second);
            }

            for (int i = 0; i < intPrevDays; i++)
            {
                datesPrev[i] = new DateTime(datPrevMonth.Year, datPrevMonth.Month, i + 1, currentDateTime.Hour, currentDateTime.Minute, currentDateTime.Second);
            }

            for (int i = 0; i < intNextDays; i++)
            {
                datesNext[i] = new DateTime(datNextMonth.Year, datNextMonth.Month, i + 1, currentDateTime.Hour, currentDateTime.Minute, currentDateTime.Second);
            }

            //array to hold dates corresponding to grid
            arrDates = new DateTime[7, 6];//dates ahead of current date

            //where does the first day of the week land?
            intDayofWeek = Array.IndexOf(strDays, datesCurr[0].DayOfWeek.ToString());


            for (int intDay = 0; intDay < intCurrDays; intDay++)
            {
                //populate array of dates for active month, this is used to tell what day of the week each day is

                intDayofWeek = Array.IndexOf(strDays, datesCurr[intDay].DayOfWeek.ToString());


                //fill the array with the day numbers
                arrDates[intDayofWeek, intWeek] = datesCurr[intDay];
                if (intDayofWeek == 6)
                {
                    intWeek++;
                }

                //Back fill any days from the previous month
                //this is does here because I needed to know where the first day of the active month fell in the grid
                if (intDay == 0)
                {
                    intFirstDay = intDayofWeek;
                    //Days in previous month
                    int intDaysPrev = DateTime.DaysInMonth(datPrevMonth.Year, datPrevMonth.Month);

                    //if the first day of the active month is not sunday, count backwards and fill in day number
                    if (intDayofWeek > 0)
                    {
                        for (int i = intDayofWeek - 1; i >= 0; i--)
                        {
                            arrDates[i, 0] = datesPrev[intDaysPrev - 1];
                            intDaysPrev--;
                            intTotalDays++;
                        }
                    }
                }
                intTotalDays++;
            }//for

            //fill in the remaining days of the grid with the beginning of the next month

            intTotalDays++;
            //what row did we leave off in for active month?
            int intRow = intTotalDays / 7;

            int intCol;

            int intDayNext = 0;

            for (int i = intRow; i < 6; i++)
            {
                intCol = intTotalDays - (intRow * 7);
                for (int j = intCol; j < 7; j++)
                {
                    arrDates[j, i] = datesNext[intDayNext];
                    intDayNext++;
                    intTotalDays++;
                }
                intRow++;
            }
        }
        #endregion Private functions

        #region Form events
        int count = 0;
        private void dateSelector_Load(object sender, System.EventArgs e)
        {
            this.rects = this.CreateGrid(this.dateSelectorPictureBox.Width, this.dateSelectorPictureBox.Height);
            this.CreateGraphicObjects();
            this.bDesign = false;
            if (count == 0)
            {
                this.InitializePresets();
            }
            this.InitializeDateTimeRangeSelector();
            if (this.IsNullable == false)
            {
                this.startCheckBox.Visible = false;
                this.endCheckBox.Visible = false;
            }
            else
            {
                this.startCheckBox.CheckedChanged += startCheckBox_CheckedChanged;
                this.endCheckBox.CheckedChanged += endCheckBox_CheckedChanged;
            }
        }
        public void InitializeDateTimeRangeSelector()
        {
            this.monthGroupBox.Enabled = true;
            this.dayGroupBox.Enabled = true;
            this.yearGroupBox.Enabled = true;
            this.hoursGroupBox.Enabled = true;
            this.presetsGroupBox.Enabled = true;
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
                    this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year} {this.DateTimeToFormattedDate(this.EndSelectedDateTime)}";
                }
                else
                {
                    // start == true & end == false                  
                    this.startLabel.Enabled = true;
                    //changed

                    this.endLabel.Font = new Font(this.endLabel.Font.Name, 8, FontStyle.Regular);

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

                    this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year} {this.DateTimeToFormattedDate(this.EndSelectedDateTime)}";
                }
            }
            else
            {
                if (this.EndCheckBox == true)
                {
                    // start == false & end == true

                    //changed

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
                    //changed
                    this.endLabel.Font = new Font(endLabel.Font.Name, 8, FontStyle.Bold | FontStyle.Underline);

                    this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year} {this.DateTimeToFormattedDate(this.StartSelectedDateTime)}";
                }
                else
                {
                    // start == false & end == false 

                    //this.startEndButton.Enabled = false;                    

                    this.monthGroupBox.Enabled = false;
                    this.dayGroupBox.Enabled = false;
                    this.yearGroupBox.Enabled = false;
                    this.hoursGroupBox.Enabled = false;
                    this.minutesGroupBox.Enabled = false;
                    this.presetsGroupBox.Enabled = false;

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
                    this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year} {this.DateTimeToFormattedDate(this.EndSelectedDateTime)}";
                }
            }
            this.InitializeMonthButtons();
            this.InitializeYearButtons();
            this.GetWeekNumber(this.SelectedDateTime);
            //this.InitializePresentLabels();
        }
        #endregion Form events

        private void InitializeYearButtons()
        {
            //this.firstYearButton.Text = Convert.ToString(this.SelectedDateTime.Year - 2);
            //this.secondYearButton.Text = Convert.ToString(this.SelectedDateTime.Year - 1);
            //this.thirdButtonYear.Text = Convert.ToString(this.SelectedDateTime.Year);
            //this.fourthButtonYear.Text = Convert.ToString(this.SelectedDateTime.Year + 1);

            //this.thirdButtonYear.BackColor = this.SelectedObjectColor;

            if (this.isStart)
            {
                this.firstYearButton.Text = Convert.ToString(this.StartSelectedDateTime.Year - 2);
                this.secondYearButton.Text = Convert.ToString(this.StartSelectedDateTime.Year - 1);
                this.thirdButtonYear.Text = Convert.ToString(this.StartSelectedDateTime.Year);
                this.fourthButtonYear.Text = Convert.ToString(this.StartSelectedDateTime.Year + 1);
            }
            else
            {
                this.firstYearButton.Text = Convert.ToString(this.EndSelectedDateTime.Year - 2);
                this.secondYearButton.Text = Convert.ToString(this.EndSelectedDateTime.Year - 1);
                this.thirdButtonYear.Text = Convert.ToString(this.EndSelectedDateTime.Year);
                this.fourthButtonYear.Text = Convert.ToString(this.EndSelectedDateTime.Year + 1);
            }
            IEnumerable<Button> buttons = this.yearGroupBox.Controls
                                                   .OfType<Button>();
            foreach (Button button in buttons)
            {
                if (button.Text == "")
                {
                    button.BackColor = Color.Gainsboro;
                    continue;
                }
                if (Convert.ToInt32(button.Text) >= this.StartSelectedDateTime.Year && Convert.ToInt32(button.Text) <= this.EndSelectedDateTime.Year)
                {
                    button.BackColor = this.selectedRangeColor;

                }
                else
                {
                    button.BackColor = Color.Gainsboro;
                }

                if (isStart == true)
                {
                    if (Convert.ToInt32(button.Text) == this.SelectedDateTime.Year)
                    {
                        button.BackColor = this.SelectedObjectColor;
                    }
                    if (Convert.ToInt32(button.Text) != this.StartSelectedDateTime.Year)
                    {
                        if (Convert.ToInt32(button.Text) == this.EndSelectedDateTime.Year)
                        {
                            button.BackColor = this.selectedRangeColor;
                        }
                    }
                }
                else
                {
                    if (Convert.ToInt32(button.Text) == this.SelectedDateTime.Year)
                    {
                        button.BackColor = this.SelectedObjectColor;
                    }
                    if (Convert.ToInt32(button.Text) != this.EndSelectedDateTime.Year)
                    {
                        if (Convert.ToInt32(button.Text) == this.StartSelectedDateTime.Year)
                        {
                            button.BackColor = this.selectedRangeColor;
                        }
                    }
                }
            }
        }
        private void InitializeMonthButtons()
        {
            if (this.StartSelectedDateTime.Year < this.EndSelectedDateTime.Year)
            {
                IEnumerable<Button> buttons = this.monthGroupBox.Controls
                                                        .OfType<Button>();
                foreach (Button button in buttons)
                {
                    if (isStart)
                    {
                        if (Convert.ToInt32(button.Tag) >= this.StartSelectedDateTime.Month)
                        {
                            button.BackColor = this.selectedRangeColor;
                        }
                        else
                        {
                            button.BackColor = Color.Gainsboro;
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(button.Tag) <= this.EndSelectedDateTime.Month)
                        {
                            button.BackColor = this.selectedRangeColor;
                        }
                        else
                        {
                            button.BackColor = Color.Gainsboro;
                        }
                    }
                    if (isStart == true)
                    {
                        if (Convert.ToInt32(button.Tag) == this.SelectedDateTime.Month)
                        {
                            button.BackColor = this.SelectedObjectColor;
                        }
                        if (Convert.ToInt32(button.Tag) != this.StartSelectedDateTime.Month)
                        {
                            if (Convert.ToInt32(button.Tag) == this.EndSelectedDateTime.Month)
                            {
                                if (Convert.ToInt32(button.Tag) < this.StartSelectedDateTime.Month)
                                {
                                    button.BackColor = Color.Gainsboro;
                                }
                                else if (Convert.ToInt32(button.Tag) > this.StartSelectedDateTime.Month)
                                {
                                    button.BackColor = this.selectedRangeColor;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(button.Tag) == this.SelectedDateTime.Month)
                        {
                            button.BackColor = this.SelectedObjectColor;
                        }
                        if (Convert.ToInt32(button.Tag) != this.EndSelectedDateTime.Month)
                        {
                            if (Convert.ToInt32(button.Tag) == this.StartSelectedDateTime.Month)
                            {
                                if (Convert.ToInt32(button.Tag) > this.EndSelectedDateTime.Month)
                                {
                                    button.BackColor = Color.Gainsboro;
                                }
                                else if (Convert.ToInt32(button.Tag) < this.EndSelectedDateTime.Month)
                                {
                                    button.BackColor = this.selectedRangeColor;
                                }
                            }
                        }

                    }
                }
            }
            else
            {
                IEnumerable<Button> buttons = this.monthGroupBox.Controls
                                                          .OfType<Button>();
                foreach (Button button in buttons)
                {
                    if (Convert.ToInt32(button.Tag) >= this.StartSelectedDateTime.Month && Convert.ToInt32(button.Tag) <= this.EndSelectedDateTime.Month)
                    {
                        button.BackColor = this.selectedRangeColor;

                    }
                    else
                    {
                        button.BackColor = Color.Gainsboro;
                    }

                    if (isStart == true)
                    {
                        if (Convert.ToInt32(button.Tag) == this.SelectedDateTime.Month)
                        {
                            button.BackColor = this.SelectedObjectColor;
                        }
                        if (Convert.ToInt32(button.Tag) != this.StartSelectedDateTime.Month)
                        {
                            if (Convert.ToInt32(button.Tag) == this.EndSelectedDateTime.Month)
                            {
                                if (Convert.ToInt32(button.Tag) < this.StartSelectedDateTime.Month)
                                {
                                    button.BackColor = Color.Gainsboro;
                                }
                                else if (Convert.ToInt32(button.Tag) > this.StartSelectedDateTime.Month)
                                {
                                    button.BackColor = this.selectedRangeColor;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(button.Tag) == this.SelectedDateTime.Month)
                        {
                            button.BackColor = this.SelectedObjectColor;
                        }
                        if (Convert.ToInt32(button.Tag) != this.EndSelectedDateTime.Month)
                        {
                            if (Convert.ToInt32(button.Tag) == this.StartSelectedDateTime.Month)
                            {
                                if (Convert.ToInt32(button.Tag) > this.EndSelectedDateTime.Month)
                                {
                                    button.BackColor = Color.Gainsboro;
                                }
                                else if (Convert.ToInt32(button.Tag) < this.EndSelectedDateTime.Month)
                                {
                                    button.BackColor = this.selectedRangeColor;
                                }
                            }
                        }

                    }
                }
            }
        }

        private void InitializePresentLabels()
        {
            if (count > 0)
            {
                if (this.isStart)
                {
                    string[] words = startLabel.Text.Split(' ');
                    string dayNameAbb = words[0].Substring(0, 3);
                    this.dayButton.Text = $"{dayNameAbb} {words[1].Substring(0, words[1].Length - 1)}";

                    string monthNameAbb = words[2].Substring(0, 3).ToUpper();
                    this.monthLabel.Text = $"{monthNameAbb}";

                    this.yearLabel.Text = $"{words[3]}";

                    int month = 0;
                    for (int i = 0; i < 12; i++)
                    {
                        if (words[2] == strMonths[i])
                        {
                            month = i;
                        }
                    }
                    int day = Convert.ToInt32(words[1].Substring(0, words[1].Length - 1));
                    CultureInfo ciCurr = CultureInfo.CurrentCulture;
                    var date = new DateTime(Convert.ToInt32(this.yearLabel.Text), month + 1, day);
                    int weekNum = ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                    this.weekLabel.Text = Convert.ToString(weekNum);
                    if (date.Month == 1 && date.Day == 1)
                    {
                        weekNum = 1;
                        this.weekLabel.Text = Convert.ToString(1);
                    }
                    if (date.Month == 1)
                    {

                        this.weekLabel.Text = Convert.ToString(weekNum);
                    }
                    else
                    {
                        this.weekLabel.Text = Convert.ToString(weekNum);
                    }
                    dateTimeNow = new DateTime(Convert.ToInt32(words[3]),
                                               Convert.ToInt32(month+1),
                                               Convert.ToInt32(words[1].Substring(0, words[1].Length - 1))
                                                );
                }
                else
                {
                    string[] words = endLabel.Text.Split(' ');
                    string dayNameAbb = words[0].Substring(0, 3);
                    this.dayButton.Text = $"{dayNameAbb} {words[1].Substring(0, words[1].Length - 1)}";

                    string monthNameAbb = words[2].Substring(0, 3).ToUpper();
                    this.monthLabel.Text = $"{monthNameAbb}";

                    this.yearLabel.Text = $"{words[3]}";
                    int month = 0;
                    for (int i = 0; i < 12; i++)
                    {
                        if (words[2] == strMonths[i])
                        {
                            month = i;
                        }
                    }
                    CultureInfo ciCurr = CultureInfo.CurrentCulture;
                    var date = new DateTime(this.EndSelectedDateTime.Year, this.EndSelectedDateTime.Month, this.EndSelectedDateTime.Day);
                    int weekNum = ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                    this.weekLabel.Text = Convert.ToString(weekNum);
                    dateTimeNow = new DateTime(Convert.ToInt32(words[3]),
                                               Convert.ToInt32(month + 1),
                                               Convert.ToInt32(words[1].Substring(0, words[1].Length - 1))
                                                );
                }
                
                
            }
        }

        DateTime dateTimeNow = DateTime.Now;

        private void InitializePresets()
        {
            
            string presetsLabels = $"{dateTimeNow.DayOfWeek} {dateTimeNow.Day}, {this.strMonths[dateTimeNow.Month - 1]} {dateTimeNow.Year}";
            string[] words = presetsLabels.Split(' ');
            string dayNameAbb = words[0].Substring(0, 3);
            this.dayButton.Text = $"{dayNameAbb} {words[1].Substring(0, words[1].Length - 1)}";

            string monthNameAbb = words[2].Substring(0, 3).ToUpper();
            this.monthLabel.Text = $"{monthNameAbb}";

            this.yearLabel.Text = $"{words[3]}";

            int month = 0;
            for (int i = 0; i < 12; i++)
            {
                if (words[2] == strMonths[i])
                {
                    month = i;
                }
            }
            int day = Convert.ToInt32(words[1].Substring(0, words[1].Length - 1));
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            var date = new DateTime(Convert.ToInt32(this.yearLabel.Text), month + 1, day);
            int weekNum = ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            this.weekLabel.Text = Convert.ToString(weekNum);
            if (date.Month == 1 && date.Day == 1)
            {
                weekNum = 1;
                this.weekLabel.Text = Convert.ToString(1);
            }
            if (date.Month == 1)
            {

                this.weekLabel.Text = Convert.ToString(weekNum);
            }
            else
            {
                this.weekLabel.Text = Convert.ToString(weekNum);
            }
        }
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

            if (this.startCheckBox.Checked == true && this.endCheckBox.Checked == true)
            {
                if (this.StartSelectedDateTime > this.EndSelectedDateTime)
                {
                    DialogResult result = MessageBox.Show("You entered incorrect date range, please check it!", "Error message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (result == DialogResult.OK)
                    {
                        return;
                    }
                }
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
            //year
            Button priorYearSelectedButton = this.yearGroupBox.Controls.OfType<Button>()
                                                 .Where(b => b.Text.Trim() != string.Empty &&
                                                             Convert.ToInt32(b.Text) == this.SelectedDateTime.Year)
                                                 .FirstOrDefault();
            if (priorYearSelectedButton != null)
            {
                priorYearSelectedButton.BackColor = Color.Gainsboro;
            }

            //month
            Button priorMonthSelectedButton = this.monthGroupBox
                                                  .Controls
                                                  .OfType<Button>()
                                                  .Where(b => Convert.ToInt32(b.Tag) == this.SelectedDateTime.Month)
                                                  .FirstOrDefault();
            if (priorMonthSelectedButton != null)
            {
                priorMonthSelectedButton.BackColor = Color.Gainsboro;
            }

            Button priorSelectedHourButton = this.hoursGroupBox.Controls.OfType<Button>()
                                                               .Where(b => Convert.ToInt32(b.Text) == this.SelectedDateTime.Hour)
                                                               .FirstOrDefault();

            if (priorSelectedHourButton != null)
            {
                priorSelectedHourButton.BackColor = Color.Gainsboro;
            }

            Button priorSelectedMinuteButton = this.minutesGroupBox.Controls.OfType<Button>()
                                                   .Where(b => Convert.ToInt32(b.Text) == this.SelectedDateTime.Minute)
                                                   .FirstOrDefault();
            if (priorSelectedMinuteButton != null)
            {
                priorSelectedMinuteButton.BackColor = Color.Gainsboro;
            }
        }

        private void SetButtonsAsSelected(int year, int month, int hour, int minute)
        {
            //year
            Button selectedYearButton = this.yearGroupBox.Controls.OfType<Button>()
                                            .Where(b => b.Text == year.ToString())
                                            .FirstOrDefault();
            if (selectedYearButton != null)
            {
                selectedYearButton.BackColor = SelectedObjectColor;
            }

            //month
            Button selectedMonthButton = this.monthGroupBox.Controls.OfType<Button>()
                                             .Where(b => Convert.ToInt32(b.Tag) == month)
                                             .FirstOrDefault();
            if (selectedMonthButton != null)
            {
                selectedMonthButton.BackColor = SelectedObjectColor;
            }

            Button selectedHourButton = this.hoursGroupBox.Controls.OfType<Button>()
                                                          .Where(b => Convert.ToInt32(b.Text) == hour)
                                                          .FirstOrDefault();
            selectedHourButton.BackColor = SelectedObjectColor;

            //minute
            Button selectedMinuteButton = this.minutesGroupBox.Controls.OfType<Button>()
                                              .Where(b => Convert.ToInt32(b.Text) == minute)
                                              .FirstOrDefault();
            if (selectedMinuteButton != null)
            {
                selectedMinuteButton.BackColor = SelectedObjectColor;
            }
        }

        private void StartEndButtonClick()
        {
            this.isStart = !this.isStart;

            this.SetButtonsToDefaultColor();
            this.InitializeMonthButtons();
            this.InitializeYearButtons();

            if (isStart)
            {
                this.GetWeekNumber(this.StartSelectedDateTime);
            }
            else
            {
                this.GetWeekNumber(this.EndSelectedDateTime);
            }
            if (this.isStart == true)
            {
                this.startCheckBox.Checked = true;
                startLabel.Font = new Font(startLabel.Font.Name, 8, FontStyle.Bold | FontStyle.Underline);
                endLabel.Font = new Font(endLabel.Font.Name, 8, FontStyle.Regular | FontStyle.Regular);

                this.EndSelectedDateTime = this.SelectedDateTime;
                this.SelectedDateTime = this.StartSelectedDateTime;
            }
            else
            {
                this.endCheckBox.Checked = true;
                endLabel.Font = new Font(startLabel.Font.Name, 8, FontStyle.Bold | FontStyle.Underline);
                startLabel.Font = new Font(endLabel.Font.Name, 8, FontStyle.Regular | FontStyle.Regular);

                this.StartSelectedDateTime = this.SelectedDateTime;
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

            this.SetButtonsAsSelected(this.SelectedDateTime.Year,
                                this.SelectedDateTime.Month,
                                this.SelectedDateTime.Hour,
                                this.SelectedDateTime.Minute);
            this.dateSelectorPictureBox.Invalidate();
        }
        //private void startEndButton_Click(object sender, EventArgs e)
        //{
        //    //this.isStart = !this.isStart;
        //    //this.SetButtonsToDefaultColor();
        //    ////changed
        //    //this.InitializeMonthButtons();
        //    //this.InitializeYearButtons();
        //    //this.InitializePresentLabels();
        //    //this.GetWeekNumber(this.SelectedDateTime);
        //    ////changed
        //    //if (this.isStart == true)
        //    //{
        //    //    this.startCheckBox.Checked = true;
        //    //    startLabel.Font = new Font(startLabel.Font.Name, 8, FontStyle.Bold | FontStyle.Underline);
        //    //    endLabel.Font = new Font(endLabel.Font.Name, 8, FontStyle.Regular | FontStyle.Regular);

        //    //    this.EndSelectedDateTime = this.SelectedDateTime;
        //    //    this.SelectedDateTime = this.StartSelectedDateTime;
        //    //}
        //    //else
        //    //{
        //    //    this.endCheckBox.Checked = true;
        //    //    endLabel.Font = new Font(startLabel.Font.Name, 8, FontStyle.Bold | FontStyle.Underline);
        //    //    startLabel.Font = new Font(endLabel.Font.Name, 8, FontStyle.Regular | FontStyle.Regular);

        //    //    this.StartSelectedDateTime = this.SelectedDateTime;
        //    //    this.SelectedDateTime = this.EndSelectedDateTime;
        //    //}
        //    //if (this.SelectedDateTime.Hour / 10 == 0)
        //    //{
        //    //    this.hourTextBox.Text = "0" + this.SelectedDateTime.Hour.ToString();
        //    //}
        //    //else
        //    //{
        //    //    this.hourTextBox.Text = this.SelectedDateTime.Hour.ToString();
        //    //}

        //    //if (this.SelectedDateTime.Minute / 10 == 0)
        //    //{
        //    //    this.minutesTextBox.Text = "0" + this.SelectedDateTime.Minute.ToString();
        //    //}
        //    //else
        //    //{
        //    //    this.minutesTextBox.Text = this.SelectedDateTime.Minute.ToString();
        //    //}

        //    //this.SetButtonsAsSelected(this.SelectedDateTime.Year,
        //    //                    this.SelectedDateTime.Month,
        //    //                    this.SelectedDateTime.Hour,
        //    //                    this.SelectedDateTime.Minute);
        //    //this.dateSelectorPictureBox.Invalidate();
        //}

        private void dateSelectorPictureBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (this.StartCheckBox == true && this.EndCheckBox == true)
            {
                if (isStart == true)
                {
                    //startEndButton.PerformClick();
                    this.StartEndButtonClick();
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

        private void dateSelectorPictureBox_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            if (this.StartCheckBox == true && this.EndCheckBox == true)
            {
                if (isStart == true)
                {
                    //startEndButton.PerformClick();
                    this.StartEndButtonClick();
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
        public void ShowDateRangeSelectorState01()
        {
            this.yearGroupBox.Enabled = true;
            this.monthGroupBox.Enabled = true;
            this.dayGroupBox.Enabled = true;
            this.hoursGroupBox.Enabled = true;
            this.minutesGroupBox.Enabled = true;
            this.presetsGroupBox.Enabled = true;
            //this.startEndButton.Enabled = true;

            if (this.StartCheckBox == true)
            {
                if (this.EndCheckBox == true)
                {
                    this.startLabel.Enabled = true;
                    this.startLabel.Font = new Font(startLabel.Font.Name, 8, FontStyle.Regular);

                    this.endLabel.Text = $"{this.SelectedDateTime.DayOfWeek} {this.SelectedDateTime.Day}, {this.strMonths[this.SelectedDateTime.Month - 1]} {this.SelectedDateTime.Year} {this.DateTimeToFormattedDate(this.SelectedDateTime)}";
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
                        //this.startEndButton.PerformClick();
                        this.StartEndButtonClick();
                    }
                    //changed                 
                }
                else
                {
                    // start == false & end == false 
                    //this.startEndButton.Enabled = false;
                    this.monthGroupBox.Enabled = false;
                    this.dayGroupBox.Enabled = false;
                    this.yearGroupBox.Enabled = false;
                    this.hoursGroupBox.Enabled = false;
                    this.minutesGroupBox.Enabled = false;
                    this.presetsGroupBox.Enabled = false;

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

        protected virtual void startCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.ShowDateRangeSelectorState01();
        }
        public void ShowDateRangeSelectorState02()
        {
            this.dayGroupBox.Enabled = true;
            this.yearGroupBox.Enabled = true;
            this.monthGroupBox.Enabled = true;
            this.hoursGroupBox.Enabled = true;
            this.minutesGroupBox.Enabled = true;
            this.presetsGroupBox.Enabled = true;
            //this.startEndButton.Enabled = true;          

            if (this.StartCheckBox == true)
            {
                if (this.EndCheckBox == true)
                {
                    //this.endLabel.Enabled = true;
                    this.endLabel.Enabled = true;
                }
                else
                {
                    // start == true & end == false 

                    if (this.isStart == false)
                    {
                        //this.startEndButton.PerformClick();
                        this.StartEndButtonClick();
                    }
                    //changed
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
                    this.SetButtonsAsSelected(this.SelectedDateTime.Year, this.SelectedDateTime.Month, this.SelectedDateTime.Hour, this.SelectedDateTime.Minute);

                    this.dateSelectorPictureBox.Invalidate();
                }
                else
                {
                    // start == false & end == false 

                    this.monthGroupBox.Enabled = false;
                    this.dayGroupBox.Enabled = false;
                    this.yearGroupBox.Enabled = false;
                    this.hoursGroupBox.Enabled = false;
                    this.minutesGroupBox.Enabled = false;
                    this.presetsGroupBox.Enabled = false;

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
                    this.SetButtonsAsSelected(this.SelectedDateTime.Year, this.SelectedDateTime.Month, this.SelectedDateTime.Hour, this.SelectedDateTime.Minute);

                    this.dateSelectorPictureBox.Invalidate();
                }
            }
        }
        protected virtual void endCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.ShowDateRangeSelectorState02();
        }

        private void startLabel_MouseClick(object sender, MouseEventArgs e)
        {
            //this.startCheckBox.Checked = !this.startCheckBox.Checked;
            if (this.isStart)
            {
                this.startCheckBox.Checked = !this.startCheckBox.Checked;
            }
            else
            {
                //this.startEndButton.PerformClick();
                this.StartEndButtonClick();
            }
        }

        private void endLabel_MouseClick(object sender, MouseEventArgs e)
        {
            //this.endCheckBox.Checked = !this.endCheckBox.Checked;
            if (this.isStart)
            {
                if (this.startCheckBox.Checked)
                {
                    //this.startEndButton.PerformClick();
                    this.StartEndButtonClick();
                }
                else
                {
                    //this.startEndButton.Enabled = true;
                    //this.startEndButton.PerformClick();
                    this.StartEndButtonClick();
                }

            }
            else
            {
                this.endCheckBox.Checked = !this.endCheckBox.Checked;
            }
        }


        private void DateTimeRangeSelectorPopup1_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.monthGroupBox.Enabled == false)
            {
                #region check the clicking of this.monthGroupBox.Controls
                foreach (Control ctl in this.monthGroupBox.Controls)
                {
                    if (ctl is Button)
                    {
                        Point point = new Point(e.X - this.monthGroupBox.Location.X, e.Y - this.monthGroupBox.Location.Y);

                        Button button = (Button)ctl;

                        if (button.Bounds.Contains(point))
                        {
                            this.yearGroupBox.Enabled = true;
                            this.monthGroupBox.Enabled = true;
                            this.dayGroupBox.Enabled = true;
                            this.hoursGroupBox.Enabled = true;
                            this.minutesGroupBox.Enabled = true;
                            this.presetsGroupBox.Enabled = true;
                            this.startCheckBox.Checked = true;
                            button.PerformClick();
                        }
                    }
                }
                #endregion check the clicking of this.monthGroupBox.Controls
            }
            if (this.yearGroupBox.Enabled == false)
            {
                #region check the clicking of this.yearGroupBox.Controls
                foreach (Control ctl in this.yearGroupBox.Controls)
                {
                    if (ctl is Button)
                    {
                        Point point = new Point(e.X - this.yearGroupBox.Location.X, e.Y - this.yearGroupBox.Location.Y);
                        Button button = (Button)ctl;
                        if (button.Bounds.Contains(point))
                        {
                            this.yearGroupBox.Enabled = true;
                            this.monthGroupBox.Enabled = true;
                            this.dayGroupBox.Enabled = true;
                            this.hoursGroupBox.Enabled = true;
                            this.minutesGroupBox.Enabled = true;
                            this.presetsGroupBox.Enabled = true;
                            this.startCheckBox.Checked = true;
                            button.PerformClick();
                        }
                    }
                }
                #endregion check the clicking of this.yearGroupBox.Controls
            }
            if (this.dayGroupBox.Enabled == false)
            {
                #region check the clicking of this.dayGroupBox.Controls

                for (int line = 0; line < 6; line++)
                {
                    for (int column = 0; column < 7; column++)
                    {
                        if (this.rects[column, line].Contains(e.X - this.dayGroupBox.Location.X - this.dateSelectorPictureBox.Location.X, e.Y - this.dayGroupBox.Location.Y - this.dateSelectorPictureBox.Location.Y))
                        {
                            if (this.arrDates[column, line] != DateTime.MinValue)
                            {
                                this.SelectedDateTime = this.arrDates[column, line];

                                this.yearGroupBox.Enabled = true;
                                this.monthGroupBox.Enabled = true;
                                this.dayGroupBox.Enabled = true;
                                this.hoursGroupBox.Enabled = true;
                                this.minutesGroupBox.Enabled = true;
                                this.presetsGroupBox.Enabled = true;
                                this.startCheckBox.Checked = true;
                                if (isStart == true)
                                {
                                    if (this.SelectedDateTime > this.EndSelectedDateTime)
                                    {
                                        this.EndSelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                                                                this.SelectedDateTime.Month,
                                                                                this.SelectedDateTime.Day,
                                                                                23,
                                                                                59,
                                                                                59).AddSeconds(1);

                                    }
                                    this.endLabel.Text = this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year}";
                                }
                                else
                                {
                                    if (this.StartSelectedDateTime > this.SelectedDateTime)
                                    {
                                        this.StartSelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                                                     this.SelectedDateTime.Month,
                                                                     this.SelectedDateTime.Day,
                                                                     this.SelectedDateTime.Hour,
                                                                     this.SelectedDateTime.Minute,
                                                                     this.SelectedDateTime.Second).AddDays(-1);

                                    }
                                    this.startLabel.Text = this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year}";
                                }
                            }
                        }
                    }
                }
                
                #endregion check the clicking of this.dayGroupBox.Controls
            }
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
                            this.yearGroupBox.Enabled = true;
                            this.monthGroupBox.Enabled = true;
                            this.dayGroupBox.Enabled = true;
                            this.hoursGroupBox.Enabled = true;
                            this.minutesGroupBox.Enabled = true;
                            this.presetsGroupBox.Enabled = true;
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
                            this.yearGroupBox.Enabled = true;
                            this.monthGroupBox.Enabled = true;
                            this.dayGroupBox.Enabled = true;
                            this.hoursGroupBox.Enabled = true;
                            this.minutesGroupBox.Enabled = true;
                            this.startCheckBox.Checked = true;
                            this.presetsGroupBox.Enabled = true;
                            button.PerformClick();
                        }
                    }
                }
                #endregion check the clicking of this.minutesGroupBox.Controls
            }
            if (this.presetsGroupBox.Enabled == false)
            {
                #region check the clicking of this.presetsGroupBox.Controls
                foreach (Control ctl in this.presetsGroupBox.Controls)
                {
                    if (ctl is Button)
                    {
                        Point point = new Point(e.X - this.presetsGroupBox.Location.X, e.Y - this.presetsGroupBox.Location.Y);

                        Button button = (Button)ctl;
                        if (button.Bounds.Contains(point))
                        {
                            this.yearGroupBox.Enabled = true;
                            this.monthGroupBox.Enabled = true;
                            this.dayGroupBox.Enabled = true;
                            this.presetsGroupBox.Enabled = true;
                            this.startCheckBox.Checked = true;
                            this.presetsGroupBox.Enabled = true;
                            button.PerformClick();
                            //count--;
                        }
                    }
                }
                #endregion check the clicking of this.presetsGroupBox.Controls
                //count++;
                this.MonthClickByNextPrevButtons(SelectedDateTime);
                this.dateSelectorPictureBox.Invalidate();
                this.InitializeMonthButtons();
                this.InitializeYearButtons();
                this.InitializePresentLabels();
                this.GetWeekNumber(this.SelectedDateTime);
                this.InitializeHourMinuteButtons();
            }
        }
        public void GetWeekNumber(DateTime selectedDateTime)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            var lastDayOfYear = new DateTime(selectedDateTime.Year, 12, 31);
            var date = new DateTime(selectedDateTime.Year, selectedDateTime.Month, 1);
            int lastWeekNum = ciCurr.Calendar.GetWeekOfYear(lastDayOfYear, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            int weekNum = ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            IEnumerable<Label> labels = this.weekGroupBox.Controls
                                             .OfType<Label>();
            int i = 0;
            int j = 1;
            foreach (Label label in labels)
            {
                if (lastWeekNum >= weekNum + i)
                {
                    if (date.Month == 1 && date.Day == 1)
                    {
                        weekNum = 1;
                        label.Text = Convert.ToString(weekNum + i);
                        i++;
                    }
                    else
                    {
                        label.Text = Convert.ToString(weekNum + i);
                        i++;
                    }
                }
                else
                {
                    label.Text = Convert.ToString(j);
                    j++;
                }
            }
        }


        private void prevDay_Click(object sender, EventArgs e)
        {
            if (this.isStart)
            {
                SelectedDateTime = new DateTime(this.dateTimeNow.Year,
                                           this.dateTimeNow.Month,
                                           this.dateTimeNow.Day,
                                           0,
                                           0,
                                           0
                                           ).AddDays(-1);
            }
            else
            {
                SelectedDateTime = new DateTime(this.dateTimeNow.Year,
                                           this.dateTimeNow.Month,
                                           this.dateTimeNow.Day,
                                           23,
                                           59,
                                           59
                                           ).AddDays(-1);
            }
            if (isStart == true)
            {
                this.EndSelectedDateTime = new DateTime(SelectedDateTime.Year,
                                                        SelectedDateTime.Month,
                                                        SelectedDateTime.Day,
                                                        23,
                                                        59,
                                                        59);

                this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year} {this.EndSelectedDateTime.Hour}:{this.EndSelectedDateTime.Minute}";
            }
            else
            {

                this.StartSelectedDateTime = new DateTime(SelectedDateTime.Year,
                                             SelectedDateTime.Month,
                                             SelectedDateTime.Day,
                                             0,
                                             0,
                                             0);

                this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year}  {DateTimeToFormattedDate(EndSelectedDateTime)}";
            }
            count++;
            this.MonthClickByNextPrevButtons(SelectedDateTime);
            this.dateSelectorPictureBox.Invalidate();
            this.InitializeMonthButtons();
            this.InitializeYearButtons();
            this.InitializePresentLabels();
            this.GetWeekNumber(this.SelectedDateTime);
            this.InitializeHourMinuteButtons();
        }

        private void nextDay_Click(object sender, EventArgs e)
        {
            if (this.isStart)
            {
                SelectedDateTime = new DateTime(this.dateTimeNow.Year,
                                           this.dateTimeNow.Month,
                                           this.dateTimeNow.Day,
                                           0,
                                           0,
                                           0
                                           ).AddDays(+1);
            }
            else
            {
                SelectedDateTime = new DateTime(this.dateTimeNow.Year,
                                           this.dateTimeNow.Month,
                                           this.dateTimeNow.Day,
                                           23,
                                           59,
                                           59
                                           ).AddDays(+1);
            }
            if (isStart == true)
            {
                this.EndSelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                                              this.SelectedDateTime.Month,
                                                              this.SelectedDateTime.Day,
                                                              23,
                                                              59,
                                                              59);
                this.endLabel.Text = this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year}  {this.EndSelectedDateTime.Hour}:{this.EndSelectedDateTime.Minute}";
            }
            else
            {
                this.StartSelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                             this.SelectedDateTime.Month,
                                             this.SelectedDateTime.Day,
                                             0,
                                             0,
                                             0);

                this.startLabel.Text = this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year}  {this.StartSelectedDateTime.Hour}:{this.StartSelectedDateTime.Minute}";
            }
            count++;
            this.dateSelectorPictureBox.Invalidate();
            this.MonthClickByNextPrevButtons(this.SelectedDateTime);
            this.InitializeMonthButtons();
            this.InitializeYearButtons();
            this.InitializePresentLabels();
            this.GetWeekNumber(this.SelectedDateTime);
            this.InitializeHourMinuteButtons();
        }
        private void MonthClickByNextPrevButtons(DateTime SelectedDateTime)
        {
            if (isStart == true)
            {
                DateTime firstDay = new DateTime(this.SelectedDateTime.Year, this.SelectedDateTime.Month, 1);
                DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
                DateTime firstDayEndDate = new DateTime(EndSelectedDateTime.Year, EndSelectedDateTime.Month, 1);
                DateTime lastDayEndDate = firstDayEndDate.AddMonths(1).AddDays(-1);

                if (this.SelectedDateTime > this.EndSelectedDateTime)
                {
                    this.StartSelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                                              this.SelectedDateTime.Month,
                                                              firstDay.Day,
                                                              this.SelectedDateTime.Hour,
                                                              this.SelectedDateTime.Minute,
                                                              this.SelectedDateTime.Second);
                    this.SelectedDateTime = this.StartSelectedDateTime;

                    if (this.EndSelectedDateTime.Day > lastDay.Day)
                    {
                        this.EndSelectedDateTime = new DateTime(EndSelectedDateTime.Year,
                                                                this.SelectedDateTime.Month,
                                                                lastDay.Day,
                                                                this.EndSelectedDateTime.Hour,
                                                                this.EndSelectedDateTime.Minute,
                                                                this.EndSelectedDateTime.Second);

                    }
                    else
                    {
                        this.EndSelectedDateTime = new DateTime(EndSelectedDateTime.Year,
                                                                this.SelectedDateTime.Month,
                                                                this.EndSelectedDateTime.Day,
                                                                this.EndSelectedDateTime.Hour,
                                                                this.EndSelectedDateTime.Minute,
                                                                this.EndSelectedDateTime.Second);
                    }

                }
                this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year}  {this.EndSelectedDateTime.Hour}:{this.EndSelectedDateTime.Minute}";
            }
            else
            {
                DateTime firstDay = new DateTime(this.SelectedDateTime.Year, this.SelectedDateTime.Month, 1);
                DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);

                if (this.StartSelectedDateTime > this.SelectedDateTime)
                {
                    this.EndSelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                                            this.SelectedDateTime.Month,
                                                            lastDay.Day,
                                                            this.SelectedDateTime.Hour,
                                                            this.SelectedDateTime.Minute,
                                                            this.SelectedDateTime.Second);

                    this.SelectedDateTime = this.EndSelectedDateTime;

                    if (this.StartSelectedDateTime.Day > lastDay.Day)
                    {
                        this.StartSelectedDateTime = new DateTime(StartSelectedDateTime.Year,
                                                                  this.SelectedDateTime.Month,
                                                                  lastDay.Day,
                                                                  this.StartSelectedDateTime.Hour,
                                                                  this.StartSelectedDateTime.Minute,
                                                                  this.StartSelectedDateTime.Second);
                    }
                    else
                    {
                        this.StartSelectedDateTime = new DateTime(StartSelectedDateTime.Year,
                                                                  this.SelectedDateTime.Month,
                                                                  this.StartSelectedDateTime.Day,
                                                                  this.StartSelectedDateTime.Hour,
                                                                  this.StartSelectedDateTime.Minute,
                                                                  this.StartSelectedDateTime.Second);


                    }
                }
                this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year}  {this.StartSelectedDateTime.Hour}:{this.StartSelectedDateTime.Minute}";
            }
            if (isStart == true)
            {
                DateTime firstDay = new DateTime(this.SelectedDateTime.Year, this.SelectedDateTime.Month, 1);
                DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
                DateTime firstDayEndDate = new DateTime(EndSelectedDateTime.Year, EndSelectedDateTime.Month, 1);
                DateTime lastDayEndDate = firstDayEndDate.AddMonths(1).AddDays(-1);

                if (this.SelectedDateTime > this.EndSelectedDateTime)
                {
                    this.StartSelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                                              this.SelectedDateTime.Month,
                                                              firstDay.Day,
                                                              this.SelectedDateTime.Hour,
                                                              this.SelectedDateTime.Minute,
                                                              this.SelectedDateTime.Second);
                    this.SelectedDateTime = this.StartSelectedDateTime;

                    if (this.EndSelectedDateTime.Day > lastDay.Day)
                    {
                        this.EndSelectedDateTime = new DateTime(EndSelectedDateTime.Year,
                                                                this.SelectedDateTime.Month,
                                                                lastDay.Day,
                                                                this.EndSelectedDateTime.Hour,
                                                                this.EndSelectedDateTime.Minute,
                                                                this.EndSelectedDateTime.Second);

                    }
                    else
                    {
                        this.EndSelectedDateTime = new DateTime(EndSelectedDateTime.Year,
                                                                this.SelectedDateTime.Month,
                                                                this.EndSelectedDateTime.Day,
                                                                this.EndSelectedDateTime.Hour,
                                                                this.EndSelectedDateTime.Minute,
                                                                this.EndSelectedDateTime.Second);
                    }
                }
                this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year}  {this.EndSelectedDateTime.Hour}:{this.EndSelectedDateTime.Minute}";
            }
            else
            {
                DateTime firstDay = new DateTime(this.SelectedDateTime.Year, this.SelectedDateTime.Month, 1);
                DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);

                if (this.StartSelectedDateTime > this.SelectedDateTime)
                {
                    this.EndSelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                                            this.SelectedDateTime.Month,
                                                            lastDay.Day,
                                                            this.SelectedDateTime.Hour,
                                                            this.SelectedDateTime.Minute,
                                                            this.SelectedDateTime.Second);

                    this.SelectedDateTime = this.EndSelectedDateTime;

                    if (this.StartSelectedDateTime.Day > lastDay.Day)
                    {
                        this.StartSelectedDateTime = new DateTime(StartSelectedDateTime.Year,
                                                                  this.SelectedDateTime.Month,
                                                                  lastDay.Day,
                                                                  this.StartSelectedDateTime.Hour,
                                                                  this.StartSelectedDateTime.Minute,
                                                                  this.StartSelectedDateTime.Second);
                    }
                    else
                    {
                        this.StartSelectedDateTime = new DateTime(StartSelectedDateTime.Year,
                                                                  this.SelectedDateTime.Month,
                                                                  this.StartSelectedDateTime.Day,
                                                                  this.StartSelectedDateTime.Hour,
                                                                  this.StartSelectedDateTime.Minute,
                                                                  this.StartSelectedDateTime.Second);


                    }
                }

                this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year}  {this.StartSelectedDateTime.Hour}:{this.StartSelectedDateTime.Minute}";
            }
            if (this.isStart == true)
            {
                this.StartSelectedDateTime = this.SelectedDateTime;
            }
            else
            {
                this.EndSelectedDateTime = this.SelectedDateTime;
            }

            if (this.StartSelectedDateTime.Year < EndSelectedDateTime.Year)
            {
                IEnumerable<Button> buttons = this.monthGroupBox.Controls.OfType<Button>();
                foreach (Button button in buttons)
                {
                    if (isStart)
                    {
                        if (Convert.ToInt32(button.Tag) >= this.StartSelectedDateTime.Month)
                        {
                            button.BackColor = this.selectedRangeColor;
                        }
                        else
                        {
                            button.BackColor = Color.Gainsboro;
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(button.Tag) <= this.EndSelectedDateTime.Month)
                        {
                            button.BackColor = this.selectedRangeColor;
                        }
                        else
                        {
                            button.BackColor = Color.Gainsboro;
                        }
                    }
                    if (isStart == true)
                    {
                        if (Convert.ToInt32(button.Tag) == this.SelectedDateTime.Month)
                        {
                            button.BackColor = this.SelectedObjectColor;
                        }
                        if (Convert.ToInt32(button.Tag) != this.StartSelectedDateTime.Month)
                        {
                            if (Convert.ToInt32(button.Tag) == this.EndSelectedDateTime.Month)
                            {
                                if (Convert.ToInt32(button.Tag) < this.StartSelectedDateTime.Month)
                                {
                                    button.BackColor = Color.Gainsboro;
                                }
                                else if (Convert.ToInt32(button.Tag) > this.StartSelectedDateTime.Month)
                                {
                                    button.BackColor = this.selectedRangeColor;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(button.Tag) == this.SelectedDateTime.Month)
                        {
                            button.BackColor = this.SelectedObjectColor;
                        }
                        if (Convert.ToInt32(button.Tag) != this.EndSelectedDateTime.Month)
                        {
                            if (Convert.ToInt32(button.Tag) == this.StartSelectedDateTime.Month)
                            {
                                //button.BackColor = this.selectedRangeColor;
                                if (Convert.ToInt32(button.Tag) > this.EndSelectedDateTime.Month)
                                {
                                    button.BackColor = Color.Gainsboro;
                                }
                                else if (Convert.ToInt32(button.Tag) < this.EndSelectedDateTime.Month)
                                {
                                    button.BackColor = this.selectedRangeColor;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                IEnumerable<Button> buttons = this.monthGroupBox.Controls.OfType<Button>();
                foreach (Button button in buttons)
                {
                    if (Convert.ToInt32(button.Tag) >= this.StartSelectedDateTime.Month && Convert.ToInt32(button.Tag) <= this.EndSelectedDateTime.Month)
                    {
                        button.BackColor = this.selectedRangeColor;
                    }
                    else
                    {
                        button.BackColor = Color.Gainsboro;
                    }
                }
            }
        }

        private void prevWeek_Click(object sender, EventArgs e)
        {
            if (this.weekLabel.Text == "1")
            {
                return;
            }
            this.weekLabel.Text = Convert.ToString(Convert.ToInt32(this.weekLabel.Text) - 1);

            DateTime selectedDate = new DateTime(this.SelectedDateTime.Year,
                                       this.SelectedDateTime.Month,
                                       this.SelectedDateTime.Day,
                                       this.SelectedDateTime.Hour,
                                       this.SelectedDateTime.Minute,
                                       this.SelectedDateTime.Second).AddDays(-7);
            string dayOfWeek = selectedDate.DayOfWeek.ToString();
            int countDay = 0;
            for (int i = 0; i < strDays.Length; i++)
            {
                if (dayOfWeek == strDays[i])
                {
                    break;
                }
                else
                {
                    countDay++;
                }
            }
            DateTime mondayDate = new DateTime(selectedDate.Year,
                                       selectedDate.Month,
                                       selectedDate.Day,
                                       selectedDate.Hour,
                                       selectedDate.Minute,
                                       selectedDate.Second).AddDays(-countDay);
            DateTime sundayDate = new DateTime(mondayDate.Year,
                                       mondayDate.Month,
                                       mondayDate.Day,
                                       selectedDate.Hour,
                                       selectedDate.Minute,
                                       selectedDate.Second).AddDays(+6);

            if (this.isStart)
            {
                SelectedDateTime = new DateTime(mondayDate.Year,
                                       mondayDate.Month,
                                       mondayDate.Day,
                                       0,
                                       0,
                                       0
                                       );
            }
            else
            {
                SelectedDateTime = new DateTime(sundayDate.Year,
                                       sundayDate.Month,
                                       sundayDate.Day,
                                       23,
                                       59,
                                       59
                                       );
            }

            if (isStart == true)
            {
                this.EndSelectedDateTime = new DateTime(sundayDate.Year,
                                                              sundayDate.Month,
                                                              sundayDate.Day,
                                                              23,
                                                              59,
                                                              59);
                this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year} {this.EndSelectedDateTime.Hour}:{this.EndSelectedDateTime.Minute}";
            }
            else
            {
                this.StartSelectedDateTime = new DateTime(mondayDate.Year,
                                             mondayDate.Month,
                                             mondayDate.Day,
                                             0,
                                             0,
                                             0);

                this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year}  {this.StartSelectedDateTime.Hour}:{this.StartSelectedDateTime.Minute}";
            }
            count++;
            this.MonthClickByNextPrevButtons(SelectedDateTime);
            this.dateSelectorPictureBox.Invalidate();
            this.InitializeMonthButtons();
            this.InitializeYearButtons();
            this.InitializePresentLabels();
            this.GetWeekNumber(this.SelectedDateTime);
            this.InitializeHourMinuteButtons();
        }
        private void InitializeHourMinuteButtons()
        {
            IEnumerable<Button> buttons = this.hoursGroupBox.Controls
                                                        .OfType<Button>();
            foreach (Button button in buttons)
            {


                button.BackColor = Color.Gainsboro;
            }
            Button selectedHourButton = this.hoursGroupBox.Controls.OfType<Button>()
                                                          .Where(b => Convert.ToInt32(b.Text) == this.SelectedDateTime.Hour)
                                                          .FirstOrDefault();
            selectedHourButton.BackColor = SelectedObjectColor;
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
            if (isStart)
            {
                this.startLabel.Text = $"{this.SelectedDateTime.DayOfWeek} {this.SelectedDateTime.Day}, {this.strMonths[this.SelectedDateTime.Month - 1]} {this.SelectedDateTime.Year} {this.DateTimeToFormattedDate(this.SelectedDateTime)}";
            }
            else
            {
                this.endLabel.Text = $"{this.SelectedDateTime.DayOfWeek} {this.SelectedDateTime.Day}, {this.strMonths[this.SelectedDateTime.Month - 1]} {this.SelectedDateTime.Year} {this.DateTimeToFormattedDate(this.SelectedDateTime)}";

            }
            if (isStart == false)
            {
                string hour = "";
                string minute = "";
                if (this.StartSelectedDateTime.Hour / 10 == 0)
                {
                    hour = "0" + this.StartSelectedDateTime.Hour.ToString();
                }
                else
                {
                    hour = this.StartSelectedDateTime.Hour.ToString();
                }

                if (this.StartSelectedDateTime.Minute / 10 == 0)
                {
                    minute = "0" + this.StartSelectedDateTime.Minute.ToString();
                }
                else
                {
                    minute = this.StartSelectedDateTime.Minute.ToString();
                }
                this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year} {hour}:{minute}";
            }
        }
        private void nextWeek_Click(object sender, EventArgs e)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            var lastDayOfYear = new DateTime(this.SelectedDateTime.Year, 12, 31);
            int lastWeekNum = ciCurr.Calendar.GetWeekOfYear(lastDayOfYear, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            if (this.weekLabel.Text == Convert.ToString(lastWeekNum))
            {
                return;
            }
            this.weekLabel.Text = Convert.ToString(Convert.ToInt32(this.weekLabel.Text) + 1);


            DateTime selectedDate = new DateTime(SelectedDateTime.Year,
                                       this.SelectedDateTime.Month,
                                       this.SelectedDateTime.Day,
                                       this.SelectedDateTime.Hour,
                                       this.SelectedDateTime.Minute,
                                       this.SelectedDateTime.Second).AddDays(+7);
            string dayOfWeek = selectedDate.DayOfWeek.ToString();
            int countDay = 0;
            for (int i = 0; i < strDays.Length; i++)
            {
                if (dayOfWeek == strDays[i])
                {
                    break;
                }
                else
                {
                    countDay++;
                }
            }
            DateTime mondayDate = new DateTime(selectedDate.Year,
                                       selectedDate.Month,
                                       selectedDate.Day,
                                       selectedDate.Hour,
                                       selectedDate.Minute,
                                       selectedDate.Second).AddDays(-countDay);
            DateTime sundayDate = new DateTime(mondayDate.Year,
                                       mondayDate.Month,
                                       mondayDate.Day,
                                       selectedDate.Hour,
                                       selectedDate.Minute,
                                       selectedDate.Second).AddDays(+6);

            if (this.isStart)
            {
                SelectedDateTime = new DateTime(mondayDate.Year,
                                       mondayDate.Month,
                                       mondayDate.Day,
                                       0,
                                       0,
                                       0
                                       );
            }
            else
            {
                SelectedDateTime = new DateTime(sundayDate.Year,
                                       sundayDate.Month,
                                       sundayDate.Day,
                                       23,
                                       59,
                                       59
                                       );
            }

            if (isStart == true)
            {
                this.EndSelectedDateTime = new DateTime(sundayDate.Year,
                                                              sundayDate.Month,
                                                              sundayDate.Day,
                                                              23,
                                                              59,
                                                              59);
                this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year} {this.EndSelectedDateTime.Hour}:{this.EndSelectedDateTime.Minute}";
            }
            else
            {
                this.StartSelectedDateTime = new DateTime(mondayDate.Year,
                                             mondayDate.Month,
                                             mondayDate.Day,
                                             0,
                                             0,
                                             0);

                this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year}  {this.StartSelectedDateTime.Hour}:{this.StartSelectedDateTime.Minute}";
            }
            count++;
            this.MonthClickByNextPrevButtons(SelectedDateTime);
            this.dateSelectorPictureBox.Invalidate();
            this.InitializeMonthButtons();

            this.InitializeYearButtons();
            this.InitializePresentLabels();
            this.GetWeekNumber(this.SelectedDateTime);
            this.InitializeHourMinuteButtons();
        }

        private void prevMonth_Click(object sender, EventArgs e)
        {
            if (SelectedDateTime.Month == 1)
            {
                return;
            }
            DateTime firstDay = new DateTime(SelectedDateTime.Year, SelectedDateTime.Month - 1, 1);
            DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
            if (this.isStart)
            {
                SelectedDateTime = new DateTime(SelectedDateTime.Year,
                                       SelectedDateTime.Month,
                                       firstDay.Day,
                                       0,
                                       0,
                                       0
                                       ).AddMonths(-1);
            }
            else
            {
                SelectedDateTime = new DateTime(SelectedDateTime.Year,
                                       SelectedDateTime.Month - 1,
                                       lastDay.Day,
                                       23,
                                       59,
                                       59
                                       );
            }

            if (isStart == true)
            {
                this.EndSelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                                              this.SelectedDateTime.Month,
                                                              lastDay.Day,
                                                              23,
                                                              59,
                                                              59);
                this.endLabel.Text = this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year}  {this.EndSelectedDateTime.Hour}:{this.EndSelectedDateTime.Minute}";
            }
            else
            {
                this.StartSelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                             this.SelectedDateTime.Month,
                                             firstDay.Day,
                                             0,
                                             0,
                                             0);

                this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year}  {this.StartSelectedDateTime.Hour}:{this.StartSelectedDateTime.Minute}";
            }
            count++;
            this.MonthClickByNextPrevButtons(SelectedDateTime);
            this.dateSelectorPictureBox.Invalidate();
            this.InitializeMonthButtons();
            this.InitializeYearButtons();
            this.InitializePresentLabels();
            this.GetWeekNumber(this.SelectedDateTime);
            this.InitializeHourMinuteButtons();
        }

        private void nextMonth_Click(object sender, EventArgs e)
        {
            if (SelectedDateTime.Month == 12)
            {
                return;
            }
            DateTime firstDay = new DateTime(SelectedDateTime.Year, SelectedDateTime.Month + 1, 1);
            DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
            if (this.isStart)
            {
                SelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                       this.SelectedDateTime.Month,
                                       firstDay.Day,
                                       0,
                                       0,
                                       0
                                       ).AddMonths(1);
            }
            else
            {
                SelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                       this.SelectedDateTime.Month + 1,
                                       lastDay.Day,
                                       23,
                                       59,
                                       59
                                       );
            }


            if (isStart == true)
            {
                this.EndSelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                                              this.SelectedDateTime.Month,
                                                              lastDay.Day,
                                                              23,
                                                              59,
                                                              59);
                this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year} {this.EndSelectedDateTime.Hour}:{this.EndSelectedDateTime.Minute}";
            }
            else
            {
                this.StartSelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                             this.SelectedDateTime.Month,
                                             firstDay.Day,
                                             0,
                                             0,
                                             0);

                this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year}  {this.StartSelectedDateTime.Hour}:{this.StartSelectedDateTime.Minute}";
            }
            count++;
            this.MonthClickByNextPrevButtons(SelectedDateTime);
            this.dateSelectorPictureBox.Invalidate();
            this.InitializeMonthButtons();
            this.InitializeYearButtons();
            this.InitializePresentLabels();
            this.GetWeekNumber(this.SelectedDateTime);
            this.InitializeHourMinuteButtons();
        }
        private void YearClickByNextPrevButtons(DateTime SelectedDateTime)
        {
            int day = this.SelectedDateTime.Day;
            int month = this.SelectedDateTime.Month;
            int year = this.SelectedDateTime.Year;

            DateTime tempDateTime = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            if (day > tempDateTime.Day)
            {
                day = tempDateTime.Day;
            }

            this.SelectedDateTime = new DateTime(year,
                                                 month,
                                                 day,
                                                 this.SelectedDateTime.Hour,
                                                 this.SelectedDateTime.Minute,
                                                 this.SelectedDateTime.Second);

            if (isStart == true)
            {
                this.StartSelectedDateTime = this.SelectedDateTime;
            }
            else
            {
                this.EndSelectedDateTime = this.SelectedDateTime;
            }


            int y = default(int);
            IEnumerable<Button> buttons = this.yearGroupBox.Controls.OfType<Button>();
            foreach (Button button in buttons)
            {
                if (button.Text == "")
                {
                    button.BackColor = Color.Gainsboro;
                    continue;
                }

                y = Convert.ToInt32(button.Text);
                if (y >= this.StartSelectedDateTime.Year && y <= this.EndSelectedDateTime.Year)
                {
                    button.BackColor = this.selectedRangeColor;
                }
                else
                {
                    button.BackColor = Color.Gainsboro;
                }
            }

            this.dateSelectorPictureBox.Invalidate();
            if (isStart == true)
            {
                if (this.SelectedDateTime > this.EndSelectedDateTime)
                {
                    this.EndSelectedDateTime = new DateTime(year,
                                                            month,
                                                            day,
                                                            23,
                                                            59,
                                                            59).AddSeconds(1);

                }
                this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year} {this.EndSelectedDateTime.Hour}:{this.EndSelectedDateTime.Minute}";
            }
            else
            {
                if (this.StartSelectedDateTime > this.SelectedDateTime)
                {
                    this.StartSelectedDateTime = new DateTime(year,
                                                              month,
                                                              day,
                                                              this.SelectedDateTime.Hour,
                                                              this.SelectedDateTime.Minute,
                                                              this.SelectedDateTime.Second).AddDays(-1);

                }
                this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year} {this.StartSelectedDateTime.Hour}:{this.StartSelectedDateTime.Minute}";
            }
        }

        private void prevYear_Click(object sender, EventArgs e)
        {
            if (this.isStart)
            {
                SelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                       1,
                                       1,
                                       0,
                                       0,
                                       0
                                       ).AddYears(-1);
            }
            else
            {
                SelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                       12,
                                       31,
                                       23,
                                       59,
                                       59
                                       ).AddYears(-1);
            }

            if (isStart == true)
            {
                this.EndSelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                                              12,
                                                              31,
                                                              23,
                                                              59,
                                                              59);
                this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year} {this.EndSelectedDateTime.Hour}:{this.EndSelectedDateTime.Minute}";
            }
            else
            {
                this.StartSelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                             1,
                                             1,
                                             0,
                                             0,
                                             0);

                this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year}  {this.StartSelectedDateTime.Hour}:{this.StartSelectedDateTime.Minute}";
            }
            count++;
            this.YearClickByNextPrevButtons(SelectedDateTime);
            this.dateSelectorPictureBox.Invalidate();
            this.InitializeMonthButtons();
            this.InitializeYearButtons();
            this.InitializePresentLabels();
            this.GetWeekNumber(this.SelectedDateTime);
            this.InitializeHourMinuteButtons();
        }

        private void nextYear_Click(object sender, EventArgs e)
        {
            if (this.isStart)
            {
                SelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                       1,
                                       1,
                                       0,
                                       0,
                                       0
                                       ).AddYears(1);
            }
            else
            {
                SelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                       12,
                                       31,
                                       23,
                                       59,
                                       59
                                       ).AddYears(1);
            }

            if (isStart == true)
            {
                this.EndSelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                                              12,
                                                              31,
                                                              23,
                                                              59,
                                                              59);
                this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year} {this.EndSelectedDateTime.Hour}:{this.EndSelectedDateTime.Minute}";
            }
            else
            {
                this.StartSelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                             1,
                                             1,
                                             0,
                                             0,
                                             0);

                this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year}  {this.StartSelectedDateTime.Hour}:{this.StartSelectedDateTime.Minute}";
            }
            count++;
            this.YearClickByNextPrevButtons(SelectedDateTime);
            this.dateSelectorPictureBox.Invalidate();
            this.InitializeMonthButtons();
            this.InitializeYearButtons();
            this.InitializePresentLabels();
            this.GetWeekNumber(this.SelectedDateTime);
            this.InitializeHourMinuteButtons();
        }
        //int m = 0;
        private void dayButton_Click(object sender, EventArgs e)
        {
            
            if (this.isDoubleClick == true)
            {
               
                this.isDoubleClick = false;
                return;
            }
            
            if (this.isStart)
            {
                SelectedDateTime = new DateTime(this.dateTimeNow.Year,
                                           this.dateTimeNow.Month,
                                           this.dateTimeNow.Day,
                                           0,
                                           0,
                                           0
                                           );
            }
            else
            {
                SelectedDateTime = new DateTime(this.dateTimeNow.Year,
                                           this.dateTimeNow.Month,
                                           this.dateTimeNow.Day,
                                           23,
                                           59,
                                           59
                                           );
            }
            if (isStart == true)
            {
                this.EndSelectedDateTime = new DateTime(SelectedDateTime.Year,
                                                        SelectedDateTime.Month,
                                                        SelectedDateTime.Day,
                                                        23,
                                                        59,
                                                        59);

                this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year} {this.EndSelectedDateTime.Hour}:{this.EndSelectedDateTime.Minute}";
            }
            else
            {

                this.StartSelectedDateTime = new DateTime(SelectedDateTime.Year,
                                             SelectedDateTime.Month,
                                             SelectedDateTime.Day,
                                             0,
                                             0,
                                             0);

                this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year}  {this.StartSelectedDateTime.Hour}:{this.StartSelectedDateTime.Minute}";
            }
            //m++;
            count++;
            this.MonthClickByNextPrevButtons(SelectedDateTime);
            this.dateSelectorPictureBox.Invalidate();
            this.InitializeMonthButtons();
            this.InitializeYearButtons();
            this.InitializePresentLabels();
            this.GetWeekNumber(this.SelectedDateTime);
        }

        private void dayLabel_Click(object sender, EventArgs e)
        {

        }

        private void PresetsButton_Click(object sender, MouseEventArgs e)
        {

        }
    }
}
