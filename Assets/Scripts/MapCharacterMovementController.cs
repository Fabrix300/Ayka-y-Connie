using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCharacterMovementController : MonoBehaviour
{
    public float offSet;
    public float movementSpeed;

    private Transform mainCamTransform;
    private Rigidbody2D rb2d;
    private Animator anim;
    private SpriteRenderer sR;
    private int dirX;

    void Start()
    {
        dirX = 0;
        //rb2d = GetComponent<Rigidbody2D>();
        sR = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        mainCamTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            new Vector2(
                mainCamTransform.position.x + offSet,
                transform.position.y),
            movementSpeed * Time.deltaTime
            );
        if (Vector2.Distance(transform.position, new Vector2(
                mainCamTransform.position.x + offSet,
                transform.position.y)) < 0.01f)
        {
            dirX = 0;
        }
        else
        {
            if (mainCamTransform.position.x + offSet > transform.position.x)
            {
                dirX = 1;
            }
            else if (mainCamTransform.position.x + offSet < transform.position.x)
            {
                dirX = -1;
            }
        }
        UpdateAnimation();
    }

    /*private void FixedUpdate()
    {
        //rb2d.velocity = new Vector2(dirX * movementSpeed * Time.fixedDeltaTime, rb2d.velocity.y);
        //transform.position = Vector3.Lerp(transform.position, new Vector3(mainCamTransform.position.x + offSet, transform.position.y, transform.position.z), Time.fixedDeltaTime);
        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(mainCamTransform.position.x + offSet, transform.position.y, transform.position.z), movementSpeed * Time.fixedDeltaTime);
    }*/

    void UpdateAnimation()
    {
        int state;
        if (dirX > 0f)
        {
            sR.flipX = false;
            //transform.rotation = Quaternion.Euler(0, 0, 0);
            state = 1;
        }
        else if (dirX < 0f)
        {
            sR.flipX = true;
            //transform.rotation = Quaternion.Euler(0, 180, 0);
            state = 1;
        }
        else
        {
            sR.flipX = false;
            state = 0;
        }
        anim.SetInteger("state", state);
    }
}
