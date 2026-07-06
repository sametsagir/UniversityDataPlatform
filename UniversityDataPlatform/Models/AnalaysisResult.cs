public class DatasetAnalysis
{
    public int Id { get; set; }
    public int DatasetId { get; set; }
    public string TaskType { get; set; } = "";
    public int Instances { get; set; }
    public int Features { get; set; }

    // JSON formatında saklayacağız
    public string PerformanceJson { get; set; } = "";
    public string VariablesJson { get; set; } = "";

    // YENİ EKLEDİĞİMİZ ALAN: Grafik Base64 kodlarını saklar
    public string? ChartsJson { get; set; } = "";

    public DateTime AnalysisDate { get; set; } = DateTime.Now;

    public virtual Dataset? Dataset { get; set; }
}