using System;
using Model;

namespace GameSerialization
{
    public interface IGameSerialization
    {
        void SaveGame(GameState state, Action doneCallack);
        void LoadGame(Action<GameState> doneCallack);
    }
}