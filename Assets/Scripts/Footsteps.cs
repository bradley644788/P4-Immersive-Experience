using UnityEngine;

public class FootstepScript : MonoBehaviour
{
    public GameObject footstep;

    void Start()
    {
        footstep.SetActive(false);
    }

    void Update()
    {
        // Detect if player is pressing any movement key using Unity’s axis system
        bool isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;

        footstep.SetActive(isMoving);
    }
}
