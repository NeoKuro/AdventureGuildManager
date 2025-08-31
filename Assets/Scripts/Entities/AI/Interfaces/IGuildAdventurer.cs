
    public interface IGuildAdventurer : IGuildCore
    {
        bool HasJob();
        float GetJobPriority();
        bool ShouldGetJob();
        SJobData GetJob();
    }