using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int opossumDirX;
    public float opossumMovementSpeed;
    public bool opossumNotDead = true;

    private Rigidbody2D opossumRb2d;
    private Animator opossumAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (opossumRb2d)
        {
            opossumRb2d.velocity = new Vector2(opossumDirX * opossumMovementSpeed * Time.fixedDeltaTime, opossumRb2d.velocity.y);
            OpossumUpdateAnimation();
        }
    }

    void OpossumUpdateAnimation()
    {

    }
}
