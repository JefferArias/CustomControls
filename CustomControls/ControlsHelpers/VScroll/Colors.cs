using CustomControls.Genericos;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace CustomControls.Controls
{
    public sealed partial class VScroll
    {
        public class CColores : IColores, INotifyPropertyChanged
        {
            private Color borde;
            private Color focus;
            private Color mouseEnter;
            private Color btn;
            private Color btnFocus;

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

            public Color MouseEnter
            {
                get => mouseEnter;
                set
                {
                    mouseEnter = value;
                    NotifyPropertyChanged(Propiedades.MouseEnter.ToString());
                }
            }

            public Color VSBtn
            {
                get => btn;
                set
                {
                    btn = value;
                    NotifyPropertyChanged(nameof(VSBtn));
                }
            }

            public Color VSBtnFocus
            {
                get => btnFocus;
                set
                {
                    btnFocus = value;
                    NotifyPropertyChanged(nameof(VSBtnFocus));
                }
            }

            private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            public enum Propiedades : byte
            {
                Borde,
                Focus,
                MouseEnter,
                Btn,
                BtnFocus,
            }
        }
    }

}
