using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]
public class FitToCamera : MonoBehaviour
{
    void Start()
    {
        Camera cam = Camera.main;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        float worldScreenHeight = cam.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight * cam.aspect;

        transform.localScale = new Vector3(worldScreenWidth / width, worldScreenHeight / height, 1f);
    }
}
