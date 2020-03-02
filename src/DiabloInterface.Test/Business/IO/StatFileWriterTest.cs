namespace DiabloInterface.Test.Business.IO
{
    using System.Collections.Generic;
    using System.IO;
    using DiabloInterface.Test.Mocks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Zutatensuppe.DiabloInterface.Business.Data;
    using Zutatensuppe.DiabloInterface.Business.IO;

    [TestClass]
    public class StatFileWriterTest
    {
        TextFileWriterMock fileWriter;
        CharacterStatFileWriter statWriter;

        [TestInitialize]
        public void Initialize()
        {
            fileWriter = new TextFileWriterMock();
            statWriter = new CharacterStatFileWriter(fileWriter, @"tests\files");
        }

        [TestMethod]
        public void EnsureAllFilesAreWritten()
        {
            statWriter.WriteFiles(GetTestCharacterData());
            CompareStatFileValues(new Dictionary<string, string>()
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
            });
        }

        void CompareStatFileValues(IReadOnlyDictionary<string, string> comparisons)
        {
            Assert.AreEqual(comparisons.Count, fileWriter.FileContents.Count);

            foreach (var pair in comparisons)
            {
                var content = GetStatFileContents(pair.Key);
                Assert.AreEqual(pair.Value, content);
            }
        }

        static ICharacterStats GetTestCharacterData()
        {
            return new CharacterStatsMock
            {
                Name = "TestCharacter",
                Level = 1,
                Strength = 2,
                Dexterity = 3,
                Vitality = 4,
                Energy = 5,
                FireResist = 6,
                ColdResist = 7,
                LightningResist = 8,
                PoisonResist = 9,
                FasterHitRecovery = 10,
                FasterRunWalk = 11,
                FasterCastRate = 12,
                IncreasedAttackSpeed = 13,
                Gold = 14,
                GoldStash = 10,
                Deaths = 15
            };
        }

        string GetStatFileContents(string fileName)
        {
            var path = Path.Combine(@"tests\files", $"{fileName}.txt");
            return fileWriter.FileContents[path];
        }
    }
}
