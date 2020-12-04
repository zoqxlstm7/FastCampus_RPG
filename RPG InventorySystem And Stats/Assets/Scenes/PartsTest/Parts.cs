using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts : MonoBehaviour
{
    public GameObject modelPrefab;

    public List<string> boneNames = new List<string>();

    private void OnValidate()
    {
        boneNames.Clear();

        if (modelPrefab == null || modelPrefab.GetComponentInChildren<SkinnedMeshRenderer>() == null)
            return;

        SkinnedMeshRenderer renderer = modelPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
        Transform[] bones = renderer.bones;

        foreach (Transform boneTransform in bones)
        {
            boneNames.Add(boneTransform.name);
        }
    }
}
