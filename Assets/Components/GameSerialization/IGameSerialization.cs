using System;

namespace Gram.GameSerialization
{
    public interface IGameSerialization
    {
        void SaveGame(string state, Action doneCallack);
        void LoadGame(Action<string> doneCallack);
    }
}