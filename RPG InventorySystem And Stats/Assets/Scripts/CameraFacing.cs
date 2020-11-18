using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacing : MonoBehaviour
{
    #region Variables
    Camera referenceCameara;

    public bool reverseFace = false;

    public enum Axis
    {
        up, down, left, right, forward, back
    };

    public Axis axis = Axis.up;
    #endregion Variables
    private void Awake()
    {
        referenceCameara = Camera.main;
    }

    private void LateUpdate()
    {
        Vector3 targetPos = transform.position + referenceCameara.transform.rotation * (reverseFace ? Vector3.forward : Vector3.back);
        Vector3 targetOrientation = referenceCameara.transform.rotation * GetAXis(axis);

        transform.LookAt(targetPos, targetOrientation);
    }
    #region Unity Methods

    #endregion Unity Methods

    #region Main Methods
    public Vector3 GetAXis(Axis refAxis)
    {
        switch (refAxis)
        {
            case Axis.up:
                return Vector3.up;
            case Axis.down:
                return Vector3.down;
            case Axis.left:
                return Vector3.left;
            case Axis.right:
                return Vector3.right;
            case Axis.forward:
                return Vector3.forward;
            case Axis.back:
                return Vector3.back;
        }

        return Vector3.up;
    }
    #endregion Main Methods
}
