using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    // 인터랙트 가능 거리
    float Distance { get; }

    // 인터랙트 시 처리 함수
    bool Interact(GameObject other);
    // 인터랙트 정지시 처리 함수
    void StopInteract(GameObject other);
}
