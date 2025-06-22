using UnityEngine;

public class startScreen_Script : MonoBehaviour
{
    [SerializeField]
    private GameObject startScreen;

    [SerializeField]
    private GameObject startMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("return"))
        {

            startScreen.SetActive(false);
            startMenu.SetActive(true);

        }
    }
}
