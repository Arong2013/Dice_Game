using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameDataSO", menuName = "Game/GameDataSO")]
public class GameDataSO : ScriptableObject
{
    public List<BoardTileData> boardTileDatas = new List<BoardTileData>();
    // 필요한 다른 데이터 리스트들도 추가 가능
}