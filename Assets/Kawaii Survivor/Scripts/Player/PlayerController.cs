//using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.Windows;

//[RequireComponent(typeof(Rigidbody2D))]
//public class PlayerController : MonoBehaviour
//{
//    [Header("Elements")]
//    [SerializeField] private MobileJoystick playerJoystick;
//    [SerializeField] private float moveSpeed;
//    private float xInput;

//    [Header(" Settings ")]
//    private Rigidbody2D rb;

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        rb.velocity = Vector2.right;
//    }

//    private void Update()
//    {
//        HandleMovement();
//    }

//    // Update is called once per frame
//    private void FixedUpdate()
//    {
//        rb.velocity = playerJoystick.GetMoveVector() * moveSpeed * Time.deltaTime;
//    }
//    private void HandleMovement()
//    {
//        xInput = UnityEngine.Input.GetAxis("Horizontal");
//        rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
//    }
//}
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private MobileJoystick playerJoystick;
    [SerializeField] private float moveSpeed;

    [Header("Settings")]
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Lấy đầu vào từ joystick
        Vector2 joystickInput = playerJoystick.GetMoveVector();

        // Lấy đầu vào từ phím AWSD
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Kết hợp đầu vào
        Vector2 moveInput = new Vector2(horizontalInput + joystickInput.x, verticalInput + joystickInput.y);

        // Cập nhật velocity
        rb.velocity = moveInput.normalized * moveSpeed;
    }
}