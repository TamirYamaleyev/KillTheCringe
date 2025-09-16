using UnityEngine;
using UnityEngine.UI;

public class LifeController : MonoBehaviour
{
    public Image livesImage;
    public float lifeWidth = 32;

    void Start()
    {
        livesImage = GetComponent<Image>();
    }
    public void UpdateLives(int lives)
    {
        RectTransform rt = livesImage.rectTransform;
        rt.sizeDelta = new Vector2(lifeWidth * lives, rt.sizeDelta.y);
    }
}
