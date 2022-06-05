using CustomControls.Genericos;
using CustomControls.Themes;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CustomControls.Controls
{
    //Variables
    public sealed partial class VScroll : Control
    {
        private Themes.Themes.Tema tema;
        private CColores colores = new CColores();
        private ushort movim = 1;
        private const byte btnHg = 17;
        private const byte wdMin = 18;
        private Rectangle rectTop = new Rectangle(0, 0, wdMin, btnHg);
        private RectangleF rectScroll = new RectangleF(1, btnHg, 16, btnHg);
        private Rectangle rectDown = new Rectangle(0, 0, wdMin, btnHg);
        private IntoRect into = IntoRect.None;
        private MDown mDown = MDown.None;
        private int ejeY;
        private int ejeYNew;
        private readonly Timer tm = new Timer() { Interval = 1 };
        private Slice slice;
        private float despl = 0;
        private ushort ubiSB = 0;
        private byte rep = 1;

        public event EventHandler Up = (sender, e) => { };

        public event EventHandler Down = (sender, e) => { };
    }

    //Constructor
    public sealed partial class VScroll
    {
        public VScroll()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            Tema = Themes.Themes.Tema.Claro;
            TabStop = false;
            Size = new Size(wdMin, 100);
            Dock = DockStyle.Right;
            MinimumSize = new Size(wdMin, (btnHg * 3) + 5);
            tm.Tick += Tm_Tick;
        }
    }

    //Propiedades
    public sealed partial class VScroll
    {
        [Browsable(false)]
        public override Size MinimumSize => base.MinimumSize;

        [Browsable(false)]
        public override Size MaximumSize => base.MaximumSize;

        [Browsable(false)]
        public new int Width { get => base.Width; }

        [Browsable(false)]
        public new DockStyle Dock
        {
            get => base.Dock;
            private set => base.Dock = value;
        }

        [Browsable(false), TypeConverter(typeof(ColorsConverter))]
        public CColores Colores
        {
            get => colores;
            set
            {
                colores.PropertyChanged -= Colors_PropertyChanged;
                colores = value;
                colores.PropertyChanged += Colors_PropertyChanged;
                Invalidate();
            }
        }

        [Category("Apariencia")]
        public Themes.Themes.Tema Tema
        {
            get => tema;
            set
            {
                tema = value;
                Themes.Themes.SetTheme(this, value);
            }
        }

        [Category("Diseño")]
        public ushort Movimientos
        {
            get => movim;
            set
            {
                if (value > 1)
                {
                    movim = value;
                    OnSizeChanged(null);
                }
            }
        }
    }

    //Graficos
    public sealed partial class VScroll
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics G = e.Graphics;
            Pen pe = new Pen(GetColor(), 2);

            G.Clear(BackColor);
            ArrowTop(G, pe);
            ArrowDown(G, pe);
            PaintScroll(G);

            pe.Dispose();
        }

        private void ArrowTop(Graphics G, Pen pe)
        {
            Point[] points = new Point[3]
            {
                new Point(5, 10),
                new Point(9, 6),
                new Point(13, 10)
            };

            G.DrawLines(pe, points);
        }

        private void ArrowDown(Graphics G, Pen pe)
        {
            Point[] points = new Point[3]
            {
                new Point(5, rectDown.Location.Y + 6),
                new Point(9, rectDown.Location.Y + 10),
                new Point(13, rectDown.Location.Y + 6)
            };

            G.DrawLines(pe, points);
        }

        private void PaintScroll(Graphics G)
        {
            SolidBrush br = new SolidBrush(colores.Borde);

            if (into == IntoRect.Scroll)
                br.Color = colores.Focus;

            if (mDown == MDown.Scroll)
                br.Color = colores.MouseEnter;

            G.FillRectangle(br, rectScroll);
            br.Dispose();
        }

        private Color GetColor()
        {
            Color color = colores.VSBtn;

            if (into == IntoRect.Top | into == IntoRect.Down)
                color = colores.VSBtnFocus;

            if (mDown == MDown.Scroll)
                color = colores.MouseEnter;

            return color;
        }
    }

    //Metodos
    public sealed partial class VScroll
    {
        private void Redraw()
        {
            switch (into)
            {
                case IntoRect.None:
                    Invalidate(new Region(rectScroll));
                    break;

                case IntoRect.Top:
                    if (mDown != MDown.Scroll)
                        into = IntoRect.None;
                    Invalidate(rectTop);
                    break;

                case IntoRect.Scroll:
                    if (mDown != MDown.Scroll)
                        into = IntoRect.None;
                    Invalidate(new Region(rectScroll));
                    break;

                case IntoRect.Down:
                    if (mDown != MDown.Scroll)
                        into = IntoRect.None;
                    Invalidate(rectDown);
                    break;
            }
        }

        public void ResetUbiSB() => ubiSB = 0;
    }

    //Eventos
    public sealed partial class VScroll
    {
        protected override void OnMouseMove(MouseEventArgs e)
        {
            ejeYNew = e.Y;

            if (rectTop.Contains(e.Location))
            {
                if (into == IntoRect.Top || mDown == MDown.Scroll)
                    return;
                Redraw();
                into = IntoRect.Top;
                Invalidate(rectTop);
            }
            else if (rectScroll.Contains(e.Location))
            {
                if (into == IntoRect.Scroll)
                    return;

                Redraw();
                into = IntoRect.Scroll;
                Invalidate(new Region(rectScroll));
            }
            else if (rectDown.Contains(e.Location))
            {
                if (into == IntoRect.Down || mDown == MDown.Scroll)
                    return;

                Redraw();
                into = IntoRect.Down;
                Invalidate(rectDown);
            }
            else
            {
                Redraw();
                into = IntoRect.None;
            }
        }

        protected override void OnMouseLeave(EventArgs e) => Redraw();

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ejeY = e.Y;

                if (rectTop.Contains(e.Location))
                {
                    mDown = MDown.Top;
                    Redraw();
                    tm.Start();
                }
                else if (rectScroll.Contains(e.Location))
                {
                    mDown = MDown.Scroll;
                    Redraw();
                    tm.Start();
                }
                else if (rectDown.Contains(e.Location))
                {
                    mDown = MDown.Down;
                    Redraw();
                    tm.Start();
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            mDown = MDown.None;
            tm.Stop();

            if (into == IntoRect.Scroll | into == IntoRect.None)
            {
                Invalidate(new Region(rectScroll));
                rectScroll.Y = btnHg + (ubiSB * despl);
            }

            Redraw();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (into == IntoRect.Top || into == IntoRect.Down)
                return;

            if (e.Delta > 0)
            {
                slice = Slice.Up;
                UpSB();
            }
            else
            {
                slice = Slice.Down;
                DownSB();
            }

            if (rectScroll.Contains(e.Location))
            {
                into = IntoRect.Scroll;
                Invalidate(new Region(rectScroll));
            }
            else
                into = IntoRect.None;
        }

        private void Colors_PropertyChanged(object sender, PropertyChangedEventArgs e) => Invalidate();

        public void ScrollParent(MouseEventArgs e) => OnMouseWheel(e);

        private void Tm_Tick(object sender, EventArgs e)
        {
            switch (mDown)
            {
                case MDown.Top:
                    slice = Slice.Up;
                    UpSB();
                    break;

                case MDown.Scroll:
                    if (ejeY > ejeYNew)
                    {
                        slice = Slice.Up;
                        for (ushort index = 0; index < ejeY - ejeYNew; ++index)
                            SliceScroll();
                    }
                    else if (ejeY < ejeYNew)
                    {
                        slice = Slice.Down;
                        for (ushort index = 0; index < ejeYNew - ejeY; ++index)
                            SliceScroll();
                    }
                    ejeY = ejeYNew;
                    break;

                case MDown.Down:
                    slice = Slice.Down;
                    DownSB();
                    break;
            }
        }

        private void SliceScroll()
        {
            if (Ejecutar(Tipo.Mouse))
            {
                Invalidate(new Region(rectScroll));
                switch (slice)
                {
                    case Slice.Up:
                        --rectScroll.Y;
                        Invalidate(new Region(rectScroll));
                        for (int index = 0; index < rep; ++index)
                            RaiseEventUp();
                        break;

                    case Slice.Down:
                        ++rectScroll.Y;
                        Invalidate(new Region(rectScroll));
                        for (int index = 0; index < rep; ++index)
                            RaiseEventDown();
                        break;
                }
            }
        }

        private void RaiseEventUp()
        {
            if (rectScroll.Y <= btnHg + (ubiSB * despl) - (despl / 2))
            {
                --ubiSB;
                Up(this, EventArgs.Empty);
            }
        }

        private void RaiseEventDown()
        {
            if ((rectScroll.Y + rectScroll.Height) >= btnHg + rectScroll.Height + (ubiSB * despl) + (despl / 2))
            {
                ++ubiSB;
                Down(this, EventArgs.Empty);
            }
        }

        private bool Ejecutar(Tipo tipo)
        {
            switch (tipo)
            {
                case Tipo.Boton:
                    switch (slice)
                    {
                        case Slice.Up:
                            if (ubiSB > 0)
                            {
                                Up(this, EventArgs.Empty);
                                return true;
                            }
                            break;

                        case Slice.Down:
                            if (ubiSB < movim)
                            {
                                Down(this, EventArgs.Empty);
                                return true;
                            }
                            break;
                    }
                    break;

                case Tipo.Mouse:
                    switch (slice)
                    {
                        case Slice.Up:
                            if (rectScroll.Y > (btnHg + 0.5))
                                return true;
                            break;

                        case Slice.Down:
                            if (rectScroll.Y + rectScroll.Height < (Height - btnHg) - 0.5)
                                return true;
                            break;
                    }
                    break;
            }
            return false;
        }

        private void UpSB()
        {
            if (Ejecutar(Tipo.Boton))
            {
                --ubiSB;
                Invalidate(new Region(rectScroll));
                rectScroll.Y -= despl;
                Invalidate(new Region(rectScroll));
            }
            tm.Stop();
        }

        private void DownSB()
        {
            if (Ejecutar(Tipo.Boton))
            {
                ++ubiSB;
                Invalidate(new Region(rectScroll));
                rectScroll.Y += despl;
                Invalidate(new Region(rectScroll));
            }
            tm.Stop();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            despl = ((float)Height - (btnHg * 2)) / (movim + 1);

            if (despl < btnHg)
            {
                rectScroll.Height = btnHg;
                despl = ((float)Height - (btnHg * 3)) / movim;
            }
            else
                rectScroll.Height = despl;

            rep = despl >= 1 ? (byte)1 : (byte)((byte)Math.Truncate(1 / despl) + 1);
            rectDown.Location = new Point(0, Height - btnHg);
            rectScroll.Y = btnHg + (ubiSB * despl);
            Invalidate();
        }
    }

    //Enumeraciones
    public sealed partial class VScroll
    {
        private enum IntoRect
        {
            None,
            Top,
            Scroll,
            Down,
        }

        private enum MDown
        {
            None,
            Top,
            Scroll,
            Down,
        }

        private enum Slice : byte
        {
            Up,
            Down,
        }

        private enum Tipo : byte
        {
            Boton,
            Mouse,
        }
    }
}