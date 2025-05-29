using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ExcelWebSource", menuName = "Tools/ExcelWebSource")]
public class ExcelWebSource : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        public string name; // 예: "무기 데이터"
        [TextArea(1, 3)]
        public string url;
        public ScriptableObject targetSO; // 데이터를 넣을 대상 SO
    }

    public List<Entry> entries = new();
}