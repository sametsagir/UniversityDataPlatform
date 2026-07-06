using System.Collections.Concurrent;

namespace UniversityDataPlatform.Services
{
    public static class DatasetViewTracker
    {
        // Thread-safe dictionary to keep track of view counts in memory
        public static ConcurrentDictionary<int, int> ViewCounts { get; } = new();

        public static void IncrementView(int datasetId)
        {
            ViewCounts.AddOrUpdate(datasetId, 1, (id, count) => count + 1);
        }

        public static int GetViews(int datasetId)
        {
            return ViewCounts.TryGetValue(datasetId, out var count) ? count : 0;
        }
    }
}
