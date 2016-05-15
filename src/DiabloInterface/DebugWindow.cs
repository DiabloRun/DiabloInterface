using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiabloInterface
{
    public partial class DebugWindow : Form
    {
        Label[] actlabels;
        Label[,] questlabels;
        Label[,,] questBits;


        byte[] questData;
        public DebugWindow()
        {
            InitializeComponent();
        }
        private byte reverseBits(byte b)
        {
            return (byte)(((b * 0x80200802ul) & 0x0884422110ul) * 0x0101010101ul >> 32);
        }
        public void setQuestData(byte[] questBuffer)
        {
            this.questData = questBuffer;


            short value;
            int act, quest;
            for (int i = 0; i < this.questData.Length - 1; i += 2)
            {
                value = BitConverter.ToInt16(new byte[2] { reverseBits(questBuffer[i]), reverseBits(questBuffer[i + 1]), }, 0);
                switch (i)
                {
                    case D2Data.QUEST_A1Q1:
                        act = 1;quest = 1;
                        break;
                    case D2Data.QUEST_A1Q2:
                        act = 1; quest = 2;
                        break;
                    case D2Data.QUEST_A1Q3:
                        act = 1; quest = 3;
                        break;
                    case D2Data.QUEST_A1Q4:
                        act = 1; quest = 4;
                        break;
                    case D2Data.QUEST_A1Q5:
                        act = 1; quest = 5;
                        break;
                    case D2Data.QUEST_A1Q6:
                        act = 1; quest = 6;
                        break;

                    case D2Data.QUEST_A2Q1:
                        act = 2; quest = 1;
                        break;
                    case D2Data.QUEST_A2Q2:
                        act = 2; quest = 2;
                        break;
                    case D2Data.QUEST_A2Q3:
                        act = 2; quest = 3;
                        break;
                    case D2Data.QUEST_A2Q4:
                        act = 2; quest = 4;
                        break;
                    case D2Data.QUEST_A2Q5:
                        act = 2; quest = 5;
                        break;
                    case D2Data.QUEST_A2Q6:
                        act = 2; quest = 6;
                        break;

                    case D2Data.QUEST_A3Q1:
                        act = 3; quest = 1;
                        break;
                    case D2Data.QUEST_A3Q2:
                        act = 3; quest = 2;
                        break;
                    case D2Data.QUEST_A3Q3:
                        act = 3; quest = 3;
                        break;
                    case D2Data.QUEST_A3Q4:
                        act = 3; quest = 4;
                        break;
                    case D2Data.QUEST_A3Q5:
                        act = 3; quest = 5;
                        break;
                    case D2Data.QUEST_A3Q6:
                        act = 3; quest = 6;
                        break;

                    case D2Data.QUEST_A4Q1:
                        act = 4; quest = 1;
                        break;
                    case D2Data.QUEST_A4Q2:
                        act = 4; quest = 2;
                        break;
                    case D2Data.QUEST_A4Q3:
                        act = 4; quest = 3;
                        break;

                    case D2Data.QUEST_A5Q1:
                        act = 5; quest = 1;
                        break;
                    case D2Data.QUEST_A5Q2:
                        act = 5; quest = 2;
                        break;
                    case D2Data.QUEST_A5Q3:
                        act = 5; quest = 3;
                        break;
                    case D2Data.QUEST_A5Q4:
                        act = 5; quest = 4;
                        break;
                    case D2Data.QUEST_A5Q5:
                        act = 5; quest = 5;
                        break;
                    case D2Data.QUEST_A5Q6:
                        act = 5; quest = 6;
                        break;

                    default:
                        act = 0;
                        quest = 0;
                        break;
                }
                if (act > 0 && quest > 0)
                {
                    for ( int x = 0; x < 16; x++ )
                    {

                        questBits[act - 1, quest - 1, x].Invoke(new Action(delegate () {
                            if (isNthBitSet(value, x))
                            {
                                if (questBits[act - 1, quest - 1, x].BackColor != Color.GreenYellow)
                                {
                                    questBits[act - 1, quest - 1, x].BackColor = Color.GreenYellow;
                                }
                            } else if (questBits[act - 1, quest - 1, x].BackColor != Color.LightGray)
                            {
                                questBits[act - 1, quest - 1, x].BackColor = Color.LightGray;
                            }
                        }));
                    }
                }
            }
        }
        private bool isNthBitSet(short c, int n)
        {
            int[] mask = { 128, 64, 32, 16, 8, 4, 2, 1 };
            if (n >= 8)
            {
                c = (short)((int)c >> 8);
                n -= 8;
            }
            return ((c & mask[n]) != 0);
        }

        private void DebugWindow_Load(object sender, EventArgs e)
        {
            actlabels = new Label[5];
            questlabels = new Label[5,6];
            questBits = new Label[5, 6, 16];
            for ( int i = 0; i < 5; i ++ )
            {
                actlabels[i] = new Label();
                actlabels[i].Text = "Act " + (i + 1);
                actlabels[i].SetBounds(20, i * 100, 40, 16);
                this.Controls.Add(actlabels[i]);
                for ( int j = 0; j< 6; j++ )
                {
                    questlabels[i, j] = new Label();
                    questlabels[i, j].Text = "Quest " + (j + 1);
                    questlabels[i, j].SetBounds(100, i * 100+(j * 16), 80, 16);
                    this.Controls.Add(questlabels[i, j]);
                    for (int k = 0; k < 16; k++)
                    {
                        questBits[i, j, k] = new Label();
                        questBits[i, j, k].Text = ""+k;
                        questBits[i, j, k].Font = new Font(Font.SystemFontName, 6.5f);
                        questBits[i, j, k].SetBounds(100 +80+ (k)*24, i * 100 + (j * 16), 20, 14);
                        questBits[i, j, k].BorderStyle = BorderStyle.FixedSingle;
                        questBits[i, j, k].BackColor = Color.LightGray;
                        this.Controls.Add(questBits[i, j, k]);
                    }
                }
            }
        }
    }
}
