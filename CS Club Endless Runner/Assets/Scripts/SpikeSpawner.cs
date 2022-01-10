using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject spike, portal;

    [SerializeField]
    Transform[] spawnPositions;

    [SerializeField]
    int maxSpikesPerSpawn;

    [SerializeField]
    int maxSpikeLength;

    [SerializeField]
    int spikeCount;

    [SerializeField]
    AnimationCurve SpikeSpeedCurve, SpawnIntervalCurve;

    [SerializeField]
    float acceleration, speedIncrease;

    float spawnTimer;
    float spawnInterval;
    int spikesLeft;
    int portalHits = 0;
    int spawnPositionIndex = 0;
    Transform spawnPosition;

    public static SpikeSpawner Instance;

    public float SpikeSpeed { get; private set; }
    public int DirectionMultiplier { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        PortalCollision.OnPortalCollision += HitPortal;
    }

    void OnDisable()
    {
        PortalCollision.OnPortalCollision -= HitPortal;
    }

    void Start()
    {
        SpikeSpeed = SpikeSpeedCurve.Evaluate(portalHits);
        spawnInterval = SpawnIntervalCurve.Evaluate(portalHits);
        spawnTimer = spawnInterval;
        spikesLeft = spikeCount;
        spawnPosition = spawnPositions[spawnPositionIndex];
        DirectionMultiplier = -1;
    }

    void Update()
    {
        SpikeSpeed = Mathf.Lerp(SpikeSpeed, SpikeSpeedCurve.Evaluate(portalHits) + speedIncrease, acceleration * Time.deltaTime);

        spawnTimer -= Time.deltaTime;
        if (spawnTimer < 0)
        {
            if (spikesLeft == 0)
            {
                SpawnPortal();
            }
            else if (spikesLeft > 0)
            {
                SpawnSpike();
            }
            spikesLeft--;
            spawnTimer = spawnInterval;
        }
    }

    void SpawnSpike()
    {
        int spikes = Random.Range(1, maxSpikesPerSpawn + 1);
        int spikePlacementDirection = 1;
        const float SpikeWidth = 0.75f;

        for (int i = 0; i < spikes; i++)
        {
            spikePlacementDirection *= -1;
            Instantiate(spike, (Vector2)spawnPosition.position + new Vector2((i + 1) / 2 * spikePlacementDirection * SpikeWidth, 0), Quaternion.identity);
        }
    }

    void SpawnPortal()
    {
        Instantiate(portal, spawnPosition.position, Quaternion.identity);
    }

    void HitPortal()
    {
        portalHits++;
        DirectionMultiplier *= -1;
        spawnPositionIndex++;
        spawnPositionIndex %= spawnPositions.Length;
        spawnPosition = spawnPositions[spawnPositionIndex];
        UpdateDifficulty();
    }

    void UpdateDifficulty()
    {
        spikeCount++;
        spikesLeft = spikeCount;

        if (maxSpikesPerSpawn < maxSpikeLength)
        {
            maxSpikesPerSpawn++;
        }

        SpikeSpeed = SpikeSpeedCurve.Evaluate(portalHits);
        spawnInterval = SpawnIntervalCurve.Evaluate(portalHits);
    }
}
