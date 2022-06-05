using CustomControls.Genericos;
using CustomControls.Themes;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CustomControls.Controls
{
    //Variables
    public sealed partial class TBox : Control
    {
        private readonly TextBox tBox = new TextBox();
        private Themes.Themes.Tema tema;
        private Colors colores = new Colors();
        private Estilo style;
        private bool mEL;
        private bool focus;
        private Filtrar filtro;
        private string filtrar;
    }

    //Constructor
    public sealed partial class TBox
    {
        public TBox()
        {
            Size = new Size(116, 24);
            Tema = Themes.Themes.Tema.Multicolor;

            tBox.AutoSize = false;
            tBox.Font = Themes.Themes.GetFont();
            tBox.BorderStyle = BorderStyle.None;
            tBox.Location = new Point(3, 3);
            tBox.MouseEnter += (sender, e) => OnMouseEnter(e);
            tBox.MouseLeave += (sender, e) => OnMouseLeave(e);
            tBox.GotFocus += (sender, e) => OnGotFocus(e);
            tBox.LostFocus += (sender, e) => OnLostFocus(e);
            tBox.KeyPress += Txt_KeyPress;
            Controls.Add(tBox);
        }
    }

    //Propiedades
    public sealed partial class TBox
    {
        public new Color BackColor
        {
            get => tBox.BackColor;
            set
            {
                tBox.BackColor = value;
                Invalidate();
            }
        }

        public new Color ForeColor
        {
            get => tBox.ForeColor;
            set => tBox.ForeColor = value;
        }

        [Browsable(false)]
        public new string Text
        {
            get => tBox.Text;
            set => tBox.Text = value;
        }

        public new Font Font
        {
            get => tBox.Font;
            set => tBox.Font = value;
        }

        [Category("Datos")]
        public Filtrar Filtro
        {
            get => filtro;
            set
            {
                filtro = value;
                EstablecerFiltro();
            }
        }

        [Category("Datos"), DisplayName("Filtro Personalizado")]
        public string FiltroCustom { get; set; }

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
    public sealed partial class TBox
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            switch (style)
            {
                case Estilo.Linea:
                    PaintLinea(e.Graphics);
                    break;

                case Estilo.ConBorde:
                    PaintConBorde(e.Graphics);
                    break;
            }
        }

        private void PaintLinea(Graphics G)
        {
            Pen pen = new Pen(tBox.BackColor, 5);
            G.DrawRectangle(pen, new Rectangle(0, 0, Width - 1, Height - 1));

            if (mEL)
                pen.Color = colores.MouseEnter;
            else if (focus)
                pen.Color = colores.Focus;
            else
                pen.Color = colores.Borde;

            pen.Width = 2;
            G.DrawRectangle(pen, new Rectangle(0, Height - 2, Width - 1, 2));
            pen.Dispose();
        }

        private void PaintConBorde(Graphics G)
        {
            Pen pen = new Pen(colores.Borde);

            G.DrawRectangle(pen, new Rectangle(0, 0, Width - 1, Height - 1));
            pen.Color = tBox.BackColor;
            pen.Width = 2;
            G.DrawRectangle(pen, new Rectangle(2, 2, Width - 4, Height - 4));

            pen.Dispose();
        }
    }

    //Metodos
    public sealed partial class TBox
    {
        private void EstablecerFiltro()
        {
            FiltroCustom = string.Empty;
            switch (filtro)
            {
                case Filtrar.SinFiltro:
                    filtrar = string.Empty;
                    break;

                case Filtrar.SoloNumeros:
                    filtrar = "abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ<>ºª\\!¡|\"@·#$~%€&¬/()=?¿+]*`[^´¨{çÇ}-_.:,;' ";
                    break;

                case Filtrar.SoloLetras:
                    filtrar = "0123456789<>ºª\\!¡|\"@·#$~%€&¬/()=?¿+]*`[^´¨{çÇ}-_.:,;' ";
                    break;

                case Filtrar.LetrasConCaracteres:
                    filtrar = "0123456789";
                    break;
            }
        }
    }

    //Eventos
    public sealed partial class TBox
    {
        protected override void OnMouseEnter(EventArgs e)
        {
            if (mEL) { return; }
            mEL = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (!mEL) { return; }
            mEL = false;
            Invalidate();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (focus) { return; }
            focus = true;
            tBox.Focus();
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (!focus) { return; }
            focus = false;
            Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e) => tBox.Size = new Size(Size.Width - 6, Size.Height - 6);

        private void Colors_PropertyChanged(object sender, PropertyChangedEventArgs e) => Invalidate();

        private void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnKeyPress(e);
            switch (filtro)
            {
                case Filtrar.SoloNumeros:
                case Filtrar.SoloLetras:
                case Filtrar.LetrasConCaracteres:
                    if (filtrar.Contains(e.KeyChar))
                        e.Handled = true;
                    break;

                case Filtrar.Custom:
                    if (FiltroCustom.Contains(e.KeyChar))
                        e.Handled = true;
                    break;
            }
        }
    }

    //Enumeraciones
    public sealed partial class TBox
    {
        public enum Estilo : byte
        {
            Linea,
            ConBorde,
        }

        public enum Filtrar : byte
        {
            SinFiltro,
            SoloNumeros,
            SoloLetras,
            LetrasConCaracteres,
            Custom,
        }
    }
}