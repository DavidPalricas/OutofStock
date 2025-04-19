[System.Serializable]
public class RootObject
{
    public GeneralData generalData;

    public DaysData daysData;
}

[System.Serializable]
public class GeneralData
{
    public int shiftDurationIRL;
    public int startHour;
    public int shiftDuration;
    public int lunchBreakTime;
}


[System.Serializable]
public class DaysData
{
    public DayData mon;

    public DayData tue;

    public DayData wed;

    public DayData thu;

    public DayData fri;
}
