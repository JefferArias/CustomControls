using CustomControls.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CustomControls.Controls
{
    //Variables
    [DefaultProperty("BackColor")]
    public sealed partial class TabC : TabControl
    {
        private Color backC;
        private Color tabsBaseC;
        private Color tabActBackC;
        private Color tabInacBackC;
        private Color tabInacForetC;
        private Color tabActSelectC;
        private readonly List<TabPage> hiddenTabPages = new List<TabPage>();
        private Themes.Themes.Tema tema;
    }

    //Constructor
    public sealed partial class TabC
    {
        public TabC()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            Tema = Themes.Themes.Tema.Multicolor;
            SizeMode = TabSizeMode.Fixed;
            ItemSize = new Size(100, 25);
            Size = new Size(350, 250);
        }
    }

    //Propiedades
    public sealed partial class TabC
    {
        [Category("AD_Apariencia"), Description("Color de fondo del control.")]
        public new Color BackColor
        {
            get => backC;
            set
            {
                backC = value;
                Invalidate();
            }
        }

        [Category("AD_Apariencia"), Description("Color del fondo donde se apoyan las pestañas."), DisplayName("Tabs BaseColor")]
        public Color TabsBaseColor
        {
            get => tabsBaseC;
            set
            {
                tabsBaseC = value;
                Invalidate();
            }
        }

        [Category("AD_Apariencia"), Description("BackColor de la pestaña activa."), DisplayName("Tab Activa")]
        public Color TabActBackColor
        {
            get => tabActBackC;
            set
            {
                tabActBackC = value;
                Invalidate();
            }
        }

        [Category("AD_Apariencia"), Description("BackColor de la pestaña inactiva."), DisplayName("Tab Inactiva")]
        public Color TabInacBackColor
        {
            get => tabInacBackC;
            set
            {
                tabInacBackC = value;
                Invalidate();
            }
        }

        [Category("AD_Apariencia"), Description("ForeColor de la pestaña inactiva."), DisplayName("Tab Inactiva FC")]
        public Color TabInacForeColor
        {
            get => tabInacForetC;
            set
            {
                tabInacForetC = value;
                Invalidate();
            }
        }

        [Category("AD_Apariencia"), Description("Color seleccionador de la pestaña activa."), DisplayName("Seleccionador Tab Active")]
        public Color TabActSelectColor
        {
            get => tabActSelectC;
            set
            {
                tabActSelectC = value;
                Invalidate();
            }
        }

        [Category("AD_Apariencia"), Description("Ubicación de las pestañas."), DisplayName("Alineación")]
        public new TabAlignment Alignment
        {
            get => base.Alignment;
            set
            {
                switch (value)
                {
                    case TabAlignment.Top:
                        if (Alignment != TabAlignment.Bottom)
                        {
                            Size itemSize = ItemSize;
                            int height = itemSize.Height;
                            itemSize = ItemSize;
                            int width = itemSize.Width;
                            ItemSize = new Size(height, width);
                            break;
                        }
                        break;

                    case TabAlignment.Bottom:
                        if (Alignment != TabAlignment.Top)
                        {
                            Size itemSize = ItemSize;
                            int height = itemSize.Height;
                            itemSize = ItemSize;
                            int width = itemSize.Width;
                            ItemSize = new Size(height, width);
                            break;
                        }
                        break;

                    case TabAlignment.Left:
                        if (Alignment != TabAlignment.Right)
                        {
                            Size itemSize = ItemSize;
                            int height = itemSize.Height;
                            itemSize = ItemSize;
                            int width = itemSize.Width;
                            ItemSize = new Size(height, width);
                            break;
                        }
                        break;

                    case TabAlignment.Right:
                        if (Alignment != TabAlignment.Left)
                        {
                            Size itemSize = ItemSize;
                            int height = itemSize.Height;
                            itemSize = ItemSize;
                            int width = itemSize.Width;
                            ItemSize = new Size(height, width);
                            break;
                        }
                        break;
                }
                base.Alignment = value;
            }
        }

        [Category("AD_Apariencia")]
        public Themes.Themes.Tema Tema
        {
            get => tema;
            set
            {
                tema = value;
                Themes.Themes.SetTheme(this, value);
            }
        }

        [Browsable(false), Obsolete("Propiedad Obsoleta", true)]
        public new bool Multiline { get; set; }
    }

    //Graficos
    public sealed partial class TabC
    {
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            Graphics G = pevent.Graphics;
            if (TabCount == 0)
            {
                G.Clear(backC);
            }
            else
            {
                Pen pen = new Pen(backC, 8);
                SolidBrush br = new SolidBrush(tabsBaseC);
                G.DrawRectangle(pen, new Rectangle(0, 0, Width, Height));
                switch (Alignment)
                {
                    case TabAlignment.Top:
                        G.FillRectangle(br, new Rectangle(4, 0, Width - 8, ItemSize.Height));
                        break;

                    case TabAlignment.Bottom:
                        G.FillRectangle(br, new Rectangle(4, Height - ItemSize.Height, Width - 8, ItemSize.Height));
                        break;

                    case TabAlignment.Left:
                        G.FillRectangle(br, new Rectangle(0, 0, ItemSize.Height, Height));
                        break;

                    case TabAlignment.Right:
                        Rectangle rect = new Rectangle(Width - ItemSize.Height, 0, ItemSize.Height * TabCount, Height);
                        G.FillRectangle(br, rect);
                        break;
                }
                pen.Dispose();
                br.Dispose();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (TabCount <= 0)
                return;

            Graphics G = e.Graphics;
            SolidBrush br = new SolidBrush(tabInacBackC);
            StringFormat strF = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };
            Rectangle rect;

            for (int i = 0; i <= TabCount - 1; ++i)
            {
                if (i != SelectedIndex)
                {
                    rect = RectTab(i);
                    br.Color = tabInacBackC;
                    G.FillRectangle(br, rect);
                    br.Color = tabInacForetC;
                    PaintImgStr(i, G, br, rect, strF);
                }
            }

            if (SelectedIndex != -1)
            {
                rect = RectTab(SelectedIndex);
                br.Color = tabActBackC;
                G.FillRectangle(br, rect);
                br.Color = ForeColor;
                PaintImgStr(SelectedIndex, G, br, rect, strF);
                Rectangle tabRect = GetTabRect(SelectedIndex);

                switch (Alignment)
                {
                    case TabAlignment.Top:
                        rect = new Rectangle(tabRect.X + 2, 0, 4, tabRect.Height);
                        break;

                    case TabAlignment.Bottom:
                        rect = new Rectangle(tabRect.X + 2, tabRect.Y + 2, 4, tabRect.Height);
                        break;

                    case TabAlignment.Left:
                        rect = new Rectangle(0, tabRect.Y + 2, 4, tabRect.Height);
                        break;

                    case TabAlignment.Right:
                        rect = new Rectangle(tabRect.X + tabRect.Width - 2, tabRect.Y + 2, 4, tabRect.Height);
                        break;
                }
                br.Color = tabActSelectC;
                G.FillRectangle(br, rect);
            }

            switch (Alignment)
            {
                case TabAlignment.Top:
                    rect = new Rectangle(4, ItemSize.Height, Width - 8, 4);
                    break;

                case TabAlignment.Bottom:
                    rect = new Rectangle(4, Height - ItemSize.Height - 4, Width - 8, 4);
                    break;

                case TabAlignment.Left:
                    rect = new Rectangle(ItemSize.Height, 4, 4, Height - 8);
                    break;

                default:
                    rect = new Rectangle(Width - ItemSize.Height - 4, 4, 4, Height - 8);
                    break;
            }

            br.Color = tabActBackC;
            G.FillRectangle(br, rect);
            strF.Dispose();
            br.Dispose();
        }

        private void PaintImgStr(int i, Graphics G, SolidBrush br, Rectangle rect, StringFormat strF)
        {
            if (ImageList != null)
            {
                try
                {
                    if (ImageList.Images[TabPages[i].ImageIndex] != null)
                    {
                        G.DrawImage(ImageList.Images[TabPages[i].ImageIndex], new Point(rect.Location.X + 8, rect.Location.Y + 6));
                        G.DrawString("      " + TabPages[i].Text, Font, br, rect, strF);
                    }
                    else
                        G.DrawString(TabPages[i].Text, Font, br, rect, strF);
                }
                catch
                {
                    G.DrawString(TabPages[i].Text, Font, br, rect, strF);
                }
            }
            else
                G.DrawString(TabPages[i].Text, Font, br, rect, strF);
        }
    }

    //Metodos
    public sealed partial class TabC
    {
        private Rectangle RectTab(int i)
        {
            Rectangle tabRect = GetTabRect(i);
            switch (Alignment)
            {
                case TabAlignment.Top:
                    return new Rectangle(tabRect.Location.X + 2, 0, tabRect.Width, tabRect.Height);

                case TabAlignment.Bottom:
                    return new Rectangle(tabRect.X + 2, tabRect.Y + 2, tabRect.Width, tabRect.Height);

                case TabAlignment.Left:
                    return new Rectangle(0, tabRect.Location.Y + 2, tabRect.Width, tabRect.Height);

                default:
                    return new Rectangle(tabRect.X + 2, tabRect.Y + 2, tabRect.Width, tabRect.Height);
            }
        }

        public void HideTabPage(TabPage tb)
        {
            hiddenTabPages.Add(tb);
            TabPages.Remove(tb);
        }

        public void ShowTabPage(TabPage tb)
        {
            if (!hiddenTabPages.Contains(tb))
                throw new ArgumentException("El objeto TabPage que desea mostrar no se ha ocultado.", new NullReferenceException());

            TabPages.Insert(tb.TabIndex, tb);
            SelectedTab = tb;
            hiddenTabPages.Remove(tb);
        }
    }

    //Eventos
    public sealed partial class TabC
    {
        protected override void OnSelecting(TabControlCancelEventArgs e)
        {
            if (e.TabPageIndex <= -1)
                return;
            if (!TabPages[e.TabPageIndex].Enabled)
                e.Cancel = true;
            else
                base.OnSelecting(e);
        }

        protected override void OnControlAdded(ControlEventArgs e) => e.Control.Padding = new Padding(0);
    }
}