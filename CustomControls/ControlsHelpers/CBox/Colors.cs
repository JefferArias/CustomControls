using CustomControls.Genericos;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace CustomControls.Controls
{
    public sealed partial class CBox
    {
        public class Colors : IColores, INotifyPropertyChanged
        {
            private Color back;
            private Color fore;
            private Color arrow;
            private Color focus;
            private Color mouseEnter;
            private Color mouseMoveBC;
            private Color mouseMoveFC;
            private Color selectItemBC;

            public event PropertyChangedEventHandler PropertyChanged;

            [Browsable(false)]
            public Color BackC
            {
                get => back;
                set
                {
                    back = value;
                    NotifyPropertyChanged(Propiedades.BackC.ToString());
                }
            }

            [Browsable(false)]
            public Color ForeC
            {
                get => fore;
                set
                {
                    fore = value;
                    NotifyPropertyChanged(Propiedades.ForeC.ToString());
                }
            }

            [DisplayName("Arrow")]
            public Color Borde
            {
                get => arrow;
                set
                {
                    arrow = value;
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

            [DisplayName("Move BackColor")]
            public Color MouseMoveBC
            {
                get => mouseMoveBC;
                set
                {
                    mouseMoveBC = value;
                    NotifyPropertyChanged(Propiedades.MouseMoveBC.ToString());
                }
            }

            [DisplayName("Move ForeColor")]
            public Color MouseMoveFC
            {
                get => mouseMoveFC;
                set
                {
                    mouseMoveFC = value;
                    NotifyPropertyChanged(Propiedades.MouseMoveFC.ToString());
                }
            }

            [DisplayName("Selector de Item")]
            public Color SelectItemBC
            {
                get => selectItemBC;
                set
                {
                    selectItemBC = value;
                    NotifyPropertyChanged(Propiedades.SelectItemBC.ToString());
                }
            }

            private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            public enum Propiedades : byte
            {
                BackC,
                ForeC,
                Borde,
                Focus,
                MouseEnter,
                MouseMoveBC,
                MouseMoveFC,
                SelectItemBC,
                SelectItemFC
            }
        }
    }
}
