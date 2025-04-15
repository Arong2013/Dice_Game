using System;
using System.Collections.Generic;

[Serializable]
public class BoardTileData
{
    private int index;
    private string tileType;
    private string eventID;
    private string effect;
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