namespace Zutatensuppe.DiabloInterface.Plugin.FileWriter.Writer
{
    using System;
    using System.IO;

    public class CharacterStatFileWriter
    {
        readonly ITextFileWriter writer;
        readonly string directoryPath;

        public CharacterStatFileWriter(ITextFileWriter writer, string directoryPath)
        {
            this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
            this.directoryPath = directoryPath ?? throw new ArgumentNullException(nameof(directoryPath));
        }

        public void WriteFiles(ICharacterStats characterData)
        {
            EnsureStatFileDirectoryExists();
            WriteCharacterStatFiles(characterData);
        }

        void EnsureStatFileDirectoryExists()
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        void WriteCharacterStatFiles(ICharacterStats characterData)
        {
            WriteStatFile("name", characterData.Name);
            WriteStatFile("level", characterData.Level);
            WriteStatFile("strength", characterData.Strength);
            WriteStatFile("dexterity", characterData.Dexterity);
            WriteStatFile("vitality", characterData.Vitality);
            WriteStatFile("energy", characterData.Energy);
            WriteStatFile("fire_res", characterData.FireResist);
            WriteStatFile("cold_res", characterData.ColdResist);
            WriteStatFile("light_res", characterData.LightningResist);
            WriteStatFile("poison_res", characterData.PoisonResist);
            WriteStatFile("gold", characterData.Gold + characterData.GoldStash);
            WriteStatFile("deaths", characterData.Deaths);
            WriteStatFile("fcr", characterData.FasterCastRate);
            WriteStatFile("frw", characterData.FasterRunWalk);
            WriteStatFile("fhr", characterData.FasterHitRecovery);
            WriteStatFile("ias", characterData.IncreasedAttackSpeed);
        }

        void WriteStatFile<T>(string fileName, T data)
        {
            var path = Path.Combine(directoryPath, $"{fileName}.txt");
            writer.WriteFile(path, data.ToString());
        }
    }
}
