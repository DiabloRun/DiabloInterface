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
    using Zutatensuppe.DiabloInterface.Core.Extensions;
    using Zutatensuppe.DiabloInterface.Core.Logging;
    using Zutatensuppe.DiabloInterface.Settings;

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
                if (value == null)
                {
                    settingsList = new List<ClassRuneSettings>();
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

        ClassRuneSettings CurrentSettings
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

        void InitializeComboBoxes()
        {
            InitializeCharacterClassComboBox();
            InitializeDifficultyComboBox();
            InitializeRuneComboBox();
            InitializeRunewordComboBox();
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
            else currentSettings.Class = (CharacterClass)comboBox.SelectedItem;

            RefreshSettingsItemText(currentSettings);
        }

        void DifficultyComboBoxOnSelectedValueChanged(object sender, EventArgs e)
        {
            if (currentSettings == null) return;
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
