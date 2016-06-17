using System;

namespace DiabloInterface.Serialization
{
    public interface ISettingsWriter : IDisposable
    {
        void Write(ApplicationSettings settings);
    }
}
