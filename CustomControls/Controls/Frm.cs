using CustomControls.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CustomControls.Themes;

namespace CustomControls.Controls
{
    //Variables
    public partial class Frm : Form
    {
        private int attrvalue = 2;

        private WIN.MARGINS margins = new WIN.MARGINS() { bottomHeight = 0, leftWidth = 0, rightWidth = 0, topHeight = 1 };
        private Color tittleBarTemp;
        private Color tittleBar;

        private Rectangle rFrm;
        private Rectangle rControlBox;
        private Rectangle rIcon;
        private Rectangle rTextTittle;
        private Rectangle rMini;
        private Rectangle rMaxi;
        private Rectangle rClose;
        private Rectangle rBtnBack;
        private RActive rMEnter;
        private readonly byte iHg = (byte)Resources.Cerrar.Height;
        private readonly byte iWd = (byte)Resources.Cerrar.Width;

        private bool showIcon = true;
        private FrmBordeStyle formBordeStyle;

        private Point pt;

        private bool controlBox = true;
        private bool minimizeBox = true;
        private bool maximizeBox = true;
        private bool showBtnBack = false;

        private delegate void AdjustChildren();
        private ControlCollection controls;

    }

    //Constructor
    public partial class Frm
    {
        public Frm(IContainerControl controlMain)
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            Padding = new Padding(0, 30, 0, 0);
            base.FormBorderStyle = FormBorderStyle.None;
            MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            MinimumSize = new Size(iWd * 6, iHg * 6);
            TittleBar = Color.RoyalBlue;
            base.Font = Themes.Themes.GetFont();
            StartPosition = FormStartPosition.CenterParent;
            controls = new ControlCollection(this);
            controls.Add((Control)controlMain);
            controls.AddControl += Controls_AddControl;
            controls.RemoveControl += Controls_RemoveControl;
            SetRectangles();
        }

        private void Controls_AddControl(object sender, EventArgs e)
        {
            if (controls.Count > 1)
            {
                rBtnBack.Width = iWd;
                showBtnBack = true;
                OnSizeChanged(EventArgs.Empty);
            }
        }

        private void Controls_RemoveControl(object sender, EventArgs e)
        {
            if (controls.Count == 1)
            {
                rBtnBack.Width = 0;
                showBtnBack = false;
                OnSizeChanged(EventArgs.Empty);
            }
        }

