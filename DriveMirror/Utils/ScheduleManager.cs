using System.Reflection;
using System.Text.Json;

public class ScheduleManager
{
    private readonly string _filePath;

    public ScheduleSettings Settings { get; private set; }

    public ScheduleManager()
    {
        // Get folder where the executable runs from:
        var exeFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        
        // Combine with your folder and filename:
        var scheduleFolder = Path.Combine(exeFolder, "DriveMirrorData");
        _filePath = Path.Combine(scheduleFolder, "schedule.json");

        Load();
    }

    public void Save()
    {
        var directory = Path.GetDirectoryName(_filePath);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        var json = JsonSerializer.Serialize(Settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }

    public void Load()
    {
        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            Settings = JsonSerializer.Deserialize<ScheduleSettings>(json);
        }
        else
        {
            Settings = new ScheduleSettings();
        }
    }
}
