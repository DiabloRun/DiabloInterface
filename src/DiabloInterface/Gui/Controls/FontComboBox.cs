using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DiabloInterface.Gui.Controls
{
    public class FontComboBox : ComboBox
    {
        Dictionary<string, Font> fontCache = new Dictionary<string, Font>();
        StringFormat stringFormat;
        int itemHeight;

        [Category("Appearance"), DefaultValue(12)]
        public int DropDownFontSize { get; set; } = 12;

        public FontComboBox()
        {
            Sorted = true;
            DrawMode = DrawMode.OwnerDrawVariable;

            stringFormat = new StringFormat(StringFormatFlags.NoWrap);
            stringFormat.Trimming = StringTrimming.EllipsisCharacter;
            stringFormat.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat.Alignment = StringAlignment.Near;
            stringFormat.LineAlignment = StringAlignment.Center;

            CalculateItemHeight();

            HandleCreated += FontComboBox_HandleCreated;
        }

        private void FontComboBox_HandleCreated(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                // Only initialize fonts during runtime.
                PopulateFonts();
            }
        }

        void PopulateFonts()
        {
            if (Items.Count != 0) return;
            foreach (FontFamily family in FontFamily.Families)
            {
                Items.Add(family.Name);
            }
        }

        void CalculateItemHeight()
        {
            using (Font font = new Font(Font.FontFamily, DropDownFontSize))
            {
                var size = TextRenderer.MeasureText("yY", font);
                itemHeight = size.Height + 2;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                foreach (var font in fontCache.Values)
                {
                    font.Dispose();
                }
                fontCache.Clear();

                if (stringFormat != null)
                {
                    stringFormat.Dispose();
                    stringFormat = null;
                }
            }
        }

        Font GetDisplayFont(string family)
        {
            Font font;

            // Font already loaded into cache.
            if (fontCache.TryGetValue(family, out font))
            {
                return font;
            }

            FontStyle[] styles = new FontStyle[]
            {
                FontStyle.Regular,
                FontStyle.Bold,
                FontStyle.Italic,
                FontStyle.Bold | FontStyle.Italic
            };

            font = null;

            // Try to load one of the specified font styles.
            foreach (FontStyle style in styles)
            {
                font = GetDisplayFont(family, style);
                if (font != null) break;
            }

            // Failed to load any font styles, default to combobox font.
            if (font == null)
            {
                font = Font;
            }

            fontCache[family] = font;
            return font;
        }

        Font GetDisplayFont(string family, FontStyle style)
        {
            try
            {
                return new Font(family, DropDownFontSize, style);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            base.OnMeasureItem(e);

            e.ItemHeight = itemHeight;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);

            // Nothing to draw.
            if (e.Index < 0 || e.Index >= Items.Count)
                return;

            e.DrawBackground();
            if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
                e.DrawFocusRectangle();

            using (var brush = new SolidBrush(e.ForeColor))
            {
                string fontFamily = Items[e.Index].ToString();
                e.Graphics.DrawString(fontFamily, GetDisplayFont(fontFamily), brush, e.Bounds, stringFormat);
            }
        }
    }
}
