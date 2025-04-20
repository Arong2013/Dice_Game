using System;
using UnityEngine;


[CreateAssetMenu(menuName = "GameData/QuestData")]
public class QuestDataSO : ScriptableObject
{
  [SerializeField]   private int id;
  [SerializeField]  private string name;
}
