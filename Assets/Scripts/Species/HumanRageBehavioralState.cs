using Items;
using UnityEngine;

namespace Species
{
    public class HumanRageBehavioralState : BehavioralState
    {
        private GameObject _target;
        public HumanRageBehavioralState(BehavioralStateContext behavioralStateContext, bool isRoot = false, BehavioralState parent = null, GameObject target = null) : base(behavioralStateContext, isRoot, parent)
        {
            _target = target;
        }

        public override void Enter()
        {
            _target = GetClosestAttackable();
            if (_target != null)
                BehavioralStateContext.SpeciesGameObject.CombatSystem.Attack(_target.GetComponent<SpeciesGameObject>(), AttackType.Melee);
        }

        public override void Update()
        {
            if (_target == null)
            {
                ParentBehavioralState?.SwitchStateInRoot(new HumanBehavioralStateWander(behavioralStateContext: BehavioralStateContext));
            }
        }

        public override void Exit()
        {
            BehavioralStateContext.SpeciesGameObject.CombatSystem.StopCombat();
        }

        private GameObject GetClosestAttackable()
        {
            var touchedObjects = Physics2D.OverlapCircleAll(BehavioralStateContext.SpeciesGameObject.transform.position, 20f);
            foreach (var touchedObject in touchedObjects)
            {
                var attackable = touchedObject.GetComponent<SpeciesGameObject>();
                if (attackable != null)
                {
                    if (attackable.gameObject == BehavioralStateContext.SpeciesGameObject.gameObject)
                        continue;
                    return attackable.gameObject;
                }
            }
            return null;
        }
    }
}
