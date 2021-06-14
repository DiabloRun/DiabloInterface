using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using Zutatensuppe.D2Reader;
using Zutatensuppe.D2Reader.Models;
using Zutatensuppe.D2Reader.Struct.Item;
using Zutatensuppe.DiabloInterface.Gui.Controls;
using Zutatensuppe.DiabloInterface.Gui.Forms;
using Zutatensuppe.DiabloInterface.Lib;
using Zutatensuppe.DiabloInterface.Lib.Plugin;

namespace Zutatensuppe.DiabloInterface.Gui
{
    public class DebugWindow : WsExCompositedForm
    {
        static readonly Lib.ILogger Logger = Lib.Logging.CreateLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDiabloInterface di;

        readonly Dictionary<GameDifficulty, QuestDebugRow[,]> questRows =
            new Dictionary<GameDifficulty, QuestDebugRow[,]>();

        List<ItemInfo> items;

        Label clickedLabel;
        Label hoveredLabel;

        private Dictionary<Label, BodyLocation> locs;
        private RichTextBox textItemDesc;

        public DebugWindow(IDiabloInterface di)
        {
            Logger.Info("Creating debug window.");

            this.di = di;

            RegisterServiceEventHandlers();

            // Unregister event handlers when we are done.
            Disposed += (sender, args) =>
            {
                Logger.Info("Disposing debug window.");
                UnregisterServiceEventHandlers();
            };

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            TabPage itemsTab()
            {
                Color bg = Color.FromArgb(64, 64, 64);
                Color fg = Color.Aquamarine;
                Label l(Point loc, Size s, string text)
                {
                    var label = new Label();
                    label.BackColor = bg;
                    label.ForeColor = fg;
                    label.Location = loc;
                    label.Size = s;
                    label.Text = text;
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.Click += new EventHandler(LabelClick);
                    label.MouseEnter += new EventHandler(LabelMouseEnter);
                    label.MouseLeave += new EventHandler(LabelMouseLeave);
                    return label;
                }

                locs = new Dictionary<Label, BodyLocation>
                {
                    {l(new Point(80, 0), new Size(40, 40), "Head"), BodyLocation.Head},
                    {l(new Point(130, 20), new Size(20, 20), "A"), BodyLocation.Amulet},
                    {l(new Point(160, 40), new Size(40, 80), "Right Arm"), BodyLocation.PrimaryRight},
                    {l(new Point(0, 40), new Size(40, 80), "Left Arm"), BodyLocation.PrimaryLeft},
                    {l(new Point(80, 50), new Size(40, 60), "Body"), BodyLocation.BodyArmor},
                    {l(new Point(50, 130), new Size(20, 20), "L"), BodyLocation.RingLeft},
                    {l(new Point(130, 130), new Size(20, 20), "R"), BodyLocation.RingRight},
                    {l(new Point(0, 130), new Size(40, 40), "Gloves"), BodyLocation.Gloves},
                    {l(new Point(80, 130), new Size(40, 20), "Belt"), BodyLocation.Belt},
                    {l(new Point(160, 130), new Size(40, 40), "Boots"), BodyLocation.Boots},
                };

                textItemDesc = new RichTextBox();
                textItemDesc.Location = new Point(0, 180);
                textItemDesc.Size = new Size(200, 180);
                textItemDesc.Text = "";

                var itemsPanel = new TabPage();
                itemsPanel.Text = "Items";
                itemsPanel.Controls.Add(textItemDesc);
                foreach (var p in locs)
                    itemsPanel.Controls.Add(p.Key);
                return itemsPanel;
            }

            TabPage questsTab()
            {
                TabPage tabpage(GameDifficulty difficulty)
                {
                    var t = new TabPage();
                    t.AutoScroll = true;
                    t.Text = Enum.GetName(typeof(GameDifficulty), difficulty);
                    questRows[difficulty] = CreateQuestRow(t);
                    return t;
                }

                var questTabs = new TabControl();
                questTabs.Controls.Add(tabpage(GameDifficulty.Normal));
                questTabs.Controls.Add(tabpage(GameDifficulty.Nightmare));
                questTabs.Controls.Add(tabpage(GameDifficulty.Hell));
                questTabs.Dock = DockStyle.Fill;
                questTabs.Location = new Point(3, 16);
                questTabs.SelectedIndex = 0;
                questTabs.Size = new Size(549, 536);
                foreach (TabPage c in questTabs.Controls)
                {
                    c.UseVisualStyleBackColor = true;
                    c.Dock = DockStyle.Fill;
                }

                var quests = new TabPage();
                quests.Controls.Add(questTabs);
                quests.Text = "Quest-Bits";
                return quests;
            }

            var tabs = new TabControl();
            tabs.Dock = DockStyle.Fill;
            tabs.Controls.Add(itemsTab());
            tabs.Controls.Add(questsTab());
            foreach (var p in di.plugins.CreateControls<IPluginDebugRenderer>())
            {
                var tp = new TabPage();
                tp.Controls.Add(p.Value);
                tp.Text = p.Key;
                tabs.Controls.Add(tp);
            }

            foreach (TabPage c in tabs.Controls)
            {
                c.UseVisualStyleBackColor = true;
                c.Dock = DockStyle.Fill;
            }

            Controls.Add(tabs);

            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(875, 571);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = Properties.Resources.di;
            Text = "Debug";
            ResumeLayout(false);
        }

