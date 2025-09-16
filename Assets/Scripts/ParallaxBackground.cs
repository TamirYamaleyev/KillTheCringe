using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private RawImage image;
    [SerializeField] private float x;
    [SerializeField] private float y;

    void Start()
    {
        image = GetComponent<RawImage>();
    }

    void Update()
    {
        image.uvRect = new Rect(image.uvRect.position + new Vector2(x, y)  * Time.deltaTime, image.uvRect.size);
    }
}
