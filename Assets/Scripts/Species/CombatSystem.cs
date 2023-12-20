using System.Collections;
using Items;
using UnityEngine;

namespace Species
{
    public class CombatSystem : MonoBehaviour
    {
        private SpeciesGameObject _attackTarget { get; set; }
        private AttackType _attackType { get; set; }
        
        public SpeciesGameObject ThisChar { get; private set; }
        
        private float _timeSinceLastAttack { get; set; }
        
        private void Awake()
        {
            ThisChar = GetComponent<SpeciesGameObject>();
        }
        
        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            if (_attackTarget != null)
            {
                if (_attackType == AttackType.Melee)
                {
                    ThisChar.Navigator.GoTo(_attackTarget.transform.position);
                    if (Vector2.Distance(transform.position, _attackTarget.transform.position) < 1f)
                    {
                        if (_timeSinceLastAttack > 1f)
                        {
                            Debug.Log("Attacking");
                            _timeSinceLastAttack = 0;
                            _attackTarget.Species.HealthSystem.Damage(1);
                            ThisChar.AnimationController.PlayAttackAnimation(_attackTarget.transform.position);
                        }
                    }
                }
            }
        }
        
        public void Attack(SpeciesGameObject attackTarget, AttackType attackType)
        {
            _attackTarget = attackTarget;
            _attackType = attackType;
        }
        
        public void StopCombat()
        {
            _attackTarget = null;
            ThisChar.Navigator.Stop();
        }
    }
}
