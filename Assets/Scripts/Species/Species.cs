using Items;
using UnityEngine;

namespace Species
{
    public abstract class Species: IAttackable
    {
        public string SpeciesName { get; protected set; }
        public Color32 Color { get; protected set; }
    
        public float DefaultSpeed { get; protected set; }
        
        public HealthSystem HealthSystem { get; }
        
        public InventorySystem InventorySystem { get; protected set; }
        protected Species(string speciesName, Color32 color, float defaultSpeed, HealthSystem speciesHealthSystem, 
            InventorySystem speciesInventorySystem)
        {
            SpeciesName = speciesName;
            Color = color;
            DefaultSpeed = defaultSpeed;
            HealthSystem = speciesHealthSystem;
            InventorySystem = speciesInventorySystem;
        }
    }
}
