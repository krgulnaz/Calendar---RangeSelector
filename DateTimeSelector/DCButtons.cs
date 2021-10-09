using System.ComponentModel;
using System.Windows.Forms;

namespace NaitonControls
{
  [ToolboxItem(false)]
  public class DCButtons : Button
  {
    public DCButtons() : base()
    {
      SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, true);
    }
  }
}
