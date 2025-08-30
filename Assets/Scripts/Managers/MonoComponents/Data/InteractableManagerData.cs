using System;

using Hzn.Framework;

using UnityEngine;

[Serializable]
public class InteractableManagerData : MonoComponentData
{
    [SerializeField]
    private float _interactableRange = 5f;

    [SerializeField]
    private float _raycastMoveThreshold = 0.1f;

    [SerializeField]
    private float _raycastRotationThreshold = 5f;

    [SerializeField]
    private LayerMask _raycastLayerMask;
    
    public float InteractableRange => _interactableRange;
    public float RaycastMoveThreshold => _raycastMoveThreshold;
    public float RaycastRotationThreshold => _raycastRotationThreshold;
    public LayerMask RaycastLayerMask => _raycastLayerMask;
}