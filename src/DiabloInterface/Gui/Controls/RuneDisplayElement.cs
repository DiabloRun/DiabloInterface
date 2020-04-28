namespace Zutatensuppe.DiabloInterface.Gui.Controls
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Collections.Generic;

    using Zutatensuppe.D2Reader.Models;

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

    public class RuneDisplayElement : UserControl
    {
        // Size of rune images (width and height are same).
        const int RuneSize = 28;
        
        public Rune Rune { get; private set; }

        private bool haveRune = false;

        Bitmap image;
        Bitmap imageRed;

        private PictureBox pictureBox1;

        public event EventHandler<EventArgs> RemoveButtonClicked;

        public RuneDisplayElement(Rune rune) : this(rune, false, true, true) { }

        public RuneDisplayElement(Rune rune, bool highContrast, bool removable, bool haveRune)
        {
            Rune = rune;
            using (Bitmap sprite = highContrast ? Properties.Resources.runes_high_contrast : Properties.Resources.runes)
            {
                image = sprite.Clone(new Rectangle(0, (int)Rune * RuneSize, RuneSize, RuneSize), sprite.PixelFormat);
                imageRed = sprite.Clone(new Rectangle(RuneSize, (int)Rune * RuneSize, RuneSize, RuneSize), sprite.PixelFormat);
            }

            pictureBox1 = new PictureBox();
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Margin = new Padding(0);
            pictureBox1.Size = new Size(RuneSize, RuneSize);

            SetHaveRune(haveRune);

            Controls.Add(pictureBox1);

            if (removable)
            {
                var btnRemove = new Button();
                btnRemove.Location = new Point(36, 3);
                btnRemove.Size = new Size(17, 23);
                btnRemove.Text = "X";
                btnRemove.Click += (object s, EventArgs e) => RemoveButtonClicked?.Invoke(this, e);
                Controls.Add(btnRemove);
            }

            // twice as wide if removable, cause of remove button
            Size = new Size(RuneSize * (removable ? 2 : 1), RuneSize);

            Disposed += OnDisposed;
        }

        void OnDisposed(object sender, EventArgs eventArgs)
        {
            pictureBox1.BackgroundImage = null;

            image?.Dispose();
            imageRed?.Dispose();
        }

        internal void SetHaveRune(bool haveRune)
        {
            this.haveRune = haveRune;
            pictureBox1.BackgroundImage = haveRune ? image : imageRed;
        }
    }
}
