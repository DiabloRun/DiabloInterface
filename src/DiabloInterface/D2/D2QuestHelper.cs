using System.Collections.Generic;

namespace DiabloInterface.D2
{
    public class D2QuestHelper
    {
        public class D2Quest
        {
            public int Act { get; set; }
            public int Quest { get; set; }
            public bool BossQuest { get; set; } = false;
            public int BufferIndex { get; set; }
            public ushort CompletionBits { get; set; } = (1 << 0) | (1 << 1);
            public ushort ReallyCompletionBits { get; set; } = (1 << 0); // reward collected
            public string Name { get; set; } // quest name used in the game
            public string CommonName { get; set; } // name commonly used to reference the quest
        }
        
        // static initialization of the quests
        public static readonly Dictionary<Quest, D2Quest> Quests = new Dictionary<Quest, D2Quest>()
        {
            { Quest.A1Q6, new D2Quest { BufferIndex=6, Act=1, Quest=6, Name="Sisters to the Slaughter", CommonName="Andariel", BossQuest=true, } },
            { Quest.A2Q6, new D2Quest { BufferIndex=14, Act=2, Quest=6, Name="The Seven Tombs", CommonName="Duriel", BossQuest=true, CompletionBits=(1 << 5), } },
            { Quest.A3Q6, new D2Quest { BufferIndex=22, Act=3, Quest=6, Name="The Guardian", CommonName="Mephisto", BossQuest=true, } },
            { Quest.A4Q2, new D2Quest { BufferIndex=26, Act=4, Quest=3, Name="Terror's End", CommonName="Diablo", BossQuest=true, } },
            { Quest.A5Q6, new D2Quest { BufferIndex=40, Act=5, Quest=6, Name="Eve of Destruction", CommonName="Baal", BossQuest=true, } },

            { Quest.A1Q1, new D2Quest { BufferIndex=1, Act=1, Quest=1, Name="Den of Evil", CommonName="Den of Evil", } },
            { Quest.A1Q2, new D2Quest { BufferIndex=2, Act=1, Quest=2, Name="Sisters' Burial Grounds", CommonName="Blood Raven", } },
            { Quest.A1Q3, new D2Quest { BufferIndex=3, Act=1, Quest=5, Name="Tools of the Trade", CommonName="Charsie Imbue", } },
            { Quest.A1Q4, new D2Quest { BufferIndex=4, Act=1, Quest=3, Name="The Search for Cain", CommonName="Cain", } },
            { Quest.A1Q5, new D2Quest { BufferIndex=5, Act=1, Quest=4, Name="The Forgotten Tower", CommonName="Countess", } },

            { Quest.A2Q1, new D2Quest { BufferIndex=9, Act=2, Quest=1, Name="Radament's Lair", CommonName="Radament", } },
            { Quest.A2Q2, new D2Quest { BufferIndex=10, Act=2, Quest=2, Name="The Horadric Staff", CommonName="Horadric Staff", } },
            { Quest.A2Q3, new D2Quest { BufferIndex=11, Act=2, Quest=3, Name="Tainted Sun", CommonName="Claw Viper Temple", } },
            { Quest.A2Q4, new D2Quest { BufferIndex=12, Act=2, Quest=4, Name="Arcane Sanctuary", CommonName="Arcane Sancuary", } },
            { Quest.A2Q5, new D2Quest { BufferIndex=13, Act=2, Quest=5, Name="The Summoner", CommonName="Horazon", } },

            { Quest.A3Q1, new D2Quest { BufferIndex=17, Act=3, Quest=4, Name="Lam Esen's Tome", CommonName="Battlemaid Sarina", } },
            { Quest.A3Q2, new D2Quest { BufferIndex=18, Act=3, Quest=3, Name="Khalim's Will", CommonName="Khalim's Will", } },
            { Quest.A3Q3, new D2Quest { BufferIndex=19, Act=3, Quest=2, Name="Blade of the Old Religion", CommonName="Gibdinn", } },
            { Quest.A3Q4, new D2Quest { BufferIndex=20, Act=3, Quest=1, Name="The Golden Bird", CommonName="Alkor Quest", } },
            { Quest.A3Q5, new D2Quest { BufferIndex=21, Act=3, Quest=5, Name="The Blackened Temple", CommonName="Travincal Council", } },

            { Quest.A4Q1, new D2Quest { BufferIndex=25, Act=4, Quest=1, Name="The Fallen Angel", CommonName="Izual", } },
            { Quest.A4Q3, new D2Quest { BufferIndex=27, Act=4, Quest=2, Name="Hell's Forge", CommonName="Hell Forge", } },

            { Quest.A5Q1, new D2Quest { BufferIndex=35, Act=5, Quest=1, Name="Siege on Harrogath", CommonName="Shenk", } },
            { Quest.A5Q2, new D2Quest { BufferIndex=36, Act=5, Quest=2, Name="Rescue on Mount Arreat", CommonName="Rescue Barbs", } },
            { Quest.A5Q3, new D2Quest { BufferIndex=37, Act=5, Quest=3, Name="Prison of Ice", CommonName="Rescue Anya", } },
            { Quest.A5Q4, new D2Quest { BufferIndex=38, Act=5, Quest=4, Name="Betrayal of Harrogath", CommonName="Kill Nihlathak", } },
            { Quest.A5Q5, new D2Quest { BufferIndex=39, Act=5, Quest=5, Name="Rite of Passage", CommonName="Ancients" } },

            // special
            { Quest.COW_KING, new D2Quest { BufferIndex=4, Act=0, Quest=0, Name="Cow King", CommonName="Cow King", CompletionBits=(1 << 10), ReallyCompletionBits=(1 << 10), } },
        };
        
