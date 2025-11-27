using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;

    public AudioSource sound;
    public float detectionRange = 10f;
    public float wanderRadius = 20f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Wander();
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist < detectionRange)
        {
            agent.SetDestination(player.position);
        }
        else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Wander();
        }

        sound.volume = Mathf.Clamp01(1f - (dist / detectionRange));
        if (!sound.isPlaying) sound.Play();
    }

    void Wander()
    {
        if (NavMesh.SamplePosition(transform.position + Random.insideUnitSphere * wanderRadius, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}
