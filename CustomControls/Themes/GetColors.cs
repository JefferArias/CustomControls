using System.Collections.Generic;
using System.Drawing;

namespace CustomControls.Themes
{
    public sealed partial class Themes
    {
        private static Dictionary<Propiedades, Color> GetBtnTxtColors(Tema tema)
        {
            Dictionary<Propiedades, Color> colors = new Dictionary<Propiedades, Color>();
            switch (tema)
            {
                case Tema.Claro:
                    colors.Add(Propiedades.BackC, Color.FromArgb(210, 210, 210));
                    colors.Add(Propiedades.ForeC, Color.Black);
                    colors.Add(Propiedades.BordeC, Color.RoyalBlue);
                    colors.Add(Propiedades.FocusC, Color.CornflowerBlue);
                    colors.Add(Propiedades.MouseE, Color.Blue);
                    break;

                case Tema.Oscuro:
                    colors.Add(Propiedades.BackC, Color.FromArgb(90, 90, 90));
                    colors.Add(Propiedades.ForeC, Color.Gainsboro);
                    colors.Add(Propiedades.BordeC, Color.RoyalBlue);
                    colors.Add(Propiedades.FocusC, Color.CornflowerBlue);
                    colors.Add(Propiedades.MouseE, Color.DodgerBlue);
                    break;

                case Tema.Multicolor:
                    colors.Add(Propiedades.BackC, Color.FromArgb(220, 220, 220));
                    colors.Add(Propiedades.ForeC, Color.Black);
                    colors.Add(Propiedades.BordeC, Color.FromArgb(33, 115, 70));
                    colors.Add(Propiedades.FocusC, Color.FromArgb(63, 145, 100));
                    colors.Add(Propiedades.MouseE, Color.FromArgb(93, 175, 130));
                    break;

                case Tema.Gris:
                    colors.Add(Propiedades.BackC, Color.FromArgb(90, 90, 90));
                    colors.Add(Propiedades.ForeC, Color.White);
                    colors.Add(Propiedades.BordeC, Color.SeaGreen);
                    colors.Add(Propiedades.FocusC, Color.FromArgb(58, 180, 85));
                    colors.Add(Propiedades.MouseE, Color.FromArgb(83, 205, 110));
                    break;

                case Tema.Negro:
                    colors.Add(Propiedades.BackC, Color.FromArgb(54, 54, 54));
                    colors.Add(Propiedades.ForeC, Color.White);
                    colors.Add(Propiedades.BordeC, Color.Indigo);
                    colors.Add(Propiedades.FocusC, Color.FromArgb(100, 25, 155));
                    colors.Add(Propiedades.MouseE, Color.FromArgb(125, 50, 180));
                    break;

                case Tema.Blanco:
                    colors.Add(Propiedades.BackC, Color.FromArgb(230, 230, 230));
                    colors.Add(Propiedades.ForeC, Color.Black);
                    colors.Add(Propiedades.BordeC, Color.RoyalBlue);
                    colors.Add(Propiedades.FocusC, Color.FromArgb(95, 135, 255));
                    colors.Add(Propiedades.MouseE, Color.FromArgb(125, 165, 255));
                    break;
            }
            return colors;
        }

        private static Dictionary<Propiedades, Color> GetVScrollColors(Tema tema)
        {
            Dictionary<Propiedades, Color> colors = new Dictionary<Propiedades, Color>();
            Color backCColor;
            switch (tema)
            {
                case Tema.Claro: backCColor = Color.FromArgb(200, 200, 200); break;
                case Tema.Oscuro: backCColor = Color.FromArgb(80, 80, 80); break;
                case Tema.Multicolor: backCColor = Color.FromArgb(210, 210, 210); break;
                case Tema.Gris: backCColor = Color.FromArgb(80, 80, 80); break;
                case Tema.Negro: backCColor = Color.FromArgb(64, 64, 64); break;
                default: backCColor = Color.FromArgb(220, 220, 220); break;    // Tema Blanco
            }

            if (tema == Tema.Claro | tema == Tema.Multicolor | tema == Tema.Blanco)
            {
                colors.Add(Propiedades.BackC, backCColor);
                colors.Add(Propiedades.BordeC, Color.FromArgb(180, 180, 180));
                colors.Add(Propiedades.FocusC, Color.FromArgb(150, 150, 150));
                colors.Add(Propiedades.MouseE, Color.FromArgb(120, 120, 120));
                colors.Add(Propiedades.VSBtn, Color.FromArgb(90, 90, 90));
                colors.Add(Propiedades.VSBtnFocus, Color.Black);
            }
            else
            {
                colors.Add(Propiedades.BackC, backCColor);
                colors.Add(Propiedades.BordeC, Color.Gray);
                colors.Add(Propiedades.FocusC, Color.DarkGray);
                colors.Add(Propiedades.MouseE, Color.FromArgb(200, 200, 200));
                colors.Add(Propiedades.VSBtn, Color.FromArgb(180, 180, 180));
                colors.Add(Propiedades.VSBtnFocus, Color.White);
            }
            return colors;
        }

