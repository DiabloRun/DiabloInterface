namespace Zutatensuppe.DiabloInterface.Gui.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    using Zutatensuppe.D2Reader;
    using Zutatensuppe.DiabloInterface.Business.Services;
    using Zutatensuppe.DiabloInterface.Business.Settings;

    public partial class AbstractLayout : UserControl
    {
        readonly ISettingsService settingsService;
        readonly IGameService gameService;

        protected AbstractLayout(ISettingsService settingsService, IGameService gameService)
        {
            if (settingsService == null) throw new ArgumentNullException(nameof(settingsService));
            if (gameService == null) throw new ArgumentNullException(nameof(gameService));

            this.settingsService = settingsService;
            this.gameService = gameService;

            RegisterServiceEventHandlers();
            InitializeComponent();
            InitializeElements();

            Load += (sender, e) => UpdateSettings(settingsService.CurrentSettings);

            // Clean up events when disposed because services outlive us.
            Disposed += (sender, e) => UnregisterServiceEventHandlers();
        }

        protected IEnumerable<Label> InfoLabels { get; set; }

        protected IEnumerable<FlowLayoutPanel> RunePanels { get; set; }

        protected Dictionary<Control, string> DefaultTexts { get; set; }

        void InitializeElements()
        {
            InfoLabels = Enumerable.Empty<Label>();
            RunePanels = Enumerable.Empty<FlowLayoutPanel>();
            DefaultTexts = new Dictionary<Control, string>();
        }

        void RegisterServiceEventHandlers()
        {
            settingsService.SettingsChanged += SettingsServiceOnSettingsChanged;
            gameService.CharacterCreated += GameServiceOnCharacterCreated;
            gameService.DataRead += GameServiceOnDataRead;
        }

        void UnregisterServiceEventHandlers()
        {
            settingsService.SettingsChanged -= SettingsServiceOnSettingsChanged;

            gameService.CharacterCreated -= GameServiceOnCharacterCreated;
            gameService.DataRead -= GameServiceOnDataRead;
        }

        void SettingsServiceOnSettingsChanged(object sender, ApplicationSettingsEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => SettingsServiceOnSettingsChanged(sender, e)));
                return;
            }

            UpdateSettings(e.Settings);
        }

        void GameServiceOnCharacterCreated(object sender, CharacterCreatedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => GameServiceOnCharacterCreated(sender, e)));
                return;
            }

            Reset();
        }

        void GameServiceOnDataRead(object sender, DataReadEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => GameServiceOnDataRead(sender, e)));
                return;
            }

            UpdateLabels(e.Character);
            UpdateRuneDisplay(e.ItemIds);
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

            foreach (KeyValuePair<Control, string> pair in DefaultTexts)
            {
                pair.Key.Text = pair.Value;
            }
        }

        protected virtual void UpdateSettings(ApplicationSettings settings)
        {
        }

        protected virtual void UpdateLabels(Character player)
        {
        }

        protected void UpdateLabelWidthAlignment(params Label[] labels)
        {
            var maxWidth = 0;

            foreach (var label in labels)
            {
                var measuredSize = TextRenderer.MeasureText(label.Text, label.Font, Size.Empty, TextFormatFlags.SingleLine);
                maxWidth = Math.Max(measuredSize.Width, maxWidth);
            }

            foreach (var label in labels)
            {
                label.MinimumSize = new Size(maxWidth, 0);
            }
        }

        void UpdateRuneDisplay(IEnumerable<int> itemIds)
        {
            foreach (FlowLayoutPanel fp in RunePanels)
            {
                if (fp.Controls.Count <= 0)
                    continue;

                // Used for keeping track of how many items of each type there is.
                Dictionary<int, int> itemClassCounts = itemIds
                    .GroupBy(id => id)
                    .ToDictionary(g => g.Key, g => g.Count());

                foreach (RuneDisplayElement c in fp.Controls)
                {
                    int eClass = (int)c.Rune + 610;

                    if (itemClassCounts.ContainsKey(eClass) && itemClassCounts[eClass] > 0)
                    {
                        itemClassCounts[eClass]--;
                        c.SetHaveRune(true);
                    }
                }
            }
        }
    }
}
