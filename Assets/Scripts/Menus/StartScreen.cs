using UnityEngine;

public class StartScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject startScreen;

    [SerializeField]
    private GameObject startMenu;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            startScreen.SetActive(false);
            startMenu.SetActive(true);

        }
    }
}
