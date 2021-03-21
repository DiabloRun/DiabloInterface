namespace Zutatensuppe.DiabloInterface.Gui.Controls
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    using Newtonsoft.Json;

    using Zutatensuppe.D2Reader.Models;
    using Zutatensuppe.DiabloInterface.Lib;
    using Zutatensuppe.DiabloInterface.Lib.Extensions;

    public partial class RuneSettingsPage : UserControl
    {
        static readonly ILogger Logger = Logging.CreateLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public RuneSettingsPage()
        {
            Logger.Info("Initializing rune settings page.");

            InitializeComponent();

            InitializeCharacterClassComboBox();
            InitializeDifficultyComboBox();
            InitializeRuneComboBox();
            InitializeRunewordComboBox();
        }

        private SplitContainer splitContainer1;
        private GroupBox classSettingsGroupBox;
        private ListBox characterListBox;
        private FlowLayoutPanel runeFlowLayout;
        private ComboBox runeComboBox;
        private Button runeButton;
        private Label difficultyLabel;
        private Label classLabel;
        private ComboBox difficultyComboBox;
        private ComboBox characterClassComboBox;
        private GroupBox runesGroupBox;
        private GroupBox runeListEditBox;
        private Button deleteSettingsButton;
        private Button addSettingsButton;
        private TableLayoutPanel tableLayoutPanel1;
        private ComboBox runewordComboBox;
        private Button runewordButton;

        IClassRuneSettings currentSettings;

        IReadOnlyList<IClassRuneSettings> settingsList;

        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.classSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.deleteSettingsButton = new System.Windows.Forms.Button();
            this.addSettingsButton = new System.Windows.Forms.Button();
            this.characterListBox = new System.Windows.Forms.ListBox();
            this.runesGroupBox = new System.Windows.Forms.GroupBox();
            this.runeFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.runeListEditBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.difficultyComboBox = new System.Windows.Forms.ComboBox();
            this.characterClassComboBox = new System.Windows.Forms.ComboBox();
            this.classLabel = new System.Windows.Forms.Label();
            this.difficultyLabel = new System.Windows.Forms.Label();
            this.runeComboBox = new System.Windows.Forms.ComboBox();
            this.runeButton = new System.Windows.Forms.Button();
            this.runewordComboBox = new System.Windows.Forms.ComboBox();
            this.runewordButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.classSettingsGroupBox.SuspendLayout();
            this.runesGroupBox.SuspendLayout();
            this.runeListEditBox.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.classSettingsGroupBox);
            this.splitContainer1.Panel1MinSize = 140;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.runesGroupBox);
            this.splitContainer1.Panel2.Controls.Add(this.runeListEditBox);
            this.splitContainer1.Panel2MinSize = 200;
            this.splitContainer1.Size = new System.Drawing.Size(497, 440);
            this.splitContainer1.SplitterDistance = 149;
            // 
            // classSettingsGroupBox
            // 
            this.classSettingsGroupBox.Controls.Add(this.deleteSettingsButton);
            this.classSettingsGroupBox.Controls.Add(this.addSettingsButton);
            this.classSettingsGroupBox.Controls.Add(this.characterListBox);
            this.classSettingsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.classSettingsGroupBox.Location = new System.Drawing.Point(0, 0);
            this.classSettingsGroupBox.MinimumSize = new System.Drawing.Size(140, 0);
            this.classSettingsGroupBox.Size = new System.Drawing.Size(149, 440);
            this.classSettingsGroupBox.Text = "Class Settings";
            // 
            // deleteSettingsButton
            // 
            this.deleteSettingsButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.deleteSettingsButton.Location = new System.Drawing.Point(31, 414);
            this.deleteSettingsButton.Size = new System.Drawing.Size(59, 23);
            this.deleteSettingsButton.Text = "Remove";
            this.deleteSettingsButton.Click += new System.EventHandler(this.DeleteSettingsButtonOnClick);
            // 
            // addSettingsButton
            // 
            this.addSettingsButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.addSettingsButton.Location = new System.Drawing.Point(96, 414);
            this.addSettingsButton.Size = new System.Drawing.Size(50, 23);
            this.addSettingsButton.Text = "Add";
            this.addSettingsButton.Click += new System.EventHandler(this.AddSettingsButtonOnClick);
            // 
            // characterListBox
            // 
            this.characterListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.characterListBox.FormattingEnabled = true;
            this.characterListBox.Location = new System.Drawing.Point(3, 16);
            this.characterListBox.Size = new System.Drawing.Size(143, 394);
            this.characterListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CharacterListBoxOnMouseDoubleClick);
            // 
            // runesGroupBox
            // 
            this.runesGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.runesGroupBox.Controls.Add(this.runeFlowLayout);
            this.runesGroupBox.Location = new System.Drawing.Point(3, 3);
            this.runesGroupBox.Size = new System.Drawing.Size(337, 313);
            this.runesGroupBox.Text = "Runes";
            // 
            // runeFlowLayout
            // 
            this.runeFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runeFlowLayout.Location = new System.Drawing.Point(3, 16);
            this.runeFlowLayout.Size = new System.Drawing.Size(331, 294);
            // 
            // runeListEditBox
            // 
            this.runeListEditBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.runeListEditBox.Controls.Add(this.tableLayoutPanel1);
            this.runeListEditBox.Controls.Add(this.runeComboBox);
            this.runeListEditBox.Controls.Add(this.runeButton);
            this.runeListEditBox.Controls.Add(this.runewordComboBox);
            this.runeListEditBox.Controls.Add(this.runewordButton);
            this.runeListEditBox.Location = new System.Drawing.Point(3, 322);
            this.runeListEditBox.Size = new System.Drawing.Size(337, 115);
            this.runeListEditBox.Text = "Edit";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.difficultyComboBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.characterClassComboBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.classLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.difficultyLabel, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 14);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(331, 38);
            // 
            // difficultyComboBox
            // 
            this.difficultyComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.difficultyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.difficultyComboBox.FormattingEnabled = true;
            this.difficultyComboBox.Location = new System.Drawing.Point(168, 17);
            this.difficultyComboBox.Size = new System.Drawing.Size(160, 21);
            this.difficultyComboBox.SelectedValueChanged += new System.EventHandler(this.DifficultyComboBoxOnSelectedValueChanged);
            // 
            // characterClassComboBox
            // 
            this.characterClassComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.characterClassComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.characterClassComboBox.FormattingEnabled = true;
            this.characterClassComboBox.Location = new System.Drawing.Point(3, 17);
            this.characterClassComboBox.Size = new System.Drawing.Size(159, 21);
            this.characterClassComboBox.SelectedValueChanged += new System.EventHandler(this.CharacterClassComboBoxOnSelectedValueChanged);
            // 
            // classLabel
            // 
            this.classLabel.AutoSize = true;
            this.classLabel.Location = new System.Drawing.Point(3, 0);
            this.classLabel.Size = new System.Drawing.Size(32, 13);
            this.classLabel.Text = "Class";
            // 
            // difficultyLabel
            // 
            this.difficultyLabel.AutoSize = true;
            this.difficultyLabel.Location = new System.Drawing.Point(168, 0);
            this.difficultyLabel.Size = new System.Drawing.Size(47, 13);
            this.difficultyLabel.Text = "Difficulty";
            // 
            // runeComboBox
            // 
            this.runeComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.runeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.runeComboBox.FormattingEnabled = true;
            this.runeComboBox.Location = new System.Drawing.Point(6, 57);
            this.runeComboBox.Size = new System.Drawing.Size(229, 21);
            // 
            // runeButton
            // 
            this.runeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.runeButton.Location = new System.Drawing.Point(241, 55);
            this.runeButton.Size = new System.Drawing.Size(90, 23);
            this.runeButton.Text = "Add";
            this.runeButton.Click += new System.EventHandler(this.RuneButtonOnClick);
            // 
            // runewordComboBox
            // 
            this.runewordComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.runewordComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.runewordComboBox.FormattingEnabled = true;
            this.runewordComboBox.Location = new System.Drawing.Point(6, 86);
            this.runewordComboBox.Size = new System.Drawing.Size(229, 21);
            // 
            // runewordButton
            // 
            this.runewordButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.runewordButton.Location = new System.Drawing.Point(241, 84);
            this.runewordButton.Size = new System.Drawing.Size(90, 23);
            this.runewordButton.Text = "Add";
            this.runewordButton.Click += new System.EventHandler(this.RunewordButtonOnClick);
            // 
            // RuneSettingsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "RuneSettingsPage";
            this.Size = new System.Drawing.Size(497, 440);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.classSettingsGroupBox.ResumeLayout(false);
            this.runesGroupBox.ResumeLayout(false);
            this.runeListEditBox.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
        }

        public IReadOnlyList<IClassRuneSettings> SettingsList
        {
            get => settingsList;
            set
            {
                if (value == null)
                {
                    settingsList = new List<IClassRuneSettings>();
                } else
                {
                    settingsList = value;
                }

                var index = characterListBox.SelectedIndex;
                characterListBox.Items.Clear();
                IList<CharacterClassListItem> items =
                    settingsList.Select(item => new CharacterClassListItem(item)).ToList();
                characterListBox.Items.AddRange(items.OfType<object>().ToArray());
                if (index >= characterListBox.Items.Count)
                {
                    index = characterListBox.Items.Count - 1;
                }
                
                characterListBox.SelectedIndex = items.Any() ? (index < 0 ? 0 : index) : -1;

                DisplayCharacterClassSettings();
            }
        }

        IClassRuneSettings CurrentSettings
        {
            set
            {
                currentSettings = value;
                if (currentSettings == null)
                {
                    runeButton.Enabled = false;
                    runewordButton.Enabled = false;
                    characterClassComboBox.Enabled = false;
                    characterClassComboBox.SelectedIndex = -1;
                    difficultyComboBox.Enabled = false;
                    difficultyComboBox.SelectedIndex = -1;
                    runeFlowLayout.Controls.ClearAndDispose();

                    return;
                }

                runeButton.Enabled = true;
                runewordButton.Enabled = true;

                characterClassComboBox.Enabled = true;
                characterClassComboBox.SelectedIndex = currentSettings.Class.HasValue
                    ? (int)currentSettings.Class.Value + 1 : 0;

                difficultyComboBox.Enabled = true;
                difficultyComboBox.SelectedIndex = currentSettings.Difficulty.HasValue
                    ? (int)currentSettings.Difficulty.Value + 1 : 0;

                runeFlowLayout.Controls.ClearAndDispose();
                currentSettings.Runes.ForEach(AddRune);
            }
        }

        void InitializeCharacterClassComboBox()
        {
            characterClassComboBox.Items.Clear();
            characterClassComboBox.Items.Add("Default");
            AddEnumValues(characterClassComboBox, typeof(CharacterClass));
        }

        void InitializeDifficultyComboBox()
        {
            difficultyComboBox.Items.Clear();
            difficultyComboBox.Items.Add("Default");
            AddEnumValues(difficultyComboBox, typeof(GameDifficulty));
        }

        void InitializeRuneComboBox()
        {
            runeComboBox.Items.Clear();
            AddEnumValues(runeComboBox, typeof(Rune));
            runeComboBox.SelectedIndex = Math.Min(runeComboBox.Items.Count, 0);
        }

        void InitializeRunewordComboBox()
        {
            runewordComboBox.Items.Clear();
            AddRunewords(runewordComboBox);
            runewordComboBox.SelectedIndex = Math.Min(runewordComboBox.Items.Count, 0);
        }

        void AddRunewords(ComboBox comboBox)
        {
            IEnumerable<Runeword> runewords = ReadRuneworsFile();
            IEnumerable<RunewordListItem> items =
                from runeword in runewords
                select new RunewordListItem(runeword);
            comboBox.Items.AddRange(items.OfType<object>().ToArray());
        }

        void AddEnumValues(ComboBox comboBox, Type enumType)
        {
            if (!enumType.IsEnum) throw new ArgumentException(@"Must be enum type.", nameof(enumType));
            object[] enumValues = Enum.GetValues(enumType).OfType<object>().ToArray();
            comboBox.Items.AddRange(enumValues);
        }

        IEnumerable<Runeword> ReadRuneworsFile()
        {
            using (var stream = new MemoryStream(Properties.Resources.runewords))
            using (var streamReader = new StreamReader(stream))
            using (var reader = new JsonTextReader(streamReader))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<List<Runeword>>(reader);
            }
        }

        void DisplayCharacterClassSettings()
        {
            if (characterListBox.SelectedIndex < 0)
            {
                if (characterListBox.Items.Count <= 0)
                    CurrentSettings = null;

                return;
            }

            var item = characterListBox.SelectedItem as CharacterClassListItem;
            if (item?.Settings == null) return;

            CurrentSettings = item.Settings;
        }

        void RuneButtonOnClick(object sender, EventArgs e)
        {
            if (runeComboBox.SelectedIndex < 0) return;
            AddRune((Rune)runeComboBox.SelectedItem);
        }

        void RunewordButtonOnClick(object sender, EventArgs e)
        {
            var item = runewordComboBox.SelectedItem as RunewordListItem;
            item?.Runeword.Runes.ForEach(AddRune);
        }

        void AddRune(Rune rune)
        {
            var element = new RuneDisplayElement(rune);
            element.RemoveButtonClicked += RuneElementOnRemoveButtonClicked;
            runeFlowLayout.Controls.Add(element);

            UpdateSettingsRuneList();
        }

        void RuneElementOnRemoveButtonClicked(object sender, EventArgs eventArgs)
        {
            var control = sender as RuneDisplayElement;
            if (control == null) return;

            runeFlowLayout.Controls.Remove(control);
            control.Dispose();

            UpdateSettingsRuneList();
        }

        void UpdateSettingsRuneList()
        {
            currentSettings.Runes =
                (from RuneDisplayElement element in runeFlowLayout.Controls
                 select element.Rune).ToList();
        }

        void CharacterClassComboBoxOnSelectedValueChanged(object sender, EventArgs e)
        {
            if (currentSettings == null) return;
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;

            if (comboBox.SelectedIndex == 0)
                currentSettings.Class = null;
            else
                currentSettings.Class = (CharacterClass)comboBox.SelectedItem;

            RefreshSettingsItemText(currentSettings);
        }

        void DifficultyComboBoxOnSelectedValueChanged(object sender, EventArgs e)
        {
            if (currentSettings == null) return;
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;

            if (comboBox.SelectedIndex == 0)
                currentSettings.Difficulty = null;
            else
                currentSettings.Difficulty = (GameDifficulty)comboBox.SelectedItem;

            RefreshSettingsItemText(currentSettings);
        }

        void RefreshSettingsItemText(IClassRuneSettings settings)
        {
            var items = characterListBox.Items;
            for (var i = 0; i < items.Count; ++i)
            {
                var item = (CharacterClassListItem)items[i];
                if (item.Settings != settings) continue;

                // Modifying the item collection forces the text to update.
                items[i] = item;
                break;
            }
        }

        void AddSettingsButtonOnClick(object sender, EventArgs e)
        {
            List<IClassRuneSettings> settings = settingsList.ToList();
            settings.Add(new ClassRuneSettings());
            SettingsList = settings;

            characterListBox.SelectedIndex = characterListBox.Items.Count - 1;
            CurrentSettings = settings.Last();
        }

        void DeleteSettingsButtonOnClick(object sender, EventArgs e)
        {
            if (characterListBox.SelectedIndex < 0) return;

            var selected = characterListBox.SelectedItem as CharacterClassListItem;
            var settings = selected?.Settings;

            if (settings == null) return;

            SettingsList = settingsList.Where(s => s != settings).ToList();
        }

        void CharacterListBoxOnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            var listBox = (ListBox)sender;
            var index = listBox.IndexFromPoint(e.Location);
            if (index == ListBox.NoMatches) return;

            var item = listBox.Items[index] as CharacterClassListItem;
            if (item == null) return;

            CurrentSettings = item.Settings;
        }

        class CharacterClassListItem
        {
            public CharacterClassListItem(IClassRuneSettings settings)
            {
                Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            }

            public IClassRuneSettings Settings { get; }

            public override string ToString()
            {
                if (!Settings.Difficulty.HasValue)
                    return Settings.Class.HasValue ? $"{Settings.Class}" : "Default";

                return Settings.Class.HasValue
                    ? $"{Settings.Class} {Settings.Difficulty}"
                    : $"Default {Settings.Difficulty}";
            }
        }

        class RunewordListItem
        {
            public Runeword Runeword { get; }

            public RunewordListItem(Runeword runeword)
            {
                Runeword = runeword ?? throw new ArgumentNullException(nameof(runeword));
            }

            public override string ToString() => Runeword.Name;
        }
    }
}
