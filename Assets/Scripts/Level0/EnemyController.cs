using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool isEnabled;
    public int opossumDirX;
    public float opossumMovementSpeed;
    public bool opossumNotDead;

    private Rigidbody2D opossumRb2d;
    private Animator opossumAnimator;

    // Start is called before the first frame update
    void Start()
    {
        opossumRb2d = GetComponent<Rigidbody2D>();
        opossumAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (isEnabled && opossumRb2d)
        {
            opossumRb2d.velocity = new Vector2(opossumDirX * opossumMovementSpeed * Time.fixedDeltaTime, opossumRb2d.velocity.y);
            OpossumUpdateAnimation();
        }
        
    }

    void OpossumUpdateAnimation()
    {
        int state;
        if (opossumNotDead)
        {
            if (opossumDirX > 0f) { transform.rotation = Quaternion.Euler(0, 180, 0); state = 1; }
            else if (opossumDirX < 0f) { transform.rotation = Quaternion.Euler(0, 0, 0); state = 1; }
            else state = 0;
        }
        else state = 2;
        opossumAnimator.SetInteger("state", state);
    }
}
