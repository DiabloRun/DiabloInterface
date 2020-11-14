using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Zutatensuppe.D2Reader;
using Zutatensuppe.D2Reader.Models;
using Zutatensuppe.D2Reader.Struct.Item;
using Zutatensuppe.DiabloInterface.Lib;
using Zutatensuppe.DiabloInterface.Lib.Services;
using Zutatensuppe.DiabloInterface.Plugin.Autosplits.AutoSplits;
using static Zutatensuppe.D2Reader.D2Data;

namespace Zutatensuppe.DiabloInterface.Plugin.Autosplits.Test
{
    [TestClass]
    public class PluginTest
    {
        [TestMethod]
        public void ShouldNotSplitIfDisabled()
        {
            var split = new AutoSplit(
                "one",
                AutoSplit.SplitType.CharLevel,
                10,
                (short)GameDifficulty.Normal
            );

            var cfg = new Config();
            cfg.Enabled = false;
            cfg.Splits = new List<AutoSplit>();
            cfg.Splits.Add(split);

            var config = new Mock<ApplicationConfig>();
            config.Setup(x => x.PluginConf(It.IsAny<string>())).Returns(cfg);
            var configService = new Mock<IConfigService>();
            configService.SetupGet(x => x.CurrentConfig).Returns(config.Object);

            var gameService = new Mock<IGameService>();

            var diabloInterface = new Mock<IDiabloInterface>();
            diabloInterface.Setup(x => x.configService).Returns(configService.Object);
            diabloInterface.Setup(x => x.game).Returns(gameService.Object);

            var autoSplitService = new Plugin();
            autoSplitService.Initialize(diabloInterface.Object);

            var quests = new Quests(new List<List<Quest>>
            {
                new List<Quest>(), // NORMAL
                new List<Quest>(), // NM
                new List<Quest>(), // HELL
            });
            
            var character = new Mock<Character>();
            character.SetupGet(x => x.Level).Returns(9);
            character.SetupGet(x => x.IsNewChar).Returns(true);
            character.SetupGet(x => x.InventoryItemIds).Returns(new List<int>());

            var processInfo = new ProcessInfo();
            var game = new Game();
            game.Area = 0;
            game.Difficulty = GameDifficulty.Normal;
            game.PlayersX = 1;
            game.GameCount = 0;
            game.CharCount = 0;
            game.Character = character.Object;
            game.Quests = quests;

            List<Monster> killedMonsters = null;
            var args = new DataReadEventArgs(processInfo, game, killedMonsters);

            // test autosplit by level
            gameService.Raise(g => g.DataRead += null, args);
            Assert.AreEqual(false, split.IsReached);

            character.SetupGet(x => x.Level).Returns(10);
            gameService.Raise(g => g.DataRead += null, args);
            Assert.AreEqual(false, split.IsReached);
        }

