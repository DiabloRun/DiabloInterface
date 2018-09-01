namespace Zutatensuppe.DiabloInterface.Gui.Controls
{
    using Zutatensuppe.D2Reader.Models;

    using System.Drawing;
    using System.Windows.Forms;

    public class QuestDebugRow : UserControl
    {
        private Label[] labels;

        private Label lblQuestName;

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public QuestDebugRow(Quest quest)
        {
            SuspendLayout();

            lblQuestName = new Label();
            labels = new Label[16];
            for (var i = 0; i < 16; i++)
            {
                var lbl = new Label();
                lbl.BackColor = Color.AliceBlue;
                lbl.BorderStyle = BorderStyle.FixedSingle;
                lbl.Location = new Point(122 + (i * 22), 1);
                lbl.Margin = new Padding(0);
                lbl.Name = "label" + i;
                lbl.Size = new Size(18, 14);
                lbl.TabIndex = 0;
                lbl.Text = "" + i;
                lbl.TextAlign = ContentAlignment.MiddleCenter;
                labels[i] = lbl;
            };

            lblQuestName.AutoSize = true;
            lblQuestName.Font = new Font(
                "Microsoft Sans Serif",
                8.25F,
                FontStyle.Regular,
                GraphicsUnit.Point,
                0
            );
            lblQuestName.Location = new Point(3, 2);
            lblQuestName.Name = "lblText";
            lblQuestName.Size = new Size(57, 13);
            lblQuestName.TabIndex = 1;
            lblQuestName.Text = quest.CommonName;

            AutoScaleDimensions = new SizeF(6F, 12F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lblQuestName);
            Controls.AddRange(labels);
            Font = new Font("Microsoft Sans Serif", 6.5F);
            Margin = new Padding(2, 3, 2, 3);
            Name = "QuestDebugRow";
            Size = new Size(472, 16);
            ResumeLayout(false);
            PerformLayout();
        }

        public void Update(Quest quest)
        {
            if (!IsHandleCreated) return;

            for (var i = 0; i < 16; i++)
            {
                labels[i].BackColor = IsBitSet(quest.CompletionBits, i)
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
