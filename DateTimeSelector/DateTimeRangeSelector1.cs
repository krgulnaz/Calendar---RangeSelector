using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using NLog;
using PopupControl;

namespace NaitonControls
{
  [Designer(typeof(FixedHeightUserControlDesigner))]
  public partial class DateTimeRangeSelector1 : UserControl
  {
    private readonly static Logger logger = LogManager.GetCurrentClassLogger();
    private const int FIXED_HEIGHT = 20;
    private Popup popup;

    public DateTimeRangeSelector1()
    {
      InitializeComponent();

      this.Height = FIXED_HEIGHT;

      this.MouseClick += this.ControlOnMouseClick;
      if (this.HasChildren == true)
      {
        this.AddOnMouseClickHandlerRecursive(Controls);
      }
    }

    protected override void OnLoad(EventArgs e)
    {
      this.startLabel.Text = this.StartSelectedDateTime.ToString(this.DateStringFormat);
      this.endLabel.Text = this.EndSelectedDateTime.ToString(this.DateStringFormat);
      this.ShowDateTimeState();
      base.OnLoad(e);
    }

    private DateTime startSelectedDateTime = DateTime.Now;

    private DateTime StartSelectedDateTime
    {
      get
      {
        return this.startSelectedDateTime;
      }
      set
      {
        this.startSelectedDateTime = value;
        this.startLabel.Text = this.startSelectedDateTime.ToString(this.DateStringFormat);
      }
    }

    [Description("The selected start date.")]
    [Category(nameof(DateTimeRangeSelector1))]
    public DateTime? StartDate
    {
      get
      {
        if (this.StartCheckBox == true)
        {
          return this.StartSelectedDateTime;
        }
        else
        {
          return null;
        }
      }
      set
      {
        if (value.HasValue)
        {
          this.StartSelectedDateTime = value.Value;
        }
      }
    }

    private DateTime endSelectedDateTime = DateTime.Now;

    private DateTime EndSelectedDateTime
    {
      get
      {
        return this.endSelectedDateTime;
      }
      set
      {
        this.endSelectedDateTime = value;
        this.endLabel.Text = this.endSelectedDateTime.ToString(this.DateStringFormat);
      }
    }

    [Description("The selected end date.")]
    [Category(nameof(DateTimeRangeSelector1))]
    public DateTime? EndDate
    {
      get
      {
        if (this.EndCheckBox == true)
        {
          return this.EndSelectedDateTime;
        }
        else
        {
          return null;
        }
      }
      set
      {
        if (value.HasValue)
        {
          this.EndSelectedDateTime = value.Value;
        }
      }
    }

    private bool startCheckBox = true;

    [Category(nameof(DateTimeRangeSelector1))]
    public bool StartCheckBox
    {
      get
      {
        return this.startCheckBox;
      }
      set
      {
        if (this.IsNullable == false)
        {
          value = true;
        }
        this.startCheckBox = value;

                //if (this.startCheckBox == false)
                //{
                //  this.startLabel.Enabled = false;
                //}
                //else
                //{
                //  this.startLabel.Enabled = true;
                //}                        
               this.startLabel.Enabled = this.startCheckBox;            
            }
    }

    private bool endCheckBox = true;

    [Category(nameof(DateTimeRangeSelector1))]
    public bool EndCheckBox
    {
      get
      {
        return this.endCheckBox;
      }
      set
      {
        if (this.IsNullable == false)
        {
          value = true;
        }

        this.endCheckBox = value;

                //if (this.endCheckBox == false)
                //{
                //  this.endLabel.Enabled = false;
                //}
                //else
                //{
                //  this.endLabel.Enabled = true;
                //}                
                this.endLabel.Enabled = this.endCheckBox;                               
            }
    }

    private bool isNullable = true;

