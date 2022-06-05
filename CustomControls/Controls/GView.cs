using CustomControls.Themes;
using System.ComponentModel;
using System.Windows.Forms;

namespace CustomControls.Controls
{
    public sealed class GView : DataGridView
    {
        private Themes.Themes.Tema tema;

        public GView()
        {
            Tema = Themes.Themes.Tema.Multicolor;
            OtrasConfiguraciones();
        }

        [Category("Apariencia")]
        public Themes.Themes.Tema Tema
        {
            get => tema;
            set
            {
                tema = value;
                Themes.Themes.SetTheme(this, value);
            }
        }

        private void OtrasConfiguraciones()
        {
            ReadOnly = true;
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            BorderStyle = BorderStyle.None;
            EnableHeadersVisualStyles = false;
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ColumnHeadersHeight = 25;
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            MultiSelect = false;
            RowHeadersVisible = false;
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
        }
    }
}