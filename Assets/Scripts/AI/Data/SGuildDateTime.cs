using System;
using System.Text;

[Serializable]
public struct SGuildDateTime
{
    // private const int MONTHS_IN_YEAR  = 12;
    // private const int DAYS_IN_MONTH   = 30;
    // private const int HOURS_IN_DAY    = 24;
    // private const int MINUTES_IN_HOUR = 60;

    public int Year   { get; private set; }
    public int Month  { get; private set; }
    public int Day    { get; private set; }
    public int Hour   { get; private set; }
    public int Minute { get; private set; }

    public SGuildDateTime(int year, int month, int day, int hour, int minute)
    {
        Year   = year;
        Month  = month;
        Day    = day;
        Hour   = hour;
        Minute = minute;
    }

    public int GetTimeAsDurationInMinutes()
    {
        int hour  = Hour * TimeManager.MINUTES_IN_HOUR;
        int day   = Day * TimeManager.HOURS_IN_DAY * TimeManager.MINUTES_IN_HOUR;
        int month = Month * TimeManager.DAYS_IN_MONTH * TimeManager.HOURS_IN_DAY * TimeManager.MINUTES_IN_HOUR;
        int year  = Year * TimeManager.MONTHS_IN_YEAR * TimeManager.DAYS_IN_MONTH * TimeManager.HOURS_IN_DAY * TimeManager.MINUTES_IN_HOUR;
        return hour + day + month + year;
    }

    public override string ToString()
    {
        return new StringBuilder().Append(Year.ToString())
                                  .Append("/")
                                  .Append(Month.ToString())
                                  .Append("/")
                                  .Append(Day.ToString())
                                  .Append(" ")
                                  .Append(Hour.ToString())
                                  .Append(":")
                                  .Append(Minute.ToString())
                                  .ToString();
    }

    public readonly string ToFormattedString(bool dateOnly = true)
    {
        if (Month > TimeManager.MONTHS_IN_YEAR ||
            Day > TimeManager.DAYS_IN_MONTH ||
            Hour > TimeManager.HOURS_IN_DAY ||
            Minute > TimeManager.MINUTES_IN_HOUR)
        {
            SGuildDateTime normalised = NormalisGuildDateTime(this);
            return normalised.ToFormattedString(dateOnly);
        }

        StringBuilder sb          = new StringBuilder();
        bool          didPrevious = false;
        if (Year > 0)
        {
            sb.Append($"{Year} year{(Year > 1 ? "s" : "")}");
            if (dateOnly)
                return sb.ToString();
            didPrevious = true;
        }

        if (Month > 0)
        {
            sb.Append($"{(didPrevious ? ", " : "")}{Month} month{(Month > 1 ? "s" : "")}");
            if (dateOnly)
                return sb.ToString();
            didPrevious = true;
        }

        if (Day > 0)
        {
            sb.Append($"{(didPrevious ? ", " : "")}{Day} day{(Day > 1 ? "s" : "")}");
            if (dateOnly)
                return sb.ToString();
            didPrevious = true;
        }

        if (Hour > 0)
        {
            sb.Append($"{(didPrevious ? ", " : "")}{Hour} hour{(Hour > 1 ? "s" : "")}");
            if (dateOnly)
                return sb.ToString();
            didPrevious = true;
        }

        if (Minute > 0)
        {
            sb.Append($"{(didPrevious ? ", " : "")}{Minute} minute{(Minute > 1 ? "s" : "")}");
            if (dateOnly)
                return sb.ToString();
        }

        return sb.ToString();
    }

    public void NormaliseThis()
    {
        SGuildDateTime dt = NormalisGuildDateTime(this);
        Year   = dt.Year;
        Month  = dt.Month;
        Day    = dt.Day;
        Hour   = dt.Hour;
        Minute = dt.Minute;
    }

    public void NextDay(SGuildDateTime date)
    {
        Day++;
        if (Day >= TimeManager.DAYS_IN_MONTH)
        {
            Day = 1;
            Month++;
            if (Month >= TimeManager.MONTHS_IN_YEAR)
            {
                Month = 1;
                Year++;
            }
        }

        Hour   = date.Hour;
        Minute = date.Minute;
    }

    public void SetTime(int hour, int minute)
    {
        Hour   = hour;
        Minute = minute;
    }

    public void Tick()
    {
        Minute++;
        if (Minute >= TimeManager.MINUTES_IN_HOUR)
        {
            Minute = 0;
            Hour++;
            if (Hour >= TimeManager.HOURS_IN_DAY)
            {
                Hour = 0;
                Day++;
                if (Day >= TimeManager.DAYS_IN_MONTH)
                {
                    Day = 1;
                    Month++;
                    if (Month >= TimeManager.MONTHS_IN_YEAR)
                    {
                        Month = 1;
                        Year++;
                    }
                }
            }
        }
    }

    public static SGuildDateTime NormalisGuildDateTime(SGuildDateTime date)
    {
        // Start with minutes
        int totalMinutes = date.Minute;
        int minutes      = totalMinutes % 60;
        int extraHours   = totalMinutes / 60;

        // Add up hours
        int totalHours = date.Hour + extraHours;
        int hours      = totalHours % 24;
        int extraDays  = totalHours / 24;

        // Add up days
        int totalDays   = date.Day + extraDays;
        int days        = totalDays % 30; // Assuming 30 days per month
        int extraMonths = totalDays / 30;

        // Add up months
        int totalMonths = date.Month + extraMonths;
        int months      = totalMonths % 12;
        int extraYears  = totalMonths / 12;

        // Final years
        int years = date.Year + extraYears;

        return new SGuildDateTime(years, months, days, hours, minutes);
    }

    public static SGuildDateTime Zero
    {
        get { return new SGuildDateTime(0, 0, 0, 0, 0); }
    }
}