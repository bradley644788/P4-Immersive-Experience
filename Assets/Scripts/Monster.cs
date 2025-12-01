using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;

    public AudioSource sound;
    public float detectionRange = 10f;
    public float wanderRadius = 20f;
    // public RawImage redOverlay;
    // public Light torchLight;
    // public float torchLightDefault = 400f;
    private Color normalFog;
    private Color dangerFog;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Wander();

        ColorUtility.TryParseHtmlString("#080808", out normalFog);
        ColorUtility.TryParseHtmlString("#320808", out dangerFog);
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist < detectionRange)
        {
            agent.SetDestination(player.position);

            float closeness = 1f - (dist / detectionRange);
            RenderSettings.ambientLight = Color.Lerp(normalFog, dangerFog, closeness);
            
            // if (Time.frameCount % 5 == 0 && FindFirstObjectByType<PlayerController>().canMove)
            //     torchLight.intensity = Random.Range(100f, 500f);
        }
        else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Wander();
            // torchLight.intensity = torchLightDefault;
            RenderSettings.fogColor = normalFog;
        }

        sound.volume = Mathf.Clamp01(1f - (dist / detectionRange));
        if (!sound.isPlaying) sound.Play();

        // if (redOverlay)
        // {
        //     float alpha = Mathf.Clamp01(1f - (dist / detectionRange)) * .33f;
        //     Color c = redOverlay.color;
        //     c.a = alpha;
        //     redOverlay.color = c;
        // }
    }

    void Wander()
    {
        if (NavMesh.SamplePosition(transform.position + Random.insideUnitSphere * wanderRadius, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindFirstObjectByType<GameOverMenu>().ShowGameOver();
            FindFirstObjectByType<PlayerController>().DisableControl();
        }
    }
}