        private static Dictionary<Propiedades, Color> GetCBoxColors(Tema tema)
        {
            Dictionary<Propiedades, Color> colors = new Dictionary<Propiedades, Color>();
            switch (tema)
            {
                case Tema.Claro:
                    colors.Add(Propiedades.BackC, Color.FromArgb(210, 210, 210));
                    colors.Add(Propiedades.ForeC, Color.Black);
                    colors.Add(Propiedades.BordeC, Color.RoyalBlue);
                    colors.Add(Propiedades.FocusC, Color.FromArgb(30, 140, 255));
                    colors.Add(Propiedades.MouseE, Color.Blue);
                    colors.Add(Propiedades.CBoxMouseMoveBC, Color.CornflowerBlue);
                    colors.Add(Propiedades.CBoxMouseMoveFC, Color.WhiteSmoke);
                    colors.Add(Propiedades.CBoxItemSelectBC, Color.RoyalBlue);
                    break;

                case Tema.Oscuro:
                    colors.Add(Propiedades.BackC, Color.FromArgb(90, 90, 90));
                    colors.Add(Propiedades.ForeC, Color.Gainsboro);
                    colors.Add(Propiedades.BordeC, Color.RoyalBlue);
                    colors.Add(Propiedades.FocusC, Color.CornflowerBlue);
                    colors.Add(Propiedades.MouseE, Color.DodgerBlue);
                    colors.Add(Propiedades.CBoxMouseMoveBC, Color.CornflowerBlue);
                    colors.Add(Propiedades.CBoxMouseMoveFC, Color.WhiteSmoke);
                    colors.Add(Propiedades.CBoxItemSelectBC, Color.RoyalBlue);
                    break;

                case Tema.Multicolor:
                    colors.Add(Propiedades.BackC, Color.FromArgb(220, 220, 220));
                    colors.Add(Propiedades.ForeC, Color.Black);
                    colors.Add(Propiedades.BordeC, Color.FromArgb(33, 115, 70));
                    colors.Add(Propiedades.FocusC, Color.FromArgb(63, 145, 100));
                    colors.Add(Propiedades.MouseE, Color.FromArgb(93, 175, 130));
                    colors.Add(Propiedades.CBoxMouseMoveBC, Color.FromArgb(159, 205, 179));
                    colors.Add(Propiedades.CBoxMouseMoveFC, Color.White);
                    colors.Add(Propiedades.CBoxItemSelectBC, Color.FromArgb(14, 92, 47));
                    break;

                case Tema.Gris:
                    colors.Add(Propiedades.BackC, Color.FromArgb(90, 90, 90));
                    colors.Add(Propiedades.ForeC, Color.White);
                    colors.Add(Propiedades.BordeC, Color.FromArgb(58, 180, 85));
                    colors.Add(Propiedades.FocusC, Color.FromArgb(73, 200, 100));
                    colors.Add(Propiedades.MouseE, Color.FromArgb(94, 220, 121));
                    colors.Add(Propiedades.CBoxMouseMoveBC, Color.MediumSeaGreen);
                    colors.Add(Propiedades.CBoxMouseMoveFC, Color.White);
                    colors.Add(Propiedades.CBoxItemSelectBC, Color.SeaGreen);
                    break;

                case Tema.Negro:
                    colors.Add(Propiedades.BackC, Color.FromArgb(54, 54, 54));
                    colors.Add(Propiedades.ForeC, Color.White);
                    colors.Add(Propiedades.BordeC, Color.FromArgb(94, 0, 163));
                    colors.Add(Propiedades.FocusC, Color.FromArgb(108, 0, 188));
                    colors.Add(Propiedades.MouseE, Color.FromArgb(133, 2, 229));
                    colors.Add(Propiedades.CBoxMouseMoveBC, Color.FromArgb(133, 2, 229));
                    colors.Add(Propiedades.CBoxMouseMoveFC, Color.White);
                    colors.Add(Propiedades.CBoxItemSelectBC, Color.Indigo);
                    break;

                case Tema.Blanco:
                    colors.Add(Propiedades.BackC, Color.FromArgb(230, 230, 230));
                    colors.Add(Propiedades.ForeC, Color.Black);
                    colors.Add(Propiedades.BordeC, Color.RoyalBlue);
                    colors.Add(Propiedades.FocusC, Color.FromArgb(95, 135, 255));
                    colors.Add(Propiedades.MouseE, Color.FromArgb(125, 165, 255));
                    colors.Add(Propiedades.CBoxMouseMoveBC, Color.LightSteelBlue);
                    colors.Add(Propiedades.CBoxMouseMoveFC, Color.Black);
                    colors.Add(Propiedades.CBoxItemSelectBC, Color.CornflowerBlue);
                    break;
            }
            return colors;
        }

