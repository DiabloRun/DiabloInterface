using DiabloInterface.D2;
using DiabloInterface.D2.Readers;
using DiabloInterface.D2.Struct;
using DiabloInterface.Gui.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiabloInterface.Gui
{
    public partial class DebugWindow : Form
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
                    catch (System.NullReferenceException ex)
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
                actLabels[i].Width = 80;
                actLabels[i].Location = new Point(20, y);
                tabPage.Controls.Add(actLabels[i]);
                for (int j = 0; j < (i == 3 ? 3 : 6); j++)
                {
                    questRows[i, j] = new QuestDebugRow(D2QuestHelper.GetByActAndQuest(i + 1, j + 1));
                    questRows[i, j].Location = new Point(100, y + (j * 16));
                    tabPage.Controls.Add(questRows[i, j]);
                }
            }
        }

        private void DebugWindow_Load(object sender, EventArgs e)
        {
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

        public void UpdateItemStats(ProcessMemoryReader r, D2MemoryTable memory, D2Unit pl)
        {
            InventoryReader inventoryReader = new InventoryReader(r, memory);
            UnitReader unitReader = new UnitReader(r, memory);

            // Build filter to get only equipped items.
            Func<D2ItemData, bool> filter = data => data.BodyLoc != BodyLocation.None;
            foreach (D2Unit item in inventoryReader.EnumerateInventory(filter))
            {
                List<D2Stat> itemStats = unitReader.GetStats(item);
                if (itemStats == null) continue;

                StringBuilder statBuilder = new StringBuilder();
                statBuilder.Append(inventoryReader.ItemReader.GetFullItemName(item));

                statBuilder.Append("\n");
                List<string> magicalStrings = inventoryReader.ItemReader.GetMagicalStrings(item);
                foreach (string str in magicalStrings)
                {
                    statBuilder.Append("    ");
                    statBuilder.Append(str);
                    statBuilder.Append("\n");
                }

                Control c = null;
                D2ItemData itemData = r.Read<D2ItemData>(item.UnitData);
                switch (itemData.BodyLoc)
                {
                    case BodyLocation.Head: c = tabPageHead; break;
                    case BodyLocation.Amulet: c = tabPageAmulet; break;
                    case BodyLocation.BodyArmor: c = tabPageBody; break;
                    case BodyLocation.PrimaryRight: c = tabPageWeaponRight; break;
                    case BodyLocation.PrimaryLeft: c = tabPageWeaponLeft; break;
                    case BodyLocation.RingRight: c = tabPageRingRight; break;
                    case BodyLocation.RingLeft: c = tabPageRingLeft; break;
                    //case BodyLocation.SecondaryLeft: c = tabPageRingRight; break;
                    //case BodyLocation.SecondaryRight: c = tabPageRingLeft; break;
                    case BodyLocation.Belt: c = tabPageBelt; break;
                    case BodyLocation.Boots: c = tabPageFeet; break;
                    case BodyLocation.Gloves: c = tabPageHand; break;
                }
                if (c != null)
                {
                    if (c.Controls.Count == 0)
                    {
                        c.Invoke(new Action(delegate () {
                            c.Controls.Add(new RichTextBox());
                            c.Controls[0].Dock = DockStyle.Fill;
                        }));
                    }
                    c.Controls[0].Invoke(new Action(() => c.Controls[0].Text = statBuilder.ToString()));
                }
            }
        }
    }
}
