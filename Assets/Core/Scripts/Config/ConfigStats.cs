using UnityEngine;
using Utilities.Service.RFirebase;
using Utilities.Services;
using System;

public class ConfigStats
{
    public const int A = 5;

    public static float GetStat(float statBase, float statAdd, int level)
    {
        return statBase + statAdd * level;
    }
    
    public static int GetStat(int statBase, int statAdd, int level)
    {
        return statBase + statAdd * level;
    }

    public static int GetPower(float HP, float damage, float armor, float attackSpeed, float critRate, float accuracy, float dodge, float critDamage)
    {
        var power = HP + damage * 10 + armor * A * 10 + attackSpeed * 100 + critRate * 10 + accuracy * 10 + dodge * 100 + critDamage;

        return (int) power;
    }
}