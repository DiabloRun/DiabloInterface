using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Zutatensuppe.D2Reader;
using Zutatensuppe.D2Reader.Models;
using Zutatensuppe.DiabloInterface.Core.Logging;
using Zutatensuppe.DiabloInterface.Plugin.Autosplits.AutoSplits;
using Zutatensuppe.DiabloInterface.Plugin.Autosplits.Hotkeys;

[assembly: InternalsVisibleTo("DiabloInterface.Plugin.Autosplits.Test")]
namespace Zutatensuppe.DiabloInterface.Plugin.Autosplits
{
    public class Plugin : IPlugin
    {
        public string Name => "Autosplit";

        internal Config config { get; private set; } = new Config();

        internal Dictionary<Type, Type> RendererMap => new Dictionary<Type, Type> {
            {typeof(IPluginSettingsRenderer), typeof(SettingsRenderer)},
            {typeof(IPluginDebugRenderer), typeof(DebugRenderer)},
        };

        public PluginConfig Config { get => config; set {
            config = new Config(value);
            ApplyConfig();
            LogAutoSplits();
        }}

        private ILogger Logger;

        public readonly KeyService keyService = new KeyService();

        private DiabloInterface di;

        public void Initialize(DiabloInterface di)
        {
            Logger = di.Logger(this);
            Logger.Info("Creating auto split service.");
            this.di = di;

            Config = di.settings.CurrentSettings.PluginConf(Name);
            di.game.CharacterCreated += Game_CharacterCreated;
            di.game.DataRead += Game_DataRead;
        }

        private void Game_CharacterCreated(object sender, CharacterCreatedEventArgs e)
        {
            Logger.Info($"A new character was created. Auto splits enabled for {e.Character.Name}");
            ResetAutoSplits();

            if (config.Enabled)
            {
                keyService.TriggerHotkey(config.ResetHotkey.ToKeys());
            }
        }

        private void Game_DataRead(object sender, DataReadEventArgs e)
        {
            DoAutoSplits(e);
        }

        private void LogAutoSplits()
        {
            if (config.Splits.Count == 0)
            {
                Logger.Info("No auto splits configured.");
                return;
            }

            var logMessage = new StringBuilder();
            logMessage.Append("Configured auto splits:");

            int i = 0;
            foreach (var split in config.Splits)
            {
                logMessage.AppendLine();
                logMessage.Append(AutoSplitString(i++, split));
            }

            Logger.Info(logMessage.ToString());
        }

        private string AutoSplitString(int i, AutoSplit s) => $"#{i} [{s.Type}, {s.Value}, {s.Difficulty}] \"{s.Name}\"";

        private void DoAutoSplits(DataReadEventArgs e)
        {
            // TODO: fix bug... when splits are add during the run, the last split seems to trigger again on save
            // setup autosplits:
            // - start game
            // - area (cold plains)
            // start game, go to cold plains (2 splits should have happened)
            // add another autosplit:
            // - area (stony fields)
            // should not trigger another split automatically, but does
            if (!config.Enabled || !e.Character.IsNewChar)
                return;

            int i = 0;
            foreach (var split in config.Splits)
            {
                if (!IsCompleteableAutoSplit(split, e))
                    continue;

                split.IsReached = true;
                keyService.TriggerHotkey(config.Hotkey.ToKeys());
                Logger.Info($"AutoSplit reached: {AutoSplitString(i++, split)}");
            }
            ApplyChanges();
        }

        private bool IsCompleteableAutoSplit(AutoSplit split, DataReadEventArgs args)
        {
            if (split.IsReached || !split.MatchesDifficulty(args.Game.Difficulty))
                return false;

            switch (split.Type)
            {
                case AutoSplit.SplitType.Special:
                    switch (split.Value)
                    {
                        case (int)AutoSplit.Special.GameStart:
                            return true;
                        case (int)AutoSplit.Special.Clear100Percent:
                            return args.Quests.DifficultyFullyCompleted(args.Game.Difficulty);
                        case (int)AutoSplit.Special.Clear100PercentAllDifficulties:
                            return args.Quests.FullyCompleted();
                        default:
                            return false;
                    }
                case AutoSplit.SplitType.Quest:
                    return args.Quests.QuestCompleted(args.Game.Difficulty, (QuestId)split.Value);
                case AutoSplit.SplitType.CharLevel:
                    return split.Value <= args.Character.Level;
                case AutoSplit.SplitType.Area:
                    return split.Value == args.Game.Area;
                case AutoSplit.SplitType.Item:
                case AutoSplit.SplitType.Gems:
                    return args.Character.InventoryItemIds.Contains(split.Value);
                default:
                    return false;
            }
        }

        public void ResetAutoSplits()
        {
            foreach (var autoSplit in config.Splits)
            {
                autoSplit.IsReached = false;
            }
            ApplyChanges();
        }

        public void Reset()
        {
            ResetAutoSplits();
        }

        public void Dispose()
        {
        }

        Dictionary<Type, IPluginRenderer> renderers = new Dictionary<Type, IPluginRenderer>();
        private void ApplyChanges()
        {
            foreach (var p in renderers)
                p.Value.ApplyChanges();
        }

        private void ApplyConfig()
        {
            foreach (var p in renderers)
                p.Value.ApplyConfig();
        }

        public T GetRenderer<T>() where T : IPluginRenderer
        {
            var type = typeof(T);
            if (!RendererMap.ContainsKey(type))
                return default(T);
            if (!renderers.ContainsKey(type))
                renderers[type] = (T)Activator.CreateInstance(RendererMap[type], this);
            return (T)renderers[type];
        }
    }
}
