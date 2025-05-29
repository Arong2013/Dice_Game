using System;
using System.Collections.Generic;

[Serializable]
public class BoardTileData
{
     public int index;
    public string tileType;
    public string eventID;
    public string effect;
}
[Serializable]
public class BoardMapData
{
    private string id;
    private int tileCount;
    private int startTileIndex;
    private int bossTileIndex;
    private List<string> tileTypes; // e.g. ["Battle", "Shop", "Trap", ...]
}