        [TestMethod]
        public void ShouldReachAutosplits()
        {
            var splitOnGameStart = new AutoSplit(
                "game start",
                AutoSplit.SplitType.Special,
                1,
                (short)GameDifficulty.Normal
            );
            var splitOnGameClear = new AutoSplit(
                "100% done",
                AutoSplit.SplitType.Special,
                2,
                (short)GameDifficulty.Normal
            );
            var splitOnGameClearAllDifficulties = new AutoSplit(
                "100% done all difficulties",
                AutoSplit.SplitType.Special,
                3,
                (short)GameDifficulty.Normal
            );
            var splitOnCharLevel10 = new AutoSplit(
                "char level 10",
                AutoSplit.SplitType.CharLevel,
                10,
                (short)GameDifficulty.Normal
            );
            var splitOnArea1 = new AutoSplit(
                "area 1",
                AutoSplit.SplitType.Area,
                (int)Area.ROGUE_ENCAMPMENT,
                (short)GameDifficulty.Normal
            );
            var splitOnItem1 = new AutoSplit(
                "item 1",
                AutoSplit.SplitType.Item,
                1,
                (short)GameDifficulty.Normal
            );
            var splitOnGem1 = new AutoSplit(
                "gem 1",
                AutoSplit.SplitType.Gems,
                1,
                (short)GameDifficulty.Normal
            );
            var splitOnQuest81 = new AutoSplit(
                "quest 81 (den of evil)",
                AutoSplit.SplitType.Quest,
                81,
                (short)GameDifficulty.Normal
            );

            var cfg = new Config();
            cfg.Enabled = true;
            cfg.Splits = new List<AutoSplit>();
            cfg.Splits.Add(splitOnGameStart);
            cfg.Splits.Add(splitOnGameClear);
            cfg.Splits.Add(splitOnGameClearAllDifficulties);
            cfg.Splits.Add(splitOnCharLevel10);
            cfg.Splits.Add(splitOnArea1);
            cfg.Splits.Add(splitOnItem1);
            cfg.Splits.Add(splitOnGem1);
            cfg.Splits.Add(splitOnQuest81);

            var config = new Mock<ApplicationConfig>();
            config.Setup(x => x.PluginConf(It.IsAny<string>())).Returns(cfg);
            var configService = new Mock<IConfigService>();
            configService.SetupGet(x => x.CurrentConfig).Returns(config.Object);

            var gameService = new Mock<IGameService>();

            var diabloInterface = new Mock<IDiabloInterface>();
            diabloInterface.Setup(x => x.configService).Returns(configService.Object);
            diabloInterface.Setup(x => x.game).Returns(gameService.Object);

            var autoSplitService = new Plugin();
            autoSplitService.Initialize(diabloInterface.Object);

            var normalQuests = new List<Quest>() {
                QuestFactory.Create(QuestId.DenOfEvil, 0),
                QuestFactory.Create(QuestId.Andariel, 0),
            };

            var nightmareQuests = new List<Quest>() {
                QuestFactory.Create(QuestId.DenOfEvil, 0),
                QuestFactory.Create(QuestId.Andariel, 0),
            };

            var hellQuests = new List<Quest>() {
                QuestFactory.Create(QuestId.DenOfEvil, 0),
                QuestFactory.Create(QuestId.Andariel, 0),
            };

            var quests = new Quests(new List<List<Quest>>
            {
                normalQuests,
                nightmareQuests,
                hellQuests,
            });

            var itemStrings = new Dictionary<BodyLocation, string>();
            var itemsIds = new List<int>();

            var characterMock = new Mock<Character>();
            characterMock.SetupGet(x => x.Level).Returns(9);
            characterMock.SetupGet(x => x.IsNewChar).Returns(true);
            characterMock.SetupGet(x => x.InventoryItemIds).Returns(itemsIds);

            var processInfo = new ProcessInfo();

            var game = new Game();
            game.Area = 0;
            game.Difficulty = GameDifficulty.Normal;
            game.PlayersX = 1;
            game.GameCount = 0;
            game.CharCount = 0;
            game.Character = characterMock.Object;
            game.Quests = quests;

            List<Monster> killedMonsters = null;
            var args = new DataReadEventArgs(processInfo, game, killedMonsters);

            // test autosplit by game start
            Assert.AreEqual(false, splitOnGameStart.IsReached);
            gameService.Raise(g => g.DataRead += null, args);
            Assert.AreEqual(true, splitOnGameStart.IsReached);

            // test autosplit by level
            Assert.AreEqual(false, splitOnCharLevel10.IsReached);
            gameService.Raise(g => g.DataRead += null, args);
            Assert.AreEqual(false, splitOnCharLevel10.IsReached);

            characterMock.SetupGet(x => x.Level).Returns(10);
            gameService.Raise(g => g.DataRead += null, args);
            Assert.AreEqual(true, splitOnCharLevel10.IsReached);

            // test autosplit by area
            Assert.AreEqual(false, splitOnArea1.IsReached);
            game.Area = (int)Area.ROGUE_ENCAMPMENT;
            gameService.Raise(g => g.DataRead += null, args);
            game.Area = (int)Area.ROGUE_ENCAMPMENT + 1;
            Assert.AreEqual(true, splitOnArea1.IsReached);

            // test autosplit by item
            Assert.AreEqual(false, splitOnItem1.IsReached);
            Assert.AreEqual(false, splitOnGem1.IsReached);
            itemsIds.Add(1);
            gameService.Raise(g => g.DataRead += null, args);
            Assert.AreEqual(true, splitOnItem1.IsReached);
            Assert.AreEqual(true, splitOnGem1.IsReached);

            // test autosplit by quest
            Assert.AreEqual(false, splitOnQuest81.IsReached);
            normalQuests.Clear();
            normalQuests.Add(QuestFactory.Create(QuestId.DenOfEvil, 1 << 0));
            normalQuests.Add(QuestFactory.Create(QuestId.Andariel, 0));
            gameService.Raise(g => g.DataRead += null, args);
            Assert.AreEqual(true, splitOnQuest81.IsReached);

            // test autosplit on game clear
            Assert.AreEqual(false, splitOnGameClear.IsReached);
            normalQuests.Clear();
            normalQuests.Add(QuestFactory.Create(QuestId.DenOfEvil, 1 << 0));
            normalQuests.Add(QuestFactory.Create(QuestId.Andariel, 1 << 0));
            gameService.Raise(g => g.DataRead += null, args);
            Assert.AreEqual(true, splitOnGameClear.IsReached);

            // test autosplit on game clear all difficulties
            Assert.AreEqual(false, splitOnGameClearAllDifficulties.IsReached);
            nightmareQuests.Clear();
            nightmareQuests.Add(QuestFactory.Create(QuestId.DenOfEvil, 1 << 0));
            nightmareQuests.Add(QuestFactory.Create(QuestId.Andariel, 1 << 0));
            gameService.Raise(g => g.DataRead += null, args);
            Assert.AreEqual(false, splitOnGameClearAllDifficulties.IsReached);
            hellQuests.Clear();
            hellQuests.Add(QuestFactory.Create(QuestId.DenOfEvil, 1 << 0));
            hellQuests.Add(QuestFactory.Create(QuestId.Andariel, 1 << 0));
            gameService.Raise(g => g.DataRead += null, args);
            Assert.AreEqual(true, splitOnGameClearAllDifficulties.IsReached);
        }
    }
}