        private void SetRectangles()
        {
            rFrm = new Rectangle(0, 0, Width - 1, Height - 1);
            rControlBox = new Rectangle(rFrm.Width - (iWd * 3) - 1, 1, (iWd * 3), iHg);
            rIcon = new Rectangle(5, 5, 20, 20);
            rClose = new Rectangle(Width - iWd - 1, 1, iWd, iHg);
            rMaxi = new Rectangle(rClose.X - iWd, 1, iWd, iHg);
            rMini = new Rectangle(rMaxi.X - iWd, 1, iWd, iHg);
            rTextTittle = new Rectangle(1, 1, rMini.X - iHg, iHg);
            rBtnBack = new Rectangle(1, 1, iWd, iHg);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= (int)WIN.WS.WS_MINIMIZEBOX;
                return cp;
            }
        }
    }

    //Propiedades
    public partial class Frm
    {
        [Category("Apariencia")]
        public Color TittleBar
        {
            get => tittleBar;
            set
            {
                tittleBar = value;
                tittleBarTemp = value;
                Invalidate(new Rectangle(0, 0, Width - 1, iHg + 2));
            }
        }

        [Browsable(false), Obsolete("Propiedad Obsoleta.", true)]
        public new FormBorderStyle FormBorderStyle { get; set; }

        [Category("Apariencia")]
        public FrmBordeStyle FormBordeStyle
        {
            get => formBordeStyle;
            set => formBordeStyle = value;
        }

        public new ControlCollection Controls { get => controls; set => controls = value; }

        public new bool ShowIcon
        {
            get => showIcon;
            set
            {
                showIcon = value;
                if (value)
                    rIcon.Width = 20;
                else
                    rIcon.Width = 0;
                OnSizeChanged(EventArgs.Empty);
            }
        }

        public new bool MinimizeBox
        {
            get => minimizeBox;
            set
            {
                minimizeBox = value;
                if (value)
                    rMini.Width = iWd;
                else
                    rMini.Width = 0;
                WindowStyle();
                OnSizeChanged(EventArgs.Empty);
            }
        }

        public new bool MaximizeBox
        {
            get => maximizeBox;
            set
            {
                maximizeBox = value;
                if (value)
                    rMaxi.Width = iWd;
                else
                    rMaxi.Width = 0;
                WindowStyle();
                OnSizeChanged(EventArgs.Empty);
            }
        }

        [Category("Estilo de ventana")]
        public new bool ControlBox
        {
            get => controlBox;
            set
            {
                controlBox = value;
                if (value)
                {
                    rControlBox.Width = 0;
                    rMini.Width = iWd;
                    rMaxi.Width = iWd;
                    rClose.Width = iWd;
                }
                else
                {
                    rControlBox.Width = iWd * 3;
                    rMini.Width = 0;
                    rMaxi.Width = 0;
                    rClose.Width = 0;
                }
                WindowStyle();
                OnSizeChanged(EventArgs.Empty);
            }
        }

        public new bool ShowInTaskbar
        {
            get => base.ShowInTaskbar;
            set
            {
                base.ShowInTaskbar = value;
                WindowStyle();
            }
        }
    }

    //Metodos
    public partial class Frm
    {
        public new void Dispose()
        {
            for (int i = 0; i < controls.Count; i++)
                controls[i].Dispose();

            controls.AddControl -= Controls_AddControl;
            controls.RemoveControl -= Controls_RemoveControl;

            base.Dispose();
        }
        private Icon GetIconMaxRes()
        {
            if (WindowState == FormWindowState.Maximized)
                return Resources.Restaurar;
            else
                return Resources.Maximizar;
        }

        private Icon GetIcon()
        {
            if (showIcon)
                return Icon;
            else
                return null;
        }

        private void PaintBT()
        {
            AdjustChildren ac = () =>
            {
                Graphics G = Graphics.FromHwnd(Handle);
                SolidBrush br = new SolidBrush(tittleBar);
                Pen pe = new Pen(tittleBar);

                G.DrawRectangle(pe, new Rectangle(0, 0, Width - 1, iHg + 1));

                G.FillRectangle(br, new Rectangle(1, 1, Width - 2, iHg));

                G.DrawIcon(Resources.Minimizar, rMini);
                G.DrawIcon(GetIconMaxRes(), rMaxi);
                G.DrawIcon(Resources.Cerrar, rClose);

                if (showBtnBack)
                    G.DrawIcon(Resources.Atras, rBtnBack); 
                
                Icon icon = GetIcon();
                if (icon != null)
                    G.DrawIcon(icon, rIcon);
                
                br.Color = Color.White;
                Rectangle rectTemp = rTextTittle;
                rectTemp.Y = 5;
                G.DrawString(Text, Font, br, rectTemp);

                pe.Dispose();
                br.Dispose();
                G.Dispose();
            };
            try { BeginInvoke(ac); }    //En try porque al crear el form genera error
            catch (Exception) { }
        }

        private void PRLeave()
        {
            switch (rMEnter)
            {
                case RActive.Close:
                    PREnter(rClose, Resources.Cerrar, true);
                    break;

                case RActive.Maxi:
                    PREnter(rMaxi, GetIconMaxRes(), true);
                    break;

                case RActive.Mini:
                    PREnter(rMini, Resources.Minimizar, true);
                    break;

                case RActive.Back:
                    PREnter(rBtnBack, Resources.Atras, true);
                    break;
            }
        }

        private void PREnter(Rectangle rect, Icon ico, bool PRleave = false)
        {
            Graphics G = Graphics.FromHwnd(Handle);
            Color color;

            if (PRleave)
                color = tittleBar;
            else
            {
                switch (rMEnter)
                {
                    case RActive.Close:
                        color = Color.FromArgb(232, 17, 35);
                        break;

                    default:
                        color = Color.FromArgb(50, Color.White);
                        break;
                }
            }

            SolidBrush br = new SolidBrush(color);

            G.FillRectangle(br, rect);
            if (ico != null)
                G.DrawIcon(ico, rect);

            br.Dispose();
            G.Dispose();
        }

        private void WindowStyle()
        {
            if (!controlBox)
            {
                WIN.SetWindowLongPtr(Handle, -16, (IntPtr)0x02000000L);
                WIN.SetClassLongPtr(Handle, -26, (IntPtr)0x0200);
            }
            else if (minimizeBox & maximizeBox)
                WIN.SetWindowLongPtr(Handle, -16, (IntPtr)(WIN.WS.WS_MINIMIZEBOX | WIN.WS.WS_MAXIMIZEBOX));
            else if (minimizeBox)
                WIN.SetWindowLongPtr(Handle, -16, (IntPtr)WIN.WS.WS_MINIMIZEBOX);
            else if (maximizeBox)
                WIN.SetWindowLongPtr(Handle, -16, (IntPtr)WIN.WS.WS_MAXIMIZEBOX);
            else
                WIN.SetWindowLongPtr(Handle, -16, (IntPtr)0x02000000L);
        }

        //private Color GetColorTB()
        //{
        //    byte[] RGB = new byte[3];
        //    if (BackColor.ToArgb() < -8355712)  //Gris
        //    {
        //        RGB[0] = (byte)(BackColor.R + 40);
        //        RGB[1] = (byte)(BackColor.G + 40);
        //        RGB[2] = (byte)(BackColor.B + 40);
        //    }
        //    else
        //    {
        //        RGB[0] = (byte)(BackColor.R - 40);
        //        RGB[1] = (byte)(BackColor.G - 40);
        //        RGB[2] = (byte)(BackColor.B - 40);
        //    }
        //    return Color.FromArgb(RGB[0], RGB[1], RGB[2]);
        //}

        private int GET_X_LPARAM(IntPtr lParam)
        {
            return Convert.ToInt32((lParam.ToInt32() & 0xFFFF)); //LOWORD
        }

        private int GET_Y_LPARAM(IntPtr lParam)
        {
            return Convert.ToInt32((lParam.ToInt32() >> 16) & 0xFFFF); //HIWORD;
        }
    }

    //Eventos
    public partial class Frm
    {
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case (int)WIN.WM.NCPAINT:
                    AdjustChildren ac = () =>
                    {
                        WIN.DwmSetWindowAttribute(Handle, 2, ref attrvalue, 4);
                        WIN.DwmExtendFrameIntoClientArea(Handle, ref margins);
                    };
                    try { BeginInvoke(ac); }    //En try porque al crear el form genera error
                    catch (Exception) { }
                    break;

                case (int)WIN.WM.ACTIVATEAPP:
                    if (m.WParam.ToInt32() == 1)
                        tittleBar = tittleBarTemp;
                    else
                        tittleBar = Color.FromArgb(180, 180, 180);
                    PaintBT();
                    break;

                case (int)WIN.WM.PAINT:
                    PaintBT();
                    break;

                case (int)WIN.WM.NCHITTEST:
                    if (WindowState == FormWindowState.Maximized)
                        m.Result = HitTestNCAFrmStyleNone(m.LParam);
                    else
                    {
                        if (formBordeStyle == FrmBordeStyle.None)
                            m.Result = HitTestNCAFrmStyleNone(m.LParam);
                        else
                            m.Result = HitTestNCA(m.LParam);
                    }
                    break;
            }
        }

        private IntPtr HitTestNCA(IntPtr lParam)
        {
            int borde = 2;
            pt = new Point(GET_X_LPARAM(lParam) - Location.X, GET_Y_LPARAM(lParam) - Location.Y);

            if (pt.X <= borde && pt.Y <= borde)                                 // HTTOPLEFT
                return (IntPtr)(int)HT.TOPLEFT;

            else if (pt.X >= Width - borde && pt.Y <= borde)                    // HTTOPRIGHT
                return (IntPtr)(int)HT.TOPRIGHT;

            else if (pt.X <= borde && pt.Y >= Height - borde)                   // HTBOTTOMLEFT
                return (IntPtr)(int)HT.BOTTOMLEFT;

            else if (pt.X >= Width - borde && pt.Y >= Height - borde)           // HTBOTTOMRIGHT
                return (IntPtr)(int)HT.BOTTOMRIGHT;

            else if (pt.Y <= borde)                                             // HTTOP
                return (IntPtr)(int)HT.TOP;

            else if (pt.X <= borde)                                             // HTLEFT
                return (IntPtr)(int)HT.LEFT;

            else if (pt.X >= Width - borde)                                     // HTRIGHT
                return (IntPtr)(int)HT.RIGHT;

            if (pt.Y >= Height - borde)                                         // HTBOTTOM
                return (IntPtr)(int)HT.BOTTOM;

            else if (rTextTittle.Contains(pt))                                  // HTCAPTON
                return (IntPtr)(int)HT.CAPTION;

            else                                                                // HTCLIENT
                return (IntPtr)(int)HT.CLIENT;
        }

        private IntPtr HitTestNCAFrmStyleNone(IntPtr lParam)
        {
            pt = new Point(GET_X_LPARAM(lParam) - Location.X, GET_Y_LPARAM(lParam) - Location.Y);
            if (rTextTittle.Contains(pt))
                return (IntPtr)(int)HT.CAPTION;
            else
                return (IntPtr)(int)HT.CLIENT;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            rFrm.Width = Width - 1;
            rFrm.Height = Height - 1;
            rControlBox.X = rFrm.Width - (iWd * 3);

            rClose.X = Width - iWd - 1;
            rMaxi.X = rClose.X - iWd;
            rMini.X = rMaxi.X - iWd;

            if (controlBox)
            {
                if (maximizeBox | minimizeBox)
                {
                    if (maximizeBox && minimizeBox)
                        AdjustRect(rMini.X);

                    else if (maximizeBox)
                        AdjustRect(rMaxi.X);
                    
                    else if (minimizeBox)
                    {
                        rMini.X = rClose.X - iWd;
                        AdjustRect(rMini.X);
                    }
                }
                else
                    AdjustRect(rClose.X);
            }
            else
                rTextTittle.Width = rClose.X + iWd - 1;
            
            PaintBT();
            controls.AdjustChildren();
        }

        private void AdjustRect(int widthRect)
        {
            if (showBtnBack)
            {
                rTextTittle.Width = widthRect - iWd;
                rTextTittle.X = iWd + 1;
                rIcon.X = iWd + 1;
            }
            else
            {
                rTextTittle.Width = widthRect;
                rTextTittle.X = 1;
                rIcon.X = 5;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (rControlBox.Contains(e.Location))
            {
                if (rClose.Contains(e.Location))
                {
                    if (rMEnter != RActive.Close)
                    {
                        PRLeave();
                        rMEnter = RActive.Close;
                        PREnter(rClose, Resources.Cerrar);
                    }
                }
                else if (rMaxi.Contains(e.Location))
                {
                    if (rMEnter != RActive.Maxi)
                    {
                        PRLeave();
                        rMEnter = RActive.Maxi;
                        PREnter(rMaxi, GetIconMaxRes());
                    }
                }
                else if (rMini.Contains(e.Location))
                {
                    if (rMEnter != RActive.Mini)
                    {
                        PRLeave();
                        rMEnter = RActive.Mini;
                        PREnter(rMini, Resources.Minimizar);
                    }
                }
            }
            else
            {
                if (showBtnBack)
                {
                    if (rBtnBack.Contains(e.Location))
                    {
                        if (rMEnter != RActive.Back)
                        {
                            PRLeave();
                            rMEnter = RActive.Back;
                            PREnter(rBtnBack, Resources.Atras);
                        }
                    }
                }
                else
                    MouseNothing();
            }
        }

        protected override void OnMouseLeave(EventArgs e) => MouseNothing();
        
        private void MouseNothing()
        {
            if (rMEnter != RActive.None)
            {
                PRLeave();
                rMEnter = RActive.None;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (rClose.Contains(e.Location))
                Close();
            else if (rMaxi.Contains(e.Location))
            {
                if (WindowState == FormWindowState.Maximized)
                    WindowState = FormWindowState.Normal;
                else
                    WindowState = FormWindowState.Maximized;
            }
            else if (rMini.Contains(e.Location))
            {
                if (WindowState == FormWindowState.Maximized)
                    WindowState = FormWindowState.Normal;
                else
                {
                    PRLeave();
                    WindowState = FormWindowState.Minimized;
                }
            }
            else if (rBtnBack.Contains(e.Location))
            {
                controls.Remove(controls.ActiveControl);
            }

        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            SolidBrush br = new SolidBrush(BackColor);
            e.Graphics.FillRectangle(br, new Rectangle(0, iHg + 2, Width, Height - iHg - 2));
            br.Dispose();
        }
    }

    //Enumeraciones
    public partial class Frm
    {
        private enum RActive
        {
            None,
            Close,
            Maxi,
            Mini,
            Back
        }

        public enum FrmBordeStyle
        {
            None,
            Sizable
        }

        private enum HT : byte
        {
            CLIENT = 1,
            CAPTION = 2,
            LEFT = 10,
            RIGHT = 11,
            TOP = 12,
            TOPLEFT = 13,
            TOPRIGHT = 14,
            BOTTOM = 15,
            BOTTOMLEFT = 16,
            BOTTOMRIGHT = 17
        }
    }
}