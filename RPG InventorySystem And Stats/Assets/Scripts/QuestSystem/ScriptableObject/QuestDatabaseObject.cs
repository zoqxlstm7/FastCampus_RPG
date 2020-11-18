using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Database", menuName = "Quest System/Quests/Quest Database")]
public class QuestDatabaseObject : ScriptableObject
{
    #region Variables
    public QuestObject[] questObjects = null;   // 퀘스트 오브젝트
    #endregion Variables

    #region Unity Methods
    /// <summary>
    /// 퀘스트 데이터 ID 초기화
    /// </summary>
    public void OnValidate()
    {
        for (int index = 0; index < questObjects.Length; index++)
        {
            questObjects[index].data.id = index;
        }
    }
    #endregion Unity Methods
}