        private static Dictionary<Propiedades, Color> GetGViewColors(Tema tema)
        {
            Dictionary<Propiedades, Color> colors = new Dictionary<Propiedades, Color>();
            switch (tema)
            {
                case Tema.Claro:
                    colors.Add(Propiedades.BackC, Color.Gainsboro);
                    colors.Add(Propiedades.ForeC, Color.Black);
                    colors.Add(Propiedades.BordeC, Color.FromArgb(140, 140, 140));
                    colors.Add(Propiedades.GVClmnHeadBC, Color.RoyalBlue);
                    colors.Add(Propiedades.GVClmnHeadFC, Color.WhiteSmoke);
                    colors.Add(Propiedades.GVRowFC, Color.FromArgb(70, 70, 70));
                    colors.Add(Propiedades.GVSelectRowBC, Color.LightSteelBlue);
                    colors.Add(Propiedades.GVSelectRowFC, Color.Black);
                    break;

                case Tema.Oscuro:
                    colors.Add(Propiedades.BackC, Color.FromArgb(70, 70, 70));
                    colors.Add(Propiedades.ForeC, Color.Gainsboro);
                    colors.Add(Propiedades.BordeC, Color.FromArgb(120, 120, 120));
                    colors.Add(Propiedades.GVClmnHeadBC, Color.RoyalBlue);
                    colors.Add(Propiedades.GVClmnHeadFC, Color.WhiteSmoke);
                    colors.Add(Propiedades.GVRowFC, Color.Gainsboro);
                    colors.Add(Propiedades.GVSelectRowBC, Color.FromArgb(156, 176, 202));
                    colors.Add(Propiedades.GVSelectRowFC, Color.FromArgb(50, 50, 50));
                    break;

                case Tema.Multicolor:
                    colors.Add(Propiedades.BackC, Color.FromArgb(230, 230, 230));
                    colors.Add(Propiedades.ForeC, Color.Black);
                    colors.Add(Propiedades.BordeC, Color.FromArgb(177, 177, 177));
                    colors.Add(Propiedades.GVClmnHeadBC, Color.FromArgb(33, 115, 70));
                    colors.Add(Propiedades.GVClmnHeadFC, Color.White);
                    colors.Add(Propiedades.GVRowFC, Color.DimGray);
                    colors.Add(Propiedades.GVSelectRowBC, Color.FromArgb(215, 215, 215));
                    colors.Add(Propiedades.GVSelectRowFC, Color.Black);
                    break;

                case Tema.Gris:
                    colors.Add(Propiedades.BackC, Color.FromArgb(102, 102, 102));
                    colors.Add(Propiedades.ForeC, Color.White);
                    colors.Add(Propiedades.BordeC, Color.FromArgb(140, 140, 140));
                    colors.Add(Propiedades.GVClmnHeadBC, Color.SeaGreen);
                    colors.Add(Propiedades.GVClmnHeadFC, Color.White);
                    colors.Add(Propiedades.GVRowFC, Color.Silver);
                    colors.Add(Propiedades.GVSelectRowBC, Color.FromArgb(135, 150, 135));
                    colors.Add(Propiedades.GVSelectRowFC, Color.White);
                    break;

                case Tema.Negro:
                    colors.Add(Propiedades.BackC, Color.FromArgb(38, 38, 38));
                    colors.Add(Propiedades.ForeC, Color.White);
                    colors.Add(Propiedades.BordeC, Color.FromArgb(100, 100, 100));
                    colors.Add(Propiedades.GVClmnHeadBC, Color.Indigo);
                    colors.Add(Propiedades.GVClmnHeadFC, Color.White);
                    colors.Add(Propiedades.GVRowFC, Color.Silver);
                    colors.Add(Propiedades.GVSelectRowBC, Color.FromArgb(70, 70, 70));
                    colors.Add(Propiedades.GVSelectRowFC, Color.White);
                    break;

                case Tema.Blanco:
                    colors.Add(Propiedades.BackC, Color.White);
                    colors.Add(Propiedades.ForeC, Color.Black);
                    colors.Add(Propiedades.BordeC, Color.FromArgb(160, 160, 160));
                    colors.Add(Propiedades.GVClmnHeadBC, Color.RoyalBlue);
                    colors.Add(Propiedades.GVClmnHeadFC, Color.WhiteSmoke);
                    colors.Add(Propiedades.GVRowFC, Color.FromArgb(70, 70, 70));
                    colors.Add(Propiedades.GVSelectRowBC, Color.FromArgb(235, 235, 235));
                    colors.Add(Propiedades.GVSelectRowFC, Color.Black);
                    break;
            }
            return colors;
        }

