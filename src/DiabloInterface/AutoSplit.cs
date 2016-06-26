using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace DiabloInterface
{
    public class AutoSplit
    {
        public enum SplitType
        {
            None = -1,
            CharLevel = 0,
            Area = 1,
            Item = 2,
            Quest = 3,
            Special = 4,
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
        [JsonConverter(typeof(StringEnumConverter))]
        public SplitType Type { get; set; }
        public short Value { get; set; }
        public string Name { get; set; }

        private bool isReached = false;
        /// <summary>
        /// Get or set wether the split has been reached.
        /// The Reached event is called when the split is reached.
        /// </summary>
        [JsonIgnore]
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
            Name = "Unnamed";
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

        public bool IsDifficultyIgnored()
        {
            if (Type == SplitType.CharLevel)
                return true;

            if (Type == SplitType.Special && (Value == (short)Special.Clear100PercentAllDifficulties || Value == (short)Special.GameStart))
                return true;

            return false;
        }

        public bool MatchesDifficulty(short difficulty)
        {
            if (IsDifficultyIgnored())
                return true;

            return Difficulty == difficulty;
        }

        /// <summary>
        /// Trigger the reached event.
        /// </summary>
        void OnReached()
        {
            var reachedEvent = Reached;
            if (reachedEvent != null)
            {
                reachedEvent(this);
            }
        }

        /// <summary>
        /// Trigger the reset event.
        /// </summary>
        void OnReset()
        {
            var resetEvent = Reset;
            if (resetEvent != null)
            {
                resetEvent(this);
            }
        }
    }
}
