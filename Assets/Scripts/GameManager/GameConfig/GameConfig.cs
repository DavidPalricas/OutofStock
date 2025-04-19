using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

/// <summary>
/// The GameConfig class is used to generate a configuration file (json format) for the game and load data
/// from it.
/// </summary>
public class GameConfig : MonoBehaviour
{
    /// <summary>
    /// The fileName property stores the name of the configuration file that will be generated and
    /// loaded.
    /// </summary>
    [SerializeField]
    private string fileName;

    /// <summary>
    /// The shiftDuration property stores the duration of the shift in minutes.
    /// It is ranged between 1 and 1440 minutes (24 hours).
    /// </summary>
    [Range(1, 1440)]
    [SerializeField]
    private int shiftDurationIRL;


    [Range(1, 24)]
    [SerializeField]
    private int shiftDuration, lunchBreakTime, startHour;

    /// <summary>
    /// The days property stores a list of configurations for each day in the game.
    /// These configurations are made in the Unity editor.
    /// </summary>
    [SerializeField]
    private List<DayConfig> days = new(5)
    {
       new DayConfig(),
       new DayConfig(),
       new DayConfig(),
       new DayConfig(),
       new DayConfig()
    };

    /// <summary>
    /// The Awake method is called when the script instance is being loaded. (Unity callback)
    /// In this method, the configuration file is generated and the first day data is loaded.
    /// </summary>
    private void Awake()
    {
        GenConfigFile();
        LoadGeneralData();
        LoadCurrentDayData();
    }

    /// <summary>
    /// The GenConfigFile method generates a configuration file for the game.
    /// </summary>
    /// <remarks>
    /// This configuration files stores all game days data in a json format.
    /// Before generating the file, the GetDaysData method is called to get the data for each day.
    /// Then the data is serialized to json and written to a file in the persistent data path.
    /// This data path is specific to the platform the game is running on (Recommend in unity).
    /// </remarks>
    private void GenConfigFile()
    {
        Dictionary<string, int> generalData = new()
        {
         { "shiftDurationIRL", shiftDurationIRL },
         { "startHour", startHour },
         { "shiftDuration", shiftDuration },
         { "lunchBreakTime", lunchBreakTime }
        };

        Dictionary<string, DayData> daysData = GetDaysData();

        string jsonData = JsonConvert.SerializeObject(new { generalData, daysData }, Formatting.Indented);

        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            File.WriteAllText(filePath, jsonData);
        }

        catch (System.UnauthorizedAccessException e)
        {
            Debug.LogError($"Access denied to file: {e.Message}");
        }

        catch (IOException e)
        {
            Debug.LogError($"Error writing to file: {e.Message}");
        }

