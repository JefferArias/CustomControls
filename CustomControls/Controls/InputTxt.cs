using CustomControls.Themes;
using CustomControls.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CustomControls.Controls
{
    //Variables
    public sealed partial class InputTxt : Form
    {
        private Themes.Themes.Tema tema;
        private const byte pnlWdHg = 39;
        private static string answer = null;
        private bool btnCancelar;
        private readonly TBox tBox;
        private Label lblMsg;
    }

    //Constructor
    public sealed partial class InputTxt
    {
        private InputTxt(string mensaje, string titulo, Btns botones, Themes.Themes.Tema tm)
        {
            tBox = new TBox() { Dock = DockStyle.Fill };
            Font = new Font(Themes.Themes.GetFont().Name, 13);
            Padding = new Padding(1, 8, 1, 1);
            MinimumSize = new Size(250, 108);
            Tema = tm;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;

            Controls.AddRange(new Control[4]
            {
             PanelTitulo(titulo),
             PanelIcono(),
             PanelTBox(),
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

        private Panel PanelTitulo(string titulo)
        {
            Panel pnl = new Panel
            {
                Height = (pnlWdHg / 2) + Padding.Top,
                Dock = DockStyle.Top
            };

            Label lbl = new Label
            {
                Font = Font,
                Dock = DockStyle.Fill,
                Text = titulo
            };

            pnl.Controls.Add(lbl);

            return pnl;
        }

        private Panel PanelIcono()
        {
            Panel pnl = new Panel
            {
                Width = pnlWdHg + 9,
                Dock = DockStyle.Left,
                BackgroundImageLayout = ImageLayout.Center,
                BackgroundImage = Resources.Pregunta
            };

            return pnl;
        }

        private Panel PanelTBox()
        {
            Panel pnl = new Panel
            {
                BackColor = PnlBtnsBC,
                Padding = new Padding(pnlWdHg + 10, 8, 1, 8),
                Height = pnlWdHg,
                Dock = DockStyle.Bottom
            };

            pnl.Controls.Add(tBox);
            return pnl;
        }

        private Panel PanelBtns(Btns botones, Themes.Themes.Tema tm)
        {
            FlowLayoutPanel fLP = new FlowLayoutPanel
            {
                Width = 89,
                Dock = DockStyle.Right,
                BackColor = PnlBtnsBC,
                FlowDirection = FlowDirection.TopDown,
            };

            string[] strArray;
            if (botones == Btns.Aceptar)
                strArray = new string[1] { BtnTxt.Aceptar.ToString() };
            else
                strArray = new string[2] { BtnTxt.Aceptar.ToString(), BtnTxt.Cancelar.ToString() };
            for (byte index = 0; index <= strArray.Length - 1; ++index)
            {
                Btn btn = new Btn(tm, Btn.Estilo.Linea, new Size(80, 28)) { Text = strArray[index] };
                btn.Margin = new Padding(5, 1, 1, 1);
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
    public sealed partial class InputTxt
    {
        public Color Borde { get; set; }
        public Color PnlBtnsBC { get; set; }

        public Themes.Themes.Tema Tema
        {
            get => tema;
            set
            {
                tema = value;
                Themes.Themes.SetTheme(this, value);
                Themes.Themes.SetTheme(tBox, value);
            }
        }
    }

    //Graficos
    public sealed partial class InputTxt
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics G = e.Graphics;
            Pen pen = new Pen(Borde);
            SolidBrush br = new SolidBrush(PnlBtnsBC);

            G.Clear(BackColor);
            G.DrawRectangle(pen, new Rectangle(0, 0, Width - 1, Height - 1));
            G.FillRectangle(br, new Rectangle(Width - (pnlWdHg * 2) - 12, 1, Controls[3].Width, 7));

            pen.Dispose();
            br.Dispose();
        }
    }

    //Metodos Show
    public sealed partial class InputTxt
    {
        public static string Show(string mensaje, string titulo, Btns botones, Filtro filtrado = Filtro.SinFiltro, string filtradoCustom = "", Themes.Themes.Tema tm = Themes.Themes.Tema.Multicolor)
        {
            InputTxt input = new InputTxt(mensaje, titulo, botones, tm);
            switch (filtrado)
            {
                case Filtro.SoloNumeros:
                    input.tBox.Filtro = TBox.Filtrar.SoloNumeros;
                    break;

                case Filtro.SoloLetras:
                    input.tBox.Filtro = TBox.Filtrar.SoloLetras;
                    break;

                case Filtro.LetrasConCaracteres:
                    input.tBox.Filtro = TBox.Filtrar.LetrasConCaracteres;
                    break;

                case Filtro.Custom:
                    input.tBox.Filtro = TBox.Filtrar.Custom;
                    input.tBox.FiltroCustom = filtradoCustom;
                    break;
            }
            input.ShowDialog();
            input.Dispose();
            string answerReturn = answer;   // Para que no se pueda cerrar la ventana del mensaje
            answer = null;

            return answerReturn;
        }
    }

    //Metodos
    public sealed partial class InputTxt
    {
        private void Tamaño() => Size = new Size(lblMsg.Width + (pnlWdHg * 4), lblMsg.Height + (pnlWdHg * 2) + Padding.Top);

        private void RedimensionarLbl(byte division) => lblMsg.Size = Dimensiones(lblMsg.Text, division);

        private void Posicion()
        {
            Rectangle wArea = Screen.PrimaryScreen.WorkingArea;
            Location = new Point(wArea.Width / 2 - Width / 2, wArea.Height / 2 - Height / 2);
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
    public sealed partial class InputTxt
    {
        private void Btn_Click(object sender, EventArgs e)
        {
            string ans = ((Btn)sender).Text;

            if (ans == BtnTxt.Aceptar.ToString())
            {
                if (tBox.Text == string.Empty)
                {
                    tBox.Select();
                    return;
                }
                answer = tBox.Text;
            }
            else if (ans == BtnTxt.Cancelar.ToString())
                btnCancelar = true;

            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (answer == null && !btnCancelar)
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
    public sealed partial class InputTxt
    {
        public enum Btns : byte
        {
            Aceptar,
            AceptarCancelar,
        }

        private enum BtnTxt : byte
        {
            Aceptar,
            Cancelar,
        }

        public enum Filtro : byte
        {
            SinFiltro,
            SoloNumeros,
            SoloLetras,
            LetrasConCaracteres,
            Custom,
        }
    }
}