    [Category(nameof(DateTimeRangeSelector1))]
    public bool IsNullable
    {
      get
      {
        return this.isNullable;
      }
      set
      {
        if (value == false)
        {
          this.StartCheckBox = true;
          this.EndCheckBox = true;
        }
        this.isNullable = value;
      }
    }

    private bool checkBoxState = false;

    [Category(nameof(DateTimeRangeSelector1))]
    public bool CheckBoxState
    {
      get
      {
        return this.checkBoxState;
      }
      set
      {
        this.checkBoxState = value;
        this.StartCheckBox = this.EndCheckBox = this.checkBoxState;
      }
    }

    private Color selectedObjectColor = Color.SpringGreen;

    private Color SelectedObjectColor
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


    public enum ControlType { Date, DateTime, Time };

    private ControlType controlType;

    [Category(nameof(DateTimeRangeSelector1))]
    public ControlType CurrentControlType
    {
      get
      {
        return this.controlType;
      }
      set
      {
        this.controlType = value;
        //if (controlType == ControlType.Time)
        //{
        //  dateStringFormat = ;
        //}
        //else
        //{
        //  dateStringFormat = "dd MMM yy"; ;
        //}
        dateStringFormat = controlType == ControlType.Time ? "hh:mm tt" : "dd MMM yy";
        this.StartSelectedDateTime = this.StartSelectedDateTime;
        this.EndSelectedDateTime = this.EndSelectedDateTime;
      }
    }

    private bool isSync = false;

    [Category(nameof(DateTimeRangeSelector1))]
    public bool IsSync
    {
      get
      {
        return this.isSync;
      }
      set
      {
        if (this.IsNullable == false)
        {
          this.isSync = false;
        }
        else
        {
          this.isSync = value;
        }
      }
    }

    private string dateStringFormat = "dd MMM yy";
    private string DateStringFormat
    {
      get
      {
        return this.dateStringFormat;
      }
      set
      {
        this.dateStringFormat = value;
      }
    }


    private void okButton_Click(object sender, EventArgs e)
    {
      this.ShowDateTimeState();
      this.popup.Hide();
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
      this.popup.Hide();
    }

    public event Action<DateTime, DateTime, bool, bool> SelectedDateChanged = null;
    protected void dateRangeSelectorButton_SelectedDateChanged(DateTime startDateTime, DateTime endDateTime, bool sCheckBox, bool eCheckBox)
    {
      this.StartSelectedDateTime = startDateTime;
      this.EndSelectedDateTime = endDateTime;

      this.StartCheckBox = sCheckBox;
      this.EndCheckBox = eCheckBox;
      if (this.SelectedDateChanged != null)
      {
        this.SelectedDateChanged(startDateTime, endDateTime, sCheckBox, eCheckBox);
      }
    }

    private void CreateDateTimeRangeSelector()
    {
      DateTimeRangeSelectorPopup1 dateRangeSelector = new DateTimeRangeSelectorPopup1();
      dateRangeSelector.okButtonClick = this.okButton_Click;
      dateRangeSelector.cancelButtonClick = this.cancelButton_Click;
      dateRangeSelector.StartSelectedDateTime = this.StartSelectedDateTime;
      dateRangeSelector.EndSelectedDateTime = this.EndSelectedDateTime;
      dateRangeSelector.IsNullable = this.IsNullable;
      dateRangeSelector.SelectedObjectColor = this.SelectedObjectColor;
      dateRangeSelector.StartCheckBox = this.StartCheckBox;
      dateRangeSelector.EndCheckBox = this.EndCheckBox;
      dateRangeSelector.SelectedDateChanged += this.dateRangeSelectorButton_SelectedDateChanged;

      this.popup = new Popup(dateRangeSelector);
      if (SystemInformation.IsComboBoxAnimationEnabled)
      {
        this.popup.ShowingAnimation = PopupAnimations.Slide | PopupAnimations.TopToBottom;
        this.popup.HidingAnimation = PopupAnimations.Slide | PopupAnimations.BottomToTop;
      }
      else
      {
        this.popup.ShowingAnimation = popup.HidingAnimation = PopupAnimations.None;
      }
    }

