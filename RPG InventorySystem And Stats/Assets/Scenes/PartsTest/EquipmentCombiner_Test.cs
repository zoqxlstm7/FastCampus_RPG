using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentCombiner_Test : MonoBehaviour
{
    Dictionary<int, Transform> rootBoneDictionary = new Dictionary<int, Transform>();
    Transform transform;

    public Parts parts;
    public List<string> boneNames = new List<string>();

    private void Start()
    {
        TraverseHierarchy(gameObject.transform);
    }

    void TraverseHierarchy(Transform root)
    {
        foreach (Transform child in root)
        {
            rootBoneDictionary.Add(child.name.GetHashCode(), child);
            boneNames.Add(child.name);
            TraverseHierarchy(child);
        }
    }

    public void AddLimb()
    {
        Transform limb = ProcessBoneObject(parts.modelPrefab.GetComponentInChildren<SkinnedMeshRenderer>(), parts.boneNames);
        limb.SetParent(transform);
    }

    Transform ProcessBoneObject(SkinnedMeshRenderer renderer, List<string> boneNames)
    {
        Transform itemTransform = new GameObject().transform;
        SkinnedMeshRenderer meshRenderer = itemTransform.gameObject.AddComponent<SkinnedMeshRenderer>();

        Transform[] boneTransform = new Transform[boneNames.Count];
        for (int i = 0; i < boneNames.Count; i++)
        {
            boneTransform[i] = rootBoneDictionary[boneNames[i].GetHashCode()];
        }

        meshRenderer.bones = boneTransform;
        meshRenderer.sharedMesh = renderer.sharedMesh;
        meshRenderer.materials = renderer.materials;

        return itemTransform;
    }
}
