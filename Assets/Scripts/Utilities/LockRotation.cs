//    Space MMO    
//  Author: Josh Hughes   


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    [SerializeField]
    private bool lockX;
    [SerializeField]
    private bool lockY;
    [SerializeField]
    private bool lockZ;

    [SerializeField, ConditionalHide("lockX", true)]
    private float lockedXRotValue = 0f;
    [SerializeField, ConditionalHide("lockY", true)]
    private float lockedYRotValue = 0f;
    [SerializeField, ConditionalHide("lockZ", true)]
    private float lockedZRotValue = 0f;

    private void Update()
    {
        Vector3 newRot = transform.eulerAngles;
        if(lockX)
        {
            newRot = newRot.SetX(lockedXRotValue);
        }
        if(lockY)
        {
            newRot = newRot.SetY(lockedYRotValue);
        }
        if(lockZ)
        {
            newRot = newRot.SetZ(lockedZRotValue);
        }

        transform.eulerAngles = newRot;
    }
}