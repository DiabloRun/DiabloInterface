using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Zutatensuppe.DiabloInterface.Plugin.FileWriter.Writer.Test
{
    [TestClass]
    public class StatFileWriterTest
    {
        [TestMethod]
        public void EnsureAllFilesAreWritten()
        {
            var files = new Dictionary<string, string>();

            var fileWriter = new Mock<ITextFileWriter>();
            fileWriter
                .Setup(w => w.WriteFile(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string path, string contents) => {
                    files[path] = contents;
                });


            var stats = new Mock<ICharacterStats>();
            stats.Setup(s => s.Name).Returns("TestCharacter");
            stats.Setup(s => s.Level).Returns(1);
            stats.Setup(s => s.Strength).Returns(2);
            stats.Setup(s => s.Dexterity).Returns(3);
            stats.Setup(s => s.Vitality).Returns(4);
            stats.Setup(s => s.Energy).Returns(5);
            stats.Setup(s => s.FireResist).Returns(6);
            stats.Setup(s => s.ColdResist).Returns(7);
            stats.Setup(s => s.LightningResist).Returns(8);
            stats.Setup(s => s.PoisonResist).Returns(9);
            stats.Setup(s => s.FasterHitRecovery).Returns(10);
            stats.Setup(s => s.FasterRunWalk).Returns(11);
            stats.Setup(s => s.FasterCastRate).Returns(12);
            stats.Setup(s => s.IncreasedAttackSpeed).Returns(13);
            stats.Setup(s => s.Gold).Returns(14);
            stats.Setup(s => s.GoldStash).Returns(10);
            stats.Setup(s => s.Deaths).Returns(15);

            var statWriter = new CharacterStatFileWriter(fileWriter.Object, @"tests\files");
            statWriter.WriteFiles(stats.Object);

            var comparisons = new Dictionary<string, string>()
            {
                { "name", "TestCharacter" },
                { "level", "1" },
                { "strength", "2" },
                { "dexterity", "3" },
                { "vitality", "4" },
                { "energy", "5" },
                { "fire_res", "6" },
                { "cold_res", "7" },
                { "light_res", "8" },
                { "poison_res", "9" },
                { "fhr", "10" },
                { "frw", "11" },
                { "fcr", "12" },
                { "ias", "13" },
                { "gold", "24" },
                { "deaths", "15" },
            };

            Assert.AreEqual(comparisons.Count, files.Count);

            foreach (var pair in comparisons)
            {
                var path = Path.Combine(@"tests\files", $"{pair.Key}.txt");
                Assert.AreEqual(pair.Value, files[path]);
            }
        }
    }
}
