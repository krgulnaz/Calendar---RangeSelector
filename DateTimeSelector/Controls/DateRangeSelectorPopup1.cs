using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

using NLog;
using System.Globalization;

namespace NaitonControls
{
    [ToolboxItem(false)]
    public partial class DateRangeSelectorPopup1 : UserControl
    {
        private readonly static Logger logger = LogManager.GetCurrentClassLogger();

        private PictureBox dateSelectorPictureBox;

        #region Private variables
        private bool isStart = true;

        private DateTime startSelectedDateTime = DateTime.Now;
        private DateTime endSelectedDateTime = DateTime.Now;

        private bool weekendsDarker = false;
        private Color headerColor = SystemColors.Control;
        private Color selectedRangeColor = Color.PaleGreen;
        private Color activeMonthColor = Color.Gainsboro;
        private Color inactiveMonthColor = SystemColors.ControlLight;

        private Color selectedDayFontColor = Color.Black;
        private Color selectedObjectColor = Color.SpringGreen;

        private Color nonselectedDayFontColor = Color.Gray;

        private Color weekendColor = Color.Red;

        private Font dayFont = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular);
        private Font headerFont = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular);

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
        private SolidBrush selectedRangeBrush;
        private SolidBrush nonselectedDayFontBrush;
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
        private ImageList imageList1;
        protected GroupBox dayGroupBox;

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
        [Category("DateRangeSelector")]
        public DateTime StartSelectedDateTime
        {
            get
            {
                return this.startSelectedDateTime;
            }
            set
            {
                DateTime dateTime = new DateTime(value.Year,
                   value.Month,
                   value.Day,
                   0,
                   0,
                   0);
                this.startSelectedDateTime = dateTime;
            }
        }

        [Description("The end selected date.")]
        [Category("DateRangeSelector")]
        public DateTime EndSelectedDateTime
        {
            get
            {
                return this.endSelectedDateTime;
            }
            set
            {
                DateTime dateTime = new DateTime(value.Year,
                  value.Month,
                  value.Day,
                  23,
                  59,
                  59);
                this.endSelectedDateTime = dateTime;
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

                string dateTimeInfo = $"{this.selectedDateTime.DayOfWeek} {this.selectedDateTime.Day}, {this.strMonths[this.selectedDateTime.Month - 1]} {this.selectedDateTime.Year}";
                if (this.isStart == true)
                {
                    this.startLabel.Text = dateTimeInfo;
                }
                else
                {
                    this.endLabel.Text = dateTimeInfo;
                }
            }
        }

        [Description("Selected object color")]
        [Category("DateRangeSelector")]
        public Color SelectedObjectColor
        {
            get
            {
                return selectedObjectColor;
            }
            set
            {
                this.selectedObjectColor = value;
            }
        }

        /// <summary>
        /// Property - Color of the text for selected day.
        /// </summary>
        [Description("Weekends darker")]
        [Category("DateRangeSelector")]
        private bool WeekendsDarker
        {
            get
            {
                return this.weekendsDarker;
            }
            set
            {
                this.weekendsDarker = value;
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

        #region Constructor

        public DateRangeSelectorPopup1()
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
        }

        bool isDoubleClick = false;
        private void DCButton_DoubleClick(object sender, EventArgs e)
        {
            this.isDoubleClick = true;

            if (this.StartCheckBox == true && this.EndCheckBox == true)
            {
                if (this.isStart == true)
                {
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
                                                 23,
                                                 59,
                                                 59).AddSeconds(1);

                }
                this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year}";
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
                this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year}";
            }
            count++;
            this.InitializeMonthButtons();
            this.dateSelectorPictureBox.Invalidate();
            this.InitializePresetsLabels();
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

            // paint selected button
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
                        this.EndSelectedDateTime = new DateTime(this.EndSelectedDateTime.Year,
                                                               month,
                                                               lastDay.Day,
                                                               this.EndSelectedDateTime.Hour,
                                                               this.EndSelectedDateTime.Minute,
                                                               this.EndSelectedDateTime.Second);

                    }
                    else
                    {
                        this.EndSelectedDateTime = new DateTime(this.EndSelectedDateTime.Year,
                                                               month,
                                                               this.EndSelectedDateTime.Day,
                                                               this.EndSelectedDateTime.Hour,
                                                               this.EndSelectedDateTime.Minute,
                                                               this.EndSelectedDateTime.Second);
                    }

                }
                this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year}";
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
                this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year}";
            }
            count++;
            this.GetWeekNumber(this.SelectedDateTime);
            this.dateSelectorPictureBox.Invalidate();
            this.InitializePresetsLabels();
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
            count++;
            this.GetWeekNumber(this.SelectedDateTime);
            this.MonthClickByNextPrevButtons(SelectedDateTime);
            this.InitializeMonthButtons();
            this.InitializeYearButtons();
            this.InitializePresetsLabels();
            this.dateSelectorPictureBox.Invalidate();
        }

        private void dateSelectorPictureBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.StartCheckBox == true && this.EndCheckBox == true)
            {
                if (isStart == true)
                {
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
            if (isStart == true)
            {
                this.StartSelectedDateTime = this.SelectedDateTime;
            }
            else
            {
                this.EndSelectedDateTime = this.SelectedDateTime;
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
                }
            }
        }
        #endregion Control events

        #region Private functions

        private void CreateGraphicObjects()
        {
            this.headerBrush = new SolidBrush(headerColor);
            this.activeMonthBrush = new SolidBrush(activeMonthColor);
            this.inactiveMonthBrush = new SolidBrush(this.inactiveMonthColor);
            this.selectedDayBrush = new SolidBrush(this.SelectedObjectColor);
            this.selectedRangeBrush = new SolidBrush(selectedRangeColor);
            this.selectedDayFontBrush = new SolidBrush(selectedDayFontColor);
            this.nonselectedDayFontBrush = new SolidBrush(nonselectedDayFontColor);
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
            //number of days in active month
            //int intCurrDays = DateTime.DaysInMonth(currentDateTime.Year, currentDateTime.Month);

            //DateTime[] datesCurr = new DateTime[intCurrDays];

            //for (int i = 0; i < intCurrDays; i++)
            //{
            //    datesCurr[i] = new DateTime(currentDateTime.Year, currentDateTime.Month, i + 1);
            //}

            ////array to hold dates corresponding to grid
            //this.arrDates = new DateTime[7, 6];//dates ahead of current date

            //int intWeek = 0;
            //int intDayOfWeek = Array.IndexOf(this.strDays, datesCurr[0].DayOfWeek.ToString());

            //for (int intDay = 0; intDay < intCurrDays; intDay++)
            //{
            //    intDayOfWeek = Array.IndexOf(strDays, datesCurr[intDay].DayOfWeek.ToString());

            //    //fill the array with the day numbers
            //    this.arrDates[intDayOfWeek, intWeek] = datesCurr[intDay];
            //    if (intDayOfWeek == 6)
            //    {
            //        intWeek++;
            //    }
            //}
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
            rects = this.CreateGrid(dateSelectorPictureBox.Width, dateSelectorPictureBox.Height);
            this.CreateGraphicObjects();
            bDesign = false;
            if (count == 0)
            {
                this.InitializePresets();
            }
            this.InitializeDateRangeSelector();

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

        public void InitializeDateRangeSelector()
        {
            if (this.StartCheckBox == true)
            {
                if (this.EndCheckBox == true)
                {
                    //this.startEndButton.Enabled = true;

                    this.monthGroupBox.Enabled = true;
                    this.dayGroupBox.Enabled = true;
                    this.yearGroupBox.Enabled = true;
                    this.presetsGroupBox.Enabled = true;

                    this.startLabel.Enabled = true;
                    this.endLabel.Enabled = true;

                    this.isStart = true;
                    this.SelectedDateTime = this.StartSelectedDateTime;

                    this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year}";
                }
                else
                {
                    // start == true & end == false 

                    //this.startEndButton.Enabled = true;

                    this.monthGroupBox.Enabled = true;
                    this.dayGroupBox.Enabled = true;
                    this.yearGroupBox.Enabled = true;
                    this.presetsGroupBox.Enabled = true;

                    this.startLabel.Enabled = true;
                    this.endLabel.Font = new Font(this.endLabel.Font.Name, 8, FontStyle.Regular);
                    this.isStart = true;
                    this.SelectedDateTime = this.StartSelectedDateTime;

                    this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year}";
                }
            }
            else
            {
                if (this.EndCheckBox == true)
                {
                    // start == false & end == true

                    //this.startEndButton.Enabled = true;

                    this.monthGroupBox.Enabled = true;
                    this.dayGroupBox.Enabled = true;
                    this.yearGroupBox.Enabled = true;
                    this.presetsGroupBox.Enabled = true;
                    this.startLabel.Font = new Font(this.startLabel.Font.Name, 8, FontStyle.Regular);
                    this.endLabel.Enabled = true;

                    this.isStart = false;
                    this.SelectedDateTime = this.EndSelectedDateTime;

                    this.endLabel.Font = new Font(endLabel.Font.Name, 8, FontStyle.Bold | FontStyle.Underline);

                    this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year}";
                }
                else
                {
                    // start == false & end == false 

                    //this.startEndButton.Enabled = false;

                    this.monthGroupBox.Enabled = false;
                    this.dayGroupBox.Enabled = false;
                    this.yearGroupBox.Enabled = false;
                    this.presetsGroupBox.Enabled = false;

                    this.isStart = true;
                    this.SelectedDateTime = this.StartSelectedDateTime;

                    this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year}";
                }
            }
            this.InitializeMonthButtons();
            this.InitializeYearButtons();
            this.GetWeekNumber(this.SelectedDateTime);
            this.InitializePresetsLabels();
        }
        #endregion Form events

        private void InitializeYearButtons()
        {
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
        private void InitializePresetsLabels()
        {
            if (count > 0)
            {
                if (this.isStart)
                {
                    string[] words = startLabel.Text.Split(' ');
                    string dayNameAbb = words[0].Substring(0, 3);
                    this.dayLabel.Text = $"{dayNameAbb} {words[1].Substring(0, words[1].Length - 1)}";

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
                else
                {
                    string[] words = endLabel.Text.Split(' ');
                    string dayNameAbb = words[0].Substring(0, 3);
                    this.dayLabel.Text = $"{dayNameAbb} {words[1].Substring(0, words[1].Length - 1)}";

                    string monthNameAbb = words[2].Substring(0, 3).ToUpper();
                    this.monthLabel.Text = $"{monthNameAbb}";

                    this.yearLabel.Text = $"{words[3]}";

                    CultureInfo ciCurr = CultureInfo.CurrentCulture;
                    var date = new DateTime(this.EndSelectedDateTime.Year, this.EndSelectedDateTime.Month, this.EndSelectedDateTime.Day);
                    int weekNum = ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                    this.weekLabel.Text = Convert.ToString(weekNum);
                }
            }
        }        
        private void InitializePresets()
        {
            DateTime dateTimeNow = DateTime.Now;
            string presetsLabels = $"{dateTimeNow.DayOfWeek} {dateTimeNow.Day}, {this.strMonths[dateTimeNow.Month - 1]} {dateTimeNow.Year}";
            string[] words = presetsLabels.Split(' ');
            string dayNameAbb = words[0].Substring(0, 3);
            this.dayLabel.Text = $"{dayNameAbb} {words[1].Substring(0, words[1].Length - 1)}";

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
            //CultureInfo ciCurr = CultureInfo.CurrentCulture;
            CultureInfo ciCurr = CultureInfo.GetCultureInfo("de-DE");
            var date = new DateTime(Convert.ToInt32(this.yearLabel.Text), month + 1, day);
            int weekNum = ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            this.weekLabel.Text = Convert.ToString(weekNum);
            //if (date.Month == 1 && date.Day == 1)
            //{
            //    weekNum = 1;
            //    this.weekLabel.Text = Convert.ToString(1);
            //}
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
            Button priorYearSelectedButton = this.yearGroupBox.Controls.OfType<Button>()
                                              .Where(b => b.Text.Trim() != string.Empty &&
                                                          Convert.ToInt32(b.Text) == this.SelectedDateTime.Year)
                                              .FirstOrDefault();
            if (priorYearSelectedButton != null)
            {
                priorYearSelectedButton.BackColor = Color.Gainsboro;
            }

            Button priorMonthSelectedButton = this.monthGroupBox
                                                  .Controls
                                                  .OfType<Button>()
                                                  .Where(b => Convert.ToInt32(b.Tag) == this.SelectedDateTime.Month)
                                                  .FirstOrDefault();
            priorMonthSelectedButton.BackColor = Color.Gainsboro;
        }

        private void SetButtonsAsSelected(int year, int month)
        {
            Button selectedYearButton = this.yearGroupBox.Controls.OfType<Button>()
                                            .Where(b => b.Text == year.ToString())
                                            .FirstOrDefault();
            if (selectedYearButton != null)
            {
                selectedYearButton.BackColor = SelectedObjectColor;
            }

            Button selectedMonthButton = this.monthGroupBox.Controls.OfType<Button>()
                                             .Where(b => Convert.ToInt32(b.Tag) == month)
                                             .FirstOrDefault();
            selectedMonthButton.BackColor = SelectedObjectColor;
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
                this.startLabel.Font = new Font(startLabel.Font.Name, 8, FontStyle.Bold | FontStyle.Underline);
                this.endLabel.Font = new Font(endLabel.Font.Name, 8, FontStyle.Regular | FontStyle.Regular);

                this.EndSelectedDateTime = this.SelectedDateTime;
                this.SelectedDateTime = this.StartSelectedDateTime;
            }
            else
            {
                this.endCheckBox.Checked = true;
                this.endLabel.Font = new Font(startLabel.Font.Name, 8, FontStyle.Bold | FontStyle.Underline);
                this.startLabel.Font = new Font(endLabel.Font.Name, 8, FontStyle.Regular | FontStyle.Regular);

                this.StartSelectedDateTime = this.SelectedDateTime;
                this.SelectedDateTime = this.EndSelectedDateTime;
            }

            this.SetButtonsAsSelected(this.SelectedDateTime.Year, this.SelectedDateTime.Month);

            this.dateSelectorPictureBox.Invalidate();
        }
        //private void startEndButton_Click(object sender, EventArgs e)
        //{
        //    this.isStart = !this.isStart;

        //    this.SetButtonsToDefaultColor();
        //    //changed
        //    this.InitializeMonthButtons();
        //    this.InitializeYearButtons();
        //    //changed
        //    if (this.isStart == true)
        //    {
        //        this.startCheckBox.Checked = true;
        //        this.startLabel.Font = new Font(startLabel.Font.Name, 8, FontStyle.Bold | FontStyle.Underline);
        //        this.endLabel.Font = new Font(endLabel.Font.Name, 8, FontStyle.Regular | FontStyle.Regular);

        //        this.EndSelectedDateTime = this.SelectedDateTime;
        //        this.SelectedDateTime = this.StartSelectedDateTime;
        //    }
        //    else
        //    {
        //        this.endCheckBox.Checked = true;
        //        this.endLabel.Font = new Font(startLabel.Font.Name, 8, FontStyle.Bold | FontStyle.Underline);
        //        this.startLabel.Font = new Font(endLabel.Font.Name, 8, FontStyle.Regular | FontStyle.Regular);

        //        this.StartSelectedDateTime = this.SelectedDateTime;
        //        this.SelectedDateTime = this.EndSelectedDateTime;
        //    }

        //    this.SetButtonsAsSelected(this.SelectedDateTime.Year, this.SelectedDateTime.Month);

        //    this.dateSelectorPictureBox.Invalidate();
        //}

        public void ShowDateRangeSelectorState01()
        {
            if (this.StartCheckBox == true)
            {

                if (this.EndCheckBox == true)
                {
                    //this.startEndButton.Enabled = true;

                    this.startLabel.Enabled = true;
                    this.startLabel.Font = new Font(startLabel.Font.Name, 8, FontStyle.Regular);

                    this.endLabel.Text = $"{this.SelectedDateTime.DayOfWeek} {this.SelectedDateTime.Day}, {this.strMonths[this.SelectedDateTime.Month - 1]} {this.SelectedDateTime.Year}";
                }
                else
                {
                    // start == true & end == false 

                    //this.startEndButton.Enabled = true;

                    this.monthGroupBox.Enabled = true;
                    this.dayGroupBox.Enabled = true;
                    this.yearGroupBox.Enabled = true;
                    this.presetsGroupBox.Enabled = true;

                    this.startLabel.Enabled = true;

                    this.isStart = true;
                    this.SelectedDateTime = this.StartSelectedDateTime;

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
                        this.StartEndButtonClick();
                    }

                    //this.startEndButton.Enabled = true;
                }
                else
                {
                    // start == false & end == false 
                    this.monthGroupBox.Enabled = false;
                    this.dayGroupBox.Enabled = false;
                    this.yearGroupBox.Enabled = false;
                    this.presetsGroupBox.Enabled = false;

                    this.StartSelectedDateTime = this.SelectedDateTime;
                }
            }
        }

        protected virtual void startCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.ShowDateRangeSelectorState01();
        }

        public void ShowDateRangeSelectorState02()
        {
            if (this.StartCheckBox == true)
            {
                if (this.EndCheckBox == true)
                {
                    //this.startEndButton.Enabled = true;

                    this.endLabel.Enabled = true;
                }
                else
                {
                    // start == true & end == false 

                    if (this.isStart == false)
                    {
                        this.StartEndButtonClick();
                    }

                    //this.startEndButton.Enabled = true;
                }
            }
            else
            {
                if (this.EndCheckBox == true)
                {
                    // start == false & end == true
                    //this.startEndButton.Enabled = true;
                    this.monthGroupBox.Enabled = true;
                    this.dayGroupBox.Enabled = true;
                    this.yearGroupBox.Enabled = true;
                    this.presetsGroupBox.Enabled = true;

                    this.endLabel.Enabled = true;

                    this.startLabel.Font = new Font(this.startLabel.Font.Name, 8, FontStyle.Regular);
                    this.endLabel.Font = new Font(this.endLabel.Font.Name, 8, FontStyle.Bold | FontStyle.Underline);

                    this.SetButtonsToDefaultColor();

                    this.isStart = false;
                    this.SelectedDateTime = this.EndSelectedDateTime;

                    this.SetButtonsAsSelected(this.SelectedDateTime.Year, this.SelectedDateTime.Month);

                    this.dateSelectorPictureBox.Invalidate();
                }
                else
                {
                    // start == false & end == false 

                    this.monthGroupBox.Enabled = false;
                    this.dayGroupBox.Enabled = false;
                    this.yearGroupBox.Enabled = false;
                    this.presetsGroupBox.Enabled = false;

                    this.EndSelectedDateTime = this.SelectedDateTime;

                    this.startLabel.Font = new Font(this.startLabel.Font.Name, 8, FontStyle.Bold | FontStyle.Underline);
                    this.endLabel.Font = new Font(this.endLabel.Font.Name, 8, FontStyle.Regular);

                    this.SetButtonsToDefaultColor();

                    this.isStart = true;
                    this.SelectedDateTime = this.StartSelectedDateTime;

                    this.SetButtonsAsSelected(this.SelectedDateTime.Year, this.SelectedDateTime.Month);

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
                    this.StartEndButtonClick();
                }
                else
                {
                    //this.startEndButton.Enabled = true;
                    this.StartEndButtonClick();
                }

            }
            else
            {
                this.endCheckBox.Checked = !this.endCheckBox.Checked;
            }
        }


        private void DateRangeSelectorPopup1_MouseClick(object sender, MouseEventArgs e)
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
                                this.dayGroupBox.Enabled = true;
                                this.monthGroupBox.Enabled = true;
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
                            button.PerformClick();
                            count--;
                        }
                    }                    
                }
                #endregion check the clicking of this.presetsGroupBox.Controls
            }
            count++;
            this.MonthClickByNextPrevButtons(SelectedDateTime);
            this.dateSelectorPictureBox.Invalidate();
            this.InitializeMonthButtons();
            this.InitializeYearButtons();
            this.InitializePresetsLabels();
            this.GetWeekNumber(this.SelectedDateTime);
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
                SelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                           this.SelectedDateTime.Month,
                                           this.SelectedDateTime.Day,
                                           0,
                                           0,
                                           0
                                           ).AddDays(-1);
            }
            else
            {
                SelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                           this.SelectedDateTime.Month,
                                           this.SelectedDateTime.Day,
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

                this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year}  {this.StartSelectedDateTime.Hour}:{this.StartSelectedDateTime.Minute}";
            }
            count++;
            this.MonthClickByNextPrevButtons(SelectedDateTime);
            this.dateSelectorPictureBox.Invalidate();
            this.InitializeMonthButtons();
            this.InitializeYearButtons();
            this.InitializePresetsLabels();
            this.GetWeekNumber(this.SelectedDateTime);
        }

        private void nextDay_Click(object sender, EventArgs e)
        {
            if (this.isStart)
            {
                SelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                           this.SelectedDateTime.Month,
                                           this.SelectedDateTime.Day,
                                           0,
                                           0,
                                           0
                                           ).AddDays(+1);
            }
            else
            {
                SelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                           this.SelectedDateTime.Month,
                                           this.SelectedDateTime.Day,
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
            this.InitializePresetsLabels();
            this.GetWeekNumber(this.SelectedDateTime);
        }
        private void prevMonth_Click(object sender, EventArgs e)
        {
            if (this.SelectedDateTime.Month == 1)
            {
                return;
            }
            DateTime firstDay = new DateTime(this.SelectedDateTime.Year, this.SelectedDateTime.Month - 1, 1);
            DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
            if (this.isStart)
            {
                SelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                       this.SelectedDateTime.Month,
                                       firstDay.Day,
                                       0,
                                       0,
                                       0
                                       ).AddMonths(-1);
            }
            else
            {
                SelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                       this.SelectedDateTime.Month - 1,
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
            this.InitializePresetsLabels();
            this.GetWeekNumber(this.SelectedDateTime);
        }

        private void nextMonth_Click(object sender, EventArgs e)
        {
            if (this.SelectedDateTime.Month == 12)
            {
                return;
            }
            DateTime firstDay = new DateTime(this.SelectedDateTime.Year, this.SelectedDateTime.Month + 1, 1);
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
            this.InitializePresetsLabels();
            this.GetWeekNumber(this.SelectedDateTime);
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
                this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year}";
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
                this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year}";
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
                this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year}";
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

                this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year}";
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
            this.InitializePresetsLabels();
            this.GetWeekNumber(this.SelectedDateTime);
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
            this.InitializePresetsLabels();
            this.GetWeekNumber(this.SelectedDateTime);
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
                this.endLabel.Text = $"{this.EndSelectedDateTime.DayOfWeek} {this.EndSelectedDateTime.Day}, {this.strMonths[this.EndSelectedDateTime.Month - 1]} {this.EndSelectedDateTime.Year}";
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
                this.startLabel.Text = $"{this.StartSelectedDateTime.DayOfWeek} {this.StartSelectedDateTime.Day}, {this.strMonths[this.StartSelectedDateTime.Month - 1]} {this.StartSelectedDateTime.Year}";
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
            this.InitializePresetsLabels();
            this.GetWeekNumber(this.SelectedDateTime);
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
            this.InitializePresetsLabels();
            this.GetWeekNumber(this.SelectedDateTime);
        }
       
        private void dayLabel_Click(object sender, EventArgs e)
        {
            
            if (this.isStart)
            {
                SelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                           this.SelectedDateTime.Month,
                                           this.SelectedDateTime.Day,
                                           0,
                                           0,
                                           0
                                           );
            }
            else
            {
                SelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                           this.SelectedDateTime.Month,
                                           this.SelectedDateTime.Day,
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
            count++;
            this.MonthClickByNextPrevButtons(SelectedDateTime);
            this.dateSelectorPictureBox.Invalidate();
            this.InitializeMonthButtons();
            this.InitializeYearButtons();
            this.InitializePresetsLabels();
            this.GetWeekNumber(this.SelectedDateTime);
        }

        private void weekLabel_Click(object sender, EventArgs e)
        {            
            this.weekLabel.Text = Convert.ToString(Convert.ToInt32(this.weekLabel.Text) - 1);

            DateTime selectedDate = new DateTime(this.SelectedDateTime.Year,
                                       this.SelectedDateTime.Month,
                                       this.SelectedDateTime.Day,
                                       this.SelectedDateTime.Hour,
                                       this.SelectedDateTime.Minute,
                                       this.SelectedDateTime.Second);
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
            this.InitializePresetsLabels();
            this.GetWeekNumber(this.SelectedDateTime);
        }

        private void monthLabel_Click(object sender, EventArgs e)
        {            
            DateTime firstDay = new DateTime(this.SelectedDateTime.Year, this.SelectedDateTime.Month, 1);
            DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
            if (this.isStart)
            {
                SelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                       this.SelectedDateTime.Month,
                                       firstDay.Day,
                                       0,
                                       0,
                                       0
                                       );
            }
            else
            {
                SelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                       this.SelectedDateTime.Month,
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
            this.InitializePresetsLabels();
            this.GetWeekNumber(this.SelectedDateTime);
        }

        private void yearLabel_Click(object sender, EventArgs e)
        {
            if (this.isStart)
            {
                SelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                       1,
                                       1,
                                       0,
                                       0,
                                       0
                                       );
            }
            else
            {
                SelectedDateTime = new DateTime(this.SelectedDateTime.Year,
                                       12,
                                       31,
                                       23,
                                       59,
                                       59
                                       );
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
            this.InitializePresetsLabels();
            this.GetWeekNumber(this.SelectedDateTime);
        }
    }
}