        public enum Quest
        {
            // legacy.. old config files have these values
            A1Q6 = 12, // Andariel
            A2Q6 = 28, // Duriel
            A3Q6 = 44, // Mephisto
            A4Q2 = 52, // Diablo
            A5Q6 = 80, // Baal

            A1Q1 = 81, // Den of Evil
            A1Q2, // Sisters' Burial Grounds
            A1Q3, // Tools of the Trade
            A1Q4, // The Search for Cain
            A1Q5, // The Forgotten Tower

            A2Q1, // Radament's Lair
            A2Q2, // The Horadric Staff
            A2Q3, // Tainted Sun
            A2Q4, // Arcane Sanctuary
            A2Q5, // The Summoner

            A3Q1, // Lam Esen's Tome
            A3Q2, // Khalim's Will
            A3Q3, // Blade of the Old Religion
            A3Q4, // The Golden Bird
            A3Q5, // The Blackened Temple

            A4Q1, // The Fallen Angel
            A4Q3, // Hell's Forge

            A5Q1, // Siege on Harrogath
            A5Q2, // Rescue on Mount Arreat
            A5Q3, // Prison of Ice
            A5Q4, // Betrayal of Harrogath
            A5Q5, // Rite of Passage

            COW_KING, // The Search for Cain
        }

        public static D2Quest GetByActAndQuest(int act, int quest)
        {
            foreach (KeyValuePair<Quest, D2Quest> item in Quests)
            {
                if (item.Value.Act == act && item.Value.Quest == quest)
                {
                    return item.Value;
                }
            }
            return null;
        }

        public static D2Quest GetByQuestBufferIndex(int index)
        {
            foreach (KeyValuePair<Quest, D2Quest> item in Quests)
            {
                if (item.Value.BufferIndex == index)
                {
                    return item.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// Count the really completed quests (rewards collected)
        /// </summary>
        /// <param name="questBuffer"></param>
        /// <returns></returns>
        public static int GetReallyCompletedQuestCount(ushort[] questBuffer)
        {
            if (questBuffer == null)
                return 0;

            int count = 0;
            foreach (KeyValuePair<Quest, D2Quest> item in Quests)
            {
                if ((questBuffer[item.Value.BufferIndex] & item.Value.ReallyCompletionBits) != 0)
                {
                    count++;
                }
            }
            return count;
        }
        
        public static bool IsQuestComplete(Quest quest, ushort[] questBuffer)
        {
            if ( questBuffer == null || !Quests.ContainsKey(quest) )
                return false;

            D2Quest questObject = Quests[quest];
            
            if (questObject.BufferIndex < 0 || questObject.BufferIndex >= questBuffer.Length)
                return false;
            
            return (questBuffer[questObject.BufferIndex] & questObject.CompletionBits) != 0;
        }

    }
}
