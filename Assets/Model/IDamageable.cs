using Gram.Core;

namespace Gram.Model
{
    public interface IDamageable
    {
        void Damage(int damage);
        void Heal(int value);
        void Heal();
        
        bool IsAlive();
    }
}