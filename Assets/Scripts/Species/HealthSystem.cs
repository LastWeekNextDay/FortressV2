namespace Species
{
    public class HealthSystem
    {
        public float MaxHealth { get; protected set; }
        public float CurrentHealth { get; protected set; }
    
        public HealthSystem(float maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }

        public void Damage(float damageAmount)
        {
            CurrentHealth -= damageAmount;
            if (CurrentHealth < 0)
            {
                CurrentHealth = 0;
            }
        }

        public void Heal(float healAmount)
        {
            CurrentHealth += healAmount;
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }
    }
}
