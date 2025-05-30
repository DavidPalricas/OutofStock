using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The MainMenu class is responsible for having the buttons logic in the main menu.
/// </summary>
public class MainMenu : MonoBehaviour
{
    //// Referencia ao objecto com o script que gere o fade in/out
    //[SerializeField]
    //private GameObject fadeManager;

    //// Variavel que vai verificar qual a cena atual, DEVE EXISTIR NOS SCRIPTS DE CANVAS DAS CENAS QUE NECESSITAM DE DAR FADE IN/OUT
    //private int currentSceneIndex;



    private void Awake(){
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// The PlayGame method is responsible for loading the first level of the game.
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //Utils.PlaySoundEffect("fall");
        //fadeManager.GetComponent<LevelChanger>().FadeToLevel(currentSceneIndex + 1);
    }

    ///// <summary>
    ///// The ControlsMenu method is responsible for loading the controls menu.
    ///// </summary>
    //public void ControlsMenu()
    //{
    //    SceneManager.LoadScene("ControlsMenu");
    //}

    /// <summary>
    /// The QuitGame method is responsible for quitting the game.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
        //Utils.PlaySoundEffect("fall");
        //fadeManager.GetComponent<LevelChanger>().FadeToLevel(currentSceneIndex + 1);
    }
}