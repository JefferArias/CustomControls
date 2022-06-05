using CustomControls.Controls;
using Microsoft.Win32;
using System.Drawing;
using System.Windows.Forms;

namespace CustomControls.Themes
{
    //Clase Temas
    public sealed partial class Themes
    {
        public static void SetTheme(Control ctr, Tema tema)
        {
            switch (ctr)
            {
                case Btn btn: SetThemeToBtn(btn, GetBtnTxtColors(tema)); break;
                case TBox tBx: SetThemeToTxtBx(tBx, GetBtnTxtColors(tema)); break;
                case VScroll vS: SetThemeToVScroll(vS, GetVScrollColors(tema)); break;
                case CBox cBx: SetThemeToCBox(cBx, GetCBoxColors(tema)); break;
                case GView gView: SetThemeToGView(gView, GetGViewColors(tema)); break;
                case TabC tabC: SetThemeToTabC(tabC, GetTabCColors(tema)); break;
                case InputTxt inpTxt: SetThemeToInputTxt(inpTxt, GetInputTxtMsgColors(tema)); break;
                case Msg msg: SetThemeToMsg(msg, GetInputTxtMsgColors(tema)); break;
                case UserControl uc when uc.GetType().GetInterface("IContainerControl") != null: SetThemeToUserControl(uc, tema); break;
            }
        }

        public static Tema GetThemeExcel()
        {
            var valTheme = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common", "UI Theme", "9");
            switch (valTheme)
            {
                case 0:
                    return Tema.Multicolor;

                case 3:
                    return Tema.Gris;

                case 4:
                    return Tema.Negro;

                case 5:
                    return Tema.Blanco;

                case 6:
                    var valThemeWindows = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", "9");
                    switch (valThemeWindows)
                    {
                        case 0:
                            return Tema.Negro;
                        default:
                            return Tema.Multicolor;
                    }
                                        
                default:
                    return Tema.Negro;

            }
        }

        public static Font GetFont() => new Font("Segoe UI", 10);

        public static Color GetColor(Color color)
        {
            byte[] RGB = new byte[3];
            if (color.ToArgb() < -8355712)  //Gris
            {
                RGB[0] = (byte)(color.R + 10);
                RGB[1] = (byte)(color.G + 10);
                RGB[2] = (byte)(color.B + 10);
            }
            else
            {
                RGB[0] = (byte)(color.R - 10);
                RGB[1] = (byte)(color.G - 10);
                RGB[2] = (byte)(color.B - 10);
            }
            return Color.FromArgb(RGB[0], RGB[1], RGB[2]);
        }

        public enum Tema : byte
        {
            Claro,
            Oscuro,
            Multicolor,
            Gris,
            Negro,
            Blanco,
        }

        private enum Propiedades : byte
        {
            BackC,
            ForeC,
            BordeC,
            FocusC,
            MouseE,
            VSBtn,
            VSBtnFocus,
            CBoxMouseMoveBC,
            CBoxMouseMoveFC,
            CBoxItemSelectBC,
            GVClmnHeadBC,
            GVClmnHeadFC,
            GVRowFC,
            GVSelectRowBC,
            GVSelectRowFC,
            TCTabActBack,
            TCTabInacBack,
            TCTabInacFore,
            TCTabActSelector,
            InputMsgTPnlBtnsBC,
        }
    }

}