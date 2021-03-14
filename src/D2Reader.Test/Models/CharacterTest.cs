using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zutatensuppe.D2Reader;
using Zutatensuppe.D2Reader.Models;
using Zutatensuppe.D2Reader.Readers;
using Zutatensuppe.D2Reader.Struct;
using Zutatensuppe.D2Reader.Struct.Skill;
using Zutatensuppe.D2Reader.Struct.Stat;
using static Zutatensuppe.D2Reader.D2Data;

namespace Zutatensuppe.DiabloInterface.D2Reader.Test.Models
{
    [TestClass]
    public class CharacterTest
    {
        [TestMethod]
        public void ShouldDetermineIfNewChar()
        {
            var unit = new D2Unit();
            unit.eClass = 0;
            unit.actNo = 0;
            unit.StatListNode = new DataPointer(1);

            var processMemoryReader = new Mock<IProcessMemoryReader>();
            var gameMemoryTable = new GameMemoryTable();
            var stringReader = new Mock<IStringReader>().Object;

            var statsList = new D2StatListEx();
            statsList.BaseStats = new D2StatArray();
            statsList.BaseStats.Address = new DataPointer();
            statsList.ListFlags = StatListFlag.HasCompleteStats;
            statsList.FullStats = new D2StatArray();
            statsList.FullStats.Length = 1;
            statsList.FullStats.Address = new DataPointer();

            processMemoryReader
                .Setup(x => x.Read<D2StatListEx>(
                    It.Is<IntPtr>(p => p.Equals(unit.StatListNode.Address))
                ))
                .Returns(statsList);

            var lvlStat = new D2Stat();
            lvlStat.LoStatID = (ushort)StatIdentifier.Level;
            lvlStat.Value = 1;
            var xpStat = new D2Stat();
            xpStat.LoStatID = (ushort)StatIdentifier.Experience;
            xpStat.Value = 0;

            var d2StatArray = new D2Stat[] { lvlStat, xpStat };
            processMemoryReader
                .Setup(x => x.ReadArray<D2Stat>(
                    It.Is<IntPtr>(p => p.Equals(statsList.FullStats.Address.Address)),
                    It.IsAny<int>()
                ))
                .Returns<IntPtr, int>((p, i) => i == 0 ? new D2Stat[] { } : d2StatArray);

            // starting items for amazon
            var startingItems = new Item[] {
                new Item { Unit = new D2Unit { eClass = 0x2f } },
                new Item { Unit = new D2Unit { eClass = 0x148 } },
                new Item { Unit = new D2Unit { eClass = 0x24b } },
                new Item { Unit = new D2Unit { eClass = 0x24b } },
                new Item { Unit = new D2Unit { eClass = 0x24b } },
                new Item { Unit = new D2Unit { eClass = 0x24b } },
                new Item { Unit = new D2Unit { eClass = 0x211 } },
                new Item { Unit = new D2Unit { eClass = 0x212 } },
            };

            var inventoryReader = new Mock<IInventoryReader>();
            inventoryReader
                .Setup(x => x.EnumerateInventoryForward(
                    It.Is<D2Unit>(p => p.Equals(unit))
                ))
                .Returns(startingItems);

            var skillReader = new Mock<ISkillReader>();

            // starting skills for amazon
            var skills = new List<SkillInfo>
            {
                new SkillInfo { Id = (uint) Skill.UNKNOWN, Points = 1 },
                new SkillInfo { Id = (uint) Skill.THROW, Points = 1 },
                new SkillInfo { Id = (uint) Skill.KICK, Points = 1 },
                new SkillInfo { Id = (uint) Skill.SCROLL_IDENT, Points = 1 },
                new SkillInfo { Id = (uint) Skill.TOME_IDENT, Points = 1 },
                new SkillInfo { Id = (uint) Skill.SCROLL_TP, Points = 1 },
                new SkillInfo { Id = (uint) Skill.TOME_TP, Points = 1 },
                new SkillInfo { Id = (uint) Skill.UNSUMMON, Points = 1 },
            };

            var unitReader = new UnitReader(processMemoryReader.Object, gameMemoryTable, stringReader, skillReader.Object);

            Assert.AreEqual(true, Character.DetermineIfNewChar(unit, unitReader, inventoryReader.Object, skills));

            lvlStat.Value = 2;
            Assert.AreEqual(false, Character.DetermineIfNewChar(unit, unitReader, inventoryReader.Object, skills));
            lvlStat.Value = 1;
            Assert.AreEqual(true, Character.DetermineIfNewChar(unit, unitReader, inventoryReader.Object, skills));

            xpStat.Value = 1;
            Assert.AreEqual(false, Character.DetermineIfNewChar(unit, unitReader, inventoryReader.Object, skills));
            xpStat.Value = 0;
            Assert.AreEqual(true, Character.DetermineIfNewChar(unit, unitReader, inventoryReader.Object, skills));

            inventoryReader
                .Setup(x => x.EnumerateInventoryForward(
                    It.Is<D2Unit>(p => p.Equals(unit))
                ))
                .Returns(new Item[] {
                    new Item { Unit = new D2Unit { eClass = 0x24b } },
                    new Item { Unit = new D2Unit { eClass = 0x211 } },
                    new Item { Unit = new D2Unit { eClass = 0x212 } },
                    new Item { Unit = new D2Unit { eClass = 0x2f } },
                    new Item { Unit = new D2Unit { eClass = 0x148 } },
                    new Item { Unit = new D2Unit { eClass = 0x24b } },
                    new Item { Unit = new D2Unit { eClass = 0x24b } },
                    new Item { Unit = new D2Unit { eClass = 0x24b } },
                });

            Assert.AreEqual(false, Character.DetermineIfNewChar(unit, unitReader, inventoryReader.Object, skills));
            inventoryReader
                .Setup(x => x.EnumerateInventoryForward(
                    It.Is<D2Unit>(p => p.Equals(unit))
                ))
                .Returns(startingItems);
            Assert.AreEqual(true, Character.DetermineIfNewChar(unit, unitReader, inventoryReader.Object, skills));

            var skillRaiseSkeleton = new SkillInfo { Id = (uint)Skill.RAISE_SKELETON, Points = 0 };
            skills.Add(skillRaiseSkeleton);
            Assert.AreEqual(false, Character.DetermineIfNewChar(unit, unitReader, inventoryReader.Object, skills));
            skills.Remove(skillRaiseSkeleton);
            Assert.AreEqual(true, Character.DetermineIfNewChar(unit, unitReader, inventoryReader.Object, skills));

            statsList.FullStats.Length = 0;
            try
            {
                Character.DetermineIfNewChar(unit, unitReader, inventoryReader.Object, skills);
                Assert.Fail();
            } catch (Exception e)
            {
                Assert.AreEqual("Invalid level", e.Message);
            }
            statsList.FullStats.Length = 5;
            Assert.AreEqual(true, Character.DetermineIfNewChar(unit, unitReader, inventoryReader.Object, skills));
            statsList.FullStats.Length = 1;
            Assert.AreEqual(true, Character.DetermineIfNewChar(unit, unitReader, inventoryReader.Object, skills));
        }
    }
}
