using UnityEngine;

public class CameraClickMover : MonoBehaviour
{
    [Header("Caméra cible pour l’écran")]
    public Vector3 screenPosition = new Vector3(-1.54f, 3.3f, -1.46f);
    public Vector3 screenRotationEuler = new Vector3(18.196f, -148.158f, -2.123f);

    [Header("Paramètres de déplacement")]
    public float moveSpeed = 3f;
    public float rotateSpeed = 3f;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private bool moving = false;

    void Update()
    {
        // Déplacement progressif
        if (moving)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                moving = false;
            }
        }

        // Détection clic souris
        if (Input.GetMouseButtonDown(0))
        {
            DetectClick();
        }
    }

    void DetectClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Raycast touché : " + hit.collider.name);

            // Si on clique sur l'objet nommé "Ecran"
            if (hit.collider.name == "Ecran")
            {
                MoveTo(screenPosition, screenRotationEuler);
            }
        }
    }

    void MoveTo(Vector3 position, Vector3 rotationEuler)
    {
        targetPosition = position;
        targetRotation = Quaternion.Euler(rotationEuler);
        moving = true;
        Debug.Log("Caméra se déplace vers : " + position);
    }
}
