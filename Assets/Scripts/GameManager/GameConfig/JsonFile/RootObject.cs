/// <summary>
/// The RootObject class is used to represent the stuctured data of the JSON file with the game configuration.
/// </summary>
[System.Serializable]
public class RootObject
{
    /// <summary>
    /// The generalData property represents a key of the JSON file that contains general data 
    /// about the game configuration.
    /// </summary>
    public GeneralData generalData;

    /// <summary>
    /// The daysData property represents a key of the JSON file that contains the data for each day of the week.
    /// </summary>
    public DaysData daysData;
}

/// <summary>
/// The GeneralData class represents a key of the JSON file that contains general data
/// Each property of this class represents a key of the JSON file that contains part of this data.
/// </summary>
[System.Serializable]
public class GeneralData
{
    /// <summary>
    /// The shiftDurationIRL property represents the duration of the player's shift in real life.
    /// </summary>
    public float shiftDurationIRL;

    /// <summary>
    /// The startHourIRL property represents the start hour of the player's shift.
    /// </summary>
    public float startHour;

    /// <summary>
    /// The shiftDuration property represents the duration of the player's shift in the game.
    /// </summary>
    public float shiftDuration;

    /// <summary>  
    /// The lunchBreakTime property represents the hour which is used to calculate the lunch break time.
    /// Ex: If the lunchBreakTime is 2, the lunch break will be at 2 hours after the start hour.
    /// </summary>  
    public float lunchBreakTime;

    /// <summary>
    /// The customersPerHour property represents the probability of a normal customer becoming a thief.
    /// </summary>
    public float customerBecameThiefProb;
}

/// <summary>
/// The DayData class  represents a key of the JSON file that contains the data for each day of the week.
/// Each property represents a key of the JSON file that contains part of this data for each week day.
/// </summary>
[System.Serializable]
public class DaysData
{
    /// <summary>
    /// The mon property stores the data for Monday.
    /// </summary>
    public DayData mon;

    /// <summary>
    /// The tue property stores the data for Tuesday.
    /// </summary>
    public DayData tue;

    /// <summary>
    /// The wed property stores the data for Wednesday.
    /// </summary>
    public DayData wed;

    /// <summary>
    /// The thu property stores the data for Thursday.
    /// </summary>
    public DayData thu;

    /// <summary>
    /// The fri property stores the data for Friday.
    /// </summary>
    public DayData fri;
}
