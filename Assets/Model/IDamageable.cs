using Gram.Core;

namespace Gram.Model
{
    public interface IDamageable
    {
        event GameBasics.SimpleDelegate OnDeath;
        
        void Damage(int damage);
    }
}