using System.ComponentModel;
using System.Drawing;

namespace CustomControls.Genericos
{
    public interface IColores : INotifyPropertyChanged
    {
        Color Borde { get; set; }
        Color Focus { get; set; }
        Color MouseEnter { get; set; }
    }
}