using UnityEngine;

public class EnemyController : MonoBehaviour
{
    enum Direction
    {
        Left,
        Right
    }
    private Direction currentDirection = Direction.Right;
    public float offset = 1f;
    public float downwardDistance = 1f;

    public float speed = 5f;

    private bool waitingForEdgeExit = false;

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        bool hitEdge = HasGroupHitEdge();

        if (hitEdge && !waitingForEdgeExit)
        {
            transform.position -= new Vector3(0, downwardDistance);
            currentDirection = currentDirection == Direction.Left ? Direction.Right : Direction.Left;
            waitingForEdgeExit = true;
        }
        else if (!hitEdge)
        {
            waitingForEdgeExit = false;
        }

        float moveAmount = speed * Time.deltaTime;
        if (currentDirection == Direction.Left)
        {
            transform.position += new Vector3(-moveAmount, 0);
        }

        if (currentDirection == Direction.Right)
        {
            transform.position += new Vector3(moveAmount, 0);
        }
    }

    bool HasGroupHitEdge()
    {
        float screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;

        foreach (Transform enemy in transform)
        {
            if (enemy == null) continue;

            SpriteRenderer sr = enemy.GetComponent<SpriteRenderer>();

            float enemyLeftEdge = sr.bounds.min.x - offset;
            float enemyRightEdge = sr.bounds.max.x + offset;

            if (enemyLeftEdge < -screenHalfWidth || enemyRightEdge > screenHalfWidth)
            {
                return true;
            }
        }
        return false;
    }
}
