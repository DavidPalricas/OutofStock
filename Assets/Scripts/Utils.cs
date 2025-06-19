using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.InputSystem;

/// <summary>
/// The Utils class is responsible for storing some methods 
/// that can be used in different parts of this game.
/// </summary>
public static class Utils
{   
    /// <summary>
    /// The GetRandomSeed method is responsible for generating a random seed to be set in a random generator.
    /// The seed is generated based on the current OS time in milliseconds.
    /// </summary>
    /// <returns>The current OS time in milliseconds as an integer</returns>
    private static int GetRandomSeed()
    {
        return Environment.TickCount;
    }


    public enum SoundEffects{
        CUSTOMER_ATTACKED,
        PAY,
        KAREN_COMPLAINNING
    }

    /// <summary>
    /// The GetChildren method is responsible for retrieving the children of a game object.
    /// </summary>
    /// <param name="parent">The transform component of the game object whose children are to be retrieved.</param>
    /// <returns>The children of the specified game object.</returns>
    public static GameObject[] GetChildren(Transform parent)
    {
        GameObject[] children = new GameObject[parent.childCount];

        for (int i = 0; i < parent.childCount; i++)
        {
            children[i] = parent.GetChild(i).gameObject;
        }

        return children;
    }

    /// <summary>
    /// The RandomInt method is responsible for generating a random integer between a specified range.
    /// </summary>
    /// <remarks>
    /// This method is used instead of UnityEngine.Random.Range or System.Random.Next 
    /// to avoid some patterns in producing random numbers.
    /// Because computes cannot generate truly random numbers, they generate pseudo-random numbers using a seed.
    /// So, in this method, we are setting the seed to the current OS time in milliseconds 
    /// to avoid patterns in producing random numbers.
    /// </remarks>
    /// <param name="min">The minimum intger value of the ranged specified.</param>
    /// <param name="max">The maximum integer value of the range specified .</param>
    /// <returns>A random integer between the specified range</returns>
    public static int RandomInt(int min, int max)
    {
        int seed = GetRandomSeed();

        UnityEngine.Random.InitState(seed);

        return UnityEngine.Random.Range(min, max);
    }

    /// <summary>
    /// The RandomFloat method is responsible for generating a random float between a specified range.
    /// </summary>
    /// <remarks>
    /// This method is used instead of UnityEngine.Random.Range or System.Random.Next 
    /// to avoid some patterns in producing random numbers.
    /// Because computes cannot generate truly random numbers, they generate pseudo-random numbers using a seed.
    /// So, in this method, we are setting the seed to the current OS time in milliseconds 
    /// to avoid patterns in producing random numbers.
    /// </remarks>
    /// <param name="min">The minimum float value of the ranged specified.</param>
    /// <param name="max">The maximum float value of the range specified .</param>
    /// <returns>A random float between the specified range</returns>
    public static float RandomFloat(float min, float max)
    {
        int seed = GetRandomSeed();

        UnityEngine.Random.InitState(seed);

        return UnityEngine.Random.Range(min, max);
    }

    /// <summary>
    /// The WaitAndExecute method is responsible for waiting for a specified time and then executing a method.
    /// </summary>
    /// <param name="waitTime">The time to wait before executing the method.</param>
    /// <param name="methodToExecute">The method to execute.</param>
    /// <returns>An IEnumerator that can be used in a coroutine to wait and execute the specified method.</returns>
    public static IEnumerator WaitAndExecute(float waitTime, Action methodToExecute)
    {
        yield return new WaitForSeconds(waitTime);
        methodToExecute();
    }

    /// <summary>
    /// The  GetUnitaryVector method is responsible for getting a unitary vector from a direction vector.
    /// </summary>
    /// <returns>A unitary 3D vector </returns>
    public static Vector3 GetUnitaryVector(Vector3 directionVector)
    {
        float xDirection = Mathf.Round(directionVector.x);
        float yDirection = Mathf.Round(directionVector.y);
        float zDirection = Mathf.Round(directionVector.z);

        return new Vector3(xDirection, yDirection, zDirection);
    }

    /// <summary>
    /// The FadeIn method is responsible for fading in the screen for a specified duration.
    /// </summary>
    /// <param name="fadeImage">The UI image that is used to fading in the screen.</param>
    /// <param name="fadeDuration">Duration of the fade.</param>
    /// <returns>An IEnumerator that can be used in a coroutine to wait and execute the specified method.</returns>
    public static IEnumerator FadeIn(Image fadeImage, float fadeDuration)
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeImage.color = Color.Lerp(Color.clear, Color.black, timer / fadeDuration);
            yield return null;
        }

        fadeImage.color = Color.black;
    }

    /// <summary>
    /// The FadeOut method is responsible for fading out the screen for a specified duration.
    /// </summary>
    /// <param name="fadeImage">The UI image that is used to fading out the screen.</param>
    /// <param name="fadeDuration">The duration of the fade.</param>
    /// <returns>An IEnumerator that can be used in a coroutine to wait and execute the specified method.</returns>
    public static IEnumerator FadeOut(Image fadeImage, float fadeDuration)
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeImage.color = Color.Lerp(Color.black, Color.clear, timer / fadeDuration);
            yield return null;
        }

        fadeImage.color = Color.clear;
    }

    /// <summary>  
    /// The CastRayFromUI method is responsible for casting a ray from the UI to the game world.  
    /// This method is almost used to cast a ray from the player's crosshair to the game world.  
    /// </summary>  
    /// <param name="uiElement">The uiElement where the ray will be casted.</param>  
    /// <returns>A Ray object representing the ray cast from the UI element to the game world.</returns>  
    public static Ray CastRayFromUI(RectTransform uiElement)
    {
        Vector2 uiElementScreenPos = RectTransformUtility.WorldToScreenPoint(
            null,
            uiElement.position
        );

        return Camera.main.ScreenPointToRay(uiElementScreenPos);
    }

    /// <summary>
    /// The ExitGame method is responsible for exiting the game.
    /// It works both in the Unity Editor and in the built game.
    /// </summary>
    public static void ExitGame()
    {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>  
    /// The LoadAndApplyBindings method is responsible for loading and applying if there are any player's input preferences saved.  
    /// After applying, it prints all the current bindings to the console.  
    /// </summary>  
    public static void LoadAndApplyBindings(PlayerInput playerInput)
    {
        string rebinds = PlayerPrefs.GetString("inputBindings", string.Empty);

        if (!string.IsNullOrEmpty(rebinds))
        {
            playerInput.actions.LoadBindingOverridesFromJson(rebinds);
            playerInput.actions.Enable();
        }
    }


    /// <summary>
    /// The PlaySoundEffect method is resposinble for playing a sound effect in our game
    /// </summary>
    /// <param name="clipName">The name of the sounds'effect clip</param>
    public static void PlaySoundEffect(SoundEffects clipName)
    {
        AudioManager audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        switch (clipName)
        {
            case SoundEffects.CUSTOMER_ATTACKED:
                audioManager.PlayCustomerAttackedSFX(audioManager.transform.position);
                break;

            case SoundEffects.PAY:
                audioManager.PlaySFX(audioManager.paymentSFX);
                break;

            case SoundEffects.KAREN_COMPLAINNING:     
                int randomSFXIndex = RandomInt(0, audioManager.karenComplainingSFX.Count);
                
                audioManager.PlaySFX(audioManager.karenComplainingSFX[randomSFXIndex]);
                break;

            default:
                break;
        }
    }
}
