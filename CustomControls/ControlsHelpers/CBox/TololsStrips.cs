using System.Drawing;
using System.Windows.Forms;

namespace CustomControls.Controls
{
    public sealed partial class CBox
    {
        internal class TSCtrHost : ToolStripControlHost
        {
            public TSCtrHost(Control c) : base(c)
            {
                Padding = new Padding(0);
                Margin = new Padding(0);
                AutoSize = false;
            }
        }

        internal class TSMain : ToolStripDropDown
        {
            public Color Borde { get; set; }

            public TSMain(TSCtrHost c)
            {
                Padding = new Padding(0, 1, 0, 0);
                Margin = new Padding(0);
                AutoSize = true;
                Items.Add(c);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                Pen pen = new Pen(Borde);
                e.Graphics.DrawLine(pen, new Point(0, 0), new Point(Width, 0));
                pen.Dispose();
            }
        }
    }
}
