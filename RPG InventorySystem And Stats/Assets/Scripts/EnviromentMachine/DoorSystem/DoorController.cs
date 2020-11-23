using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 문의 애니메이션을 담당 (열림, 닫힘)
/// </summary>
public class DoorController : MonoBehaviour
{
    #region Variables
    public DoorEventObject doorEventObject = null;  // Door 이벤트 오브젝트

    public int id = 0;                              // 오브젝트 ID
    public float openOffset = 4.0f;                 // 여는 오프셋
    public float closeOffset = 1.0f;                // 닫는 오프셋
    #endregion Variables

    #region Unity Methods
    private void OnEnable()
    {
        // 이벤트 등록
        doorEventObject.OnOpenDoor += OnOpenDoor;
        doorEventObject.OnCloseDoor += OnCloseDoor;
    }

    private void OnDisable()
    {
        // 이벤트 제거
        doorEventObject.OnOpenDoor -= OnOpenDoor;
        doorEventObject.OnCloseDoor -= OnCloseDoor;
    }
    #endregion Unity Methods

    #region Main Methods
    /// <summary>
    /// 문을 여는 이벤트 함수
    /// </summary>
    /// <param name="id">Door Object ID</param>
    public void OnOpenDoor(int id)
    {
        if (this.id != id)
            return;

        StopAllCoroutines();
        StartCoroutine(OpenDoor());
    }

    /// <summary>
    /// 문을 다는 이벤트 함수
    /// </summary>
    /// <param name="id">Door Object ID</param>
    public void OnCloseDoor(int id)
    {
        if (this.id != id)
            return;

        StopAllCoroutines();
        StartCoroutine(CloseDoor());
    }

    /// <summary>
    /// 문을 여는 로직을 처리하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator OpenDoor()
    {
        while (transform.position.y < openOffset)
        {
            Vector3 calcPosition = transform.position;
            calcPosition.y += 0.01f;
            transform.position = calcPosition;

            yield return null;
        }
    }

    /// <summary>
    /// 문을 닫는 로직을 처리하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator CloseDoor()
    {
        while (transform.position.y > closeOffset)
        {
            Vector3 calcPosition = transform.position;
            calcPosition.y -= 0.01f;
            transform.position = calcPosition;

            yield return null;
        }
    }
    #endregion Main Methods
}
