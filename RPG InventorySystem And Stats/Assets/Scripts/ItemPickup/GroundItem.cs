using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    #region Variables
    public ItemObject itemObject;   // 아이템 오브젝트
    public int amount = 1;          // 아이템 수량
    #endregion Variables

    /// <summary>
    /// 스프라이트 렌더러에 아이콘 설정 
    /// </summary>
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (itemObject != null)
        {
            // ? : Optional 변수?
            GetComponent<SpriteRenderer>().sprite = itemObject?.icon;
        }
#endif
    }
}
