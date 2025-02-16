using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float jumpSpeed = 4.0f;
    public float gravity = 9.8f;
    public float terminalVelocity = 100f;

    private bool isCrouching = false;
    private bool isSprinting = false;

    private CharacterController _charCont;
    private Vector3 _moveDirection = Vector3.zero;


    void Start()
    {
        _charCont = GetComponent<CharacterController>();
    }

    void Update()
    {
        // STUB: Potentially handle movement if player is active/inactive
        if (true)
        {
            HandlePlayerMove();
        }
        else
        {
            HandlePlayerInactiveMove();
        }
    }

    private void HandlePlayerMove()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        if (verticalAxis > 0 && _charCont.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isSprinting = true;
            }
        }
        if (verticalAxis <= 0)
        {
            isSprinting = false;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = true;
            _charCont.height = 1;
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            isCrouching = false;
            _charCont.height = 1.7f;
        }

        // Move direction directly from axes
        float deltaX = horizontalAxis * moveSpeed * (isSprinting ? 1.6f : 1) * (isCrouching ? 0.5f : 1);
        float deltaZ = verticalAxis * moveSpeed * (isSprinting ? 1.6f : 1) * (isCrouching ? 0.5f : 1);
        _moveDirection = new Vector3(deltaX, _moveDirection.y, deltaZ);
        
        // Accept jump input if grounded
        if (_charCont.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _moveDirection.y = jumpSpeed;
            }
            else
            {
                _moveDirection.y = 0f;
            }

            // STUB: Handle movement processes, such as footsteps SFX
            if (deltaX != 0 || deltaZ != 0)
            {
               // ToDo: player animations
            }
        }
        else
        {
            // ToDo: player animations
        }
        ApplyMovement();
    }

    private void HandlePlayerInactiveMove()
    {
        _moveDirection = Vector3.zero;
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        _moveDirection = transform.TransformDirection(_moveDirection);
        
        // Apply gravity
        _moveDirection.y -= this.gravity * Time.deltaTime;
        // Move the controller
        _charCont.Move(_moveDirection * Time.deltaTime);
    }
}