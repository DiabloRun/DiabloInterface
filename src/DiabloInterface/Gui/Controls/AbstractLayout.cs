namespace Zutatensuppe.DiabloInterface.Gui.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    using Zutatensuppe.D2Reader;
    using Zutatensuppe.D2Reader.Models;
    using Zutatensuppe.DiabloInterface.Lib;
    using Zutatensuppe.DiabloInterface.Lib.Extensions;
    using Zutatensuppe.DiabloInterface.Lib.Services;

    class Def
    {
        public string name;
        public string maxString;
        public Func<ApplicationConfig, Tuple<bool, Color, int>> settings;
        public Label[] labels;
        public Dictionary<Label, string> defaults;
        public bool enabled;
        public Def(string name, string maxString, Func<ApplicationConfig, Tuple<bool, Color, int>> settings, string[] labels)
        {
            this.name = name;
            this.maxString = maxString;
            this.settings = settings;
            this.labels = (from n in labels select new Label() { Text = n }).ToArray();
            this.defaults = new Dictionary<Label, string>();
            this.enabled = false;
            foreach (Label l in this.labels)
            {
                this.defaults.Add(l, l.Text);
            }
        }
    }

    abstract class AbstractLayout : UserControl
    {
        static readonly Lib.ILogger Logger = Lib.Logging.CreateLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected IDiabloInterface di;

        protected Dictionary<string, Def> def = new Dictionary<string, Def>();

        protected bool realFrwIas;
        GameDifficulty? activeDifficulty;
        CharacterClass? activeCharacterClass;

        protected IEnumerable<FlowLayoutPanel> RunePanels { get; set; }

        protected abstract Panel RuneLayoutPanel { get; }

        protected void Add(
            string nam,
            string maxStr,
            Func<ApplicationConfig, Tuple<bool, Color, int>> s,
            params string[] names
        )
        {
            def.Add(nam, new Def(nam, maxStr, s, names));
        }

        protected Font CreateFont(string name, int size)
        {
            try
            {
                return new Font(name, size);
            }
            catch
            {
                return new Font(DefaultFont.FontFamily.Name, size);
            }
        }

        public static string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
                return text;
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        protected void UpdateLabel(string nam, string[] values, bool visible = true)
        {
            if (!def[nam].enabled)
                return;

            foreach (Label l in def[nam].labels)
            {
                l.Visible = visible;
                var text = def[nam].defaults[l];
                foreach (var value in values)
                {
                    text = ReplaceFirst(text, "{}", value);
                }
                l.Text = text;
            }
        }

        protected void UpdateLabel(string nam, string value, bool visible = true)
        {
            if (!def[nam].enabled)
                return;

            foreach (Label l in def[nam].labels)
            {
                l.Visible = visible;
                l.Text = def[nam].defaults[l].Replace("{}", value);
            }
        }

        protected void UpdateLabel(string nam, int value, bool visible = true)
        {
            UpdateLabel(nam, "" + value, visible);
        }

        protected void UpdateLabel(string nam, uint value, bool visible = true)
        {
            UpdateLabel(nam, "" + value, visible);
        }

        protected void RegisterServiceEventHandlers()
        {
            di.configService.Changed += SettingsServiceOnSettingsChanged;
            di.game.DataRead += GameServiceOnDataRead;
        }

        protected void UnregisterServiceEventHandlers()
        {
            di.configService.Changed -= SettingsServiceOnSettingsChanged;
            di.game.DataRead -= GameServiceOnDataRead;
        }

        void SettingsServiceOnSettingsChanged(object sender, ApplicationConfigEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => SettingsServiceOnSettingsChanged(sender, e)));
                return;
            }

            activeCharacterClass = null;

            UpdateConfig(e.Config);
        }

        string lastGuid;
        void GameServiceOnDataRead(object sender, DataReadEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => GameServiceOnDataRead(sender, e)));
                return;
            }

            if (lastGuid != e.Character.Guid)
            {
                Reset();

                lastGuid = e.Character.Guid;
            }

            UpdateLabels(e.Character, e.Quests, e.Game);
            UpdateClassRuneList(e.Character.CharClass);
            UpdateRuneDisplay(e.Character.InventoryItemIds);
        }

        public void Reset()
        {
            foreach (FlowLayoutPanel fp in RunePanels)
            {
                if (fp.Controls.Count <= 0)
                    continue;

                foreach (RuneDisplayElement c in fp.Controls)
                {
                    c.SetHaveRune(false);
                }
            }

            foreach (KeyValuePair<string, Def> pair in def)
            {
                foreach (Label l in pair.Value.labels)
                {
                    l.Text = pair.Value.defaults[l];
                }
            }
        }

        abstract protected void UpdateConfig(ApplicationConfig config);

        abstract protected void UpdateLabels(Character player, Quests quests, Game game);

        void UpdateClassRuneList(CharacterClass characterClass)
        {
            var config = di.configService.CurrentConfig;
            if (!config.DisplayRunes) return;

            var targetDifficulty = di.game.TargetDifficulty;
            var isCharacterClassChanged = activeCharacterClass == null || activeCharacterClass != characterClass;
            var isGameDifficultyChanged = activeDifficulty != targetDifficulty;

            if (!isCharacterClassChanged && !isGameDifficultyChanged)
                return;

            Logger.Info("Loading rune list.");
            
            var runeSettings = GetMostSpecificRuneSettings(characterClass, targetDifficulty);
            UpdateRuneList(config, runeSettings?.Runes?.ToList());

            activeDifficulty = targetDifficulty;
            activeCharacterClass = characterClass;
        }

        void UpdateRuneList(ApplicationConfig config, IReadOnlyList<Rune> runes)
        {
            var panel = RuneLayoutPanel;
            if (panel == null) return;

            panel.Controls.ClearAndDispose();
            panel.Visible = runes?.Count > 0;
            runes?.ForEach(rune => panel.Controls.Add(
                new RuneDisplayElement(rune, config.DisplayRunesHighContrast, false, false)));
        }

        /// <summary>
        /// Gets the most specific rune settings in the order:
        ///     Class+Difficulty > Class > Difficulty > None
        /// </summary>
        /// <param name="characterClass">Active character class.</param>
        /// <param name="targetDifficulty">Manual difficulty selection.</param>
        /// <returns>The rune settings.</returns>
        IClassRuneSettings GetMostSpecificRuneSettings(CharacterClass characterClass, GameDifficulty targetDifficulty)
        {
            IEnumerable<IClassRuneSettings> runeClassSettings = di.configService.CurrentConfig.ClassRunes.ToList();
            return runeClassSettings.FirstOrDefault(rs => rs.Class == characterClass && rs.Difficulty == targetDifficulty)
                ?? runeClassSettings.FirstOrDefault(rs => rs.Class == characterClass && rs.Difficulty == null)
                ?? runeClassSettings.FirstOrDefault(rs => rs.Class == null && rs.Difficulty == targetDifficulty)
                ?? runeClassSettings.FirstOrDefault(rs => rs.Class == null && rs.Difficulty == null);
        }

        void UpdateRuneDisplay(IEnumerable<int> itemIds)
        {
            var panel = RuneLayoutPanel;
            if (panel == null) return;

            
            // Count number of items of each type.
            Dictionary<int, int> itemClassCounts = itemIds
                .GroupBy(id => id)
                .Where(id => id.Key > 609 && id.Key < 644)
                .ToDictionary(g => g.Key, g => g.Count());

            List<int> missingRunes = new List<int>(); // Store runes we are missing to see if we can use the cube to make them.

            foreach (RuneDisplayElement runeElement in panel.Controls)
            {
                var itemClassId = (int)runeElement.Rune + 610;

                if (itemClassCounts.ContainsKey(itemClassId) && itemClassCounts[itemClassId] > 0)
                {
                    itemClassCounts[itemClassId]--;
                    runeElement.SetHaveRune(true);

                    // No use to store an empty element.
                    if (itemClassCounts[itemClassId] == 0) itemClassCounts.Remove(itemClassId);

                    continue;
                }

                // Since you can't make El Rune we dont need to add it to the list.
                if (itemClassId > 610) missingRunes.Add(itemClassId);

            }
            
            // If we don't miss any runes or the function is not enabled just return.
            if (!di.configService.CurrentConfig.PossibleRuneUpg || missingRunes.Count == 0) return;

            // The list of runes we can make.
            List<int> canMake = new List<int>();

            bool upgrade = true;
            while (upgrade)
            {

                foreach (KeyValuePair<int,int> kvp in itemClassCounts.ToArray())
                {

                    int neededForUpg = (kvp.Key > 631) ? 2 : 3; // Um and higher runes only need two of the same rune to upgrade.
                    // bool needGem = (kvp.Key >= 620); // Amn and higher runes also need a gem to upgrade.

                    int newRunes = (kvp.Value / neededForUpg);

                    if (newRunes > 0)
                    {

                        int newRune = kvp.Key + 1;

                        // Remove the runes we have used up
                        itemClassCounts[kvp.Key] = itemClassCounts[kvp.Key] - (newRunes * neededForUpg);
                        if (itemClassCounts[kvp.Key] == 0) itemClassCounts.Remove(kvp.Key);

                        // Do we need this rune?
                        while ((missingRunes.Contains(newRune) && newRunes > 0))
                        {
                            canMake.Add(newRune);
                            missingRunes.Remove(newRune);
                            newRunes--;
                        }

                        // Do we still have runes?
                        if (newRunes > 0)
                        {
                            // Add upgraded runes to the itemClassCount
                            if (itemClassCounts.ContainsKey(newRune))
                            { itemClassCounts[newRune] = itemClassCounts[newRune] + newRunes; }
                            else
                            { itemClassCounts.Add(newRune, newRunes); }
                        }
                    }
                }

                // Have we filled our order?
                if (missingRunes.Count == 0) break;
                // Can we still upgrade runes?
                upgrade = (itemClassCounts.Where(x => (x.Key < 631 && x.Value >= 3) || (x.Key > 630 && x.Value >= 2)).ToList().Count > 0);
            }

            if (canMake.Count > 0)
            {
                foreach (RuneDisplayElement runeElement in panel.Controls)
                {
                    if (runeElement.haveRune) continue;

                    int runeId = (int)runeElement.Rune + 610;

                    if (canMake.Contains(runeId))
                    {
                        runeElement.SetCanMakeRune(true);
                        canMake.Remove(runeId);
                        if (canMake.Count == 0) break;
                    }

                }
            }

        }

        protected Size MeasureText(string str, Control control)
        {
            return TextRenderer.MeasureText(str, control.Font, Size.Empty, TextFormatFlags.SingleLine);
        }
    }
}
