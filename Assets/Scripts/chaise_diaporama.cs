using UnityEngine;
using UnityEngine.UI;

public class ChaiseDiaporama : MonoBehaviour
{
    [Header("Images du diaporama")]
    public Sprite[] images; // Liste des 7 images (0-6)
    
    [Header("Références UI")]
    public Image displayImage; // L'Image UI qui affiche la diapo
    public GameObject buttonPanel; // Panel contenant les 3 boutons (visible uniquement sur image 4)
    
    private int currentIndex = 0; // Index de l'image actuelle (0-6)
    private bool isActive = false; // Le diaporama est-il actif ?

    void Start()
    {
        // Cacher les boutons au départ
        if (buttonPanel != null)
        {
            buttonPanel.SetActive(false);
        }
    }

    void Update()
    {
        // Ne gérer les inputs que si le diaporama est actif
        if (!isActive) return;

        // Navigation avec flèches
        bool rightPressed = false;
        bool leftPressed = false;

        #if ENABLE_INPUT_SYSTEM
        if (UnityEngine.InputSystem.Keyboard.current != null)
        {
            rightPressed = UnityEngine.InputSystem.Keyboard.current.rightArrowKey.wasPressedThisFrame;
            leftPressed = UnityEngine.InputSystem.Keyboard.current.leftArrowKey.wasPressedThisFrame;
        }
        #else
        rightPressed = Input.GetKeyDown(KeyCode.RightArrow);
        leftPressed = Input.GetKeyDown(KeyCode.LeftArrow);
        #endif

        if (rightPressed)
        {
            NextImage();
        }
        else if (leftPressed)
        {
            PreviousImage();
        }
    }

    // Appelée quand le CanvasChaise s'active
    public void StartDiaporama()
    {
        isActive = true;
        currentIndex = 0;
        ShowImage(currentIndex);
        Debug.Log(">>> DIAPORAMA DÉMARRÉ <<<");
    }

    // Appelée quand on quitte (Échap)
    public void StopDiaporama()
    {
        isActive = false;
        currentIndex = 0;
        if (buttonPanel != null)
        {
            buttonPanel.SetActive(false);
        }
        Debug.Log(">>> DIAPORAMA ARRÊTÉ <<<");
    }

    void NextImage()
    {
        // Bloquer la navigation si on est sur une réponse (indices 4, 5, 6)
        if (currentIndex >= 4)
        {
            Debug.Log("Navigation bloquée - Appuyez sur flèche gauche pour revenir à la question");
            return;
        }

        // Ne pas dépasser l'image de la question (index 3)
        if (currentIndex < 3)
        {
            currentIndex++;
            ShowImage(currentIndex);
            Debug.Log("Image suivante : " + currentIndex);
        }
    }

    void PreviousImage()
    {
        // Si on est sur une réponse (4, 5, 6), revenir directement à la question (3)
        if (currentIndex >= 4)
        {
            currentIndex = 3; // Retour à la question
            ShowImage(currentIndex);
            Debug.Log("Retour à la question");
            return;
        }

        // Navigation normale pour les autres images
        if (currentIndex > 0)
        {
            currentIndex--;
            ShowImage(currentIndex);
            Debug.Log("Image précédente : " + currentIndex);
        }
    }

    void ShowImage(int index)
    {
        // Afficher l'image correspondante
        if (displayImage != null && index >= 0 && index < images.Length)
        {
            displayImage.sprite = images[index];
        }

        // Afficher les boutons UNIQUEMENT sur l'image 4 (index 3)
        if (buttonPanel != null)
        {
            buttonPanel.SetActive(index == 3);
        }
    }

    // Fonctions appelées par les boutons
    public void OnButton1Click()
    {
        Debug.Log("Bouton 1 cliqué - Affichage réponse 1");
        currentIndex = 4; // Image 5 (index 4)
        ShowImage(currentIndex);
    }

    public void OnButton2Click()
    {
        Debug.Log("Bouton 2 cliqué - Affichage réponse 2");
        currentIndex = 5; // Image 6 (index 5)
        ShowImage(currentIndex);
    }

    public void OnButton3Click()
    {
        Debug.Log("Bouton 3 cliqué - Affichage réponse 3");
        currentIndex = 6; // Image 7 (index 6)
        ShowImage(currentIndex);
    }
}