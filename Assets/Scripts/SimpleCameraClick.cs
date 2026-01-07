using UnityEngine;
using System.Collections;

public class SimpleCameraClick : MonoBehaviour
{
    [Header("Position écran")]
    public Vector3 screenPosition = new Vector3(-1.54f, 3.3f, -1.46f);
    public Vector3 screenRotation = new Vector3(18.196f, -148.158f, -2.123f);

    [Header("Position chaise")]
    public Vector3 chairPosition = new Vector3(2.68f, 4.43f, 2.59f);
    public Vector3 chairRotation = new Vector3(33.003f, -80.654f, 0f);

    [Header("Canvas UI")]
    public GameObject canvasEcran;
    public GameObject canvasChaise;
    public float delayBeforeShowingCanvas = 0.1f;
    
    [Header("Diaporama Chaise")]
    public ChaiseDiaporama chaiseDiaporama; // Référence au script de diaporama

    [Header("Vitesse")]
    public float moveSpeed = 2f;
    public float rotateSpeed = 2f;

    [Header("Bouton Retour")]
    public KeyCode returnKey = KeyCode.Escape;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private bool isMoving = false;
    private Camera cam;
    private bool wasMousePressed = false;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool hasMoved = false;
    
    private string currentView = ""; // "ecran", "chaise", ou ""
    private Coroutine showCanvasCoroutine;
    private bool canvasVisible = false; // Pour savoir si un canvas est affiché

    void Start()
    {
        cam = GetComponent<Camera>();
        
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        
        targetPosition = initialPosition;
        targetRotation = initialRotation;

        // Cacher les canvas au départ
        if (canvasEcran != null) canvasEcran.SetActive(false);
        if (canvasChaise != null) canvasChaise.SetActive(false);
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
                
                // Démarrer le timer pour afficher le canvas si on n'est pas en retour
                if (hasMoved && showCanvasCoroutine == null)
                {
                    showCanvasCoroutine = StartCoroutine(ShowCanvasAfterDelay());
                }
            }
        }

        // Détecter le bouton Retour - Compatible avec TOUS les systèmes
        bool returnPressed = false;
        
        #if ENABLE_INPUT_SYSTEM
        if (UnityEngine.InputSystem.Keyboard.current != null)
        {
            returnPressed = UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame;
        }
        #else
        returnPressed = Input.GetKeyDown(returnKey);
        #endif
        
        if (hasMoved && returnPressed)
        {
            ReturnToInitialPosition();
        }

        // Détecter les clics
        bool mousePressed = false;
        
        #if ENABLE_INPUT_SYSTEM
        if (UnityEngine.InputSystem.Mouse.current != null)
        {
            mousePressed = UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame;
        }
        #else
        mousePressed = Input.GetMouseButtonDown(0);
        #endif

        if (mousePressed && !wasMousePressed)
        {
            // Ne détecter les clics QUE si aucun canvas n'est visible
            if (!canvasVisible)
            {
                DetectClick();
            }
        }
        
        wasMousePressed = mousePressed;
    }

    IEnumerator ShowCanvasAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeShowingCanvas);
        
        Debug.Log(">>> AFFICHAGE DU CANVAS <<<");
        
        if (currentView == "ecran" && canvasEcran != null)
        {
            canvasEcran.SetActive(true);
            canvasVisible = true;
        }
        else if (currentView == "chaise" && canvasChaise != null)
        {
            canvasChaise.SetActive(true);
            canvasVisible = true;
            
            // Démarrer le diaporama de la chaise
            if (chaiseDiaporama != null)
            {
                chaiseDiaporama.StartDiaporama();
            }
        }
        
        showCanvasCoroutine = null;
    }

    void DetectClick()
    {
        Vector3 mousePos = Vector3.zero;
        
        #if ENABLE_INPUT_SYSTEM
        if (UnityEngine.InputSystem.Mouse.current != null)
        {
            mousePos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
        }
        #else
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
                hasMoved = true;
                currentView = "ecran";
                
                // Annuler le timer précédent si existe
                if (showCanvasCoroutine != null)
                {
                    StopCoroutine(showCanvasCoroutine);
                    showCanvasCoroutine = null;
                }
                
                // Cacher tous les canvas
                HideAllCanvas();
            }
            else if (objName.Contains("chaise") || objName.Contains("chair"))
            {
                Debug.Log(">>> DÉPLACEMENT VERS LA CHAISE <<<");
                targetPosition = chairPosition;
                targetRotation = Quaternion.Euler(chairRotation);
                isMoving = true;
                hasMoved = true;
                currentView = "chaise";
                
                // Annuler le timer précédent si existe
                if (showCanvasCoroutine != null)
                {
                    StopCoroutine(showCanvasCoroutine);
                    showCanvasCoroutine = null;
                }
                
                // Cacher tous les canvas
                HideAllCanvas();
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

    void ReturnToInitialPosition()
    {
        Debug.Log(">>> RETOUR À LA POSITION INITIALE <<<");
        
        // Arrêter le diaporama si actif
        if (chaiseDiaporama != null)
        {
            chaiseDiaporama.StopDiaporama();
        }
        
        // Annuler le timer d'affichage du canvas si en cours
        if (showCanvasCoroutine != null)
        {
            StopCoroutine(showCanvasCoroutine);
            showCanvasCoroutine = null;
        }
        
        // Cacher tous les canvas
        HideAllCanvas();
        
        targetPosition = initialPosition;
        targetRotation = initialRotation;
        isMoving = true;
        hasMoved = false;
        currentView = "";
    }

    void HideAllCanvas()
    {
        if (canvasEcran != null) canvasEcran.SetActive(false);
        if (canvasChaise != null) canvasChaise.SetActive(false);
        canvasVisible = false; // Plus aucun canvas visible
    }

    public void OnReturnButtonClick()
    {
        if (hasMoved)
        {
            ReturnToInitialPosition();
        }
    }
}