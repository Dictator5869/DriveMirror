public enum Frequency
{
    Daily,
    Weekly,
    Biweekly,
    Monthly
}

public class ScheduleSettings
{
    public bool IsEnabled { get; set; } = false;
    public bool[] DaysOfWeek { get; set; } = new bool[7]; // Sunday = 0
    public TimeSpan ScheduledTime { get; set; } = new TimeSpan(22, 0, 0); // default 10 PM
    public Frequency RunFrequency { get; set; } = Frequency.Weekly;
    public DateTime LastRunDate { get; set; } = DateTime.MinValue;
    public string SourceDrive { get; set; } = "";
    public string DestinationDrive { get; set; } = "";
}
