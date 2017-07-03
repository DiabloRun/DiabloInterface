namespace Zutatensuppe.DiabloInterface.Gui
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    using Zutatensuppe.D2Reader;
    using Zutatensuppe.D2Reader.Struct.Item;
    using Zutatensuppe.DiabloInterface.Business.AutoSplits;
    using Zutatensuppe.DiabloInterface.Gui.Controls;
    using Zutatensuppe.DiabloInterface.Gui.Forms;

    public partial class DebugWindow : WsExCompositedForm
    {
        /// <summary>
        /// Helper class for binding/unbinding autosplit event handlers.
        /// </summary>
        class AutosplitBinding
        {
            bool didUnbind;
            AutoSplit autoSplit;
            Action<AutoSplit> reachedHandler;
            Action<AutoSplit> resetHandler;

            public AutosplitBinding(AutoSplit autoSplit, Action<AutoSplit> reachedHandler, Action<AutoSplit> resetHandler)
            {
                this.autoSplit = autoSplit;
                this.reachedHandler = reachedHandler;
                this.resetHandler = resetHandler;

                this.autoSplit.Reached += reachedHandler;
                this.autoSplit.Reset += resetHandler;
            }

            ~AutosplitBinding()
            {
                Unbind();
            }

            /// <summary>
            /// Unbding the autosplit handlers.
            /// </summary>
            public void Unbind()
            {
                if (didUnbind) return;

                didUnbind = true;
                autoSplit.Reached -= reachedHandler;
                autoSplit.Reset -= resetHandler;
            }
        }

        Label[] ActLabelsNormal;
        Label[] ActLabelsNightmare;
        Label[] ActLabelsHell;

        QuestDebugRow[,] QuestRowsNormal;
        QuestDebugRow[,] QuestRowsNightmare;
        QuestDebugRow[,] QuestRowsHell;

        List<AutosplitBinding> autoSplitBindings;

        Dictionary<BodyLocation, string> itemStrings;

        public DebugWindow()
        {
            InitializeComponent();
        }

        public void UpdateQuestData(ushort[] questBuffer, int difficulty)
        {
            QuestDebugRow[,] questRows;
            ushort questBits;

            switch (difficulty)
            {
                case 2: questRows = QuestRowsHell; break;
                case 1: questRows = QuestRowsNightmare; break;
                case 0:
                default: questRows = QuestRowsNormal; break;
            }

            for (int i = 0; i < questBuffer.Length; i++)
            {
                questBits = questBuffer[i];

                D2QuestHelper.D2Quest q = D2QuestHelper.GetByQuestBufferIndex(i);
                if (q != null)
                {
                    try
                    {
                        questRows[q.Act - 1, q.Quest - 1].Update(questBits);
                    }
                    catch (NullReferenceException)
                    {
                        // System.NullReferenceException
                    }
                }
            }
        }

        void ClearAutoSplitBindings()
        {
            if (autoSplitBindings == null)
            {
                autoSplitBindings = new List<AutosplitBinding>();
            }

            foreach (var binding in autoSplitBindings)
            {
                binding.Unbind();
            }

            autoSplitBindings.Clear();
        }

        public void UpdateAutosplits(List<AutoSplit> autoSplits)
        {
            if (DesignMode) return;
            // Unbinds and clears the binding list.
            ClearAutoSplitBindings();

            int y = 0;
            autosplitPanel.Controls.Clear();
            foreach (AutoSplit autoSplit in autoSplits)
            {
                Label splitLabel = new Label();
                splitLabel.SetBounds(0, y, autosplitPanel.Bounds.Width, 16);
                splitLabel.Text = autoSplit.Name;
                splitLabel.ForeColor = autoSplit.IsReached ? Color.Green : Color.Red;

                Action<AutoSplit> splitReached = s => splitLabel.ForeColor = Color.Green;
                Action<AutoSplit> splitReset = s => splitLabel.ForeColor = Color.Red;

                // Bind autosplit events.
                var binding = new AutosplitBinding(autoSplit, splitReached, splitReset);
                autoSplitBindings.Add(binding);

                autosplitPanel.Controls.Add(splitLabel);
                y += 16;
            }
        }

        private void LoadQuests(Label[] actLabels, QuestDebugRow[,] questRows, TabPage tabPage)
        {
            int y;
            for (int i = 0; i < 5; i++)
            {
                y = i * 100 - (i > 3 ? 3 * 16 : 0);
                actLabels[i] = new Label();
                actLabels[i].Text = "Act " + (i + 1);
                actLabels[i].Width = 40;
                actLabels[i].Location = new Point(20, y);
                tabPage.Controls.Add(actLabels[i]);
                for (int j = 0; j < (i == 3 ? 3 : 6); j++)
                {
                    questRows[i, j] = new QuestDebugRow(D2QuestHelper.GetByActAndQuest(i + 1, j + 1));
                    questRows[i, j].Location = new Point(60, y + (j * 16));
                    tabPage.Controls.Add(questRows[i, j]);
                }
            }
        }

        private void DebugWindow_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            ActLabelsNormal = new Label[5];
            QuestRowsNormal = new QuestDebugRow[5, 6];
            LoadQuests(ActLabelsNormal, QuestRowsNormal, tabPage1);

            ActLabelsNightmare = new Label[5];
            QuestRowsNightmare = new QuestDebugRow[5, 6];
            LoadQuests(ActLabelsNightmare, QuestRowsNightmare, tabPage2);

            ActLabelsHell = new Label[5];
            QuestRowsHell = new QuestDebugRow[5, 6];
            LoadQuests(ActLabelsHell, QuestRowsHell, tabPage3);
        }

        public void UpdateItemStats(Dictionary<BodyLocation, string> itemStrings)
        {
            if (DesignMode) return;


            this.itemStrings = itemStrings;
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            if (itemStrings == null || !itemStrings.ContainsKey(BodyLocation.Head)) return;
            textItemDesc.Text = itemStrings[BodyLocation.Head];
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            textItemDesc.Text = "";
        }

        private void label10_MouseEnter(object sender, EventArgs e)
        {
            if (itemStrings == null || !itemStrings.ContainsKey(BodyLocation.Amulet)) return;
            textItemDesc.Text = itemStrings[BodyLocation.Amulet];
        }

        private void label10_MouseLeave(object sender, EventArgs e)
        {
            textItemDesc.Text = "";
        }

        private void label3_MouseEnter(object sender, EventArgs e)
        {
            if (itemStrings == null || !itemStrings.ContainsKey(BodyLocation.PrimaryRight)) return;
            textItemDesc.Text = itemStrings[BodyLocation.PrimaryRight];
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            textItemDesc.Text = "";
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            if (itemStrings == null || !itemStrings.ContainsKey(BodyLocation.PrimaryLeft)) return;
            textItemDesc.Text = itemStrings[BodyLocation.PrimaryLeft];
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            textItemDesc.Text = "";
        }

        private void label4_MouseEnter(object sender, EventArgs e)
        {
            if (itemStrings == null || !itemStrings.ContainsKey(BodyLocation.BodyArmor)) return;
            textItemDesc.Text = itemStrings[BodyLocation.BodyArmor];
        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {
            textItemDesc.Text = "";
        }

        private void label7_MouseEnter(object sender, EventArgs e)
        {
            if (itemStrings == null || !itemStrings.ContainsKey(BodyLocation.RingLeft)) return;
            textItemDesc.Text = itemStrings[BodyLocation.RingLeft];
        }

        private void label7_MouseLeave(object sender, EventArgs e)
        {
            textItemDesc.Text = "";
        }

        private void label8_MouseEnter(object sender, EventArgs e)
        {
            if (itemStrings == null || !itemStrings.ContainsKey(BodyLocation.RingRight)) return;
            textItemDesc.Text = itemStrings[BodyLocation.RingRight];
        }

        private void label8_MouseLeave(object sender, EventArgs e)
        {
            textItemDesc.Text = "";
        }

        private void label5_MouseEnter(object sender, EventArgs e)
        {
            if (itemStrings == null || !itemStrings.ContainsKey(BodyLocation.Gloves)) return;
            textItemDesc.Text = itemStrings[BodyLocation.Gloves];
        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            textItemDesc.Text = "";
        }

        private void label6_MouseEnter(object sender, EventArgs e)
        {
            if (itemStrings == null || !itemStrings.ContainsKey(BodyLocation.Belt)) return;
            textItemDesc.Text = itemStrings[BodyLocation.Belt];
        }

        private void label6_MouseLeave(object sender, EventArgs e)
        {
            textItemDesc.Text = "";
        }

        private void label9_MouseEnter(object sender, EventArgs e)
        {
            if (itemStrings == null || !itemStrings.ContainsKey(BodyLocation.Boots)) return;
            textItemDesc.Text = itemStrings[BodyLocation.Boots];
        }

        private void label9_MouseLeave(object sender, EventArgs e)
        {
            textItemDesc.Text = "";
        }
    }
}