        void RegisterServiceEventHandlers()
        {
            di.game.DataRead += GameServiceOnDataRead;
        }

        void UnregisterServiceEventHandlers()
        {
            di.game.DataRead -= GameServiceOnDataRead;
        }

        void GameServiceOnDataRead(object sender, DataReadEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => GameServiceOnDataRead(sender, e)));
                return;
            }

            UpdateQuestData(e.Quests, GameDifficulty.Normal);
            UpdateQuestData(e.Quests, GameDifficulty.Nightmare);
            UpdateQuestData(e.Quests, GameDifficulty.Hell);

            UpdateItemStats(e.Character.Items);
        }

        void UpdateQuestData(Quests quests, GameDifficulty difficulty)
        {
            var q = quests.ByDifficulty(difficulty);
            if (q == null)
                return;

            QuestDebugRow[,] rows = questRows[difficulty];

            foreach (var quest in q)
            {
                if (quest.Act <= 0 || quest.ActOrder <= 0)
                {
                    // cow king quest is set to act 0 act order 0 atm
                    continue;
                }

                try
                {
                    rows[quest.Act - 1, quest.ActOrder - 1].Update(quest);
                }
                catch (NullReferenceException)
                {
                }
            }
        }

        static QuestDebugRow[,] CreateQuestRow(Control tabPage)
        {
            var questRows = new QuestDebugRow[5, 6];
            for (int actIndex = 0; actIndex < 5; actIndex++)
            {
                int y = actIndex * 100 - (actIndex > 3 ? 3 * 16 : 0);
                tabPage.Controls.Add(new Label
                {
                    Text = "Act " + (actIndex + 1),
                    Width = 40,
                    Location = new Point(20, y)
                });

                for (int questIndex = 0; questIndex < (actIndex == 3 ? 3 : 6); questIndex++)
                {
                    var quest = QuestFactory.CreateByActAndOrder(actIndex + 1, questIndex + 1);
                    var row = new QuestDebugRow(quest);
                    row.Location = new Point(60, y + (questIndex * 16));
                    tabPage.Controls.Add(row);
                    questRows[actIndex, questIndex] = row;
                }
            }
            return questRows;
        }

        void UpdateItemStats(List<ItemInfo> items)
        {
            this.items = items;
            UpdateItemDebugInformation();
        }
        
        void UpdateItemDebugInformation()
        {
            // hover has precedence vs clicked labels
            Label l = hoveredLabel ?? clickedLabel ?? null;
            if (l != null && locs.ContainsKey(l) && items != null)
            {
                foreach (var item in items)
                {
                    if (item.Location.BodyLocation == locs[l])
                    {
                        textItemDesc.Text = ItemString(item);
                        return;
                    }
                }
            }

            textItemDesc.Text = "";
        }

        private string ItemString(ItemInfo item)
        {
            StringBuilder s = new StringBuilder();
            s.Append(item.ItemName);
            s.Append(Environment.NewLine);
            foreach (string str in item.Properties)
            {
                s.Append("    ");
                s.Append(str);
                s.Append(Environment.NewLine);
            }
            return s.ToString();
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
                return;

            hoveredLabel = null;
            UpdateItemDebugInformation();
        }
    }
}
