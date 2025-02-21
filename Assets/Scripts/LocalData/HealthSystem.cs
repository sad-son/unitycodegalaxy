using System;

namespace DefaultNamespace
{
    public class HealthSystem
    {
        public static int currentHealth => LocalDataSystem.localData.userInfo.health;
        public static event Action<int> OnHealthChanged;
        
        public static void Decrease()
        {
            var value = Math.Clamp(LocalDataSystem.localData.userInfo.health-1,0, LocalDataSystem.localData.userInfo.maxHealth);
            LocalDataSystem.localData.userInfo.health = value;
            OnHealthChanged?.Invoke(LocalDataSystem.localData.userInfo.health);
        }
        
        public static void Restore()
        {
            LocalDataSystem.localData.userInfo.health = LocalDataSystem.localData.userInfo.maxHealth;
            OnHealthChanged?.Invoke(LocalDataSystem.localData.userInfo.health);
        }
    }
}