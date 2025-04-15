using System;
using System.Collections.Generic;

[Serializable]
public class EventData
{
    private string id;
    private string type;
    private string condition;
    private string effect;
}
[Serializable]
public class DialogueData
{
    private string id;
    private string characterId;
    private string text;
    private List<string> choiceIds;
    private string eventTrigger; // e.g. StartBattle=Goblin
}


[Serializable]
public class ChoiceData
{
    private string id;
    private string text;
    private string nextDialogueId;
    private string effect; // e.g. Heal=50, Buff=BUFF_LUCK
}
[Serializable]
public class QuestData
{
    private string id;
    private string description;
    private string condition; // e.g. ReachTile=10, KillMonster=Slime
    private string rewardId;
    private bool repeatable;
}
