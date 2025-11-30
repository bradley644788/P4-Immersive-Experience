using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;

    public float walkSpeed = 6f;
    public float runSpeed = 10f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;

    public AudioSource footstepAudio;
    public float footstepWalkPitch = 1f;
    public float footstepRunPitch = 1.5f;

    public bool canMove = true;
    
    public float maxStamina = 5f;
    public float staminaDrainRate = 1f;
    public float staminaRegenRate = 1f;
    private bool isExhausted = false;
    public AudioSource staminaEmptySound;

    private float stamina;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        stamina = maxStamina;
    }

    void Update()
    {
        // stamina logic
        bool wantsToRun = Input.GetKey(KeyCode.LeftShift) && canMove;
        bool isMovingInput = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;

        // drain stamina if running normally (and not exhausted)
        if (!isExhausted && wantsToRun && isMovingInput && stamina > 0f)
        {
            stamina -= staminaDrainRate * Time.deltaTime;
            if (stamina < 0f)
                stamina = 0f;
        }

        // if stamina reaches 0, become exhausted
        if (stamina <= 0f && !isExhausted)
        {
            isExhausted = true; 

            if (staminaEmptySound && !staminaEmptySound.isPlaying)
            {
                staminaEmptySound.loop = true;
                staminaEmptySound.Play();
            }
        }

        // regenerate stamina
        if (!(wantsToRun && isMovingInput))
        {
            stamina += staminaRegenRate * Time.deltaTime;
            if (stamina > maxStamina)
                stamina = maxStamina;
        }

        // breathing sound logic
        if (stamina > 2f)
        {
            if (staminaEmptySound && staminaEmptySound.isPlaying)
            {
                staminaEmptySound.loop = false;
            }

            // no longer exhausted, can now run
            isExhausted = false;
        }

        // running allowed only if stamina > 0 and not exhausted
        bool isRunning = wantsToRun && !isExhausted && stamina > 0f;

        // Movement input
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float inputX = canMove ? Input.GetAxis("Vertical") : 0;
        float inputY = canMove ? Input.GetAxis("Horizontal") : 0;

        float movementDirectionY = moveDirection.y;

        Vector3 horizontalMove = forward * inputX + right * inputY;

        if (horizontalMove.magnitude > 1f)
            horizontalMove.Normalize();

        horizontalMove *= isRunning ? runSpeed : walkSpeed;

        moveDirection = new Vector3(horizontalMove.x, movementDirectionY, horizontalMove.z);

        // Jump + Gravity
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
            moveDirection.y = jumpPower;
        else
            moveDirection.y = movementDirectionY;

        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);

        // Camera rotation
        if (canMove)
        {
            rotationX -= Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        // Footsteps
        bool isMoving =
            (Input.GetAxisRaw("Horizontal") != 0 ||
             Input.GetAxisRaw("Vertical") != 0) &&
            characterController.isGrounded;

        if (footstepAudio)
        {
            footstepAudio.pitch = isRunning ? footstepRunPitch : footstepWalkPitch;

            if (isMoving && !footstepAudio.isPlaying)
                footstepAudio.Play();
            else if (!isMoving && footstepAudio.isPlaying)
                footstepAudio.Stop();
        }
    }

    public void DisableControl()
    {
        canMove = false;
    }
}