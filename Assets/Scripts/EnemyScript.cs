using System.Collections;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Sprite[] enemySprites;
    private SpriteRenderer sr;

    public LayerMask enemyLayer;
    // Max vertical screen size in 4k + some leeway
    public float raySize = 2500f;
    public bool canShoot = false;

    public float bulletSpeed = 5f;
    public float bulletDestroyDelay = 5f;

    public float minDelay = 3f;
    public float maxDelay = 8f;

    private Coroutine shootCoroutine;

    private float checkCooldown = 1f;
    private float checkTimer = 0f;

    public GameObject projectile;
    public GameObject firePoint;

    public int scoreToGive = 10;
    
    // SFX 
    [Header("SFX")]
    [SerializeField] AudioClip deathSfx;
    [SerializeField] AudioClip shootSfx;

    [SerializeField, Range(0f,1f)] float sfxVolume = 1f;
    [SerializeField] Vector2 pitchJitter = new Vector2(0.97f, 1.03f);

    AudioSource sfx;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        enemySprites = Resources.LoadAll<Sprite>("Sprites/Enemies");
        sr.sprite = enemySprites[Random.Range(0, enemySprites.Length)];
        
        sfx = gameObject.AddComponent<AudioSource>();
        sfx.playOnAwake = false;
        sfx.loop = false;
        sfx.spatialBlend = 0f;  
        sfx.dopplerLevel = 0f;
    }

    void Update()
    {
        checkTimer += Time.deltaTime;
        if (checkTimer >= checkCooldown)
        {
            checkTimer = 0f;
            CheckIfBottom();
        }

        if (canShoot && shootCoroutine == null)
        {
            shootCoroutine = StartCoroutine(ShootLoop());
        }
        else if (!canShoot && shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
            shootCoroutine = null;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            Die();
            AddScore();
        }
        if (other.gameObject.CompareTag("Lightning"))
        {
            Die();
            AddScore();
        }
        if (other.gameObject.CompareTag("Plasma"))
        {
            other.GetComponent<PlasmaExplosion>().Explode();
            Die();
            AddScore();
        }
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().GameOver();
        }
    }
    void AddScore()
    {
        // Add Score
    }

    public void Die()
    {
        WaveController waveController = FindFirstObjectByType<WaveController>();
        waveController.UnregisterEnemy();

        Destroy(gameObject);
        PlayLocal(deathSfx);

    }

    IEnumerator ShootLoop()
    {
        while (canShoot)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

            if (canShoot)
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        GameObject projectileInst = Instantiate(projectile, firePoint.transform.position, Quaternion.identity);
        var rb = projectileInst.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(0, -bulletSpeed);
        }

        Destroy(projectileInst, bulletDestroyDelay);
        
        PlayLocal(shootSfx);
    }

    void CheckIfBottom()
    {
        RaycastHit2D hit = Physics2D.Raycast(firePoint.transform.position, Vector2.down, raySize, enemyLayer);

        if (hit.collider == null || hit.collider.gameObject == gameObject)
        {
            canShoot = true;
        }
        else
        {
            canShoot = false;
        }
    }
    
    void PlayLocal(AudioClip clip)
    {
        if (!clip) return;
        sfx.pitch = Random.Range(pitchJitter.x, pitchJitter.y);
        sfx.PlayOneShot(clip, sfxVolume);
    }
}
