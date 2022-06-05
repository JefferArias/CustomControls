using System;
using System.ComponentModel;
using System.Globalization;

namespace CustomControls.Genericos
{
    public sealed class ColorsConverter : ExpandableObjectConverter
    {
        private readonly string icolores = nameof(IColores);

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType.GetInterface(icolores) == typeof(IColores) || base.CanConvertFrom(context, sourceType);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return value.GetType().GetInterface(icolores) == typeof(IColores) ? true : base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return destinationType == typeof(string) ? "Paleta de Colores" : base.ConvertTo(context, culture, value, destinationType);
        }
    }
}