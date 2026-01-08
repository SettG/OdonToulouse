using UnityEngine;
using UnityEngine.UI;

public class ImageController : MonoBehaviour
{
    [Header("Image References")]
    public Image image1;
    public Image image2;

    void Start()
    {
        // Rendre les deux images invisibles au démarrage
        if (image1 != null)
            image1.gameObject.SetActive(false);

        if (image2 != null)
            image2.gameObject.SetActive(false);
    }

    // ===== Image 1 =====
    public void ShowImage1()
    {
        if (image1 != null)
            image1.gameObject.SetActive(true);
    }

    public void HideImage1()
    {
        if (image1 != null)
            image1.gameObject.SetActive(false);
    }

    // ===== Image 2 =====
    public void ShowImage2()
    {
        if (image2 != null)
            image2.gameObject.SetActive(true);
    }

    public void HideImage2()
    {
        if (image2 != null)
            image2.gameObject.SetActive(false);
    }
}
