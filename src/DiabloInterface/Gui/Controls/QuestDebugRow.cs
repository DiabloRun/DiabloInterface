using DiabloInterface.D2;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DiabloInterface.Gui.Controls
{
    public partial class QuestDebugRow : UserControl
    {
        private Label[] labels = new Label[16];

        public QuestDebugRow(D2QuestHelper.D2Quest quest)
        {
            InitializeComponent();

            lblText.Text = quest.CommonName;

            labels[0] = label0;
            labels[1] = label1;
            labels[2] = label2;
            labels[3] = label3;
            labels[4] = label4;
            labels[5] = label5;
            labels[6] = label6;
            labels[7] = label7;
            labels[8] = label8;
            labels[9] = label9;
            labels[10] = label10;
            labels[11] = label11;
            labels[12] = label12;
            labels[13] = label13;
            labels[14] = label14;
            labels[15] = label15;
        }

        public void Update(ushort questBits)
        {
            for ( byte i = 0; i < 16; i++ )
            {
                labels[i].Invoke(new Action(() => { labels[i].BackColor = IsBitSet(questBits, i) ? Color.GreenYellow : Color.AliceBlue; }));
            }
        }

        private bool IsBitSet(ushort questBits, byte bit)
        {
            return (questBits & (1 << bit)) != 0;
        }
    }
}
