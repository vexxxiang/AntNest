using UnityEngine;

public class Mom : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Settings")]
    [SerializeField] private GameObject cam;

    private Rigidbody rb;
    private Vector3 movement;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").gameObject;
        rb = GetComponent<Rigidbody>();

        // Wyłącz grawitację i rotację
        rb.useGravity = false;
        rb.freezeRotation = true;

    }

    void Update()
    {
        // Pobierz wejście z klawiatury
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveX != 0 || moveY != 0)
        {
            cam.GetComponent<CameraScript>().focusCamOnMom = true;
            cam.GetComponent<CameraScript>().camTarget = this.gameObject;

        }
        // Ruch w osi X i Y (Z = 0)
        movement = new Vector3(moveX, moveY, 0f).normalized;
    }

    void FixedUpdate()
    {
        // Ustaw prędkość w Rigidbody
        rb.linearVelocity = movement * moveSpeed;
    }
}
