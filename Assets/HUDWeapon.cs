using UnityEngine;

public class HUDWeapon : MonoBehaviour
{
    public GameObject LaserUI;
    public GameObject PlasmaUI;
    public GameObject LightningUI;

    public int ammoWidth = 32;

    public void ChangeWeapon(PlayerController.Weapon type)
    {
        LaserUI.SetActive(type == PlayerController.Weapon.Laser);
        PlasmaUI.SetActive(type == PlayerController.Weapon.PlasmaCannon);
        LightningUI.SetActive(type == PlayerController.Weapon.Lightning);
    }

    public void SetAmmo(PlayerController.Weapon type, int currentAmmo)
    {
        RectTransform rt = GetWeaponRect(type);
        rt.sizeDelta = new Vector2(ammoWidth * currentAmmo, rt.sizeDelta.y);
    }

    private RectTransform GetWeaponRect(PlayerController.Weapon type)
    {
        switch (type)
        {
            case PlayerController.Weapon.PlasmaCannon:
                return PlasmaUI.GetComponent<RectTransform>();
            case PlayerController.Weapon.Lightning:
                return LightningUI.GetComponent<RectTransform>();
            default:
                return LaserUI.GetComponent<RectTransform>();
        }
    }
}
