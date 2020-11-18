using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quests/New Quest")]
public class QuestObject : ScriptableObject
{
    #region Variables
    public Quest data = new Quest();    // 퀘스트 데이터

    public QuestStatus status;          // 퀘스트 진행 상태
    #endregion Variables
}
