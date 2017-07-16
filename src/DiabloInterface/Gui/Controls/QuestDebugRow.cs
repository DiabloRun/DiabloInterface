namespace Zutatensuppe.DiabloInterface.Gui.Controls
{
    using Zutatensuppe.D2Reader.Models;
    using Zutatensuppe.D2Reader;

    using System.Drawing;
    using System.Windows.Forms;

    public partial class QuestDebugRow : UserControl
    {
        readonly Label[] labels;

        public QuestDebugRow(Quest quest)
        {
            InitializeComponent();

            lblText.Text = quest.CommonName;

            labels = new[]
            {
                label0,
                label1,
                label2,
                label3,
                label4,
                label5,
                label6,
                label7,
                label8,
                label9,
                label10,
                label11,
                label12,
                label13,
                label14,
                label15
            };
        }

        public void Update(Quest quest)
        {
            if (!IsHandleCreated) return;

            for (var i = 0; i < 16; i++)
            {
                labels[i].BackColor = IsBitSet(quest.CompletionBits, i)
                    ? Color.GreenYellow : Color.AliceBlue;
            }
        }

        static bool IsBitSet(ushort questBits, int bit)
        {
            return (questBits & (1 << bit)) != 0;
        }
    }
}
