using System.Collections.Generic;

namespace Items
{
    public enum AttackType
    {
        Melee,
        Ranged
    }
    
    public interface IWeapon
    {
        List<AttackType> AttackTypes { get; }
        void Attack(AttackType attackType, IAttackable target);
    }
}
