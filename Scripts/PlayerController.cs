using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float mouseSensitivity = 0.2f;

    public TextMeshProUGUI interactText;
    

    private Rigidbody rb;
    private Camera cam;
    private float xRotation = 0f;
    
    private IInteractable currentInteractable;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();

        // Bloquear cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleLook();
        HandleInteract();
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            ShopMenu.Instance.ToggleShop();
        }
    }

    void FixedUpdate()
    {
        HandleMove();
    }

    void HandleMove()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) input.y += 1;
        if (Keyboard.current.sKey.isPressed) input.y -= 1;
        if (Keyboard.current.aKey.isPressed) input.x -= 1;
        if (Keyboard.current.dKey.isPressed) input.x += 1;

        Vector3 move = transform.right * input.x + transform.forward * input.y;
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
    }

    void HandleLook()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * mouseSensitivity;
        float mouseY = mouseDelta.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleInteract()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                SetHighlight(hit.collider.gameObject, true);
                currentInteractable = interactable;
                interactText.text = "[E] " + interactable.GetInteractText();
                interactText.gameObject.SetActive(true);

                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    interactable.Interact();
                }

                return;
            }
        }

        // No apunta a nada
        if (currentInteractable != null)
        {
            // Quitar highlight del objeto anterior
            MonoBehaviour mono = currentInteractable as MonoBehaviour;
            if (mono != null) SetHighlight(mono.gameObject, false);
            currentInteractable = null;
        }

        interactText.gameObject.SetActive(false);
    }

    void SetHighlight(GameObject obj, bool active)
    {
        // Buscar Outline en el objeto o en su padre
        Outline outline = obj.GetComponent<Outline>();
        if (outline == null)
            outline = obj.GetComponentInParent<Outline>();

        if (outline != null)
        {
            outline.ShowOutline(active);
            return;
        }

        // Fallback
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer == null) return;
        renderer.material.SetColor("_BaseColor", active ? Color.green : Color.white);
    }
}