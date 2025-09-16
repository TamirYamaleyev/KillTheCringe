using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LifeController lifeUI;
    public enum Weapon
    {
        Laser,
        PlasmaCannon,
        Lightning
    }

    public Weapon currentWeapon = Weapon.Laser;
    public int maxAmmo = 5;
    public int plasmaAmmo = 0;
    public int lightningAmmo = 0;
    public HUDWeapon hudWeapon;

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

    public AudioClip laserFireSFX;
    public AudioClip plasmaFireSFX;
    public AudioClip lightningFireSFX;

    [Range(0f, 1f)] float sfxVolume = 1f;
    Vector2 pitchJitter = new Vector2(0.97f, 1.03f);

    AudioSource sfx;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sfx = GetComponent<AudioSource>();

        lifeUI = FindFirstObjectByType<LifeController>();

        rb = GetComponent<Rigidbody2D>();

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        halfPlayerWidth = sr.bounds.extents.x;

        Camera cam = Camera.main;
        float screenHalfWidth = cam.orthographicSize * cam.aspect;

        horizontalLimit = screenHalfWidth - halfPlayerWidth;

        hudWeapon = FindFirstObjectByType<HUDWeapon>();
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
                    plasmaAmmo--;
                    hudWeapon.SetAmmo(Weapon.PlasmaCannon, plasmaAmmo);

                    if (plasmaAmmo <= 0) currentWeapon = Weapon.Laser;
                    break;
                case Weapon.Lightning:
                    ShootLightning();
                    lightningAmmo--;
                    hudWeapon.SetAmmo(Weapon.Lightning, lightningAmmo);

                    if (lightningAmmo <= 0) currentWeapon = Weapon.Laser;
                    break;
            }
            hudWeapon.ChangeWeapon(currentWeapon);
        }
    }

    public void PickUpWeaponBox(Weapon type)
    {
        hudWeapon.ChangeWeapon(type);
        hudWeapon.SetAmmo(type, maxAmmo);

        switch (type)
        {
            case Weapon.PlasmaCannon:
                plasmaAmmo = maxAmmo;
                currentWeapon = Weapon.PlasmaCannon;
                break;
            case Weapon.Lightning:
                lightningAmmo = maxAmmo;
                currentWeapon = Weapon.Lightning;
                break;
            default: currentWeapon = Weapon.Laser;
                break;
        }
    }

    void ShootLaser()
    {
        GameObject bullet = Instantiate(laserProjectile, firePoint.transform.position, bulletRotation);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, bulletSpeed);
        PlaySFX(laserFireSFX);
        Destroy(bullet, bulletDestroyDelay);
    }

    void ShootPlasma()
    {
        GameObject bullet = Instantiate(plasmaProjectile, firePoint.transform.position, bulletRotation);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, bulletSpeed);
        PlaySFX(plasmaFireSFX);
        Destroy(bullet, bulletDestroyDelay);
    }

    void ShootLightning()
    {
        gameObject.GetComponent<LightningWeapon>().FireLightning();
        PlaySFX(lightningFireSFX);
    }

    void PlaySFX(AudioClip clip)
    {
        if (!clip || sfx == null) return;
        sfx.pitch = Random.Range(pitchJitter.x, pitchJitter.y);
        sfx.PlayOneShot(clip, sfxVolume);
    }

    public void TakeDamage()
    {
        lives--;
        lifeUI.UpdateLives(lives);

        if (lives <= 0)
        {
            GameOver();
        }
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
