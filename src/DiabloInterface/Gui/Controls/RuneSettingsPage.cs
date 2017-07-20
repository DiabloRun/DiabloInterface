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
    using Zutatensuppe.DiabloInterface.Business.Settings;
    using Zutatensuppe.DiabloInterface.Core.Extensions;
    using Zutatensuppe.DiabloInterface.Core.Logging;

    public partial class RuneSettingsPage : UserControl
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        public RuneSettingsPage()
        {
            Logger.Info("Initializing rune settings page.");

            InitializeComponent();
            InitializeComboBoxes();
        }

        ClassRuneSettings currentSettings;
        IReadOnlyList<ClassRuneSettings> settingsList;

        public IReadOnlyList<ClassRuneSettings> SettingsList
        {
            get => settingsList;
            set
            {
                settingsList = value;

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

        ClassRuneSettings CurrentSettings
        {
            set
            {
                currentSettings = value;
                if (currentSettings == null) return;

                characterClassComboBox.SelectedIndex = currentSettings.Class.HasValue
                    ? (int)currentSettings.Class.Value + 1 : 0;

                difficultyComboBox.SelectedIndex = currentSettings.Difficulty.HasValue
                    ? (int)currentSettings.Difficulty.Value + 1 : 0;

                foreach (Control control in runeFlowLayout.Controls)
                    control.Dispose();
                runeFlowLayout.Controls.Clear();

                currentSettings.Runes.ForEach(AddRune);
            }
        }

        void InitializeComboBoxes()
        {
            InitializeCharacterClassComboBox();
            InitializeDifficultyComboBox();
            InitializeRuneComboBox();
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

            AddRunes(runeComboBox);
            runeComboBox.Items.Add(new RuneListItem {Tag = "Separator"});
            AddRunewords(runeComboBox);
        }

        void AddRunes(ComboBox comboBox)
        {
            IEnumerable<RuneListItem> runes =
                from rune in Enum.GetValues(typeof(Rune)).OfType<Rune>()
                select RuneListItem.CreateRune(rune);
            comboBox.Items.AddRange(runes.OfType<object>().ToArray());
        }

        void AddRunewords(ComboBox comboBox)
        {
            IEnumerable<Runeword> runeWords = ReadRuneworsFile();
            object[] items = (from runeWord in runeWords
                    select RuneListItem.CreateRuneword(runeWord))
                .OfType<object>().ToArray();
            comboBox.Items.AddRange(items);
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
                return;

            var item = characterListBox.SelectedItem as CharacterClassListItem;
            if (item?.Settings == null) return;

            CurrentSettings = item.Settings;
        }

        void RuneButtonOnClick(object sender, EventArgs e)
        {
            var item = runeComboBox.SelectedItem as RuneListItem;
            if (item == null) return;

            switch (item.Tag)
            {
                case "Rune":
                    AddRune(item.Rune);
                    break;
                case "Runeword":
                    item.Runeword.Runes.ForEach(AddRune);
                    break;
                default:
                    return;
            }
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

        void RuneComboBoxOnSelectedValueChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;

            var item = comboBox?.SelectedItem as RuneListItem;
            if (item == null)
            {
                runeButton.Enabled = false;
                return;
            }

            if (item.Tag == "Separator")
                comboBox.SelectedIndex = -1;
            else runeButton.Enabled = true;
        }

        void CharacterClassComboBoxOnSelectedValueChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;

            if (comboBox.SelectedIndex == 0)
                currentSettings.Class = null;
            else currentSettings.Class = (CharacterClass)comboBox.SelectedItem;

            RefreshSettingsItemText(currentSettings);
        }

        void DifficultyComboBoxOnSelectedValueChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;

            if (comboBox.SelectedIndex == 0)
                currentSettings.Difficulty = null;
            else currentSettings.Difficulty = (GameDifficulty)comboBox.SelectedItem;

            RefreshSettingsItemText(currentSettings);
        }

        void RefreshSettingsItemText(ClassRuneSettings settings)
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
            List<ClassRuneSettings> settings = settingsList.ToList();
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
            public CharacterClassListItem(ClassRuneSettings settings)
            {
                Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            }

            public ClassRuneSettings Settings { get; }

            public override string ToString()
            {
                if (!Settings.Difficulty.HasValue)
                    return Settings.Class.HasValue ? $"{Settings.Class}" : "Default";

                return Settings.Class.HasValue
                    ? $"{Settings.Class} {Settings.Difficulty}"
                    : $"Default {Settings.Difficulty}";
            }
        }

        class RuneListItem
        {
            public string Tag { get; set; }
            public Runeword Runeword { get; private set; }
            public Rune Rune { get; private set; }

            public static RuneListItem CreateRuneword(Runeword runeword)
            {
                return new RuneListItem()
                {
                    Tag = "Runeword",
                    Runeword = runeword
                };
            }

            public static RuneListItem CreateRune(Rune rune)
            {
                return new RuneListItem()
                {
                    Tag = "Rune",
                    Rune = rune
                };
            }

            public override string ToString()
            {
                if (Tag == "Runeword") return Runeword?.Name ?? "Unknown Runeword";
                if (Tag == "Rune") return Rune.ToString();

                return new string('-', 20);
            }
        }
    }
}
