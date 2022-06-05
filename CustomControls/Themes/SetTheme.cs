using CustomControls.Controls;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;

namespace CustomControls.Themes
{
    public sealed partial class Themes
    {
        private static void SetThemeToBtn(Btn btn, Dictionary<Propiedades, Color> colores)
        {
            btn.BackColor = colores[Propiedades.BackC];
            btn.ForeColor = colores[Propiedades.ForeC];
            btn.Colores.Borde = colores[Propiedades.BordeC];
            btn.Colores.Focus = colores[Propiedades.FocusC];
            btn.Colores.MouseEnter = colores[Propiedades.MouseE];
        }

        private static void SetThemeToTxtBx(TBox txtBx, Dictionary<Propiedades, Color> colores)
        {
            txtBx.BackColor = colores[Propiedades.BackC];
            txtBx.ForeColor = colores[Propiedades.ForeC];
            txtBx.Colores.Borde = colores[Propiedades.BordeC];
            txtBx.Colores.Focus = colores[Propiedades.FocusC];
            txtBx.Colores.MouseEnter = colores[Propiedades.MouseE];
        }

        private static void SetThemeToVScroll(VScroll vscroll, Dictionary<Propiedades, Color> colores)
        {
            vscroll.BackColor = colores[Propiedades.BackC];
            vscroll.Colores.Borde = colores[Propiedades.BordeC];
            vscroll.Colores.Focus = colores[Propiedades.FocusC];
            vscroll.Colores.MouseEnter = colores[Propiedades.MouseE];
            vscroll.Colores.VSBtn = colores[Propiedades.VSBtn];
            vscroll.Colores.VSBtnFocus = colores[Propiedades.VSBtnFocus];
        }

        private static void SetThemeToCBox(CBox cBox, Dictionary<Propiedades, Color> colores)
        {
            cBox.Colores.BackC = colores[Propiedades.BackC];
            cBox.Colores.ForeC = colores[Propiedades.ForeC];
            cBox.Colores.Borde = colores[Propiedades.BordeC];
            cBox.Colores.Focus = colores[Propiedades.FocusC];
            cBox.Colores.MouseEnter = colores[Propiedades.MouseE];
            cBox.Colores.MouseMoveBC = colores[Propiedades.CBoxMouseMoveBC];
            cBox.Colores.MouseMoveFC = colores[Propiedades.CBoxMouseMoveFC];
            cBox.Colores.SelectItemBC = colores[Propiedades.CBoxItemSelectBC];
        }

        private static void SetThemeToGView(GView gView, Dictionary<Propiedades, Color> colores)
        {
            gView.BackgroundColor = colores[Propiedades.BackC];
            gView.GridColor = colores[Propiedades.BordeC];
            gView.ColumnHeadersDefaultCellStyle.BackColor = colores[Propiedades.GVClmnHeadBC];
            gView.ColumnHeadersDefaultCellStyle.ForeColor = colores[Propiedades.GVClmnHeadFC];
            gView.ColumnHeadersDefaultCellStyle.SelectionBackColor = colores[Propiedades.GVClmnHeadBC];
            gView.ColumnHeadersDefaultCellStyle.SelectionForeColor = colores[Propiedades.GVClmnHeadFC];
            gView.ColumnHeadersDefaultCellStyle.Font = new Font(GetFont(), FontStyle.Bold);
            gView.RowsDefaultCellStyle.Font = GetFont();
            gView.RowsDefaultCellStyle.BackColor = colores[Propiedades.BackC];
            gView.RowsDefaultCellStyle.ForeColor = colores[Propiedades.GVRowFC];
            gView.RowsDefaultCellStyle.SelectionBackColor = colores[Propiedades.GVSelectRowBC];
            gView.RowsDefaultCellStyle.SelectionForeColor = colores[Propiedades.GVSelectRowFC];
        }

        private static void SetThemeToTabC(TabC tabC, Dictionary<Propiedades, Color> colores)
        {
            tabC.BackColor = colores[Propiedades.BackC];
            tabC.ForeColor = colores[Propiedades.ForeC];
            tabC.Font = GetFont();
            tabC.TabsBaseColor = colores[Propiedades.BackC];
            tabC.TabActBackColor = colores[Propiedades.TCTabActBack];
            tabC.TabInacBackColor = colores[Propiedades.TCTabInacBack];
            tabC.TabInacForeColor = colores[Propiedades.TCTabInacFore];
            tabC.TabActSelectColor = colores[Propiedades.TCTabActSelector];
        }

        private static void SetThemeToInputTxt(InputTxt input, Dictionary<Propiedades, Color> colores)
        {
            input.BackColor = colores[Propiedades.BackC];
            input.ForeColor = colores[Propiedades.ForeC];
            input.Borde = colores[Propiedades.BordeC];
            input.PnlBtnsBC = colores[Propiedades.InputMsgTPnlBtnsBC];
        }

        private static void SetThemeToMsg(Msg msg, Dictionary<Propiedades, Color> colores)
        {
            msg.BackColor = colores[Propiedades.BackC];
            msg.ForeColor = colores[Propiedades.ForeC];
            msg.Borde = colores[Propiedades.BordeC];
            msg.PnlBtnsBC = colores[Propiedades.InputMsgTPnlBtnsBC];
        }

        private static void SetThemeToUserControl(UserControl uc, Tema tema)
        {
            switch (tema)
            {
                case Tema.Multicolor: uc.BackColor = Color.FromArgb(230, 230, 230); break;
                case Tema.Gris: uc.BackColor = Color.FromArgb(102, 102, 102); break;
                case Tema.Negro: uc.BackColor = Color.FromArgb(38, 38, 38); break;
                case Tema.Blanco: uc.BackColor = Color.White; break;
            }

            PropertyInfo pi;
            foreach (Control item in uc.Controls)
            {
                pi = item.GetType().GetProperty("Tema");
                if (pi != null)
                    pi.SetValue(item, tema);
                else
                {
                    if (!IconPictureBox(pi, item, tema))
                    {
                        IconButton(pi, item, tema);
                        pi = item.GetType().GetProperty("BackColor");
                        pi.SetValue(item, GetColor(uc.BackColor));
                    }
                }
            }
        }

        private static bool IconPictureBox(PropertyInfo pi, Control item, Tema tema)
        {
            pi = item.GetType().GetProperty("UseGdi");
            if (pi != null)
            {
                pi = item.GetType().GetProperty("IconColor");
                switch (tema)
                {
                    case Tema.Multicolor:
                    case Tema.Blanco:
                        pi.SetValue(item, Color.Black);
                        break;

                    case Tema.Gris:
                    case Tema.Negro:
                        pi.SetValue(item, Color.White);
                        break;
                }
                return true;
            }
            return false;
        }

        private static void IconButton(PropertyInfo pi, Control item, Tema tema)
        {
            pi = item.GetType().GetProperty("IconColor");
            if (pi != null)
            {
                switch (tema)
                {
                    case Tema.Multicolor:
                    case Tema.Blanco:
                        pi.SetValue(item, Color.Black);
                        break;

                    case Tema.Gris:
                    case Tema.Negro:
                        pi.SetValue(item, Color.White);
                        break;
                }
            }
        }

    }
}
