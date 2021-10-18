namespace Gram.Model
{
    public interface ISerializableModel
    {
        string GetSerializedGameState();
        void RestoreGameState(string newGameState);

    }
}