using UnityEngine;

public class ScreenClick : MonoBehaviour
{
    private CameraClickMover camMover;

    void Start()
    {
        camMover = Camera.main.GetComponent<CameraClickMover>();
    }

    void OnMouseDown()
    {
         Debug.Log("ECRAN CLIQUÉ");
        camMover.MoveTo(
            new Vector3(-1.54f, 3.3f, -1.46f),
            new Vector3(18.196f, -148.158f, -2.123f)
        );
    }
}
