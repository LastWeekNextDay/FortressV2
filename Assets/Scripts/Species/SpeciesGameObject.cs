using System;
using UnityEngine;

namespace Species
{
    public class SpeciesGameObject : MonoBehaviour, ISelectorSelectable
    {
        private Species _species;
        public Species Species
        {
            get => _species;
            set
            {
                _species = value;
                SpriteRenderer.color = _species.Color;
                BehavioralStateContext = new BehavioralStateContext(this);
            }
        }
        public bool PlayerControlled { get; set; }
        public Navigator Navigator { get; private set; }
        
        public CombatSystem CombatSystem { get; private set; }
        private BehavioralStateContext BehavioralStateContext { get; set; }
        private SpriteRenderer SpriteRenderer { get; set; }
        public AnimationController AnimationController { get; set; }
    
        private void Awake()
        {
            Navigator = GetComponent<Navigator>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            CombatSystem = GetComponent<CombatSystem>();
            AnimationController = GetComponent<AnimationController>();
        }

        private void Update()
        {
            if (Math.Abs(Species.DefaultSpeed - Navigator.DefaultSpeed) > 0.01f) // Floating point comparison is bad, but this is fine
            {
                Navigator.DefaultSpeed = Species.DefaultSpeed;
            }
            BehavioralStateContext?.Update();
            if (Species.HealthSystem.CurrentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
