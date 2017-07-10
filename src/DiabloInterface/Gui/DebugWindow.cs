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

        Dictionary<Label, BodyLocation> locs;
        private Label clickedLabel = null;
        private Label hoveredLabel = null;

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

            locs = new Dictionary<Label, BodyLocation>();
            locs.Add(label1, BodyLocation.Head);
            locs.Add(label10, BodyLocation.Amulet);
            locs.Add(label3, BodyLocation.PrimaryRight);
            locs.Add(label2, BodyLocation.PrimaryLeft);
            locs.Add(label4, BodyLocation.BodyArmor);
            locs.Add(label7, BodyLocation.RingLeft);
            locs.Add(label8, BodyLocation.RingRight);
            locs.Add(label5, BodyLocation.Gloves);
            locs.Add(label6, BodyLocation.Belt);
            locs.Add(label9, BodyLocation.Boots);
        }

        public void UpdateItemStats(Dictionary<BodyLocation, string> itemStrings)
        {
            if (DesignMode) return;
            this.itemStrings = itemStrings;
            UpdateItemDebugInformation();
        }
        
        private void UpdateItemDebugInformation()
        {
            if (InvokeRequired)
            {
                // Delegate call to UI thread.
                Invoke((Action)(() => UpdateItemDebugInformation()));
                return;
            }

            // hover has precedence vs clicked labels
            Label l = hoveredLabel != null ? hoveredLabel : (clickedLabel != null ? clickedLabel : null);
            if (l == null || itemStrings == null || !locs.ContainsKey(l) || !itemStrings.ContainsKey(locs[l]))
            {
                textItemDesc.Text = "";
                return;
            }
            
            textItemDesc.Text = itemStrings[locs[l]];
        }

        private void LabelClick(object sender, EventArgs e)
        {
            if (clickedLabel == sender)
            {
                clickedLabel = null;
                ((Label)sender).ForeColor = Color.Aquamarine;
            }
            else
            {
                clickedLabel = (Label)sender;
                ((Label)sender).ForeColor = Color.Yellow;
            }
            UpdateItemDebugInformation();
        }

        private void LabelMouseEnter(object sender, EventArgs e)
        {
            hoveredLabel = (Label)sender;
            UpdateItemDebugInformation();
        }

        private void LabelMouseLeave(object sender, EventArgs e)
        {
            if (hoveredLabel != sender)
            {
                return;
            }
            hoveredLabel = null;
            UpdateItemDebugInformation();
        }
        
    }
}
