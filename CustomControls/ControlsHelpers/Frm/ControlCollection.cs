using System;
using System.Drawing;
using System.Windows.Forms;

namespace CustomControls.Controls
{
    public partial class Frm
    {
        public new class ControlCollection : Control.ControlCollection
        {
            public event EventHandler AddControl = (sender, e) => { };
            public event EventHandler RemoveControl = (sender, e) => { };

            public override int Count => Owner.Controls.Count;
            public Control ActiveControl => Owner.Controls[0];

            public Control Main { get; set; }


            public ControlCollection(Frm frm) : base(frm)
            {
            }


            public override void Add(Control value)
            {
                if (value.GetType().GetInterface("IContainerControl") != null)
                {
                    int cantidad = Owner.Controls.Count;
                    if (cantidad == 0)
                    {
                        Main = value;
                        FillControl(value, true);
                    }
                    else
                    {
                        foreach (Control item in Owner.Controls)
                            item.Visible = false;
                        FillControl(value);
                        AddControl(null, EventArgs.Empty);
                    }
                }
                else
                    value.Dispose();
            }

            public override void Remove(Control value)
            {
                AdjustChildren ac = () =>
                { 
                    if (!value.Equals(Main))
                    {
                        Owner.Controls[1].Visible = true;
                        Owner.BackColor = Owner.Controls[1].BackColor;
                        Owner.Controls.Remove(value);
                        value.Dispose();
                        RemoveControl(null, EventArgs.Empty);
                    }
                };
                Owner.Invoke(ac);
            }

            private void FillControl(Control ctr, bool firstCtr = false)
            {
                ctr.Location = new Point(2, 30);
                if (firstCtr)
                    Owner.Size = new Size(ctr.Width + 4, ctr.Height + 32);
                else
                    ctr.Size = new Size(Owner.Width - 4, Owner.Height - 32);
                    
                Owner.BackColor = ctr.BackColor;
                Owner.Controls.Add(ctr);
                ctr.BringToFront();
            }

            public void AdjustChildren()
            {
                AdjustChildren ac = () =>
                {
                    foreach (Control item in Owner.Controls)
                    {
                        item.Size = new Size(Owner.Width - 4, Owner.Height - 32);
                        item.Invalidate();
                    }
                };
                try { Owner.BeginInvoke(ac); }
                catch (Exception) { }
            }
        }
    }

}
