using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    private Canvas menuCanvas;
    private Canvas commandesCanvas;

    void Start()
    {
        // Recherche les canvas par leur nom
        GameObject menuObj = GameObject.Find("Menu");
        GameObject commandesObj = GameObject.Find("Explications");

        if (menuObj != null) menuCanvas = menuObj.GetComponent<Canvas>();
        if (commandesObj != null) commandesCanvas = commandesObj.GetComponent<Canvas>();

        // Cache "CommandesDuJeu" au lancement
        if (commandesCanvas != null)
        {
            commandesCanvas.gameObject.SetActive(false);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void SwitchCanvas()
    {
        if (menuCanvas != null && commandesCanvas != null)
        {
            bool isMenuActive = menuCanvas.gameObject.activeSelf;

            menuCanvas.gameObject.SetActive(!isMenuActive);
            commandesCanvas.gameObject.SetActive(isMenuActive);
        }
        else
        {
            Debug.LogWarning("Un des Canvas est introuvable (\"Menu\" ou \"Explications\").");
        }
    }
}