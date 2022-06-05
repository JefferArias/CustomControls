using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using CustomControls.Themes;

namespace CustomControls.Controls
{
    //Variables
    public sealed partial class Pnl : Panel
    {
        private PnlChild pnlChild = new PnlChild();
        private int desp;
        private EffectTo effectActive;
    }

    //Constructor
    public sealed partial class Pnl
    {
        public Pnl()
        {
            Font = Themes.Themes.GetFont();
            base.Dock = DockStyle.Fill;
            base.Controls.Add(pnlChild);
            PnlChildActive = pnlChild;
        }
    }

    //Propiedades
    public sealed partial class Pnl
    {
        public new Color BackColor
        {
            get => base.BackColor;
            set
            {
                base.BackColor = value;
                PnlChildActive.BackColor = Themes.Themes.GetColor(value);
            }
        }

        public new ControlCollection Controls
        {
            get => PnlChildActive.Controls;
        }

        public new DockStyle Dock
        {
            get => base.Dock;
            set
            {
                if (value == DockStyle.None)
                    return;
                else
                    base.Dock = value;
            }
        }

        private PnlChild PnlChildActive { get; set; }
    }

    //Metodos
    public sealed partial class Pnl
    {
        public void ShowChild(EffectTo efecto, PnlChild panel)
        {
            desp = (int)Math.Truncate((double)PnlChildActive.Width / 10);
            effectActive = efecto;

            ControllerEffects(ShowHide.Hide, SetEffectHide(efecto));

            PnlChildActive = panel;
            SetSzLcPnlChild(efecto);
            panel.CloseEvent += Panel_CloseEvent;
            panel.BackColor = Themes.Themes.GetColor(BackColor);

            base.Controls.Add(panel);

            ControllerEffects(ShowHide.Show, efecto);

            if (panel.Controls.Count > 0)
                panel.Controls[0].Select();
            else
                panel.Select();
        }

        private void Panel_CloseEvent(object sender, EventArgs e)
        {
            ControllerEffects(ShowHide.Hide, SetEffectHide(effectActive));
            PnlChildActive = pnlChild;
            ControllerEffects(ShowHide.Show, effectActive);

            PnlChild child = (PnlChild)sender;
            child.CloseEvent -= Panel_CloseEvent;
            base.Controls.Remove(child);
            child.Dispose();
        }

        private void ControllerEffects(ShowHide showHide, EffectTo efecto)
        {
            switch (efecto)
            {
                case EffectTo.Left: EffectLeft(showHide); break;
                case EffectTo.Top: EffectTop(showHide); break;
                case EffectTo.Right: EffectRight(showHide); break;
                case EffectTo.Down: EffectDown(showHide); break;
            }
        }

        private EffectTo SetEffectHide(EffectTo efecto)
        {
            switch (efecto)
            {
                case EffectTo.Left: return EffectTo.Right;
                case EffectTo.Top: return EffectTo.Down;
                case EffectTo.Right: return EffectTo.Left;
                default: return EffectTo.Top;
            }
        }

        private void SetSzLcPnlChild(EffectTo efecto)
        {
            switch (efecto)
            {
                case EffectTo.Left:
                    PnlChildActive.Location = new Point(Width, 0);

                    break;

                case EffectTo.Right:
                    PnlChildActive.Location = new Point(-Width, 0);
                    break;

                case EffectTo.Top:
                    PnlChildActive.Location = new Point(0, Height);
                    break;

                case EffectTo.Down:
                    PnlChildActive.Location = new Point(0, -Height);
                    break;
            }
            PnlChildActive.Size = new Size(Width, Height);
        }

        private void EffectLeft(ShowHide showHide)
        {
            switch (showHide)
            {
                case ShowHide.Hide:
                    do
                    {
                        Thread.Sleep(20);
                        PnlChildActive.Location = new Point(PnlChildActive.Location.X - desp, 0);
                    } while (PnlChildActive.Location.X > -Width);
                    PnlChildActive.Location = new Point(-Width, 0);
                    break;

                case ShowHide.Show:
                    do
                    {
                        Thread.Sleep(20);
                        PnlChildActive.Location = new Point(PnlChildActive.Location.X - desp, 0);
                    } while (PnlChildActive.Location.X > 0);
                    PnlChildActive.Location = new Point(0, 0);
                    break;
            }
        }

        private void EffectTop(ShowHide showHide)
        {
            switch (showHide)
            {
                case ShowHide.Hide:
                    do
                    {
                        Thread.Sleep(20);
                        PnlChildActive.Location = new Point(0, PnlChildActive.Location.Y - desp);
                    } while (PnlChildActive.Location.Y > -Height);
                    PnlChildActive.Location = new Point(0, -Height);
                    break;

                case ShowHide.Show:
                    do
                    {
                        Thread.Sleep(20);
                        PnlChildActive.Location = new Point(0, PnlChildActive.Location.Y - desp);
                    } while (PnlChildActive.Location.Y > 0);
                    PnlChildActive.Location = new Point(0, 0);
                    break;
            }
        }

        private void EffectRight(ShowHide showHide)
        {
            switch (showHide)
            {
                case ShowHide.Hide:
                    do
                    {
                        Thread.Sleep(20);
                        PnlChildActive.Location = new Point(PnlChildActive.Location.X + desp, 0);
                    } while (PnlChildActive.Location.X < Width);
                    PnlChildActive.Location = new Point(Width, 0);
                    break;

                case ShowHide.Show:
                    do
                    {
                        Thread.Sleep(20);
                        PnlChildActive.Location = new Point(PnlChildActive.Location.X + desp, 0);
                    } while (PnlChildActive.Location.X < 0);
                    PnlChildActive.Location = new Point(0, 0);
                    break;
            }
        }

        private void EffectDown(ShowHide showHide)
        {
            switch (showHide)
            {
                case ShowHide.Hide:
                    do
                    {
                        Thread.Sleep(20);
                        PnlChildActive.Location = new Point(0, PnlChildActive.Location.Y + desp);
                    } while (PnlChildActive.Location.Y < Height);
                    PnlChildActive.Location = new Point(0, Height);
                    break;

                case ShowHide.Show:
                    do
                    {
                        Thread.Sleep(20);
                        PnlChildActive.Location = new Point(0, PnlChildActive.Location.Y + desp);
                    } while (PnlChildActive.Location.Y < 0);
                    PnlChildActive.Location = new Point(0, 0);
                    break;
            }
        }
    }

    //Eventos
    public sealed partial class Pnl
    {
        protected override void OnSizeChanged(EventArgs e)
        {
            PnlChildActive.Width = Width;
            PnlChildActive.Height = Height;
        }
    }

    //Enumeraciones
    public sealed partial class Pnl
    {
        public enum EffectTo : byte
        {
            Left,
            Top,
            Right,
            Down
        }

        private enum ShowHide : byte
        {
            Hide,
            Show
        }
    }
}