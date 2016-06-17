using System;

namespace DiabloInterface.Serialization
{
    public interface ISettingsReader : IDisposable
    {
        ApplicationSettings Read();
    }
}
