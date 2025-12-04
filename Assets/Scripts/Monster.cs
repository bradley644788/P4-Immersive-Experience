using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;

    public AudioSource sound;
    public float detectionRange = 20f;
    public float wanderRadius = 100f;
    public float wanderMinDistance = 50f;
    private Color normalFog;
    public Color dangerFog;
    private Color normalAmbient;
    public Color dangerAmbient;

    public Light flashlight;
    private float flashlightDefault;

    public WinningMenu winningMenu;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Wander();

        normalFog = RenderSettings.fogColor;
        normalAmbient = RenderSettings.ambientLight;
        flashlightDefault = flashlight.intensity;

        WinningMenu.hasWon = false;
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        // Player detection
        if (dist < detectionRange)
        {
            agent.SetDestination(player.position);

            float closeness = 1f - (dist / detectionRange);
            RenderSettings.fogColor = Color.Lerp(normalFog, dangerFog, closeness);
            RenderSettings.ambientLight = Color.Lerp(normalAmbient, dangerAmbient, closeness);
            flashlight.intensity = Mathf.Lerp(flashlightDefault, 15f, closeness);
        }
        else
        {
            // Wandering
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                Wander();
            }
            // RenderSettings.fogColor = normalFog;
            // RenderSettings.ambientLight = normalAmbient;
            // flashlight.intensity = flashlightDefault;
        }
    }

    void Wander()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius + transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
        {
            if (Vector3.Distance(transform.position, hit.position) >= wanderMinDistance)
            {
                agent.SetDestination(hit.position);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (WinningMenu.hasWon) return;
            
            FindFirstObjectByType<GameOverMenu>().ShowGameOver();
            FindFirstObjectByType<PlayerController>().DisableControl(); 
            
            if (!FindFirstObjectByType<PlayerController>().canMove)
            {
                RenderSettings.fogColor = Color.black;
                RenderSettings.ambientLight = Color.black;

                agent.updateRotation = false; 
                Vector3 targetPos = player.position;
                targetPos.y = transform.position.y;
                transform.LookAt(targetPos);
                
                return;
            }
            else
            {
                agent.updateRotation = true;
            }
        }
    }
}