using System;
using System.Collections.Generic;
using System.Linq;

using Hzn.Framework;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(ReceptionWorkstation))]
public class ReceptionController : GuildServiceController
{
    [SerializeField]
    private ReceptionWorkstation _thisWorkstationReference;
    
    [Header("Queue Settings")]
    [SerializeField]
    private int _pregenerateQueueSize = 5;

    [SerializeField]
    private float _queueSeparationDistance = 3f;

    [SerializeField]
    private float _entityRadius = 0.5f;

    [SerializeField]
    private float _entityHeight = 2f;

    [SerializeField]
    private LayerMask _obstaclesMask;

    [Header("Position Finding Settings")]
    [SerializeField]
    private float _maxAngleDeviation = 45f;

    [SerializeField]
    private float _angleStep = 15f;

    [SerializeField]
    private float _maxDistanceMultiplier = 1.5f;

    [SerializeField]
    private Transform _queueFrontLocation;

    private List<AIEntity> _currentQueue = new List<AIEntity>();

    private List<Vector3> _queuePositions = new List<Vector3>();

    public bool HasReachedMaxLength { get; private set; }

    public int QueueSize
    {
        get { return _currentQueue.Count; }
    }

    protected override void Awake()
    {
        base.Awake();

        if (_thisWorkstationReference == null)
        {
            _thisWorkstationReference = GetComponent<ReceptionWorkstation>();
        }
        
        Dbg.LogVerbose(Logging.Guild, "Reception Controller Awake");
    }

    private void OnEnable()
    {
        ClearQueue();
        _queuePositions.Clear();
        HasReachedMaxLength = false;
        for (int i = 0; i < _pregenerateQueueSize; i++)
        {
            if (!TryGenerateNewQueuePosition(out Vector3 position))
            {
                HasReachedMaxLength = true;
                Dbg.Warn(Logging.Guild, $"Could not generate new queue position. Total Generated: [{_queuePositions.Count.ToString()}]");
                return;
            }

            _queuePositions.Add(position);
        }
    }

    public bool TryEnqueueEntity(AIEntity entity, out Vector3 queueDestination)
    {
        queueDestination = Vector3.zero;
        if (_currentQueue.Count >= _queuePositions.Count)
        {
            Dbg.LogVerbose(Logging.Guild, "Queue positions taken. Trying to generate a new queue position");
            if (!TryGenerateNewQueuePosition(out queueDestination))
            {
                Dbg.Warn(Logging.Guild, $"Queue is full, and cannot be extended.");
                return false;
            }

            _queuePositions.Add(queueDestination);
        }

        _currentQueue.Add(entity);
        return true;
    }

    public void DequeueEntity()
    {
        if (_currentQueue.Count == 0)
        {
            Dbg.LogVerbose(Logging.Guild, "Queue is empty. Cannot dequeue.");
            return;
        }

        // First notify the first queue member they have been dequeued
        //  This only happens once they have been served
        AIEntity queuedEntity = _currentQueue.First();
        queuedEntity.NotifyRemovedFromQueue();
        _currentQueue.RemoveAt(0);

        // Tell the rest of the Enitities to move up

        for (int i = 0; i < _currentQueue.Count; i++)
        {
            AIEntity nextEntity = _currentQueue[i];
            nextEntity.MoveToNextQueuePosition(_queuePositions[i]);
        }
    }

    public void RegisterWithGuild(Adventurer_AIEntity entity)
    {
        _thisWorkstationReference.Interact(entity);
        Dbg.LogVerbose(Logging.Guild, "Registering with guild - IMPLEMENT: Need to implement 'ProcessInteractable' of the Receptionist Workstation to process queued AI");
    }

    public void TurnInQuest(Adventurer_AIEntity entity)
    {
        // May need to also pass in the quest? 
        // Unless the entity contains a way to access it IE via a QuestHandler on the AI?
        throw new NotImplementedException();
    }

    public void AcceptQuest(Adventurer_AIEntity entity)
    {
        // May need to also pass in the quest? 
        // Unless the entity contains a way to access it IE via a QuestHandler on the AI?
        throw new NotImplementedException();
    }

    private void ClearQueue()
    {
        foreach (AIEntity queuedEntity in _currentQueue)
        {
            queuedEntity.NotifyQueueCleared();
        }

        _currentQueue.Clear();
    }

