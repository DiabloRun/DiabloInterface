namespace Zutatensuppe.DiabloInterface.Gui.Controls
{
    using Zutatensuppe.D2Reader.Models;

    using System.Drawing;
    using System.Windows.Forms;

    public class QuestDebugRow : UserControl
    {
        private Label[] bits;

        public QuestDebugRow(Quest quest)
        {
            bits = new Label[16];
            for (var i = 0; i < 16; i++)
            {
                var lbl = new Label();
                lbl.BackColor = Color.AliceBlue;
                lbl.BorderStyle = BorderStyle.FixedSingle;
                lbl.Location = new Point(122 + (i * 22), 1);
                lbl.Margin = new Padding(0);
                lbl.Size = new Size(18, 14);
                lbl.Text = "" + i;
                lbl.TextAlign = ContentAlignment.MiddleCenter;
                bits[i] = lbl;
            };

            var label = new Label();
            label.Font = new Font("Microsoft Sans Serif", 8.25F);
            label.Location = new Point(3, 2);
            label.Size = new Size(57, 13);
            label.Text = quest.CommonName;

            AutoScaleDimensions = new SizeF(6F, 12F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(label);
            Controls.AddRange(bits);
            Font = new Font("Microsoft Sans Serif", 6.5F);
            Size = new Size(472, 16);
        }

        public void Update(Quest quest)
        {
            if (!IsHandleCreated) return;

            for (var i = 0; i < 16; i++)
            {
                bits[i].BackColor = IsBitSet(quest.CompletionBits, i)
                    ? Color.GreenYellow
                    : Color.AliceBlue;
            }
        }

        static bool IsBitSet(ushort questBits, int bit)
        {
            return (questBits & (1 << bit)) != 0;
        }
    }
}
