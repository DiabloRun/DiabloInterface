using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.Generic;

namespace Zutatensuppe.DiabloInterface.Gui.Controls
{
    public enum Rune
    {
        El,
        Eld,
        Tir,
        Nef,
        Eth,
        Ith,
        Tal,
        Ral,
        Ort,
        Thul,
        Amn,
        Sol,
        Shael,
        Dol,
        Hel,
        Io,
        Lum,
        Ko,
        Fal,
        Lem,
        Pul,
        Um,
        Mal,
        Ist,
        Gul,
        Vex,
        Ohm,
        Lo,
        Sur,
        Ber,
        Jah,
        Cham,
        Zod,
    };

    public class Runeword
    {
        public string Name { get; set; }
        public List<Rune> Runes { get; set; }

        public Runeword()
        {
            Runes = new List<Rune>();
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public partial class RuneDisplayElement : UserControl
    {
        // size of rune images (width and height are same)
        private const int RUNE_SIZE = 28;
        
        public Rune Rune { get; private set; }

        private bool haveRune = false;

        private Bitmap image;
        private Bitmap imageRed;

        public RuneDisplayElement(Rune rune)
        {
            Initialize(rune);
        }
        public RuneDisplayElement(Rune rune, bool highContrast, bool removable, bool haveRune)
        {
            Initialize(rune, highContrast, removable, haveRune);
        }

        private void Initialize(Rune rune, bool highContrast = false, bool removable = true, bool haveRune = true)
        {
            InitializeComponent();

            this.Rune = rune;

            using (Bitmap sprite = (highContrast ? Properties.Resources.runes_high_contrast : Properties.Resources.runes))
            {
                image = sprite.Clone(new Rectangle(0, (int)Rune * RUNE_SIZE, RUNE_SIZE, RUNE_SIZE), sprite.PixelFormat);
                imageRed = sprite.Clone(new Rectangle(RUNE_SIZE, (int)Rune * RUNE_SIZE, RUNE_SIZE, RUNE_SIZE), sprite.PixelFormat);
            }
            Width = RUNE_SIZE * (removable?2:1); // twice as wide if removable, cause of remove button

            SetHaveRune(haveRune);
        }

        public void SetHaveRune(bool haveRune)
        {
            if (InvokeRequired)
            {
                // Delegate call to UI thread.
                Invoke((Action)(() => SetHaveRune(haveRune)));
                return;
            }

            this.haveRune = haveRune;
            pictureBox1.BackgroundImage = (haveRune ? image : imageRed);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            Parent.Controls.Remove(this);
        }
    }
}
