using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum Weapon
    {
        Laser,
        PlasmaCannon,
        Lightning
    }

    public Weapon currentWeapon = Weapon.Laser;

    public GameObject firePoint;
    public GameObject laserProjectile;
    public GameObject plasmaProjectile;
    public GameObject lightningProjectile;

    public int lives = 3;

    public float moveSpeed = 5f;
    public float bulletSpeed = 10f;
    public float bulletDestroyDelay = 3f;
    private Rigidbody2D rb;

    private float halfPlayerWidth;
    private float horizontalLimit;
    

    public Quaternion bulletRotation = Quaternion.Euler(0f, 0f, 90f);
    
    // --- SFX ---
    [Header("SFX")]
    [SerializeField] AudioClip laserFireSfx;
    [SerializeField] AudioClip plasmaFireSfx;
    [SerializeField] AudioClip lightningFireSfx;

    [SerializeField, Range(0f,1f)] float sfxVolume = 1f;
    [SerializeField] Vector2 pitchJitter = new Vector2(0.97f, 1.03f);

    AudioSource sfx;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        halfPlayerWidth = sr.bounds.extents.x;

        Camera cam = Camera.main;
        float screenHalfWidth = cam.orthographicSize * cam.aspect;

        horizontalLimit = screenHalfWidth - halfPlayerWidth;
        
        // SFX source
        sfx = gameObject.AddComponent<AudioSource>();
        sfx.playOnAwake = false;
        sfx.loop = false;
        sfx.spatialBlend = 0f;  
        sfx.dopplerLevel = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        ClampPosition();
        ShootingHandler();
    }

    void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(horizontal * moveSpeed, 0);
    }

    void ClampPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -horizontalLimit, horizontalLimit);
        transform.position = pos;
    }

    void ShootingHandler()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            switch (currentWeapon)
            {
                case Weapon.Laser:
                    ShootLaser();
                    break;
                case Weapon.PlasmaCannon:
                    ShootPlasma();
                    break;
                case Weapon.Lightning:
                    ShootLightning();
                    break;
            }
        }
    }

    void ShootLaser()
    {
        GameObject bullet = Instantiate(laserProjectile, firePoint.transform.position, bulletRotation);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, bulletSpeed);
        PlaySfx(laserFireSfx);
        Destroy(bullet, bulletDestroyDelay);
    }

    void ShootPlasma()
    {
        GameObject bullet = Instantiate(plasmaProjectile, firePoint.transform.position, bulletRotation);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, bulletSpeed);
        PlaySfx(plasmaFireSfx);
        Destroy(bullet, bulletDestroyDelay);
    }

    void ShootLightning()
    {
        gameObject.GetComponent<LightningWeapon>().FireLightning();
        PlaySfx(lightningFireSfx);
    }

    public void TakeDamage()
    {
        lives--;

        // Add iFrame

        Debug.Log("Taking Damage");
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, lives * 0.25f);

        if (lives <= 0)
        {
            GameOver();
        }
    }
    
    void PlaySfx(AudioClip clip)
    {
        if (!clip || sfx == null) return;
        sfx.pitch = Random.Range(pitchJitter.x, pitchJitter.y);
        sfx.PlayOneShot(clip, sfxVolume);
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyProjectile"))
        {
            Destroy(other.gameObject);
            TakeDamage();
        }
    }
}
