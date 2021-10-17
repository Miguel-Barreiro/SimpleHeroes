using System.Collections.Generic;

namespace Gram.Core
{
    public interface ICharacterDatabase
    {
        List<CharacterConfiguration> GetHeroCharactersData(int number, List<CharacterConfiguration> excludingList = null);
        CharacterConfiguration GetHeroCharacterConfigurationById(string id);
        CharacterConfiguration GetEnemyCharacterConfigurationById(string id);
    }
}