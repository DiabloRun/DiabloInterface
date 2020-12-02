namespace Zutatensuppe.D2Reader.Models
{
    internal class QuestDetails
    {
        public QuestId Id { get; set; }
        public int Act { get; set; }
        public int ActOrder { get; set; }
        public bool IsBossQuest { get; set; }
        public ushort BufferIndex { get; set; }
        public ushort CompletionBitMask { get; set; } = (1 << 0) | (1 << 1);
        public ushort FullCompletionBitMask { get; set; } = (1 << 0);
        public string Name { get; set; }
        public string CommonName { get; set; }
    }
}