    private void CreateDateRangeSelector()
    {
      DateRangeSelectorPopup1 dateRangeSelector = new DateRangeSelectorPopup1();
      dateRangeSelector.okButtonClick = this.okButton_Click;
      dateRangeSelector.cancelButtonClick = this.cancelButton_Click;
      dateRangeSelector.StartSelectedDateTime = this.StartSelectedDateTime;
      dateRangeSelector.EndSelectedDateTime = this.EndSelectedDateTime;
      dateRangeSelector.IsNullable = this.IsNullable;
      dateRangeSelector.SelectedObjectColor = this.SelectedObjectColor;
      dateRangeSelector.StartCheckBox = this.StartCheckBox;
      dateRangeSelector.EndCheckBox = this.EndCheckBox;
      dateRangeSelector.SelectedDateChanged += dateRangeSelectorButton_SelectedDateChanged;

      this.popup = new Popup(dateRangeSelector);
      if (SystemInformation.IsComboBoxAnimationEnabled)
      {
        this.popup.ShowingAnimation = PopupAnimations.Slide | PopupAnimations.TopToBottom;
        this.popup.HidingAnimation = PopupAnimations.Slide | PopupAnimations.BottomToTop;
      }
      else
      {
        this.popup.ShowingAnimation = popup.HidingAnimation = PopupAnimations.None;
      }
    }

    private void CreateTimeRangeSelector()
    {
      TimeRangeSelectorPopup1 timeRangeSelector = new TimeRangeSelectorPopup1();
      timeRangeSelector.okButtonClick = this.okButton_Click;
      timeRangeSelector.cancelButtonClick = this.cancelButton_Click;
      timeRangeSelector.StartSelectedDateTime = this.StartSelectedDateTime;
      timeRangeSelector.EndSelectedDateTime = this.EndSelectedDateTime;
      timeRangeSelector.SelectedObjectColor = this.SelectedObjectColor;
      timeRangeSelector.StartCheckBox = this.StartCheckBox;
      timeRangeSelector.EndCheckBox = this.EndCheckBox;
      timeRangeSelector.SelectedDateChanged += dateRangeSelectorButton_SelectedDateChanged;

      this.popup = new Popup(timeRangeSelector);
      if (SystemInformation.IsComboBoxAnimationEnabled)
      {
        this.popup.ShowingAnimation = PopupAnimations.Slide | PopupAnimations.TopToBottom;
        this.popup.HidingAnimation = PopupAnimations.Slide | PopupAnimations.BottomToTop;
      }
      else
      {
        this.popup.ShowingAnimation = popup.HidingAnimation = PopupAnimations.None;
      }
    }

    private void CreateDateRangeSyncSelector()
    {
      DateRangeSelectorSyncPopup1 dateRangeSelector = new DateRangeSelectorSyncPopup1();
      dateRangeSelector.okButtonClick = this.okButton_Click;
      dateRangeSelector.cancelButtonClick = this.cancelButton_Click;
      dateRangeSelector.StartSelectedDateTime = this.StartSelectedDateTime;
      dateRangeSelector.EndSelectedDateTime = this.EndSelectedDateTime;
      dateRangeSelector.SelectedObjectColor = this.SelectedObjectColor;
      dateRangeSelector.StartCheckBox = this.StartCheckBox;
      dateRangeSelector.EndCheckBox = this.EndCheckBox;
      dateRangeSelector.SelectedDateChanged += dateRangeSelectorButton_SelectedDateChanged;

      this.popup = new Popup(dateRangeSelector);
      if (SystemInformation.IsComboBoxAnimationEnabled)
      {
        this.popup.ShowingAnimation = PopupAnimations.Slide | PopupAnimations.TopToBottom;
        this.popup.HidingAnimation = PopupAnimations.Slide | PopupAnimations.BottomToTop;
      }
      else
      {
        this.popup.ShowingAnimation = popup.HidingAnimation = PopupAnimations.None;
      }
    }

