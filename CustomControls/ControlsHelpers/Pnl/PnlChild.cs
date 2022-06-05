using System;
using System.Windows.Forms;

namespace CustomControls.Controls
{
    public sealed partial class Pnl
    {
        public class PnlChild : Panel
        {
            public event EventHandler CloseEvent = (sender, e) => { };

            public void Close() => CloseEvent(this, EventArgs.Empty);
        }

    }
}
