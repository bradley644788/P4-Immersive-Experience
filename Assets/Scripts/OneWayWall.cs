using UnityEngine;

[RequireComponent(typeof(Collider))]
public class OneWayWall : MonoBehaviour
{
    public Vector3 allowedDirection = Vector3.forward;
    private Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Vector3 toPlayer = other.transform.position - transform.position;

        // dot > 0 → player is coming from allowed side
        bool comingFromAllowedSide = Vector3.Dot(toPlayer.normalized, allowedDirection.normalized) > 0f;

        if (comingFromAllowedSide)
        {
            col.isTrigger = true;
        }
        else
        {
            col.isTrigger = false;
        }
    }
}
