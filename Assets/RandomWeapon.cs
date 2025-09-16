using UnityEngine;

public class RandomWeapon : MonoBehaviour
{
    public float objectDestroyDelay = 8f;
    private PlayerController.Weapon weaponInBox;

    void Start()
    {
        var random = (PlayerController.Weapon[])System.Enum.GetValues(typeof(PlayerController.Weapon));

        // First weapon is the default weapon, so start at 1
        weaponInBox = random[Random.Range(1, random.Length)];

        Destroy(gameObject, objectDestroyDelay);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().PickUpWeaponBox(weaponInBox);
            Destroy(gameObject);
        }    
    }
}
