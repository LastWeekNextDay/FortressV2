using UnityEngine;

namespace Species
{
    public class HumanSocialBehavioralStateRage : BehavioralState
    {
        public HumanSocialBehavioralStateRage(BehavioralStateContext behavioralStateContext, bool isRoot = false, BehavioralState parent = null) : base(behavioralStateContext, isRoot, parent)
        {
        }

        public override void Enter()
        {
            ParentBehavioralState.SwitchStateInRoot(
                new HumanRageBehavioralState(behavioralStateContext: BehavioralStateContext));
        }

        public override void Update()
        {
            Debug.Log("HumanSocialBehavioralStateRage");
        }

        public override void Exit()
        {
            ParentBehavioralState.SwitchStateInRoot(
                new HumanBehavioralStateWander(behavioralStateContext: BehavioralStateContext));
        }
    }
}
