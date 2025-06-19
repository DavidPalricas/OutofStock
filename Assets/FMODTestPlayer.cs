using UnityEngine;
using FMODUnity;

public class FMODTestPlayer : MonoBehaviour
{
    [Header("Drag your FMOD Event here")]
    [EventRef]
    public EventReference testEvent;

    void Update()
    {
        // Press Spacebar to test the sound
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RuntimeManager.PlayOneShot(testEvent, transform.position);
        }
    }
}
