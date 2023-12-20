namespace Species
{
    public class HumanBehavioralStateRoot : BehavioralState
    {
        public HumanBehavioralStateRoot(BehavioralStateContext behavioralStateContext) : base(behavioralStateContext, true)
        {
            ChildBehavioralState = new HumanBehavioralStateWander(behavioralStateContext, parent: this);
            CurrentSocialBehavioralState = new HumanSocialBehavioralStateRage(behavioralStateContext, parent: this);
        }

        public override void Enter()
        {
            ChildBehavioralState?.Enter();
            CurrentSocialBehavioralState?.Enter();
        }

        public override void Update()
        {
            ChildBehavioralState?.Update();
            CurrentSocialBehavioralState?.Update();
        }

        public override void Exit()
        {
            ChildBehavioralState?.Exit();
            CurrentSocialBehavioralState?.Exit();
        }
    }
}