    private bool TryGenerateNewQueuePosition(out Vector3 position)
    {
        const float RADIUS     = 0.5f;
        const float MAX_ANGLE  = 45f;
        const float ANGLE_STEP = 15f;

        Vector3 lastPosition = _queuePositions.Count > 0
                                   ? _queuePositions.Last()
                                   : _queueFrontLocation.position;

        // Get base direction (straight line back from queue)
        Vector3 baseDirection = -_queueFrontLocation.forward;

        // First try straight line
        Vector3 straightPosition = position = lastPosition + (baseDirection * _queueSeparationDistance);
        if (IsValidQueuePosition(straightPosition, RADIUS))
        {
            position = straightPosition;
            return true;
        }

        // If straight line fails, try alternating angles on both sides
        for (float angle = ANGLE_STEP; angle <= MAX_ANGLE; angle += ANGLE_STEP)
        {
            // Try right side
            Vector3 rightDirection = Quaternion.Euler(0, angle, 0) * baseDirection;
            Vector3 rightPosition  = lastPosition + rightDirection * _queueSeparationDistance;
            if (IsValidQueuePosition(rightPosition, RADIUS))
            {
                position = rightPosition;
                return true;
            }

            // Try left side
            Vector3 leftDirection = Quaternion.Euler(0, -angle, 0) * baseDirection;
            Vector3 leftPosition  = lastPosition + leftDirection * _queueSeparationDistance;
            if (IsValidQueuePosition(leftPosition, RADIUS))
            {
                position = leftPosition;
                return true;
            }
        }

        // If no valid position found, try increasing the distance slightly
        // float   extendedDistance = QUEUE_SEPARATION_DISTANCE * 1.5f;
        // Vector3 extendedPosition = lastPosition + (baseDirection * extendedDistance);
        // if (IsValidQueuePosition(extendedPosition, RADIUS))
        // {
        //     position = extendedPosition;
        //     return extendedPosition;
        // }

        // This just prevents unnecessary regeneration of the queue when we know it will fail
        HasReachedMaxLength = true;

        // Final fallback - return the straight line position even if invalid
        // You might want to handle this case differently based on your needs
        Dbg.Warn(Logging.Guild, "Could not find valid queue position, returning potentially invalid position");
        return false;
    }

    private bool IsValidQueuePosition(Vector3 position, float radius)
    {
        // First check if there's a clear path using raycast
        Vector3 fromPosition = _queuePositions.Count > 0
                                   ? _queuePositions.Last()
                                   : _queueFrontLocation.position;

        // Check direct line of sight
        if (Physics.Raycast(fromPosition + Vector3.up, (position - fromPosition).normalized,
                            out RaycastHit hit, _queueSeparationDistance * 1.5f, _obstaclesMask))
        {
            // Something is blocking the direct path
            return false;
        }

        // Check if position is on NavMesh
        if (!NavMesh.SamplePosition(position, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
        {
            return false;
        }

        // Check for obstacles using capsule cast
        float   height = 2.0f;
        Vector3 point1 = position + Vector3.up * radius;
        Vector3 point2 = position + Vector3.up * (height - radius);

        if (Physics.CheckCapsule(point1, point2, radius,
                                 _obstaclesMask))
        {
            return false;
        }

        // Verify NavMesh path
        NavMeshPath path = new NavMeshPath();
        if (!NavMesh.CalculatePath(fromPosition, navHit.position, NavMesh.AllAreas, path))
        {
            return false;
        }

        return true;
    }

    private bool CheckSpaceBehindQueue(out Vector3 position)
    {
        if (_currentQueue.Count == 0)
        {
            position = _queueFrontLocation.position;
            return true;
        }

        AIEntity queuedEntity = _currentQueue.LastOrDefault();
        int      index        = _currentQueue.IndexOf(queuedEntity);
        if (index > _queuePositions.Count)
        {
            Dbg.Error(Logging.Guild,
                      $"Queue index [{index.ToString()}] is greater than queue positions count. This should never happen!");
        }

        position = _queuePositions[_currentQueue.IndexOf(queuedEntity)];
        return true;
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        // Draw queue positions
        Gizmos.color = Color.blue;
        foreach (var pos in _queuePositions)
        {
            Gizmos.DrawWireSphere(pos, 0.5f);
        }

        // Draw straight line connections
        Gizmos.color = Color.yellow;
        Vector3 lastPos = _queueFrontLocation.position;
        foreach (var pos in _queuePositions)
        {
            Gizmos.DrawLine(lastPos, pos);
            lastPos = pos;
        }

        // Draw ideal straight line
        if (_queuePositions.Count > 0)
        {
            Gizmos.color = Color.green;
            Vector3 idealNextPos = _queuePositions.Last() + (-_queueFrontLocation.forward * _queueSeparationDistance);
            Gizmos.DrawWireSphere(idealNextPos, 0.3f);
            Gizmos.DrawLine(_queuePositions.Last(), idealNextPos);
        }
    }
#endif
}