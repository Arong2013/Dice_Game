using System;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/CharacterData")]
public class CharacterDataSO : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private string name;
    [SerializeField] private int hp;
}


