using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유틸리티 클래스
/// </summary>
public class EquipmentCombiner
{
    #region Variables
    // 기본 뼈대에 대한 전체 리스트
    readonly Dictionary<int, Transform> rootBoneDictionary = new Dictionary<int, Transform>();
    // 게임 오브젝트에 대한 루트 (위치)
    readonly Transform transform = null;
    #endregion Variables

    #region Main Methods
    /// <summary>
    /// 뼈대를 가진 캐릭터에 대한 뼈대 정보 저장
    /// </summary>
    /// <param name="rootGo">루트 오브젝트</param>
    public EquipmentCombiner(GameObject rootGo)
    {
        // 위치 저장
        transform = rootGo.transform;
        // 하위 뼈대를 순환하며 뼈대에 대한 정보를 저장
        TraverseHierarchy(transform);
    }

    /// <summary>
    /// 루트 오브젝트의 하위 뼈대를 순환하며 뼈대 정보를 저장하는 함수
    /// </summary>
    /// <param name="root">루트 오브젝트(위치)</param>
    void TraverseHierarchy(Transform root)
    {
        foreach (Transform child in root)
        {
            // 뼈대에 대한 정보 저장
            rootBoneDictionary.Add(child.name.GetHashCode(), child);
            // 자식의 하위 뼈대를 재귀호출로 저장
            TraverseHierarchy(child);
        }
    }
    #endregion Main Methods

    #region Skinned Mesh Process
    /// <summary>
    /// Limb: 팔, 다리
    /// 새로운 SkinnedMeshRenderer를 생성하기 위해
    /// 아이템 오브젝트와 아이템 오브젝트의 본정보를 넘겨주는 함수
    /// </summary>
    /// <param name="itemGo">아이템 오브젝트</param>
    /// <param name="boneNames">오브젝트 내의 본 이름 정보</param>
    /// <returns>새로 생성된 SkinnedMeshRenderer 오브젝트</returns>
    public Transform AddLimb(GameObject itemGo, List<string> boneNames)
    {
        // 영향을 주는 본정보를 이용하여 새로운 SkinnedMeshRenderer 오브젝트 생성
        Transform limb = ProcessBoneObject(itemGo.GetComponentInChildren<SkinnedMeshRenderer>(), boneNames);
        // 부모 지정
        limb.SetParent(transform);

        return limb;
    }

    /// <summary>
    /// SkinnedMeshRenderer의 정보와 본이름을 복사하여 새로운 
    /// SkinnedMeshRenderer 오브젝트를 생성하여 반환하는 함수
    /// </summary>
    /// <param name="renderer">아이템 오브젝트의 SkinnedMeshRenderer</param>
    /// <param name="boneNames">아이템 오브젝트의 본 정보</param>
    /// <returns>생성된 SkinnedMeshRenderer 오브젝트</returns>
    Transform ProcessBoneObject(SkinnedMeshRenderer renderer, List<string> boneNames)
    {
        // 새로운 게임 오브젝트 생성
        Transform itemTransform = new GameObject().transform;
        // SkinnedMeshRenderer 컴포넌트 추가
        SkinnedMeshRenderer meshRenderer = itemTransform.gameObject.AddComponent<SkinnedMeshRenderer>();

        Transform[] boneTransforms = new Transform[boneNames.Count];
        // 본에 대한 Transform 추출
        for (int i = 0; i < boneNames.Count; i++)
        {
            boneTransforms[i] = rootBoneDictionary[boneNames[i].GetHashCode()];
        }

        // 추출한 본으로 초기화
        meshRenderer.bones = boneTransforms;
        // 기존 아이템이 가지고 있던 sharedMesh로 초기화
        meshRenderer.sharedMesh = renderer.sharedMesh;
        // 기존 아이템이 가지고 있던 sharedMaterials로 초기화
        meshRenderer.materials = renderer.sharedMaterials;

        // 생성된 본 오브젝트 반환
        return itemTransform;
    }
    #endregion Skinned Mesh Process

    #region Static Mesh Process
    /// <summary>
    /// Static mesh를 추가하는 함수
    /// Static mesh 여러개가 한번에 장착되는 상황도 존재하여 Transform[]을 반환
    /// </summary>
    /// <param name="itemGo">아이템 오브젝트</param>
    /// <returns></returns>
    public Transform[] AddMesh(GameObject itemGo)
    {
        Transform[] itemTransforms = ProcessMeshObject(itemGo.GetComponentsInChildren<MeshRenderer>());
        return itemTransforms;
    }

    /// <summary>
    /// 아이템 오브젝트의 MeshRenderer 정보를 이용하여
    /// 메쉬를 생성하여 반환하는 함수
    /// </summary>
    /// <param name="meshRenderers">아이템 오브젝트의 메쉬 렌더러</param>
    /// <returns>새로 생성된 메쉬 Transform</returns>
    Transform[] ProcessMeshObject(MeshRenderer[] meshRenderers)
    {
        List<Transform> itemTransforms = new List<Transform>();

        foreach (MeshRenderer renderer in meshRenderers)
        {
            // 본을 가지고 있는 상위 오브젝트가 없다면 리턴
            if(renderer.transform.parent != null)
            {
                // 부모의 이름을 사용하여 Transform 저장
                Transform parent = rootBoneDictionary[renderer.transform.parent.name.GetHashCode()];
                // 새로운 게임 오브젝트를 생성하여 부모 지정
                GameObject itemGo = Object.Instantiate(renderer.gameObject, parent);

                // 생성된 게임 오브젝트 저장
                itemTransforms.Add(itemGo.transform);
            }
        }

        return itemTransforms.ToArray();
    }
    #endregion Static Mesh Process
}
