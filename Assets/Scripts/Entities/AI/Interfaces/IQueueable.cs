
    using UnityEngine;

    public interface IQueueable
    {
        // Tell the AI that it has been removed from the queue
        void NotifyRemovedFromQueue();
        
        // Extra Callback if the queue was cleared, not just removed (IE reception desk picked up)
        void NotifyQueueCleared();
        
        // Tell the AI to move to the next position in the queue
        void MoveToNextQueuePosition(Vector3 position);
        void OnReachedFrontOfQueue();
        
    }