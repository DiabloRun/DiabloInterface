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

    public partial class RuneDisplayElement : UserControl
    {
        // Size of rune images (width and height are same).
        const int RuneSize = 28;
        
        public Rune Rune { get; private set; }

        private bool haveRune = false;

        Bitmap image;
        Bitmap imageRed;

        public RuneDisplayElement(Rune rune)
        {
            Initialize(rune);

            Disposed += OnDisposed;
        }

        public RuneDisplayElement(Rune rune, bool highContrast, bool removable, bool haveRune)
        {
            Initialize(rune, highContrast, removable, haveRune);

            Disposed += OnDisposed;
        }

        public event EventHandler<EventArgs> RemoveButtonClicked; 

        void Initialize(Rune rune, bool highContrast = false, bool removable = true, bool haveRune = true)
        {
            InitializeComponent();

            this.Rune = rune;

            using (Bitmap sprite = (highContrast ? Properties.Resources.runes_high_contrast : Properties.Resources.runes))
            {
                image = sprite.Clone(new Rectangle(0, (int)Rune * RuneSize, RuneSize, RuneSize), sprite.PixelFormat);
                imageRed = sprite.Clone(new Rectangle(RuneSize, (int)Rune * RuneSize, RuneSize, RuneSize), sprite.PixelFormat);
            }
            Width = RuneSize * (removable?2:1); // twice as wide if removable, cause of remove button

            SetHaveRune(haveRune);
        }

        void OnDisposed(object sender, EventArgs eventArgs)
        {
            pictureBox1.BackgroundImage = null;

            image?.Dispose();
            imageRed?.Dispose();
        }

        public void SetHaveRune(bool haveRune)
        {
            this.haveRune = haveRune;
            pictureBox1.BackgroundImage = (haveRune ? image : imageRed);
        }

        void RemoveButtonOnClick(object sender, EventArgs e)
        {
            RemoveButtonClicked?.Invoke(this, e);
        }
    }
}
