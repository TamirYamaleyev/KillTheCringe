using UnityEngine;

public class PlasmaExplosion : MonoBehaviour
{
    private Animator animator;

    public float destroyDelay = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Explode()
    {
        animator.SetTrigger("Hit");
        gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
        Destroy(gameObject, destroyDelay);
    }
}
