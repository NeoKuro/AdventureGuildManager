using System;

using Hzn.Framework;

using UnityEngine;

public class TimeManager : Manager<TimeManager>, ITickable, ITickableFixedUpdate
{
    public const           int            MONTHS_IN_YEAR     = 12;
    public const           int            DAYS_IN_MONTH      = 30;
    public const           int            HOURS_IN_DAY       = 24;
    public const           int            MINUTES_IN_HOUR    = 60;
    private const          float          MINUTES_PER_SECOND = 1f;
    
    public static readonly SGuildDateTime DayStartTime       = new SGuildDateTime(0, 0, 0, 8, 0);

    public static SGuildDateTime CurrentTime { get; private set; }
    private       float          _nextTickTime;
    
    private Action _onDayStart;
    private Action _onDayEnd;
    private Action _onNewMonth;
    private Action _onNewYear;

    protected override void PostManagerCreated()
    {
        base.PostManagerCreated();
        ((ITickable)this).CreateTickable();
        
        // TODO: Load Date-Time from save file
        CurrentTime = SGuildDateTime.Zero;
    }

    public void FixedUpdate(float deltaT)
    {
        if (Time.time < _nextTickTime)
        {
            return;
        }

        // Ticks 1 minute ahead. By default this is 1 minute every real-life second
        //  This makes 1 full 24-hour day, a 24-minute experience
        //  Though the player should 'pass out' no later than 3am if not in bed by then (making a day at most 19 hours)
        CurrentTime.Tick();
        _nextTickTime = Time.time + MINUTES_PER_SECOND;
    }

    public void StartNextDay()
    {
        // Set time to the next day
        //  By default, this is 8am, it will increment the day (month, year etc if needed as well)
        CurrentTime.NextDay(DayStartTime);
    }


    #region -- TIME TRANSITION CALLBACKS --

    public static void RegisterNewDayEvent(Action newDayStart)
    {
        if (Get() == null)
        {
            Dbg.Error(Log.Manager, $"Trying to register new day event, but {nameof(TimeManager)} is null");
            return;
        }
        
        Get()._onDayStart += newDayStart;
    }

    public static void RegisterNewDayEndEvent(Action dayEnd)
    {
        if (Get() == null)
        {
            Dbg.Error(Log.Manager, $"Trying to register new day event, but {nameof(TimeManager)} is null");
            return;
        }
        Get()._onDayEnd += dayEnd;
    }

    public static void RegisterNewMonthEvent(Action newMonth)
    {
        if (Get() == null)
        {
            Dbg.Error(Log.Manager, $"Trying to register new day event, but {nameof(TimeManager)} is null");
            return;
        }
        Get()._onNewMonth += newMonth;
    }

    public static void RegisterNewYearEvent(Action newYear)
    {
        if (Get() == null)
        {
            Dbg.Error(Log.Manager, $"Trying to register new day event, but {nameof(TimeManager)} is null");
            return;
        }
        Get()._onNewYear += newYear;
    }

    public static void UnregisterNewDayEvent(Action newDayStart)
    {
        if (Get() == null)
        {
            Dbg.Error(Log.Manager, $"Trying to register new day event, but {nameof(TimeManager)} is null");
            return;
        }
        Get()._onDayStart -= newDayStart;
    }

    public static void UnregisterNewDayEndEvent(Action dayEnd)
    {
        if (Get() == null)
        {
            Dbg.Error(Log.Manager, $"Trying to register new day event, but {nameof(TimeManager)} is null");
            return;
        }
        Get()._onDayEnd -= dayEnd;
    }

    public static void UnregisterNewMonthEvent(Action newMonth)
    {
        if (Get() == null)
        {
            Dbg.Error(Log.Manager, $"Trying to register new day event, but {nameof(TimeManager)} is null");
            return;
        }
        Get()._onNewMonth -= newMonth;
    }

    public static void UnregisterNewYearEvent(Action newYear)
    {
        if (Get() == null)
        {
            Dbg.Error(Log.Manager, $"Trying to register new day event, but {nameof(TimeManager)} is null");
            return;
        }
        Get()._onNewYear -= newYear;
    }

    #endregion
    
    
}