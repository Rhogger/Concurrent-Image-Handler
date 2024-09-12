namespace ImageHandler.Core.Models;

public class BenchmarkResult
{
    public long SequentialTime { get; set; } 
    public long Thread1Time { get; set; }
    public long Thread2Time { get; set; }
    public long Thread3Time { get; set; }
    public long Thread4Time { get; set; }
    public long Thread6Time { get; set; }
    public long Thread8Time { get; set; }
    public long Thread10Time { get; set; }
}