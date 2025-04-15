using System;
using System.Collections.Generic;

[Serializable]
public class ItemData
{
    private string id;
    private string name;
    private string type;
    private string effect;
    private string equipSlot;
    private int price;
}

[Serializable]
public class RewardData
{
    private string id;
    private int gold;
    private int experience;
    private List<string> itemIds;
    private List<string> buffIds;
}
[Serializable]
public class ShopData
{
    private string id;
    private List<string> itemIds;
    private List<int> prices;
    private bool canBuy;
    private string discountCondition; // Optional
}
[Serializable]
public class BuffData
{
    private string id;
    private string name;
    private string type; // Buff or Debuff
    private string target; // Self, Enemy, All
    private int duration;
    private string effectType; // e.g. ATK_UP, DOT, Silence
    private bool stackable;
}