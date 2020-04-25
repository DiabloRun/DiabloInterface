namespace Zutatensuppe.DiabloInterface.Plugin.Autosplits.AutoSplits
{
    using System;
    using System.Runtime.Serialization;

    using Zutatensuppe.D2Reader.Models;

    [Serializable]
    public class AutoSplit : ISerializable
    {
        internal const string DEFAULT_NAME = "Unnamed";

        public enum SplitType
        {
            None = -1,
            CharLevel = 0,
            Area = 1,
            Item = 2,
            Quest = 3,
            Special = 4,
            Gems = 5,
        }

        public enum Special
        {
            GameStart = 1,
            Clear100Percent,
            Clear100PercentAllDifficulties,
        }

        private class Item
        {
            public string Name;
            public int Value;
            public Item(string name, int value)
            {
                Name = name; Value = value;
            }
            public override string ToString()
            {
                // Generates the text shown in the combo box
                return Name;
            }
        }

        /// <summary>
        /// Called when the split has been reached.
        /// </summary>
        public event Action<AutoSplit> Reached;
        /// <summary>
        /// Called when the split has been reset.
        /// </summary>
        public event Action<AutoSplit> Reset;

        public short Difficulty { get; set; }
        public SplitType Type { get; set; }
        public short Value { get; set; }
        public string Name { get; set; }

        private bool isReached = false;
        /// <summary>
        /// Get or set wether the split has been reached.
        /// The Reached event is called when the split is reached.
        /// </summary>
        public bool IsReached
        {
            get { return isReached; }
            set
            {
                bool wasChanged = isReached != value;
                isReached = value;

                if (wasChanged)
                {
                    if (isReached)
                    {
                        OnReached();
                    }
                    else
                    {
                        OnReset();
                    }
                }
            }
        }

        public AutoSplit()
        {
            Name = DEFAULT_NAME;
            Type = SplitType.None;
            Value = -1;
            Difficulty = 0;
        }

        public AutoSplit(AutoSplit other)
        {
            Name = other.Name;
            Type = other.Type;
            Value = other.Value;
            Difficulty = other.Difficulty;
        }

        public AutoSplit(string name, SplitType type, short value, short difficulty)
        {
            Name = name;
            Type = type;
            Value = value;
            Difficulty = difficulty;
        }

        public AutoSplit(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("Name");
            Type = (SplitType)Enum.Parse(typeof(SplitType), info.GetString("Type"));
            Value = info.GetInt16("Value");
            Difficulty = info.GetInt16("Difficulty");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("Type", Enum.GetName(typeof(SplitType), Type));
            info.AddValue("Value", Value);
            info.AddValue("Difficulty", Difficulty);
        }

        public bool IsDifficultyIgnored()
        {
            if (Type == SplitType.CharLevel)
                return true;

            if (Type == SplitType.Special && (Value == (short)Special.Clear100PercentAllDifficulties || Value == (short)Special.GameStart))
                return true;

            if (Type == SplitType.Gems)
                return true;

            return false;
        }

        public bool MatchesDifficulty(GameDifficulty difficulty)
        {
            if (IsDifficultyIgnored())
                return true;

            return Difficulty == (short)difficulty;
        }

        /// <summary>
        /// Trigger the reached event.
        /// </summary>
        void OnReached()
        {
            Reached?.Invoke(this);
        }

        /// <summary>
        /// Trigger the reset event.
        /// </summary>
        void OnReset()
        {
            Reset?.Invoke(this);
        }
    }
}