        Debug.Log($"Config file generated in : {filePath}");
    }

    /// <summary>
    /// Tje GetDaysData method generates a dictionary with stores the data for each day in the game.
    /// </summary>
    /// <remarks>
    /// This method iterates over the list of day configurations (`days`) which is editable in the Unity Editor.
    /// For each day configuration, it initializes a new `DayData` object, assigns values based on the configuration, 
    /// and populates its properties, and adds this object to the dictionary.
    /// </remarks>
    /// <returns>
    /// A dictionary where the key is the name of the day (string), and the value is the corresponding `DayData` object.
    /// </returns>
    private Dictionary <string, DayData> GetDaysData()
    {
        Dictionary<string, DayData> daysData = new();

        for (int i = 0; i < days.Count; i++)
        {
            var dayData = new DayData();

            DayConfig day = days[i];

            // The Dictionaries properties are initialized here, becuase if they are initialized in the class can be conflicts while desirializing the json data.
            dayData.customersSpawnProbs = new Dictionary<string, float>();
            dayData.managerTimes = new Dictionary<string, float>();
            dayData.tasksProbs = new Dictionary<string, float>();


            dayData.customersSpawnProbs.Add("Karen", day.karenSpawnProb);
            dayData.customersSpawnProbs.Add("AnnoyingKid", day.annoyingKidSpawnProb);

            dayData.managerTimes.Add("OfficeTime", day.managerOfficeTime);
            dayData.managerTimes.Add("PatrolTime", day.managerPatrolTime);

            dayData.numberOfTasks = day.numberOfTasks;

            dayData.tasksProbs.Add("CleanFloor", day.cleanFloorProb);
            dayData.tasksProbs.Add("FixFuseBox", day.fixFuseBoxProb);
            dayData.tasksProbs.Add("FixToilet", day.fixToiletProb);

            string weekDay = GetWeekDay(i);

            daysData.Add(weekDay.ToLower(), dayData);
        }
     
        return daysData;
    }

    /// <summary>
    /// The GetWeekDay method returns the name of the week day (abrevieated) based on its number.
    /// </summary>
    /// <param name="weekdayNumber">The weekday number.</param>
    /// <returns></returns>
    private string GetWeekDay(int weekdayNumber)
    {   
        if (weekdayNumber < 0 || weekdayNumber > 4)
        {
            Debug.LogError($"Invalid week day number: {weekdayNumber}");
            return string.Empty;
        }

        string[] weekDays = { "Mon", "Tue", "Wed", "Thu", "Fri" };

        return weekDays[weekdayNumber];
    }


    private void LoadGeneralData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(filePath))
        {
            Debug.LogError($"File not found: {filePath}");
            return;
        }

        string jsonData = File.ReadAllText(filePath);

        GeneralData generalData = JsonConvert.DeserializeObject<RootObject>(jsonData).generalData;

        PlayerPrefs.SetInt("ShiftDurationIRL", generalData.shiftDurationIRL);
        PlayerPrefs.SetInt("ShiftDuration", generalData.shiftDuration);
        PlayerPrefs.SetInt("LunchBreakTime", generalData.lunchBreakTime);
        PlayerPrefs.SetInt("StartHour", generalData.startHour);

        PlayerPrefs.Save();
    }

    private DayData GetDayData(DaysData daysData, string weekDay)
    {
        return weekDay.ToLower() switch
        {
            "mon" => daysData.mon,
            "tue" => daysData.tue,
            "wed" => daysData.wed,
            "thu" => daysData.thu,
            "fri" => daysData.fri,
            _ => null,
        };
    }

    /// <summary>
    /// The LoadCurrentDatData loads the current day's data from a JSON file and updates the game state.
    /// </summary>
    /// <remarks>
    /// This method attempts to read the game configuration file from its path. (persistent data path)
    /// If the file exists, it deserializes the JSON data into a dictionary of `DayData` objects.
    /// The method calculates the current day number based on the remaining days in the list and retrieves the corresponding day data.
    /// The loaded data is assigned to `currentDay`, and the processed day is removed from the `days` list.
    /// </remarks>
    private void LoadCurrentDayData()
    {   
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(filePath))
        {
            Debug.LogError($"File not found: {filePath}");
            return;
        }

        string currentWeekDay = GetCurrentWeekDay();

        if (string.IsNullOrEmpty(currentWeekDay))
        {
            Debug.Log("Final day completed, exiting the game");
            Utils.ExitGame();
            return;
        }

        PlayerPrefs.SetString("CurrentDay", currentWeekDay);

        string jsonData = File.ReadAllText(filePath);

        RootObject jsonRoot = JsonConvert.DeserializeObject<RootObject>(jsonData);

        DayData dayData = GetDayData(jsonRoot.daysData, currentWeekDay);

        if (dayData != null)
        {
            PlayerPrefs.SetFloat("KarenSpawnProb", dayData.customersSpawnProbs["Karen"]);
            PlayerPrefs.SetFloat("AnnoyingKidSpawnProb", dayData.customersSpawnProbs["AnnoyingKid"]);
            PlayerPrefs.SetFloat("ManagerOfficeTime", dayData.managerTimes["OfficeTime"]);
            PlayerPrefs.SetFloat("ManagerPatrolTime", dayData.managerTimes["PatrolTime"]);
            PlayerPrefs.SetInt("NumberOfTasks", dayData.numberOfTasks);
            PlayerPrefs.SetFloat("CleanFloorProb", dayData.tasksProbs["CleanFloor"]);
            PlayerPrefs.SetFloat("FixFuseBoxProb", dayData.tasksProbs["FixFuseBox"]);
            PlayerPrefs.SetFloat("FixToiletProb", dayData.tasksProbs["FixToilet"]);

            PlayerPrefs.Save();
        }
    }


    private string GetCurrentWeekDay()
    {
        if (PlayerPrefs.HasKey("CurrentDay"))
        {   return GetNextWeekDay(PlayerPrefs.GetString("CurrentDay"));
        }
   
         return "Mon";
    }

    private string GetNextWeekDay(string previousDay)
    {
        string[] weekDays = { "Mon", "Tue", "Wed", "Thu", "Fri" };

        int currentIndex = System.Array.IndexOf(weekDays, previousDay);

        if (currentIndex == -1 || currentIndex == weekDays.Length - 1)
        {
            return null; 
        }

        return weekDays[currentIndex + 1];
    }
}