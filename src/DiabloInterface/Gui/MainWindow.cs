using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Zutatensuppe.D2Reader.Models;
using Zutatensuppe.DiabloInterface.Gui.Controls;
using Zutatensuppe.DiabloInterface.Gui.Forms;
using System.Reflection;
using Zutatensuppe.DiabloInterface.Lib.Services;
using Zutatensuppe.DiabloInterface.Lib;

namespace Zutatensuppe.DiabloInterface.Gui
{
    public class MainWindow : WsExCompositedForm
    {
        static readonly Lib.ILogger Logger = Lib.Logging.CreateLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IDiabloInterface di;

        private ToolStripMenuItem loadConfigMenuItem;
        private Form debugWindow;
        private AbstractLayout layout;

        public MainWindow(IDiabloInterface di)
        {
            this.di = di;
            
            Logger.Info("Creating main window.");

            RegisterServiceEventHandlers();
            InitializeComponent();
            PopulateConfigFileListContextMenu(this.di.configService.ConfigFileCollection);
            ApplyConfig(this.di.configService.CurrentConfig);
        }

        void RegisterServiceEventHandlers()
        {
            di.configService.Changed += ConfigChanged;
            di.configService.CollectionChanged += ConfigCollectionChanged;
        }

        void UnregisterServiceEventHandlers()
        {
            di.configService.Changed -= ConfigChanged;
            di.configService.CollectionChanged -= ConfigCollectionChanged;
        }

        void ConfigChanged(object sender, ApplicationConfigEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => ConfigChanged(sender, e)));
                return;
            }

            ApplyConfig(e.Config);
        }

        void ConfigCollectionChanged(object sender, ConfigCollectionEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => ConfigCollectionChanged(sender, e)));
                return;
            }

            PopulateConfigFileListContextMenu(e.Collection);
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

            var configItem = new ToolStripMenuItem();
            configItem.Image = Properties.Resources.wrench_orange;
            configItem.ShortcutKeys = Keys.Control | Keys.U;
            configItem.Text = "Config";
            configItem.Click += new EventHandler(ConfigMenuItemOnClick);

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

            var copySeedItem = new ToolStripMenuItem();
            copySeedItem.Text = "Copy current seed";
            copySeedItem.Click += new EventHandler(CopySeedItemOnClick);

            loadConfigMenuItem = new ToolStripMenuItem();
            loadConfigMenuItem.Text = "Load Config";

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.AddRange(new ToolStripItem[] {
                difficultyItem,
                copySeedItem,
                configItem,
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

            Icon = Properties.Resources.di;
            MaximizeBox = false;
            MinimizeBox = false;
            Text = $@"DiabloInterface v{Application.ProductVersion}";
        }

        void ApplyConfig(ApplicationConfig config)
        {
            // nothing to do if correct layout already
            if (layout is HorizontalLayout && config.DisplayLayoutHorizontal)
                return;

            // remove old layout
            if (layout != null)
            {
                Controls.Remove(layout);
                layout.Dispose();
                layout = null;
            }

            // create new layout
            var nextLayout = config.DisplayLayoutHorizontal
                ? new HorizontalLayout(di) as AbstractLayout
                : new VerticalLayout(di);
            Controls.Add(nextLayout);
            layout = nextLayout;
        }

        void PopulateConfigFileListContextMenu(IEnumerable<FileInfo> configFileCollection)
        {
            var items = configFileCollection.Select(CreateConfigToolStripMenuItem).ToArray();

            loadConfigMenuItem.DropDownItems.Clear();
            loadConfigMenuItem.DropDownItems.AddRange(items);
        }

        ToolStripMenuItem CreateConfigToolStripMenuItem(FileInfo f)
        {
            var item = new ToolStripMenuItem();
            item.Text = Path.GetFileNameWithoutExtension(f.Name);
            item.Click += (object s, EventArgs e) => LoadConfigFile(s, f.FullName);
            return item;
        }

        void LoadConfigFile(object sender, string fileName)
        {
            // TODO: LoadSettings should throw a custom Exception with information about why this happened.
            if (!di.configService.Load(fileName))
            {
                Logger.Error($"Failed to load config from file: {fileName}.");
                MessageBox.Show(
                    $@"Failed to load config.{Environment.NewLine}See the error log for more details.",
                    @"Config Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        void ExitMenuItemOnClick(object sender, EventArgs e)
        {
            Close();
        }

        void CopySeedItemOnClick(object sender, EventArgs e)
        {
            if (di?.game?.DataReader?.Game?.Seed == null)
            {
                MessageBox.Show(
                    $@"No seed available.",
                    @"Info",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return;
            }

            Clipboard.SetText($"{di.game.DataReader.Game.Seed}");
            MessageBox.Show(
                $@"Seed {di.game.DataReader.Game.Seed} copied.",
                @"Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        void ResetMenuItemOnClick(object sender, EventArgs e)
        {
            di.plugins.Reset();
            layout?.Reset();
        }

        void ConfigMenuItemOnClick(object sender, EventArgs e)
        {
            using (var window = new ConfigWindow(di))
                window.ShowDialog();
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
