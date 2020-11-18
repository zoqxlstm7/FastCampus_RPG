using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    #region Variables
    public string name = null;          // NPC 이름

    [TextArea(3, 10)]
    public string[] sentences = null;   // 대화 배열
    #endregion Variables
}
