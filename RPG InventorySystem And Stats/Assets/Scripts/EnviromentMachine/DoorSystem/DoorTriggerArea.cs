using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인터랙트 가능한 영역인지 판단하는 객체
/// </summary>
public class DoorTriggerArea : MonoBehaviour
{
    #region Variables
    public DoorEventObject doorEventObject = null;  // Door 이벤트 오브젝트
    public DoorController doorController = null;    // Door 컨트롤러

    public bool autoClose = true;                   // 자동 닫힘 플래그
    #endregion Variables

    #region Unity Methods
    private void OnTriggerEnter(Collider other)
    {
        doorEventObject.OpenDoor(doorController.id);
    }

    private void OnTriggerExit(Collider other)
    {
        if(autoClose)
        {
            doorEventObject.CloseDoor(doorController.id);
        }
    }
    #endregion Unity Methods
}
