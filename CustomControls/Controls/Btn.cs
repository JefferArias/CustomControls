using CustomControls.Genericos;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CustomControls.Themes;

namespace CustomControls.Controls
{
    //Variables
    public sealed partial class Btn : Control
    {
        private Themes.Themes.Tema tema;
        private Colors colores = new Colors();
        private Image icon;
        private ContentAlignment iconAlign = ContentAlignment.MiddleCenter;
        private ContentAlignment textAlign = ContentAlignment.MiddleCenter;
        private Point textAlignAdjust;
        private Point iconAlignAdjust;
        private Size imageSize = new Size(20, 20);
        private bool iconScaleGray;
        private Estilo style;
        private bool mEL;
        private bool focus;

        public new event EventHandler Click = (sender, e) => { };
    }

    //Constructor
    public sealed partial class Btn
    {
        public Btn()
        {
            Constructor();
            Size = new Size(110, 32);
            Tema = Themes.Themes.Tema.Multicolor;
        }

        public Btn(Themes.Themes.Tema tema, Estilo estilo, Size size)
        {
            Constructor();
            Size = size;
            Tema = tema;
            style = estilo;
        }

        private void Constructor()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            Font = Themes.Themes.GetFont();
        }
    }

    //Propiedades
    public sealed partial class Btn
    {
        [Category("Apariencia")]
        public Image Icon
        {
            get => icon;
            set => icon = value;
        }

        [Category("Apariencia"), DisplayName("IconAlign")]
        public ContentAlignment IconAlign
        {
            get => iconAlign;
            set
            {
                iconAlign = value;
                Invalidate();
            }
        }

        [Category("Apariencia"), DisplayName("IconAlignAdjust")]
        public Point ImageAlignAdjust
        {
            get => iconAlignAdjust;
            set
            {
                iconAlignAdjust = value;
                Invalidate();
            }
        }

        [Category("Apariencia"), DisplayName("IconSize")]
        public Size ImageSize
        {
            get => imageSize;
            set
            {
                imageSize = value;
                Invalidate();
            }
        }

        [Category("Apariencia"), DisplayName("IconScaleGray")]
        public bool IconScaleGray
        {
            get => iconScaleGray;
            set
            {
                iconScaleGray = value;
                Invalidate();
            }
        }


        [Category("Apariencia")]
        public ContentAlignment TextAlign
        {
            get => textAlign;
            set
            {
                textAlign = value;
                Invalidate();
            }
        }

        [Category("Apariencia")]
        public Point TextAlignAdjust
        {
            get => textAlignAdjust;
            set
            {
                textAlignAdjust = value;
                Invalidate();
            }
        }

        [Browsable(false), TypeConverter(typeof(ColorsConverter))]
        public Colors Colores
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
        public Estilo Style
        {
            get => style;
            set
            {
                style = value;
                Invalidate();
            }
        }

        public new string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
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
    }

    //Graficos
    public sealed partial class Btn
    {
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics G = pevent.Graphics;
            G.Clear(BackColor);
            switch (style)
            {
                case Estilo.Linea:
                    PaintLinea(G);
                    break;

                case Estilo.ConBorde:
                    PaintConBorde(G);
                    break;

                case Estilo.Redondo:
                    PaintRedondo(G);
                    break;
            }
            if (Icon != null) { PaintIcon(G); }
            PaintText(G);
        }

        private void PaintLinea(Graphics G)
        {
            Pen pen = GetPen(2);
            Region = new Region(ClientRectangle);
            G.DrawRectangle(pen, new Rectangle(0, Height - 2, Width - 1, 2));
            pen.Dispose();
        }

        private void PaintConBorde(Graphics G)
        {
            Pen pen = GetPen(1);
            Region = new Region(ClientRectangle);
            G.DrawRectangle(pen, new Rectangle(0, 0, Width - 1, Height - 1));
            pen.Dispose();
        }

        private void PaintRedondo(Graphics G)
        {
            GraphicsPath GPath = new GraphicsPath();
            SolidBrush br = new SolidBrush(BackColor);
            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

            GPath.AddEllipse(rect);
            G.FillPath(br, GPath);
            Region = new Region(GPath);
            GPath.AddEllipse(rect);
            Pen pen = GetPen(2);
            G.DrawPath(pen, GPath);

            GPath.Dispose();
            br.Dispose();
            pen.Dispose();
        }

        private void PaintIcon(Graphics G)
        {
            PointF pointF = Pnt(iconAlign, iconAlignAdjust, Icon.Width, Icon.Height);
            
            if (iconScaleGray)
            {
                Bitmap bmp = new Bitmap(icon,30,30);
                int x;
                int y;
                Color newColor;
                Color color;

                if (tema == Themes.Themes.Tema.Oscuro || tema == Themes.Themes.Tema.Negro || tema == Themes.Themes.Tema.Gris)
                    newColor = Color.White;
                else
                    newColor = Color.Black;

                for (x = 0; x < bmp.Width; x++)
                {
                    for (y = 0; y < bmp.Height; y++)
                    {
                        color = Color.FromArgb(bmp.GetPixel(x, y).A, newColor);
                        bmp.SetPixel(x, y, color);
                    }
                }
                G.DrawImage(bmp, pointF.X, pointF.Y, imageSize.Width, imageSize.Height);
                bmp.Dispose();
            }
            else
                G.DrawImage(icon, pointF.X, pointF.Y, imageSize.Width, imageSize.Height);
        }

        private void PaintText(Graphics G)
        {
            SolidBrush br = new SolidBrush(ForeColor);
            float width = G.MeasureString(Text, Font).Width;
            float height = G.MeasureString(Text, Font).Height;
            
            var textos = Text.Split(new char[] { '/' });
            Array.Reverse(textos);
            foreach (var texto in textos)
            {
                G.DrawString(texto, Font, br, Pnt(textAlign, textAlignAdjust, width, height));
                height += height + 5;
            }
            br.Dispose();
        }
    }

    //Metodos
    public sealed partial class Btn
    {
        private Pen GetPen(int wd)
        {
            Pen pen = new Pen(colores.Borde, wd);
            if (mEL)
                pen.Color = colores.MouseEnter;
            else if (focus)
                pen.Color = colores.Focus;

            return pen;
        }

        private PointF Pnt(ContentAlignment alignment, Point alignAdjust, float wd, float hg)
        {
            switch (alignment)
            {
                case ContentAlignment.TopLeft:
                    return new PointF(alignAdjust.X, alignAdjust.Y);

                case ContentAlignment.TopCenter:
                    return new PointF((Width / 2) - wd / 2f + alignAdjust.X, alignAdjust.Y);

                case ContentAlignment.TopRight:
                    return new PointF(Width - wd + alignAdjust.X, alignAdjust.Y);

                case ContentAlignment.MiddleLeft:
                    return new PointF(alignAdjust.X, (Height / 2) - hg / 2f + alignAdjust.Y);

                case ContentAlignment.MiddleCenter:
                    return new PointF((Width / 2) - wd / 2f + alignAdjust.X, (Height / 2) - hg / 2f + alignAdjust.Y);

                case ContentAlignment.MiddleRight:
                    return new PointF(Width - wd + alignAdjust.X, (Height / 2) - hg / 2f + alignAdjust.Y);

                case ContentAlignment.BottomLeft:
                    return new PointF(alignAdjust.X, Height - hg + alignAdjust.Y);

                case ContentAlignment.BottomCenter:
                    return new PointF((Width / 2) - wd / 2f + alignAdjust.X, Height - hg + alignAdjust.Y);

                case ContentAlignment.BottomRight:
                    return new PointF(Width - wd + alignAdjust.X, Height - hg + alignAdjust.Y);

                default:
                    return new PointF(0.0f, 0.0f);
            }
        }

        private async void Efecto(MouseEventArgs mevent = null)
        {
            await Task.Run(() =>
            {
                Graphics G = CreateGraphics();
                GraphicsPath GPath = new GraphicsPath();
                SolidBrush br = !mEL ? new SolidBrush(colores.Focus) : new SolidBrush(colores.MouseEnter);
                int x;
                int y;
                if (mevent != null)
                {
                    x = mevent.Location.X - 8;
                    y = mevent.Location.Y - 8;
                }
                else
                {
                    x = Width / 2 - 8;
                    y = Height / 2 - 8;
                }
                int wd = 16;
                int hg = 8;
                bool dibujar = true;
                do
                {
                    Thread.Sleep(1);
                    Rectangle rect = new Rectangle(x, y, wd, wd);
                    GPath.AddEllipse(rect);
                    G.FillPath(br, GPath);
                    x -= 4;
                    y -= 4;
                    wd += hg;
                    if (wd > Width) { dibujar = false; }
                }
                while (dibujar);

                Invalidate();
                br.Dispose();
                GPath.Dispose();
                G.Dispose();
            });
            Click(this, EventArgs.Empty);
        }

        private void Colors_PropertyChanged(object sender, PropertyChangedEventArgs e) => Invalidate();
    }

    //Eventos
    public sealed partial class Btn
    {
        protected override void OnMouseEnter(EventArgs e)
        {
            mEL = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            mEL = false;
            Invalidate();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            focus = true;
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            focus = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            Focus();
            if (mevent.Button == MouseButtons.Left) { Efecto(mevent); }
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { Efecto(); }
        }
    }

    //Enumeraciones
    public sealed partial class Btn
    {
        public enum Estilo : byte
        {
            Linea,
            ConBorde,
            Redondo,
        }
    }
}