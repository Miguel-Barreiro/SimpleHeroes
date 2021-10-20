using System.Collections.Generic;

namespace Gram.Core
{
    public interface ICharacterDatabase
    {
        List<CharacterConfiguration> GetMultipleRandomHeroCharactersData(int number, List<CharacterConfiguration> excludingList = null);
        
        CharacterConfiguration GetCharacterConfigurationById(string id);

        CharacterConfiguration GetRandomEnemyCharacterData();
    }
}