    private void CreateDateTimeRangeSyncSelector()
    {
      DateTimeRangeSyncSelectorPopup1 dateRangeSelector = new DateTimeRangeSyncSelectorPopup1();
      dateRangeSelector.okButtonClick = this.okButton_Click;
      dateRangeSelector.cancelButtonClick = this.cancelButton_Click;
      dateRangeSelector.StartSelectedDateTime = this.StartSelectedDateTime;
      dateRangeSelector.EndSelectedDateTime = this.EndSelectedDateTime;
      dateRangeSelector.SelectedObjectColor = this.SelectedObjectColor;
      dateRangeSelector.StartCheckBox = this.StartCheckBox;
      dateRangeSelector.EndCheckBox = this.EndCheckBox;
      dateRangeSelector.SelectedDateChanged += dateRangeSelectorButton_SelectedDateChanged;

      this.popup = new Popup(dateRangeSelector);
      if (SystemInformation.IsComboBoxAnimationEnabled)
      {
        this.popup.ShowingAnimation = PopupAnimations.Slide | PopupAnimations.TopToBottom;
        this.popup.HidingAnimation = PopupAnimations.Slide | PopupAnimations.BottomToTop;
      }
      else
      {
        this.popup.ShowingAnimation = popup.HidingAnimation = PopupAnimations.None;
      }
    }

    protected override void OnSizeChanged(EventArgs e)
    {
            if (this.Size.Height != FIXED_HEIGHT)
               this.Size = new Size(this.Size.Width, FIXED_HEIGHT);
            this.startLabel.Location = new Point((this.Size.Width / 2) - this.startLabel.Width, 0);
            this.endLabel.Location = new Point((this.Size.Width / 2) + 1, 0);
           // this.label1.Location = new Point((this.Size.Width - this.label1.Width) / 2 + 1, this.label1.Location.Y);
            base.OnSizeChanged(e);
    }

    public void ShowDateTimeState()
    {
      if (this.StartCheckBox == true)
      {
        if (this.EndCheckBox == true)
        {
          //start=true end=true
          this.startLabel.Enabled = true;
          this.endLabel.Enabled = true;
        }
        else
        {
          //start=true end=false
          this.startLabel.Enabled = true;
          this.endLabel.Enabled = false;
        }
      }
      else
      {
        if (this.EndCheckBox == true)
        {
          //end=true start=false
          this.startLabel.Enabled = false;
          this.endLabel.Enabled = true;
        }
        else
        {
          //end=false start=false
          this.startLabel.Enabled = false;
          this.endLabel.Enabled = false;
        }
      }
    }

    private void AddOnMouseClickHandlerRecursive(IEnumerable controls)
    {
      foreach (Control control in controls)
      {
        control.MouseClick += this.ControlOnMouseClick;

        if (control.HasChildren)
          this.AddOnMouseClickHandlerRecursive(control.Controls);
      }
    }

    private void ControlOnMouseClick(object sender, MouseEventArgs args)
    {
      if (args.Button == MouseButtons.Right)
        return;

      if (this.CurrentControlType == ControlType.DateTime)
      {
        if (this.IsSync == false)
        {
          this.CreateDateTimeRangeSelector();
        }
        else
        {
          this.CreateDateTimeRangeSyncSelector();
        }
      }
      else if (this.CurrentControlType == ControlType.Date)
      {
        if (this.IsSync == false)
        {
          this.CreateDateRangeSelector();
        }
        else
        {
          this.CreateDateRangeSyncSelector();
        }
      }
      else
      {
        this.CreateTimeRangeSelector();
      }
      this.popup.Show(this);
    }

        private void startLabel_Click(object sender, EventArgs e)
        {

        }

        private void DateTimeRangeSelector1_Load(object sender, EventArgs e)
        {

        }
    }
}
