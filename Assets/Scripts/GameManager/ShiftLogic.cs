using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// The ShiftLogic class is responsible for handling the players shift time in the game.
/// </summary>
public class ShiftLogic : MonoBehaviour
{
    /// <summary>
    /// The dayText and timeText attributes are used to store the text components that display the day and time in the game respectively.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI dayText, timeText;

    /// <summary>
    /// The timeSlider attribute is used to store the slider component that displays the time progress in the game.
    /// </summary>
    [SerializeField]
    private Slider timeSlider;

    /// <summary>
    /// The timeSliderSpeed attribute is used to store the speed of the time slider.
    /// </summary>
    [SerializeField]
    private float timeSliderSpeed;

    /// <summary>
    /// The following attributes are used to store the game time properties.
    ///
    /// The gameMinuteInSecondsIRL attribute stores the duration of a game minute in seconds in real life.
    /// The gameHourInSecondsIRL attribute stores the duration of a game hour in seconds in real life.
    /// The currentGameMinutes attribute stores the current game time in minutes.
    /// And the timer attribute stores the time when the last game minute was updated.
    /// </summary>
    private float gameMinuteInSecondsIRL, gameHourInSecondsIRL, currentGameMinutes, timer;

    /// <summary>
    /// The day attribute stores the current week day in the game.
    /// </summary>
    private string day;

    /// <summary>
    /// The startHour attribute is used get a reference to the starting hour of the game.
    /// This atribute reads the start hour from the PlayerPrefs.
    /// </summary>
    private int startHour;

    /// <summary>
    /// The daytime atrribute is a dictionary that stores the time with the corresponding hour and AM/PM.
    /// </summary>
    private readonly Dictionary<int, string> daytime = new()
    {
        {0, "12 PM"},
        {1, "1 AM"},
        {2, "2 AM"},
        {3, "3 AM"},
        {4, "4 AM"},
        {5, "5 AM"},
        {6, "6 AM"},
        {7, "7 AM"},
        {8, "8 AM"},
        {9, "9 AM"},
        {10, "10 AM"},
        {11, "11 AM"},
        {12, "12 AM"},
        {13, "1 PM"},
        {14, "2 PM"},
        {15, "3 PM"},
        {16, "4 PM"},
        {17, "5 PM"},
        {18, "6 PM"},
        {19, "7 PM"},
        {20, "8 PM"},
        {21, "9 PM"},
        {22, "10 PM"},
        {23, "11 PM"}
    };

    /// <summary>
    /// The Awake method is called when the script instance is being loaded. (Unity Callback)
    /// In this method, the game time properties are initillizaized and the game time is set (SetGameTime method).
    /// </summary>
    private void Start()
    {   
        timer = Time.time;

        day = PlayerPrefs.GetString("CurrentDay", "Mon");

        startHour = (int) PlayerPrefs.GetFloat("StartHour", 0);

        SetGameTime();
    }

    /// <summary>
    /// The Update method is called every frame. (Unity Callback)
    /// In this method, the game time UI is updated every game minute.
    /// </summary>
    private void Update()
    {
        // Check if a game minute has passed
        if ((Time.time - timer) >= gameMinuteInSecondsIRL)
        {
            currentGameMinutes++;
            timer += gameMinuteInSecondsIRL;
            UpdateTimeUI();
        }
    }


    /// <summary>
    /// The SetGameTime method is responsible for setting the game time.
    /// It calculates the duration of a game minute and hour in  in real life seconds.
    /// And sets the initial day and time in the game.
    /// </summary>
    private void SetGameTime()
    {
        int shiftDurationMinutesIRL = (int) PlayerPrefs.GetFloat("ShiftDuration") * 60;

        float gameMinuteDuration = PlayerPrefs.GetFloat("ShiftDurationIRL") / shiftDurationMinutesIRL;

        gameMinuteInSecondsIRL = gameMinuteDuration * 60;

        gameHourInSecondsIRL = gameMinuteDuration * 3600;

        dayText.text = day;

        string[] time = daytime[startHour].Split(' ');

        timeText.text = $"{time[0]}:00 {time[1]}";
    }

    /// <summary>
    /// The UpdateTimeUI method is responsible for updating the time UI in the game.
    /// </summary>
    /// <remarks>
    /// It updates the time in the following format : HH:MM AM/PM and updates the time slider value, to show to the player if the shift is almost over.
    /// It also checks if the player has reached the end of the shift and calls the NextDay method.
    /// </remarks>
    private void UpdateTimeUI()
    {
        int hour = (int)(currentGameMinutes / 60);

        int currentHour = startHour + hour;

        int minutes = (int)(currentGameMinutes % 60);

        string[] time = daytime[currentHour].Split(' ');

        timeText.text = $"{time[0]}:{minutes:D2} {time[1]}";

        int workTime = currentHour - startHour;


        int shiftDuration = (int)PlayerPrefs.GetFloat("ShiftDuration");

        float targetSliderValue = (float) workTime / shiftDuration ;


        if (timeSlider.value != targetSliderValue)
        {
            // Smoothly update the time slider value by using a lerp function
            timeSlider.value = Mathf.Lerp(timeSlider.value, targetSliderValue, timeSliderSpeed * Time.deltaTime);
        }

        if (workTime == shiftDuration)
        {
            NextDay();
        }
    }

    /// <summary>
    /// The NextDay method is responsible for advancing to the next day of the game.
    /// For now, it just reloads the current scene for testing the days progression.
    /// </summary>
    private void NextDay()
    {   
        if (GetComponent<WinConditions>().PlayerWon())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            return;
        }

        // Go to the Main Menu
        SceneManager.LoadScene(0);
    }
}