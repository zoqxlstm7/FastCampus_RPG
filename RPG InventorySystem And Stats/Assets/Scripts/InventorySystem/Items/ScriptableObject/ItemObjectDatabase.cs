using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/New Item Database")]
public class ItemObjectDatabase : ScriptableObject
{
    #region Variables
    public ItemObject[] itemObjects = null; // 아이템 오브젝트 배열
    #endregion Variables

    #region Unity Methods
    /// <summary>
    /// 데이터를 변경하면 호출되는 함수
    /// </summary>
    private void OnValidate()
    {
        // 아이템 인덱스 초기화
        for (int index = 0; index < itemObjects.Length; index++)
        {
            itemObjects[index].data.id = index;
            itemObjects[index].data.name = itemObjects[index].icon.name;
        }
    }
    #endregion Unity Methods
}