        private static Dictionary<Propiedades, Color> GetTabCColors(Tema tema)
        {
            Dictionary<Propiedades, Color> colors = new Dictionary<Propiedades, Color>();
            switch (tema)
            {
                case Tema.Claro:
                    colors.Add(Propiedades.BackC, Color.Gainsboro);
                    colors.Add(Propiedades.ForeC, Color.White);
                    colors.Add(Propiedades.TCTabActBack, Color.RoyalBlue);
                    colors.Add(Propiedades.TCTabInacBack, Color.FromArgb(210, 210, 210));
                    colors.Add(Propiedades.TCTabInacFore, Color.Gray);
                    colors.Add(Propiedades.TCTabActSelector, Color.Red);
                    break;

                case Tema.Oscuro:
                    colors.Add(Propiedades.BackC, Color.FromArgb(70, 70, 70));
                    colors.Add(Propiedades.ForeC, Color.Gainsboro);
                    colors.Add(Propiedades.TCTabActBack, Color.RoyalBlue);
                    colors.Add(Propiedades.TCTabInacBack, Color.FromArgb(90, 90, 90));
                    colors.Add(Propiedades.TCTabInacFore, Color.Silver);
                    colors.Add(Propiedades.TCTabActSelector, Color.Red);
                    break;

                case Tema.Multicolor:
                    colors.Add(Propiedades.BackC, Color.FromArgb(230, 230, 230));
                    colors.Add(Propiedades.ForeC, Color.WhiteSmoke);
                    colors.Add(Propiedades.TCTabActBack, Color.FromArgb(14, 92, 47));
                    colors.Add(Propiedades.TCTabInacBack, Color.FromArgb(220, 220, 220));
                    colors.Add(Propiedades.TCTabInacFore, Color.DimGray);
                    colors.Add(Propiedades.TCTabActSelector, Color.SpringGreen);
                    break;

                case Tema.Gris:
                    colors.Add(Propiedades.BackC, Color.FromArgb(102, 102, 102));
                    colors.Add(Propiedades.ForeC, Color.White);
                    colors.Add(Propiedades.TCTabActBack, Color.SeaGreen);
                    colors.Add(Propiedades.TCTabInacBack, Color.FromArgb(90, 90, 90));
                    colors.Add(Propiedades.TCTabInacFore, Color.Silver);
                    colors.Add(Propiedades.TCTabActSelector, Color.SpringGreen);
                    break;

                case Tema.Negro:
                    colors.Add(Propiedades.BackC, Color.FromArgb(38, 38, 38));
                    colors.Add(Propiedades.ForeC, Color.White);
                    colors.Add(Propiedades.TCTabActBack, Color.Indigo);
                    colors.Add(Propiedades.TCTabInacBack, Color.FromArgb(54, 54, 54));
                    colors.Add(Propiedades.TCTabInacFore, Color.DimGray);
                    colors.Add(Propiedades.TCTabActSelector, Color.FromArgb(150, 50, 200));
                    break;

                case Tema.Blanco:
                    colors.Add(Propiedades.BackC, Color.White);
                    colors.Add(Propiedades.ForeC, Color.White);
                    colors.Add(Propiedades.TCTabActBack, Color.RoyalBlue);
                    colors.Add(Propiedades.TCTabInacBack, Color.FromArgb(230, 230, 230));
                    colors.Add(Propiedades.TCTabInacFore, Color.FromArgb(120, 120, 120));
                    colors.Add(Propiedades.TCTabActSelector, Color.Red);
                    break;
            }
            return colors;
        }

