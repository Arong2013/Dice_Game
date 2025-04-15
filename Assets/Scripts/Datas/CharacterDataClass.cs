
using System.Collections.Generic;
using System;

[Serializable]
public class CharacterData
{
    private string id;
    private string name;
    private string job;
    private int hp;
    private int atk;
    private int def;
    private List<string> skillIDs;
    private List<string> passiveIDs;
}