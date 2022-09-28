using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool isEnabled;
    public int enemyDirX;
    public float enemyMovementSpeed;
    public bool enemyNotDead;

    private Rigidbody2D enemyRb2d;
    private Animator enemyAnimator;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb2d = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (isEnabled && enemyRb2d)
        {
            enemyRb2d.velocity = new Vector2(enemyDirX * enemyMovementSpeed * Time.fixedDeltaTime, enemyRb2d.velocity.y);
            OpossumUpdateAnimation();
        }
        
    }

    void OpossumUpdateAnimation()
    {
        int state;
        if (enemyNotDead)
        {
            if (enemyDirX > 0f) { transform.rotation = Quaternion.Euler(0, 180, 0); state = 1; }
            else if (enemyDirX < 0f) { transform.rotation = Quaternion.Euler(0, 0, 0); state = 1; }
            else state = 0;
        }
        else state = 2;
        enemyAnimator.SetInteger("state", state);
    }
}