        private static Dictionary<Propiedades, Color> GetInputTxtMsgColors(Tema tema)
        {
            Dictionary<Propiedades, Color> colors = new Dictionary<Propiedades, Color>();
            switch (tema)
            {
                case Tema.Claro:
                    colors.Add(Propiedades.BackC, Color.Gainsboro);
                    colors.Add(Propiedades.ForeC, Color.Black);
                    colors.Add(Propiedades.BordeC, Color.FromArgb(140, 140, 140));
                    colors.Add(Propiedades.InputMsgTPnlBtnsBC, Color.FromArgb(203, 203, 203));
                    break;

                case Tema.Oscuro:
                    colors.Add(Propiedades.BackC, Color.FromArgb(70, 70, 70));
                    colors.Add(Propiedades.ForeC, Color.Gainsboro);
                    colors.Add(Propiedades.BordeC, Color.FromArgb(120, 120, 120));
                    colors.Add(Propiedades.InputMsgTPnlBtnsBC, Color.FromArgb(50, 50, 50));
                    break;

                case Tema.Multicolor:
                    colors.Add(Propiedades.BackC, Color.FromArgb(220, 220, 220));
                    colors.Add(Propiedades.ForeC, Color.Black);
                    colors.Add(Propiedades.BordeC, Color.FromArgb(177, 177, 177));
                    colors.Add(Propiedades.InputMsgTPnlBtnsBC, Color.FromArgb(210, 210, 210));
                    break;

                case Tema.Gris:
                    colors.Add(Propiedades.BackC, Color.FromArgb(90, 90, 90));
                    colors.Add(Propiedades.ForeC, Color.White);
                    colors.Add(Propiedades.BordeC, Color.FromArgb(140, 140, 140));
                    colors.Add(Propiedades.InputMsgTPnlBtnsBC, Color.FromArgb(75, 75, 75));
                    break;

                case Tema.Negro:
                    colors.Add(Propiedades.BackC, Color.FromArgb(54, 54, 54));
                    colors.Add(Propiedades.ForeC, Color.White);
                    colors.Add(Propiedades.BordeC, Color.FromArgb(100, 100, 100));
                    colors.Add(Propiedades.InputMsgTPnlBtnsBC, Color.FromArgb(38, 38, 38));
                    break;

                case Tema.Blanco:
                    colors.Add(Propiedades.BackC, Color.White);
                    colors.Add(Propiedades.ForeC, Color.Black);
                    colors.Add(Propiedades.BordeC, Color.FromArgb(160, 160, 160));
                    colors.Add(Propiedades.InputMsgTPnlBtnsBC, Color.FromArgb(240, 240, 240));
                    break;
            }
            return colors;
        }
    }
}
