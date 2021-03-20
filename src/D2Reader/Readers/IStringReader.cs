namespace Zutatensuppe.D2Reader.Readers
{
    public interface IStringReader
    {
        Language Language { get; }

        string GetString(ushort identifier);

        string ConvertCFormatString(string input, out int arguments);
    }
}
