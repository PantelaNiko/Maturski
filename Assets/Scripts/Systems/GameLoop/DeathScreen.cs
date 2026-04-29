using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    private LocalizedString gameOverText, mainMenuText, quitText;

    [SerializeField] TextMeshProUGUI gameOver, mainMenu, quit;

    void Awake()
    {
        gameOverText = new LocalizedString();
        mainMenuText = new LocalizedString();
        quitText = new LocalizedString();

        gameOverText.english = "GAME OVER";
        mainMenuText.english = "BACK TO MAIN MENU";
        quitText.english = "QUIT";

        gameOverText.serbian = "KRAJ IGRE";
        mainMenuText.serbian = "VRATI SE U GLAVNI MENI";
        quitText.serbian = "IZADJI IZ IGRE";

        gameOver.text = gameOverText.Get();
        mainMenu.text = mainMenuText.Get();
        quit.text = quitText.Get();
    }

    public void Appear()
    {
        gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
