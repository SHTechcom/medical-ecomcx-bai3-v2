using System.Collections.Generic;
using UnityEngine;

namespace _Main.Phan1.Bai1.Scripts.UI
{
    [CreateAssetMenu(fileName = "CheckListData", menuName = "Project/CheckList/Data")]
    public class CheckListData : ScriptableObject
    {
        public string Title;
        public List<CheckListItemData> CheckListItemDatas;
    }

    [System.Serializable]
    public class CheckListItemData
    {
        public string label;
        public bool isCorrect;
    }
}