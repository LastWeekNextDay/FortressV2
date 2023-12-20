using UnityEngine;

namespace Species
{
    public class HumanBehavioralStateWander : BehavioralState
    {
        public HumanBehavioralStateWander(BehavioralStateContext behavioralStateContext, bool isRoot = false, 
            BehavioralState parent = null) : base(behavioralStateContext, isRoot, parent)
        {
        }

        public override void Enter()
        {
        }

        public override void Update()
        {
            if (BehavioralStateContext.SpeciesGameObject.Navigator.IsAtDestination())
            {
                var randomPoint = new Vector2(Random.Range(0, 9f), Random.Range(0, 9f));
                BehavioralStateContext.SpeciesGameObject.Navigator.GoTo(randomPoint);
            }
        }

        public override void Exit()
        {
            BehavioralStateContext.SpeciesGameObject.Navigator.Stop();
        }
    }
}
