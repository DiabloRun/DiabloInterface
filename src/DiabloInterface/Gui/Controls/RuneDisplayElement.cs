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

        private Rune rune;

        private bool haveRune = false;

        static Bitmap runesSprite;
        static Bitmap runesSpriteHighContrast;

        private Bitmap image;
        private Bitmap imageRed;

        public bool Removed { get; set; }

        // size of rune images (width and height are same)
        private const int runeSize = 28;

        public RuneDisplayElement(Rune rune)
        {
            InitializeComponent();
            // load the sprite files first
            if (runesSprite == null || runesSpriteHighContrast == null)
            {
                var assembly = Assembly.GetExecutingAssembly();
				if (runesSprite == null) {
                    runesSprite = Properties.Resources.runes;
                }
	            if (runesSpriteHighContrast == null)
                {
                    runesSpriteHighContrast = Properties.Resources.runes_high_contrast;
	            }
            }
            setRune(rune);
        }
        public RuneDisplayElement(Rune rune, bool highContrast, bool removable, bool haveRune) : this(rune)
        {
            SetRuneSprite(highContrast);
            SetRemovable(false);
            SetHaveRune(false);
        }
        
        public Rune getRune()
        {
            return rune;
        }

        public void SetRuneSprite( bool highContrast )
        {
            if (highContrast)
            {
                image = runesSpriteHighContrast.Clone(new Rectangle(0, (int)rune * runeSize, runeSize, runeSize), runesSprite.PixelFormat);
                imageRed = runesSpriteHighContrast.Clone(new Rectangle(runeSize, (int)rune * runeSize, runeSize, runeSize), runesSprite.PixelFormat);
            } else
            {
                image = runesSprite.Clone(new Rectangle(0, (int)rune * runeSize, runeSize, runeSize), runesSprite.PixelFormat);
                imageRed = runesSprite.Clone(new Rectangle(runeSize, (int)rune * runeSize, runeSize, runeSize), runesSprite.PixelFormat);
            }
        }
        public void setRune( Rune rune )
        {

            this.rune = rune;

            image = runesSprite.Clone(new Rectangle(0, (int)rune* runeSize, runeSize, runeSize), runesSprite.PixelFormat);
            imageRed = runesSprite.Clone(new Rectangle(runeSize, (int)rune * runeSize, runeSize, runeSize), runesSprite.PixelFormat);
            SetRuneSprite(false);
            SetHaveRune(true);
		}
        public void SetRemovable(bool removable)
        {
            if (removable)
            {
                this.Width = runeSize*2; // twice as wide cause of remove button
            } else
            {
                this.Width = runeSize;
            }
        }
        public void SetHaveRune(bool haveRune)
        {
            this.haveRune = haveRune;
            if (haveRune)
            {
                pictureBox1.BackgroundImage = image;
            } else
            {
                pictureBox1.BackgroundImage = imageRed;
            }
        }
        public bool getHaveRune()
        {
            return haveRune;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                c.Enabled = false;
            }
            this.Hide();
            this.Removed = true;
        }
    }
}
