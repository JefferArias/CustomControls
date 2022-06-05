using CustomControls.Themes;
using CustomControls.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CustomControls.Controls
{
    //Variables
    public sealed partial class Msg : Form
    {
        private Themes.Themes.Tema tema;
        private const byte pnlWdHg = 39;
        private static DialogResult answer;
        private Label lblMsg;
    }

    //Constructor
    public sealed partial class Msg
    {
        private Msg(string mensaje, string titulo, Btns botones, Icono icono, Themes.Themes.Tema tm)
        {
            Font = new Font(Themes.Themes.GetFont().Name, 13);
            Padding = new Padding(1, 8, 1, 1);
            MinimumSize = new Size(250, 108);
            Tema = tm;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;

            Controls.AddRange(new Control[3]
            {
                PanelTitulo(titulo),
                PanelIcono(icono),
                PanelBtns(botones, tm)
            });

            Controls.Add(PanelMensaje(mensaje));
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style = 0x0;
                cp.ExStyle = (int)0x00000008L;  //WS_EX_TOPMOST
                cp.ClassStyle = 0x00020000; //CS_DROPSHADOW

                return cp;
            }
        }

        private Panel PanelIcono(Icono icono)
        {
            Panel pnlIcon = new Panel() { Width = pnlWdHg + (pnlWdHg / 4), Dock = DockStyle.Left, BackgroundImageLayout = ImageLayout.Center };
            switch (icono)
            {
                case Icono.Error:
                    {
                        pnlIcon.BackgroundImage = Resources.Error;
                        break;
                    }

                case Icono.Exclamacion:
                    {
                        pnlIcon.BackgroundImage = Resources.Exclamacion;
                        break;
                    }

                case Icono.Informacion:
                    {
                        pnlIcon.BackgroundImage = Resources.Informacion;
                        break;
                    }

                case Icono.Pregunta:
                    {
                        pnlIcon.BackgroundImage = Resources.Pregunta;
                        break;
                    }
            }
            return pnlIcon;
        }

        private Panel PanelTitulo(string titulo)
        {
            Panel pnlTitulo = new Panel() { Height = (pnlWdHg / 2) + Padding.Top, Dock = DockStyle.Top };
            Label lblTitulo = new Label() { Font = Font, Dock = DockStyle.Fill, Text = titulo };
            pnlTitulo.Controls.Add(lblTitulo);
            return pnlTitulo;
        }

        private Panel PanelBtns(Btns botones, Themes.Themes.Tema tm)
        {
            FlowLayoutPanel fLP = new FlowLayoutPanel()
            {
                BackColor = PnlBtnsBC,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(1),
                Height = pnlWdHg,
                Dock = DockStyle.Bottom
            };
            string[] listBtns;
            switch (botones)
            {
                case Btns.SíNo:
                    listBtns = new[] { BtnTxt.Sí.ToString(), BtnTxt.No.ToString() };
                    break;

                default:
                    listBtns = new[] { BtnTxt.Aceptar.ToString() };
                    break;
            }

            for (byte x = 0; x <= listBtns.Count() - 1; x++)
            {
                Btn btn = new Btn(tm, Btn.Estilo.Linea, new Size(80, 28)) { Text = listBtns[x] };
                btn.Margin = new Padding(2, 5, 1, 1);
                btn.Click += Btn_Click;
                fLP.Controls.Add(btn);
            }
            return fLP;
        }

        private Label PanelMensaje(string mensaje)
        {
            Font = Themes.Themes.GetFont();
            lblMsg = new Label
            {
                Location = new Point(Controls[1].Width + Padding.Left, Controls[0].Height + Padding.Top),
                Padding = new Padding(0),
                Margin = new Padding(0),
                Text = mensaje
            };

            lblMsg.Size = Dimensiones(mensaje, 4);
            return lblMsg;
        }
    }

    //Propiedades
    public sealed partial class Msg
    {
        [Category("Apariencia"), Description("Obtiene o establece el color del borde.")]
        public Color Borde { get; set; }

        [Category("Apariencia"), Description("BackColor panel botones.")]
        public Color PnlBtnsBC { get; set; }

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
    public sealed partial class Msg
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics G = e.Graphics;
            Pen pe = new Pen(Borde);

            G.Clear(BackColor);
            G.DrawRectangle(pe, new Rectangle(0, 0, Width - 1, Height - 1));

            pe.Dispose();
        }
    }

    //Metodos Show
    public sealed partial class Msg
    {
        public static DialogResult Show(string mensaje)
        {
            Msg msg = new Msg(mensaje, "", Btns.Aceptar, Icono.Informacion, Themes.Themes.GetThemeExcel());
            msg.ShowDialog();
            msg.Dispose();

            DialogResult answerReturn = answer;  // Para que no se pueda cerrar la ventana del mensaje
            answer = DialogResult.None;

            return answerReturn;
        }

        public static DialogResult Show(string mensaje, string titulo)
        {
            Msg msg = new Msg(mensaje, titulo, Btns.Aceptar, Icono.Informacion, Themes.Themes.GetThemeExcel());
            msg.ShowDialog();
            msg.Dispose();

            DialogResult answerReturn = answer;  // Para que no se pueda cerrar la ventana del mensaje
            answer = DialogResult.None;

            return answerReturn;
        }

        public static DialogResult Show(string mensaje, string titulo, Btns botones)
        {
            Msg msg = new Msg(mensaje, titulo, botones, Icono.Informacion, Themes.Themes.GetThemeExcel());
            msg.ShowDialog();
            msg.Dispose();

            DialogResult answerReturn = answer;  // Para que no se pueda cerrar la ventana del mensaje
            answer = DialogResult.None;

            return answerReturn;
        }

        public static DialogResult Show(string mensaje, string titulo, Btns botones, Icono icono)
        {
            Msg msg = new Msg(mensaje, titulo, botones, icono, Themes.Themes.GetThemeExcel());
            msg.ShowDialog();
            msg.Dispose();

            DialogResult answerReturn = answer;  // Para que no se pueda cerrar la ventana del mensaje
            answer = DialogResult.None;

            return answerReturn;
        }

        public static DialogResult Show(string mensaje, string titulo, Btns botones, Icono icono, Themes.Themes.Tema tm)
        {
            Msg msg = new Msg(mensaje, titulo, botones, icono, tm);
            msg.ShowDialog();
            msg.Dispose();

            DialogResult answerReturn = answer;  // Para que no se pueda cerrar la ventana del mensaje
            answer = DialogResult.None;

            return answerReturn;
        }
    }

    //Metodos
    public sealed partial class Msg
    {
        private void Tamaño() => Size = new Size(lblMsg.Width + (pnlWdHg * 2), lblMsg.Height + (pnlWdHg * 2) + Padding.Top);

        private void RedimensionarLbl(byte division) => lblMsg.Size = Dimensiones(lblMsg.Text, division);

        private void Posicion()
        {
            Rectangle wArea = Screen.PrimaryScreen.WorkingArea;
            Location = new Point((wArea.Width / 2) - (Width / 2), (wArea.Height / 2) - (Height / 2));
        }

        private Size Dimensiones(string mensaje, byte division)
        {
            int lineas = 0;
            List<float> list = new List<float>();
            int szWd = division != 1 ? (int)Math.Truncate((float)(Screen.PrimaryScreen.WorkingArea.Width / division) - 8) : (int)Math.Truncate((float)Screen.PrimaryScreen.WorkingArea.Width - (pnlWdHg * 2) - 8);
            Graphics G = CreateGraphics();

            if (mensaje.Contains('\n'))
            {
                string[] strArray = mensaje.Split('\n');
                for (int index = 0; index <= strArray.Length - 1; ++index)
                {
                    SeparacionEspacio(strArray[index].Split(null), szWd, list, ref lineas, ref G);
                    ++lineas;
                }
            }
            else
            {
                lineas = 1;
                SeparacionEspacio(mensaje.Split(null), szWd, list, ref lineas, ref G);
            }

            int height = (int)G.MeasureString("Hg", Font).Height;

            G.Dispose();

            return new Size(list.Max() + (pnlWdHg * 2) >= MinimumSize.Width ? (int)list.Max() : MinimumSize.Width - (pnlWdHg * 2), lineas * height);
        }

        private void SeparacionEspacio(string[] arraySpace, int szWd, List<float> listWd, ref int lineas, ref Graphics G)
        {
            float num = 0;

            for (int index = 0; index <= arraySpace.Length - 1; ++index)
            {
                string text = arraySpace[index];
                num += G.MeasureString(text, Font).Width;

                if (num > szWd)
                {
                    float width = G.MeasureString(text, Font).Width;
                    listWd.Add(num - width);
                    num = width;
                    ++lineas;
                }
                else
                    listWd.Add(num);
            }
        }
    }

    //Eventos
    public sealed partial class Msg
    {
        private void Btn_Click(object sender, EventArgs e)
        {
            string ans = ((Btn)sender).Text;

            if (ans == BtnTxt.Sí.ToString())
                answer = DialogResult.Yes;
            else if (ans == BtnTxt.No.ToString())
                answer = DialogResult.No;
            else if (ans == BtnTxt.Aceptar.ToString())
                answer = DialogResult.OK;

            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (answer == DialogResult.None)
                e.Cancel = true;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
            {
                Tamaño();
                if (Size.Height > Screen.PrimaryScreen.WorkingArea.Height)
                {
                    RedimensionarLbl(2);
                    Tamaño();
                    Posicion();
                    if (Location.Y < 0)
                    {
                        RedimensionarLbl(1);
                        Tamaño();
                        Posicion();
                        if (Location.X <= 0)
                        {
                            Rectangle wArea = Screen.PrimaryScreen.WorkingArea;
                            Size = new Size(wArea.Width, wArea.Height);
                            Location = new Point(0, 0);
                            lblMsg.Size = new Size(wArea.Width - (pnlWdHg * 4), wArea.Height - (pnlWdHg * 2));
                        }
                    }
                }
                else
                    Posicion();
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0127:   //WM_CHANGEUISTATE
                case 0x0128:   //WM_UPDATEUISTATE
                    return;
            }
            base.WndProc(ref m);
        }
    }

    //Enumeraciones
    public sealed partial class Msg
    {
        public enum Icono : byte
        {
            Error,
            Exclamacion,
            Informacion,
            Pregunta,
        }

        public enum Btns : byte
        {
            SíNo,
            Aceptar,
        }

        private enum BtnTxt : byte
        {
            Sí,
            No,
            Aceptar,
        }
    }
}