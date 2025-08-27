using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightningWeapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject lightningPrefab;

    // Firepoint is set high for other weapons, so we need an offset for this one
    public Vector3 LightningOffset = new Vector3(0.1f, -0.8f, 0);

    public void FireLightning()
    {
        Vector3 topScreenWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height, 0f));
        float distance = topScreenWorld.y - firePoint.position.y;

        GameObject bolt = Instantiate(lightningPrefab, firePoint.position + LightningOffset, Quaternion.identity);

        bolt.transform.rotation = Quaternion.Euler(0, 0, 90f);

        SpriteRenderer sr = bolt.GetComponent<SpriteRenderer>();
        float spriteWidth = sr.sprite.bounds.size.x;
        Vector3 scale = bolt.transform.localScale;
        scale.x = distance / spriteWidth;
        bolt.transform.localScale = scale;

        float halfLength = (spriteWidth * scale.x) / 2f;
        bolt.transform.position = firePoint.position + LightningOffset + bolt.transform.right * halfLength;
    }
}
