using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 싱글톤을 대신하여 ScriptableObject를 이용한 이벤트 시스템
/// </summary>
[CreateAssetMenu(fileName = "Event System", menuName = "Event Systems/Door Event Object")]
public class DoorEventObject : ScriptableObject
{
    #region Variables
    [System.NonSerialized]
    public System.Action<int> OnOpenDoor = null;    // Open 이벤트

    [System.NonSerialized]
    public System.Action<int> OnCloseDoor = null;   // Close 이벤트
    #endregion Variables

    #region Main Methods
    /// <summary>
    /// 문을 여는 로직를 호출하는 함수
    /// </summary>
    /// <param name="id">Door Object ID</param>
    public void OpenDoor(int id)
    {
        OnOpenDoor?.Invoke(id);
    }

    /// <summary>
    /// 문을 닫는 로직을 호출하는 함수
    /// </summary>
    /// <param name="id">Door Object ID</param>
    public void CloseDoor(int id)
    {
        OnCloseDoor?.Invoke(id);
    }
    #endregion Main Methods
}
