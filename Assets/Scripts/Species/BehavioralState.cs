namespace Species
{
    public abstract class BehavioralState
    {
        protected bool IsRoot { get; set; }
        protected BehavioralStateContext BehavioralStateContext { get; private set; }
        protected BehavioralState ParentBehavioralState { get; set; }
        protected BehavioralState ChildBehavioralState { get; set; }
    
        protected BehavioralState CurrentSocialBehavioralState { get; set; }
    
        protected BehavioralState(BehavioralStateContext behavioralStateContext, bool isRoot = false, BehavioralState parent = null)
        {
            IsRoot = isRoot;
            BehavioralStateContext = behavioralStateContext;
            if (!isRoot)
            {
                ParentBehavioralState = parent;
            }
        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
        public void SwitchStateInRoot(BehavioralState behavioralState)
        {
            if (IsRoot)
            {
                if (behavioralState.IsRoot)
                {
                    Exit();
                    BehavioralStateContext.CurrentBehavioralStateRoot = behavioralState;
                    BehavioralStateContext.CurrentBehavioralStateRoot.Enter();
                }
                else
                {
                    ChildBehavioralState?.Exit();
                    ChildBehavioralState = behavioralState;
                    ChildBehavioralState.ParentBehavioralState = this;
                    ChildBehavioralState.Enter();
                }
            }
            else
            {
                ParentBehavioralState?.SwitchStateInRoot(behavioralState);
            }
        }
    
        public void SwitchSocialStateInRoot(BehavioralState behavioralState)
        {
            if (IsRoot)
            {
                CurrentSocialBehavioralState?.Exit();
                CurrentSocialBehavioralState = behavioralState;
                CurrentSocialBehavioralState.Enter();
            }
            else
            {
                ParentBehavioralState?.SwitchSocialStateInRoot(behavioralState);
            }
        }
    }
}
