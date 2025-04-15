using System;
using System.Collections.Generic;

[Serializable]
public class SkillData
{
    private string id;
    private string name;
    private string description;
    private int mpCost;
    private int cooldown;
    private string targetType; // Self, Enemy, AllEnemies, Random
    private string effect; // e.g. Damage=30, Buff=BUFF_ATK_UP
    private string visualEffectId;
}

[Serializable]
public class DiceRuleData
{
    private string ruleType;
    private string condition;
    private string value;
    private string effect;
}