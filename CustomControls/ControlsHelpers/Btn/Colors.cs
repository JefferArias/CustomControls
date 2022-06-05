using CustomControls.Genericos;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace CustomControls.Controls
{
    public sealed partial class Btn
    {
        public class Colors : IColores, INotifyPropertyChanged
        {
            private Color borde;
            private Color focus;
            private Color mouseEnter;

            public event PropertyChangedEventHandler PropertyChanged;

            public Color Borde
            {
                get => borde;
                set
                {
                    borde = value;
                    NotifyPropertyChanged(Propiedades.Borde.ToString());
                }
            }

            public Color Focus
            {
                get => focus;
                set
                {
                    focus = value;
                    NotifyPropertyChanged(Propiedades.Focus.ToString());
                }
            }

            [DisplayName("Mouse Enter")]
            public Color MouseEnter
            {
                get => mouseEnter;
                set
                {
                    mouseEnter = value;
                    NotifyPropertyChanged(Propiedades.MouseEnter.ToString());
                }
            }

            private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            public enum Propiedades : byte
            {
                Borde,
                Focus,
                MouseEnter,
            }
        }
    }

}
