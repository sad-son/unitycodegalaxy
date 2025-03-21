﻿using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class HealthSystem
    {
        public static int currentHealth => LocalDataSystem.localData.userInfo.health;
        public static string nextLifeTime => LocalDataSystem.localData.userInfo.nextLifeTime;
        public static event Action<int> OnHealthChanged;
        
        public static void Increase()
        {
            var value = Math.Clamp(LocalDataSystem.localData.userInfo.health + 1, 0, LocalDataSystem.localData.userInfo.maxHealth);
            SaveIfChanged(value);
        }

        public static void SaveIfChanged(int value)
        {
            var oldValue = LocalDataSystem.localData.userInfo.health;
            if (oldValue == value) return;

            if (oldValue > value && LocalDataSystem.localData.userInfo.infiniteLives)
            {
                return;
            }
            LocalDataSystem.localData.userInfo.health = value;
            LocalDataSystem.SaveData();
            OnHealthChanged?.Invoke(LocalDataSystem.localData.userInfo.health);
        }

        public static void Decrease()
        {
            var value = Math.Clamp(LocalDataSystem.localData.userInfo.health - 1,0, LocalDataSystem.localData.userInfo.maxHealth);
            SaveIfChanged(value);
        }

        public static void SaveLives(int value, string nextLifeTime)
        {
            if (nextLifeTime != DateTime.MinValue.ToString())
                LocalDataSystem.localData.userInfo.nextLifeTime = nextLifeTime;
            SaveIfChanged(value);
        }
        
        public static void SaveNextLiftTime(string nextLifeTime)
        {
            if (nextLifeTime != DateTime.MinValue.ToString())
            {
                LocalDataSystem.localData.userInfo.nextLifeTime = nextLifeTime;
                LocalDataSystem.SaveData();
            }
        }
        
        public static void BuyInfiniteLives()
        {
            LocalDataSystem.localData.userInfo.infiniteLives = true;
            LocalDataSystem.SaveData();
        }
        
        public static void Restore()
        {
            SaveIfChanged(LocalDataSystem.localData.userInfo.maxHealth);
        }
    }
}