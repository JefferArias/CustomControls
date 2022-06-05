using CustomControls.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomControls.Controls
{
    public class Lbl : Label
    {
        Themes.Themes.Tema tema;
        public Lbl()
        {
            base.Font = Themes.Themes.GetFont();
            BackColor = Color.Transparent;
            Tema = Themes.Themes.Tema.Multicolor;
        }
        [Category("Apariencia")]
        public Themes.Themes.Tema Tema 
        {
            get => tema;
            set 
            {
                tema = value;
                switch (value)
                {
                    case Themes.Themes.Tema.Claro:
                    case Themes.Themes.Tema.Multicolor:
                    case Themes.Themes.Tema.Blanco:
                        base.ForeColor = Color.Black;
                        break;

                    case Themes.Themes.Tema.Oscuro:
                    case Themes.Themes.Tema.Gris:
                    case Themes.Themes.Tema.Negro:
                        base.ForeColor = Color.White;
                        break;
                        
                }
            }
        }
    }
}
