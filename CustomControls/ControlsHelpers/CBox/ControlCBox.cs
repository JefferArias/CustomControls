using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace CustomControls.Controls
{
    public sealed partial class CBox
    {
        //Variables
        private sealed partial class ControlCBox : Control
        {
            private List<object> items = new List<object>();
            private List<object> itemsAux;
            private ushort itemFirst;
            private readonly List<Rectangle> rects = new List<Rectangle>();
            private readonly VScroll vs = new VScroll();
            private short enterRect = -1;
            private short leaveRect = -2;
            private int indxItemSelected = -1;
            private Key key;
            private ushort hg;
            private ushort incremento;
            private readonly Timer tm = new Timer() { Interval = 10 };
            private readonly Dictionary<EColor, Color> colores = new Dictionary<EColor, Color>();
            private Style style;

            public event EventHandler ItemSelect = (sender, e) => { };
        }

        //Contructor
        private sealed partial class ControlCBox
        {
            public ControlCBox()
            {
                SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
                TabStop = false;
                Controls.Add(vs);
                vs.Up += Vs_Up;
                vs.Down += Vs_Down;
                tm.Tick += Tm_Tick;
            }
        }

        //Propiedades
        private sealed partial class ControlCBox
        {
            public List<object> Items
            {
                get
                {
                    switch (style)
                    {
                        case Style.DropDown:
                            return items;
                        default:
                            return itemsAux;
                    }
                }
                set
                {
                    switch (style)
                    {
                        case Style.DropDown:
                            items.Clear();
                            items = null;
                            items = value;
                            break;
                        
                        case Style.Filter:
                            itemsAux.Clear();
                            itemsAux = null;
                            itemsAux = value;
                            break;
                    }
                }
            }

            public List<object> ItemsDraw { get => items; }
            public List<object> ItemsAux { get => itemsAux; }

            public string DisplayMember { get; set; }
            public string ValueMember { get; set; }

            public byte ItemHeight { get; set; }
            public byte ItemsView { get; set; }

            public Style CBoxStyle 
            {
                set 
                {
                    style = value;
                    switch (value)
                    {
                        case Style.DropDown:
                            itemsAux?.Clear();
                            itemsAux = null;
                            break;
                        
                        case Style.Filter:
                            itemsAux = new List<object>();
                            foreach (var item in items)
                                itemsAux.Add(item);
                            items.Clear();
                            break;
                    }
                } 
            }
        }

        //Graficos
        private sealed partial class ControlCBox
        {
            protected override void OnPaintBackground(PaintEventArgs pevent) { }

            protected override void OnPaint(PaintEventArgs e)
            {
                SolidBrush br = new SolidBrush(Color.Empty);
                PaintItems(e.Graphics, br);
                br.Dispose();
            }

            private void PaintItems(Graphics G, SolidBrush br)
            {
                ushort index = 0;
                Rectangle rect;
                foreach (Rectangle item in rects)
                {
                    rect = item;
                    if ((index + itemFirst) == indxItemSelected)
                        PaintItemSelected(G, br, ref rect);
                    else
                    {
                        br.Color = colores[EColor.Back];
                        G.FillRectangle(br, rect);
                        RectMod(ref rect);
                        br.Color = colores[EColor.Fore];
                        G.DrawString(GetDisplayMember(items[index + itemFirst]), Font, br, rect);
                    }
                    ++index;
                }
            }

            private void PaintEnterRect(Graphics G, SolidBrush br)
            {
                if (itemFirst + enterRect != indxItemSelected)
                {
                    Rectangle rect = rects[enterRect];
                    br.Color = colores[EColor.MMBC];
                    G.FillRectangle(br, rect);
                    RectMod(ref rect);
                    br.Color = colores[EColor.MMFC];
                    Font font = new Font(Font.Name, Font.Size, FontStyle.Bold | FontStyle.Underline);
                    G.DrawString(GetDisplayMember(items[enterRect + itemFirst]), font, br, rect);
                    font.Dispose();
                }
            }

            private void PaintLeaveRect(Graphics G, SolidBrush br)
            {
                if (leaveRect < 0)
                    leaveRect = 0;

                if (itemFirst + leaveRect != indxItemSelected && items.Count > 0)
                {
                    Rectangle rect = rects[leaveRect];
                    br.Color = colores[EColor.Back];
                    G.FillRectangle(br, rect);
                    br.Color = colores[EColor.Fore];
                    RectMod(ref rect);
                    G.DrawString(GetDisplayMember(items[leaveRect + itemFirst]), Font, br, rect);
                }
            }

            private void PaintItemSelected(Graphics G, SolidBrush br, ref Rectangle rect)
            {
                br.Color = colores[EColor.SIBC];
                G.FillRectangle(br, rect);
                RectMod(ref rect);
                br.Color = colores[EColor.MMFC];
                Font font = new Font(Font.Name, Font.Size, FontStyle.Bold | FontStyle.Underline);
                G.DrawString(GetDisplayMember(items[indxItemSelected]), font, br, rect);
                font.Dispose();
            }
        }

        //Metodos
        private sealed partial class ControlCBox
        {
            private void RectMod(ref Rectangle rect)
            {
                rect.Inflate(-4, 0);
                rect.X = 2;
            }

            public void AjustarControl()
            {
                rects.Clear();
                int wd;
                byte iV;

                if (items.Count > ItemsView)
                {
                    vs.Movimientos = (ushort)(items.Count - ItemsView);
                    vs.Visible = true;
                    wd = Width - vs.Width;
                    iV = ItemsView;
                }
                else
                {
                    vs.Visible = false;
                    wd = Width;
                    iV = (byte)items.Count;
                }

                ushort y = 0;
                for (byte index = 0; index < iV; ++index)
                {
                    rects.Add(new Rectangle(0, y, wd, ItemHeight));
                    y += ItemHeight;
                }

                hg = (ushort)(iV * ItemHeight);
                incremento = (ushort)Math.Truncate((double)(hg / 10));

                switch (style)
                {
                    case Style.DropDown:
                        Height = 0;
                        tm.Start();
                        break;

                    case Style.Filter:
                        Height = hg;
                        break;
                }
            }

            private void FillRect(Mover m)
            {
                Graphics G = Graphics.FromHwnd(Handle);
                SolidBrush br = new SolidBrush(Color.Empty);

                switch (m)
                {
                    case Mover.Mouse:
                        PaintLeaveRect(G, br);
                        PaintEnterRect(G, br);
                        break;

                    case Mover.EventVS:
                        PaintItems(G, br);
                        if (key == Key.None)
                            leaveRect = -1;
                        break;
                }
                br.Dispose();
            }

            public void AplicarTema(Colors clrs, Themes.Themes.Tema tema)
            {
                colores.Clear();
                switch (style)
                {
                    case Style.DropDown:
                        colores.Add(EColor.Back, clrs.BackC);
                        break;

                    case Style.Filter:
                        colores.Add(EColor.Back, Themes.Themes.GetColor(clrs.BackC));
                        break;
                }

                colores.Add(EColor.Fore, clrs.ForeC);
                colores.Add(EColor.MMBC, clrs.MouseMoveBC);
                colores.Add(EColor.MMFC, clrs.MouseMoveFC);
                colores.Add(EColor.SIBC, clrs.SelectItemBC);

                vs.Tema = tema;
            }

            public string GetDisplayMember(object obj)
            {
                if (DisplayMember != null)
                {
                    PropertyInfo pi = obj.GetType().GetProperty(DisplayMember);
                    if (pi != null)
                        return pi.GetValue(obj).ToString().Trim();
                }
                return obj.ToString().Trim();
            }

            public string GetValueMember(object obj)
            {
                if (ValueMember != null)
                {
                    PropertyInfo pi = obj.GetType().GetProperty(ValueMember);
                    if (pi != null)
                        return pi.GetValue(obj).ToString().Trim();
                }
                return obj.ToString().Trim();
            }
        }

        //Eventos
        private sealed partial class ControlCBox
        {
            protected override void OnMouseMove(MouseEventArgs e)
            {
                enterRect = 0;
                foreach (Rectangle rect in rects)
                {
                    if (rect.Contains(e.Location))
                    {
                        if (enterRect != leaveRect)
                        {
                            FillRect(Mover.Mouse);
                            break;
                        }
                        break;
                    }
                    ++enterRect;
                }
                leaveRect = enterRect;
            }

            protected override void OnMouseWheel(MouseEventArgs e) => vs.ScrollParent(e);

            protected override void OnMouseLeave(EventArgs e)
            {
                Graphics G = Graphics.FromHwnd(Handle);
                SolidBrush br = new SolidBrush(colores[EColor.Back]);
                PaintLeaveRect(G, br);
                br.Dispose();
                leaveRect = -2;
            }

            protected override void OnMouseDown(MouseEventArgs e)
            {
                indxItemSelected = itemFirst + enterRect;
                ItemSelect(items[indxItemSelected], EventArgs.Empty);
                if (style == Style.Filter)
                    indxItemSelected = -1;
            }

            public void Vs_Up(object sender, EventArgs e)
            {
                if (itemFirst > 0)
                {
                    --itemFirst;
                    FillRect(Mover.EventVS);
                }
            }

            public void Vs_Down(object sender, EventArgs e)
            {
                if (itemFirst < items.Count - ItemsView)
                {
                    ++itemFirst;
                    FillRect(Mover.EventVS);
                }
            }

            public void ResetItemFirst()
            {
                itemFirst = 0;
                vs.ResetUbiSB();
            }

            public void ResetIndexItemSelected() => indxItemSelected = -1;

            protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
            {
                switch (e.KeyCode)
                {
                    case Keys.Down:
                        if (e.Alt)
                            return;
                        else
                            key = Key.Down;
                        break;

                    case Keys.Up:
                        key = Key.Up; break;

                    case Keys.PageUp:
                        key = Key.PageUp;
                        OnGotFocus(null);
                        break;

                    case Keys.Next:
                        key = Key.PageDown;
                        OnGotFocus(null);
                        break;

                    case Keys.Return:
                        key = Key.Enter;
                        OnMouseDown(null);
                        break;

                    default:
                        key = Key.None; break;
                }
                base.OnPreviewKeyDown(e);
            }

            protected override void OnGotFocus(EventArgs e)
            {
                if (key != Key.None)
                {
                    Graphics G = Graphics.FromHwnd(Handle);
                    SolidBrush br = new SolidBrush(Color.Empty);

                    switch (key)
                    {
                        case Key.Down:
                            if (enterRect < rects.Count - 1)
                            {
                                ++enterRect;
                                leaveRect = enterRect;
                                PaintLeaveRect(G, br);
                                break;
                            }
                            vs.ScrollParent(new MouseEventArgs(MouseButtons.Middle, 0, 0, 0, -1));
                            break;

                        case Key.Up:
                            if (enterRect > 0)
                            {
                                --enterRect;
                                leaveRect = enterRect;
                                PaintLeaveRect(G, br);
                                break;
                            }
                            vs.ScrollParent(new MouseEventArgs(MouseButtons.Middle, 0, 0, 0, 1));
                            break;

                        case Key.PageDown:
                            if ((itemFirst + ItemsView) < items.Count)
                            {
                                enterRect = (short)(ItemsView - 1);
                                leaveRect = enterRect;
                                for (int i = 0; i < ItemsView; i++)
                                {
                                    vs.ScrollParent(new MouseEventArgs(MouseButtons.Middle, 0, 0, 0, -1));
                                }
                            }
                            break;

                        case Key.PageUp:
                            if (itemFirst > 0)
                            {
                                enterRect = 0;
                                leaveRect = 0;
                                for (int i = 0; i < ItemsView; i++)
                                {
                                    vs.ScrollParent(new MouseEventArgs(MouseButtons.Middle, 0, 0, 0, 1));
                                }
                            }
                            break;
                    }
                    PaintEnterRect(G, br);
                    br.Dispose();
                    G.Dispose();
                    key = Key.None;
                }
            }

            private void Tm_Tick(object sender, EventArgs e)
            {
                if (Height < hg)
                {
                    Height += incremento;
                    if ((hg - Height) <= 10)
                    {
                        Height = hg;
                        tm.Stop();
                    }
                }
            }
        }

        //Enumeraciones
        private sealed partial class ControlCBox
        {
            private enum EColor : byte
            {
                Back,
                Fore,
                MMBC,
                MMFC,
                SIBC,
            }

            private enum Mover : byte
            {
                Mouse,
                EventVS,
            }

            private enum Key : byte
            {
                None,
                Down,
                Up,
                PageDown,
                PageUp,
                Enter,
            }
        }
    }
}
