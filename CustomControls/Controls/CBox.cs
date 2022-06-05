using CustomControls.Genericos;
using CustomControls.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CustomControls.Controls
{
    //Variables
    [DefaultProperty("Items")]
    public sealed partial class CBox : Control
    {
        private readonly ControlCBox ctrCBox = new ControlCBox();
        private readonly TSCtrHost tSCtrHost;
        private readonly TSMain tSMain;
        private Themes.Themes.Tema tema;
        private Colors colores = new Colors();
        private byte itemsView;
        private byte itemHeight;
        private bool mouseEnter;
        private bool hide = true;
        private bool keyDown;

        private object itemSelected;
        private Style style;

        private TextBox textB;
        private bool auxItemsCero;

        public event EventHandler SelectedItem = (object sender, EventArgs e) => { };
        public event EventHandler SelectedItemNull = (object sender, EventArgs e) => { };
    }

    //Constructor
    public sealed partial class CBox
    {
        public CBox()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            tSCtrHost = new TSCtrHost(ctrCBox);
            tSMain = new TSMain(tSCtrHost);

            Tema = Themes.Themes.Tema.Multicolor;
            Font = Themes.Themes.GetFont();
            Size = new Size(110, 24);
            ItemHeight = 20;
            ItemsView = 5;

            tSMain.VisibleChanged += TSMain_VisibleChanged;
            ctrCBox.PreviewKeyDown += (sender, e) => OnPreviewKeyDown(e);
            ctrCBox.ItemSelect += CtrCBox_ItemSelect;
        }
    }

    //Propiedades
    public sealed partial class CBox
    {
        public new Color BackColor
        {
            get => colores.BackC;
            set => colores.BackC = value;
        }

        public new Color ForeColor
        {
            get => colores.ForeC;
            set => colores.ForeC = value;
        }

        public new Font Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                ctrCBox.Font = value;
            }
        }

        [Category("Apariencia")]
        public Style Estilo
        {
            get => style;
            set
            {
                style = value;
                switch (value)
                {
                    case Style.DropDown:
                        ctrCBox.CBoxStyle = Style.DropDown;
                        ctrCBox.Location = new Point(0, 0);
                        Controls.Clear();
                        if (!(textB is null))
                        {
                            textB.TextChanged -= TextB_TextChanged;
                            textB.LostFocus -= TextB_LostFocus;
                            textB.Dispose();
                        }
                        break;

                    case Style.Filter:
                        ctrCBox.CBoxStyle = Style.Filter;
                        textB = new TextBox()
                        {
                            AutoSize = false,
                            BorderStyle = BorderStyle.None,
                            Size = new Size(Size.Width - 6, Height - 6),
                            Location = new Point(3, 3),
                            BackColor = colores.BackC,
                            Font = Font
                        };
                        textB.TextChanged += TextB_TextChanged;
                        textB.LostFocus += TextB_LostFocus;
                        Controls.AddRange(new Control[] { textB, ctrCBox });
                        break;
                }
            }
        }

        [Category("Datos")]
        public List<object> Items
        {
            get => ctrCBox.Items;
            set => ctrCBox.Items = value;
        }

        [Category("Datos")]
        public byte ItemsView
        {
            get => itemsView;
            set
            {
                itemsView = value;
                ctrCBox.ItemsView = value;
            }
        }

        [Category("Datos")]
        public byte ItemHeight
        {
            get => itemHeight;
            set
            {
                itemHeight = value;
                ctrCBox.ItemHeight = value;
            }
        }

        [Category("Datos")]
        public object DataSource
        {
            set
            {
                if (value.GetType().GetInterface("IEnumerable") != null)
                {
                    Items.Clear();
                    var newList = (IEnumerable<object>)value;
                    foreach (var item in newList)
                        Items.Add(item); 
                }
            }
        }

        [Category("Datos")]
        public string DisplayMember { set => ctrCBox.DisplayMember = value; }

        [Category("Datos")]
        public string ValueMember { set => ctrCBox.ValueMember = value; }

        [Browsable(false)]
        public object ItemSelected
        {
            get => itemSelected;
            set
            {
                if (value == null)
                {
                    itemSelected = null;
                    switch (style)
                    {
                        case Style.DropDown:
                            ctrCBox.ResetIndexItemSelected();
                            Invalidate();
                            break;
                    
                        case Style.Filter:
                            TextBoxText(string.Empty);
                            break;
                    }
                }
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
                ctrCBox.AplicarTema(value, tema);
                tSMain.Borde = colores.MouseEnter;
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
                ctrCBox.AplicarTema(colores, value);
                tSMain.Borde = colores.MouseEnter;
                if (style == Style.Filter)
                {
                    textB.BackColor = colores.BackC;
                    textB.ForeColor = colores.ForeC;
                }
                Invalidate();
            }
        }
    }

    //Graficos
    public sealed partial class CBox
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics G = e.Graphics;
            SolidBrush br = new SolidBrush(colores.ForeC);
            G.Clear(colores.BackC);
            switch (style)
            {
                case Style.DropDown:
                    PaintArrow(G);
                    if (itemSelected != null)
                        PaintItemSelected(G, ref br);
                    break;

                case Style.Filter:
                    if (auxItemsCero)
                    {
                        Rectangle rect = new Rectangle(0, Height - itemHeight, Width, itemHeight);
                        br.Color = Themes.Themes.GetColor(colores.BackC);
                        G.FillRectangle(br, rect);
                        br.Color = colores.ForeC;
                        rect.X = 2;
                        G.DrawString("Sin Resultados...", new Font(Font, FontStyle.Italic), br, rect);
                    }
                    break;
            }
            br.Dispose();
        }

        private void PaintArrow(Graphics G)
        {
            Pen pen = new Pen(GetColor(), 2);
            Point[] points = new Point[3]
            {
                new Point(Width - 17, 8),
                new Point(Width - 12, 13),
                new Point(Width - 7, 8)
            };

            G.DrawLines(pen, points);
            pen.Dispose();
        }

        private void PaintItemSelected(Graphics G, ref SolidBrush br)
        {
            G.DrawString(ctrCBox.GetDisplayMember(itemSelected), Font, br, 2, 2);
        }

        private Color GetColor()
        {
            if (mouseEnter) { return colores.MouseEnter; }

            return Focused ? colores.Focus : colores.Borde;
        }
    }

    //Eventos
    public sealed partial class CBox
    {
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (style == Style.DropDown && e.Button == MouseButtons.Left)
            {
                if (mouseEnter && hide)
                    Desplegar();
                else if (mouseEnter && !hide)
                    hide = true;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            mouseEnter = true;
            if (!tSMain.Visible) { Invalidate(); }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            mouseEnter = false;
            if (!tSMain.Visible) { Invalidate(); }
        }

        private void TSMain_VisibleChanged(object sender, EventArgs e)
        {
            if (!tSMain.Visible)
            {
                if (keyDown)
                    keyDown = false;
                else
                {
                    if (mouseEnter)
                        hide = false;
                    else
                        hide = true;
                }
            }
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            if (style == Style.DropDown)
            {
                switch (e.KeyCode)
                {
                    case Keys.Down:
                        if (e.Alt)
                            Desplegar();
                        break;

                    case Keys.LWin:
                    case Keys.Menu:
                    case Keys.ControlKey:
                    case Keys.Escape:
                    case Keys.F10:
                    case Keys.Left:
                    case Keys.Right:
                        keyDown = true;
                        break;
                }
            }
        }

        private void CtrCBox_ItemSelect(object sender, EventArgs e)
        {
            itemSelected = sender;
            SelectedItem(sender, EventArgs.Empty);
            if (style == Style.DropDown)
            {
                tSMain.Close();
                hide = true;
                Invalidate();
            }
            else
            {
                TextBoxText(ctrCBox.GetDisplayMember(itemSelected));
                ctrCBox.ItemsDraw.Clear();
                Height = textB.Height + 6;
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (style == Style.DropDown)
                Invalidate();
            else
                textB.Select();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (style == Style.DropDown)
                Invalidate();
            else
                LostFocusControl();
        }

        private void TextB_LostFocus(object sender, EventArgs e) => LostFocusControl() ;

        protected override void OnSizeChanged(EventArgs e) 
        {
            ctrCBox.Width = Width;
            if (style == Style.Filter)
                textB.Size = new Size(Size.Width - 6, textB.Height);
        }

        private void Colors_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string pN = e.PropertyName;
            if (pN == Colors.Propiedades.BackC.ToString() | pN == Colors.Propiedades.ForeC.ToString() | pN == Colors.Propiedades.Borde.ToString())
            {
                Invalidate();
            }
            //cCB.AplicarTema(Colores, tema);
            //tSDDown.Borde = colores.MouseEnter;
        }

        private void TextB_TextChanged(object sender, EventArgs e)
        {
            itemSelected = null;

            if (textB.Text == "")
            {
                ctrCBox.ItemsDraw.Clear();
                Height = textB.Height + 6;
                SelectedItemNull(null, EventArgs.Empty);
            }
            else
            {
                BringToFront();
                auxItemsCero = false;
                ctrCBox.ResetItemFirst();
                ctrCBox.ItemsDraw.Clear();

                string str;
                foreach (object item in ctrCBox.ItemsAux)
                {
                    str = ctrCBox.GetDisplayMember(item).ToLower();
                    if (str.Contains(textB.Text.ToLower()))
                        ctrCBox.ItemsDraw.Add(item);
                }

                if (ctrCBox.ItemsDraw.Count == 0)
                {
                    auxItemsCero = true;
                    Height = textB.Height + 6 + itemHeight;
                }
                else if (ctrCBox.ItemsDraw.Count == 1)
                {
                    CtrCBox_ItemSelect(ctrCBox.ItemsDraw[0], EventArgs.Empty);
                }
                else if (ctrCBox.ItemsDraw.Count < itemsView)
                    Height = textB.Height + 6 + (itemHeight * ctrCBox.ItemsDraw.Count);
                else
                    Height = textB.Height + 6 + (itemHeight * itemsView);

                ctrCBox.AjustarControl();
                ctrCBox.Location = new Point(0, textB.Height + 6);
                ctrCBox.Invalidate();
            }
        }
    }

    //Metodos
    public sealed partial class CBox
    {
        private void Desplegar()
        {
            Focus();
            ctrCBox.AjustarControl();
            tSMain.Show(this, new Point(0, Height));
            ctrCBox.Focus();
        }

        private void TextBoxText(string text)
        {
            textB.TextChanged -= TextB_TextChanged;
            textB.Text = text;
            textB.TextChanged += TextB_TextChanged;
        }

        private void LostFocusControl()
        {
            if (itemSelected == null)
                textB.Text = "";
        }
    }

    //Enumeraciones
    public sealed partial class CBox
    {
        public enum Style : byte
        {
            DropDown,
            Filter
        }
    }
}