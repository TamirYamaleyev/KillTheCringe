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

    public GameObject weaponBox;
    public float boxDropChance = 0.2f;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        enemySprites = Resources.LoadAll<Sprite>("Sprites/Enemies");
        sr.sprite = enemySprites[Random.Range(0, enemySprites.Length)];
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
        }
        if (other.gameObject.CompareTag("Lightning"))
        {
            Die();
        }
        if (other.gameObject.CompareTag("Plasma"))
        {
            other.GetComponent<PlasmaExplosion>().Explode();
            Die();
        }
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().GameOver();
        }
    }
    void AddScore(int amount)
    {
        ScoreController sc = FindFirstObjectByType<ScoreController>();
        sc.UpdateScore(amount);
    }

    public void Die()
    {
        WaveController waveController = FindFirstObjectByType<WaveController>();
        waveController.UnregisterEnemy();

        AddScore(scoreToGive);
        CheckIfToDropWeaponBox();
        Destroy(gameObject);

    }

    public void CheckIfToDropWeaponBox()
    {
        if (boxDropChance >= Random.Range(0f, 1f))
        {
            Instantiate(weaponBox, firePoint.transform.position, Quaternion.identity);
        }
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
}
