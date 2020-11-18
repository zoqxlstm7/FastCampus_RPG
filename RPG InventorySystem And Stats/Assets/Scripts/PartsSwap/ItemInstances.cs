using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EquipmentCombiner 클래스에서 생성된 아이템 오브젝트를
/// 저장하는 클래스
/// </summary>
public class ItemInstances : MonoBehaviour
{
    // 아이템 리스트
    public List<Transform> itemTransforms = new List<Transform>();

    /// <summary>
    /// 오브젝트가 제거될 때 하위 게임 오브젝트를 모두 제거
    /// </summary>
    private void OnDestroy()
    {
        foreach (Transform item in itemTransforms)
        {
            Destroy(item.gameObject);
        }
    }
}
