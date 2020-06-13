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

            // starting skills for amazon
            var skillData = new Dictionary<D2Skill, D2SkillData>();
            skillData.Add(new D2Skill(), new D2SkillData { SkillId = (uint)Skill.UNKNOWN });
            skillData.Add(new D2Skill(), new D2SkillData { SkillId = (uint)Skill.THROW });
            skillData.Add(new D2Skill(), new D2SkillData { SkillId = (uint)Skill.KICK });
            skillData.Add(new D2Skill(), new D2SkillData { SkillId = (uint)Skill.SCROLL_IDENT });
            skillData.Add(new D2Skill(), new D2SkillData { SkillId = (uint)Skill.TOME_IDENT });
            skillData.Add(new D2Skill(), new D2SkillData { SkillId = (uint)Skill.SCROLL_TP });
            skillData.Add(new D2Skill(), new D2SkillData { SkillId = (uint)Skill.TOME_TP });
            skillData.Add(new D2Skill(), new D2SkillData { SkillId = (uint)Skill.UNSUMMON });

            var skillReader = new Mock<ISkillReader>();
            skillReader
                .Setup(x => x.EnumerateSkills(
                    It.Is<D2Unit>(p => p.Equals(unit))
                ))
                .Returns(skillData.Keys);
            skillReader
                .Setup(x => x.ReadSkillData(
                    It.Is<D2Skill>(s => skillData.ContainsKey(s))
                ))
                .Returns<D2Skill>((skill) => skillData[skill]);
            skillReader
                .Setup(x => x.GetTotalNumberOfSkillPoints(
                    It.Is<D2Skill>(s => skillData.ContainsKey(s))
                ))
                .Returns(1); // TODO: isnt 1 for all cases. should test other values

            var unitReader = new UnitReader(processMemoryReader.Object, gameMemoryTable, stringReader, skillReader.Object);

            Assert.AreEqual(true, Character.DetermineIfNewChar(unit, unitReader, inventoryReader.Object, skillReader.Object));

            lvlStat.Value = 2;
            Assert.AreEqual(false, Character.DetermineIfNewChar(unit, unitReader, inventoryReader.Object, skillReader.Object));
            lvlStat.Value = 1;
            Assert.AreEqual(true, Character.DetermineIfNewChar(unit, unitReader, inventoryReader.Object, skillReader.Object));

            xpStat.Value = 1;
            Assert.AreEqual(false, Character.DetermineIfNewChar(unit, unitReader, inventoryReader.Object, skillReader.Object));
            xpStat.Value = 0;
            Assert.AreEqual(true, Character.DetermineIfNewChar(unit, unitReader, inventoryReader.Object, skillReader.Object));

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

            Assert.AreEqual(false, Character.DetermineIfNewChar(unit, unitReader, inventoryReader.Object, skillReader.Object));
            inventoryReader
                .Setup(x => x.EnumerateInventoryForward(
                    It.Is<D2Unit>(p => p.Equals(unit))
                ))
                .Returns(startingItems);
            Assert.AreEqual(true, Character.DetermineIfNewChar(unit, unitReader, inventoryReader.Object, skillReader.Object));

            var skillRaiseSkeleton = new D2Skill();
            skillData.Add(skillRaiseSkeleton, new D2SkillData { SkillId = (uint)Skill.RAISE_SKELETON });
            Assert.AreEqual(false, Character.DetermineIfNewChar(unit, unitReader, inventoryReader.Object, skillReader.Object));
            skillData.Remove(skillRaiseSkeleton);
            Assert.AreEqual(true, Character.DetermineIfNewChar(unit, unitReader, inventoryReader.Object, skillReader.Object));

            statsList.FullStats.Length = 0;
            try
            {
                Character.DetermineIfNewChar(unit, unitReader, inventoryReader.Object, skillReader.Object);
                Assert.Fail();
            } catch (Exception e)
            {
                Assert.AreEqual("Invalid level", e.Message);
            }
            statsList.FullStats.Length = 5;
            Assert.AreEqual(true, Character.DetermineIfNewChar(unit, unitReader, inventoryReader.Object, skillReader.Object));
            statsList.FullStats.Length = 1;
            Assert.AreEqual(true, Character.DetermineIfNewChar(unit, unitReader, inventoryReader.Object, skillReader.Object));
        }
    }
}
