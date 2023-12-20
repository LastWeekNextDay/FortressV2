using Items;
using UnityEngine;

namespace Species
{
    public class HumanSpecies : Species
    {
        public HumanSpecies(Color32 color) : base("Human", color, 1, new HealthSystem(100), new InventorySystem())
        {
        }
    }
}
