using System;

[Serializable]
public struct GuildDateTime
{
    public readonly int Year;
    public readonly int Month;
    public readonly int Day;
    public readonly int Hour;
    public readonly int Minute;

    public GuildDateTime(int year, int month, int day, int hour, int minute)
    {
        Year   = year;
        Month  = month;
        Day    = day;
        Hour   = hour;
        Minute = minute;
    }
}