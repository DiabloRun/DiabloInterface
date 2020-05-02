using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Zutatensuppe.D2Reader.Models;
using Zutatensuppe.DiabloInterface.Services;
using Zutatensuppe.DiabloInterface.Settings;
using Zutatensuppe.DiabloInterface.Core.Logging;
using Zutatensuppe.DiabloInterface.Gui.Controls;
using Zutatensuppe.DiabloInterface.Gui.Forms;
using System.Reflection;

namespace Zutatensuppe.DiabloInterface.Gui
{
    public class MainWindow : WsExCompositedForm
    {
        private readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DiabloInterface di;

        private ToolStripMenuItem loadConfigMenuItem;
        private Form debugWindow;
        private AbstractLayout layout;

        public MainWindow(DiabloInterface di)
        {
            this.di = di;
            
            Logger.Info("Creating main window.");

            RegisterServiceEventHandlers();
            InitializeComponent();
            PopulateSetingsFileListContextMenu(this.di.settings.SettingsFileCollection);
            ApplySettings(this.di.settings.CurrentSettings);
        }

        void RegisterServiceEventHandlers()
        {
            di.settings.Changed += SettingsServiceOnSettingsChanged;
            di.settings.CollectionChanged += SettingsServiceOnSettingsCollectionChanged;
        }

        void UnregisterServiceEventHandlers()
        {
            di.settings.Changed -= SettingsServiceOnSettingsChanged;
            di.settings.CollectionChanged -= SettingsServiceOnSettingsCollectionChanged;
        }

        void SettingsServiceOnSettingsChanged(object sender, ApplicationSettingsEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => SettingsServiceOnSettingsChanged(sender, e)));
                return;
            }

            ApplySettings(e.Settings);
        }

        void SettingsServiceOnSettingsCollectionChanged(object sender, SettingsCollectionEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => SettingsServiceOnSettingsCollectionChanged(sender, e)));
                return;
            }

            PopulateSetingsFileListContextMenu(e.Collection);
        }

        private void InitializeComponent()
        {
            var difficultyItem = new ToolStripMenuItem();
            foreach (GameDifficulty diff in Enum.GetValues(typeof(GameDifficulty)))
            {
                var item = new ToolStripMenuItem();
                item.Checked = difficultyItem.DropDownItems.Count == 0;
                item.Text = Enum.GetName(typeof(GameDifficulty), diff);
                item.Click += (object s, EventArgs e) => GameDifficultyClick(s, diff);
                difficultyItem.DropDownItems.Add(item);
            }
            difficultyItem.Text = "Difficulty";

            var settingsItem = new ToolStripMenuItem();
            settingsItem.Image = Properties.Resources.wrench_orange;
            settingsItem.ShortcutKeys = Keys.Control | Keys.U;
            settingsItem.Text = "Settings";
            settingsItem.Click += new EventHandler(SettingsMenuItemOnClick);

            var resetItem = new ToolStripMenuItem();
            resetItem.Image = Properties.Resources.arrow_refresh;
            resetItem.ShortcutKeys = Keys.Control | Keys.R;
            resetItem.Text = "Reset";
            resetItem.Click += new EventHandler(ResetMenuItemOnClick);

            var debugItem = new ToolStripMenuItem();
            debugItem.Image = Properties.Resources.bug;
            debugItem.Text = "Debug";
            debugItem.Click += new EventHandler(DebugMenuItemOnClick);

            var exitItem = new ToolStripMenuItem();
            exitItem.Image = Properties.Resources.cross;
            exitItem.Text = "Exit";
            exitItem.Click += new EventHandler(ExitMenuItemOnClick);

            loadConfigMenuItem = new ToolStripMenuItem();
            loadConfigMenuItem.Text = "Load Config";

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.AddRange(new ToolStripItem[] {
                difficultyItem,
                settingsItem,
                loadConfigMenuItem,
                resetItem,
                debugItem,
                exitItem
            });

            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = System.Drawing.Color.Black;
            ClientSize = new System.Drawing.Size(730, 514);
            ContextMenuStrip = contextMenu;
            Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            FormBorderStyle = FormBorderStyle.FixedSingle;

            var resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Text = $@"Diablo Interface v{Application.ProductVersion}";
        }

        void ApplySettings(ApplicationSettings s)
        {
            // nothing to do if correct layout already
            if (layout is HorizontalLayout && s.DisplayLayoutHorizontal)
                return;

            // remove old layout
            if (layout != null)
            {
                Controls.Remove(layout);
                layout.Dispose();
                layout = null;
            }

            // create new layout
            var nextLayout = s.DisplayLayoutHorizontal
                ? new HorizontalLayout(di) as AbstractLayout
                : new VerticalLayout(di);
            Controls.Add(nextLayout);
            layout = nextLayout;
        }

        void PopulateSetingsFileListContextMenu(IEnumerable<FileInfo> settingsFileCollection)
        {
            var items = settingsFileCollection.Select(CreateSettingsToolStripMenuItem).ToArray();

            loadConfigMenuItem.DropDownItems.Clear();
            loadConfigMenuItem.DropDownItems.AddRange(items);
        }

        ToolStripMenuItem CreateSettingsToolStripMenuItem(FileInfo f)
        {
            var item = new ToolStripMenuItem();
            item.Text = Path.GetFileNameWithoutExtension(f.Name);
            item.Click += (object s, EventArgs e) => LoadConfigFile(s, f.FullName);
            return item;
        }

        void LoadConfigFile(object sender, string fileName)
        {
            // TODO: LoadSettings should throw a custom Exception with information about why this happened.
            if (!di.settings.LoadSettings(fileName))
            {
                Logger.Error($"Failed to load settings from file: {fileName}.");
                MessageBox.Show(
                    $@"Failed to load settings.{Environment.NewLine}See the error log for more details.",
                    @"Settings Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        void ExitMenuItemOnClick(object sender, EventArgs e)
        {
            Close();
        }

        void ResetMenuItemOnClick(object sender, EventArgs e)
        {
            di.plugins.Reset();
            layout?.Reset();
        }

        void SettingsMenuItemOnClick(object sender, EventArgs e)
        {
            using (var settingsWindow = new SettingsWindow(di))
                settingsWindow.ShowDialog();
        }

        void DebugMenuItemOnClick(object sender, EventArgs e)
        {
            if (debugWindow == null || debugWindow.IsDisposed)
                debugWindow = new DebugWindow(di);

            debugWindow.Show();
        }

        void GameDifficultyClick(object sender, GameDifficulty difficulty)
        {
            Logger.Info($"Setting target difficulty to {difficulty}.");

            var clicked = (ToolStripMenuItem)sender;
            var parent = (ToolStripMenuItem)clicked.OwnerItem;
            foreach (ToolStripMenuItem item in parent.DropDownItems)
                item.Checked = item == clicked;

            di.game.TargetDifficulty = difficulty;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            UnregisterServiceEventHandlers();
            base.OnFormClosing(e);
        }
    }
}
