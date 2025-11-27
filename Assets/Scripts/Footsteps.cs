using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SimpleFootsteps : MonoBehaviour
{
    public CharacterController controller;
    private AudioSource footstepAudio;
    public float normalSpeed = 1f;
    public float runSpeed = 1.5f;

    void Start()
    {
        footstepAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        bool isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;

        if (!controller.isGrounded)
        {
            isMoving = false;
        }

        footstepAudio.pitch = Input.GetKey(KeyCode.LeftShift) ? runSpeed : normalSpeed;

        if (isMoving && !footstepAudio.isPlaying)
        {
            footstepAudio.Play();
        }
        else if (!isMoving && footstepAudio.isPlaying)
        {
            footstepAudio.Stop();
        }
    }
}
