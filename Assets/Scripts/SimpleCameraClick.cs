using UnityEngine;

public class SimpleCameraClick : MonoBehaviour
{
    [Header("Position écran")]
    public Vector3 screenPosition = new Vector3(-1.54f, 3.3f, -1.46f);
    public Vector3 screenRotation = new Vector3(18.196f, -148.158f, -2.123f);

    [Header("Position chaise")]
    public Vector3 chairPosition = new Vector3(2.68f, 4.43f, 2.59f);
    public Vector3 chairRotation = new Vector3(33.003f, -80.654f, 0f);

    [Header("Vitesse")]
    public float moveSpeed = 2f;
    public float rotateSpeed = 2f;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private bool isMoving = false;
    private Camera cam;
    private bool wasMousePressed = false;

    void Start()
    {
        cam = GetComponent<Camera>();
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    void Update()
    {
        // Déplacement fluide
        if (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                transform.rotation = targetRotation;
                isMoving = false;
            }
        }

        // Détecter les clics - Compatible avec TOUS les systèmes
        bool mousePressed = false;

#if ENABLE_INPUT_SYSTEM
        // Nouveau Input System
        if (UnityEngine.InputSystem.Mouse.current != null)
        {
            mousePressed = UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame;
        }
#else
        // Ancien Input System
        mousePressed = Input.GetMouseButtonDown(0);
#endif

        if (mousePressed && !wasMousePressed)
        {
            DetectClick();
        }

        wasMousePressed = mousePressed;
    }

    void DetectClick()
    {
        Vector3 mousePos = Vector3.zero;

#if ENABLE_INPUT_SYSTEM
        // Nouveau Input System
        if (UnityEngine.InputSystem.Mouse.current != null)
        {
            mousePos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
        }
#else
        // Ancien Input System
        mousePos = Input.mousePosition;
#endif

        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            Debug.Log("===== OBJET CLIQUÉ : " + hit.collider.gameObject.name + " =====");

            string objName = hit.collider.gameObject.name.ToLower();

            if (objName.Contains("ecran") || objName.Contains("screen"))
            {
                Debug.Log(">>> DÉPLACEMENT VERS L'ÉCRAN <<<");
                targetPosition = screenPosition;
                targetRotation = Quaternion.Euler(screenRotation);
                isMoving = true;
            }
            else if (objName.Contains("chaise") || objName.Contains("chair"))
            {
                Debug.Log(">>> DÉPLACEMENT VERS LA CHAISE <<<");
                targetPosition = chairPosition;
                targetRotation = Quaternion.Euler(chairRotation);
                isMoving = true;
            }
            else
            {
                Debug.Log("Objet cliqué mais pas reconnu comme écran ou chaise");
            }
        }
        else
        {
            Debug.Log("Aucun objet touché par le raycast");
        }
    }
}