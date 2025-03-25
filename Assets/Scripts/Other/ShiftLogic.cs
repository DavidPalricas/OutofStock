using System.Collections.Generic;
using System.Collections;
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
    /// The gameShiftMinutesIRL attribute is used to store the duration of the game shift in minutes in real life.
    /// It is a range attribute that allows the duration of the game shift to be between 1 and 1440 minutes (24 hours).
    /// </summary>
    [SerializeField]
    [Range(1, 1440)]
    private int gameShiftMinutesIRL;

    /// <summary>
    /// The fadeImage attribute is used to store the image component that is used to fade the screen.
    /// </summary>
    [SerializeField]
    private Image fadeImage;

    /// <summary>
    /// The startHour attribute is used to store the starting hour of the game.
    /// It is a range attribute that allows the starting hour to be between 0 and 24 (real -life hours).
    /// </summary>
    [SerializeField]
    [Range(0, 24)]
    private int startHour;

    /// <summary>
    /// The dayText and timeText attributes are used to store the text components that display the day and time in the game respectively.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI dayText, timeText;

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
    /// The day attribute stores the current day of the game.
    /// </summary>
    private int day;

    /// <summary>
    /// The isOnBreak attribute is a boolean flag that indicates if the player is on a break or not.
    /// </summary>
    private bool isOnBreak;

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

    private void OnEnable()
    {
        PlayerPrefs.SetInt("CurrentDay", 1);

        PlayerPrefs.Save();

    }

    /// <summary>
    /// The Awake method is called when the script instance is being loaded. (Unity Callback)
    /// In this method, the game time properties are initillizaized and the game time is set (SetGameTime method).
    /// </summary>
    private void Awake()
    {
        // Just for testing purposes to rest the day player prefs when its clicked on play in the editor
        if (!PlayerPrefs.HasKey("GameStarted"))
        {
            PlayerPrefs.DeleteKey("CurrentDay");
            PlayerPrefs.DeleteKey("GameStarted");

            PlayerPrefs.SetInt("GameStarted", 1);  
        }


        fadeImage.gameObject.SetActive(false);
        timer = Time.time;
        isOnBreak = false;

        day = PlayerPrefs.GetInt("CurrentDay", 1);

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
        // 9 hours in real life shift duration in minutes
        const int SHIFT_DURATION_MINUTES_IRL = 540;

        float gameMinuteDuration = (float)gameShiftMinutesIRL / SHIFT_DURATION_MINUTES_IRL;

        gameMinuteInSecondsIRL = gameMinuteDuration * 60;

        gameHourInSecondsIRL = gameMinuteDuration * 3600;

        dayText.text = $"Day {day}";

        string[] time = daytime[startHour].Split(' ');

        timeText.text = $"{time[0]}:00 {time[1]}";
    }

    /// <summary>
    /// The UpdateTimeUI method is responsible for updating the time UI in the game.
    /// It updates the time in the following format : HH:MM AM/PM and calls the CheckWorkPauses method to check if the player is on a break or if the shift is over.
    /// </summary>
    private void UpdateTimeUI()
    {
        int hour = (int)(currentGameMinutes / 60);

        int currentHour = startHour + hour;

        int minutes = (int)(currentGameMinutes % 60);

        string[] time = daytime[currentHour].Split(' ');

        timeText.text = $"{time[0]}:{minutes:D2} {time[1]}";

        int workTime = currentHour - startHour;

        if (!isOnBreak)
        {
            CheckWorkBreaks(workTime);
        }
    }

    /// <summary>
    /// ThE CheckWorkBreaks method is responsible for checking if the player is on the lunch break or if the shift is over.
    /// If any of the conditions is met, the IsOnBreak flag is set to true, and the respetive method is called.
    /// to handle the logic of each case (GoLunch or NextDay).
    /// </summary>
    /// <param name="workTime">The time tha player has benn working.</param>
    private void CheckWorkBreaks(int workTime)
    {
        const int SHIFT_DURATION = 9;
        const int LUNCH_BREAK_DURATION = SHIFT_DURATION / 2;

        if (workTime == LUNCH_BREAK_DURATION)
        {
            isOnBreak = true;
            StartCoroutine(GoLunch());

            return;
        }


        if (workTime == SHIFT_DURATION)
        {
            isOnBreak = true;
            NextDay();

            return;
        }
    }

    /// <summary>
    /// The GoLunch method is responsible for handling the lunch break logic.
    /// It fades the screen in, and shows a lunch break messa for 1 game time hour and fades the screen out.
    /// </summary>
    /// <returns>An IEnumerator that can be used in a coroutine to wait and execute the specified method.</returns>
    private IEnumerator GoLunch()
    {
        float fadeDuration = 0.5f;

        // The lunch duration is 1 hour
        float lunchDuration = gameHourInSecondsIRL;

        fadeImage.gameObject.SetActive(true);

        StartCoroutine(Utils.FadeIn(fadeImage, fadeDuration));

        yield return new WaitForSeconds(lunchDuration);
        
        StartCoroutine(Utils.FadeOut(fadeImage, fadeDuration));

        fadeImage.gameObject.SetActive(false);

        isOnBreak = false; 
    }

    /// <summary>
    /// The NextDay method is responsible for advancing to the next day of the game.
    /// For now, it just reloads the current scene to test the day increment.
    /// </summary>
    private void NextDay()
    {   
        day++;

        PlayerPrefs.SetInt("CurrentDay", day);
        PlayerPrefs.Save();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
