using System;

namespace Species
{
    public class BehavioralStateContext
    {
        public SpeciesGameObject SpeciesGameObject { get; private set; }
        public BehavioralState CurrentBehavioralStateRoot { get; set; }
    
        public BehavioralStateContext(SpeciesGameObject speciesGameObject)
        {
            SpeciesGameObject = speciesGameObject;
            switch (SpeciesGameObject.Species)
            {
                case HumanSpecies:
                    CurrentBehavioralStateRoot = new HumanBehavioralStateRoot(this);
                    CurrentBehavioralStateRoot.Enter();
                    break;
                default:
                    throw new Exception("Species not implemented");
            }
        }
    
        public void Update()
        {
            CurrentBehavioralStateRoot?.Update();
        }
    }
}
