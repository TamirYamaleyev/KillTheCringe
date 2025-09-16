using UnityEngine;

public class RandomWeapon : MonoBehaviour
{
    public float objectDestroyDelay = 8f;
    private PlayerController.Weapon weaponInBox;
    public int ammoInBox = 5;

    void Start()
    {
        var random = (PlayerController.Weapon[])System.Enum.GetValues(typeof(PlayerController.Weapon));
        weaponInBox = random[Random.Range(0, random.Length)];

        Destroy(gameObject, objectDestroyDelay);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().PickUpWeaponBox(weaponInBox, ammoInBox);
            Destroy(gameObject);
        }    
    }
}
