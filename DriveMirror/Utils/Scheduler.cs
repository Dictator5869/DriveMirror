using System;
using System.Timers;

public class Scheduler
{
    private readonly ScheduleManager _scheduleManager;
    private readonly System.Timers.Timer _timer;

    public event Action OnScheduledSync;  // Event to trigger your sync

    public Scheduler(ScheduleManager scheduleManager)
    {
        _scheduleManager = scheduleManager;

        _timer = new System.Timers.Timer(60 * 1000); // every 1 minute
        _timer.Elapsed += TimerElapsed;
    }

    public void Start() => _timer.Start();
    public void Stop() => _timer.Stop();

    private void TimerElapsed(object sender, ElapsedEventArgs e)
    {
        var settings = _scheduleManager.Settings;
        Logger.Log($"[Scheduler] Tick: Now = {DateTime.Now}, Enabled = {settings.IsEnabled}");

        if (!settings.IsEnabled)
        {
            Logger.Log("[Scheduler] Skipped - scheduling disabled.");
            return;
        }

        var now = DateTime.Now;
        int dayIndex = (int)now.DayOfWeek;

        if (!settings.DaysOfWeek[dayIndex])
        {
            Logger.Log($"[Scheduler] Skipped - {now.DayOfWeek} not selected.");
            return;
        }

        var scheduledTime = settings.ScheduledTime;
        Logger.Log($"[Scheduler] Checking scheduled time: Now = {now.TimeOfDay}, Scheduled = {scheduledTime}");

        if (Math.Abs((now.TimeOfDay - scheduledTime).TotalMinutes) > 1)
        {
            Logger.Log("[Scheduler] Skipped - not the correct time.");
            return;
        }

        if (HasRunForThisSchedule(settings, now))
        {
            Logger.Log("[Scheduler] Skipped - already ran today.");
            return;
        }

        OnScheduledSync?.Invoke();

        Logger.Log("[Scheduler] Scheduled sync starting.");
        settings.LastRunDate = now.Date;
        _scheduleManager.Save();

    }

    private bool HasRunForThisSchedule(ScheduleSettings settings, DateTime now)
    {
        var last = settings.LastRunDate;
        switch (settings.RunFrequency)
        {
            case Frequency.Daily:
                return last == now.Date;
            case Frequency.Weekly:
                return (now - last).TotalDays < 7 && last.DayOfWeek == now.DayOfWeek;
            case Frequency.Biweekly:
                return (now - last).TotalDays < 14 && last.DayOfWeek == now.DayOfWeek;
            case Frequency.Monthly:
                return last.Month == now.Month && last.Day == now.Day;
            default:
                return false;
        }
    }
}
