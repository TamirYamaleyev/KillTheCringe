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
    public int plasmaAmmo = 0;
    public int lightningAmmo = 0;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lifeUI = FindFirstObjectByType<LifeController>();

        rb = GetComponent<Rigidbody2D>();

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        halfPlayerWidth = sr.bounds.extents.x;

        Camera cam = Camera.main;
        float screenHalfWidth = cam.orthographicSize * cam.aspect;

        horizontalLimit = screenHalfWidth - halfPlayerWidth;
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
                    break;
                case Weapon.Lightning:
                    ShootLightning();
                    lightningAmmo--;
                    break;
            }
        }
    }

    public void PickUpWeaponBox(Weapon type, int ammo)
    {
        switch (type)
        {
            case Weapon.PlasmaCannon:
                plasmaAmmo = ammo;
                currentWeapon = Weapon.PlasmaCannon;
                break;
            case Weapon.Lightning:
                lightningAmmo = ammo;
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
        Destroy(bullet, bulletDestroyDelay);
    }

    void ShootPlasma()
    {
        GameObject bullet = Instantiate(plasmaProjectile, firePoint.transform.position, bulletRotation);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, bulletSpeed);
        Destroy(bullet, bulletDestroyDelay);
    }

    void ShootLightning()
    {
        gameObject.GetComponent<LightningWeapon>().FireLightning();
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
