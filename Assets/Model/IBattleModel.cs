using System.Collections.Generic;

namespace Gram.Model
{
    public interface IBattleModel : ISerializableModel
    {

        void GenerateBattle(List<int> participatingHeroIds, Enemy enemy);

    }
}