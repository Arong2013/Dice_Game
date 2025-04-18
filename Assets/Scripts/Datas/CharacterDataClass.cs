using System.Collections.Generic;
using System;

[Serializable]
public class CharacterData
{
    private int id;
    private string name;
    private int hp;
    private int atk;
    private int def;
    private List<string> skillIDs;
    private List<string> passiveIDs;
}