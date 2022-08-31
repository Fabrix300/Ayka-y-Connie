using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCharacterMovement : MonoBehaviour
{
    public float offSet;
    public float movementSpeed;

    private Transform mainCamTransform;
    private Rigidbody2D rb2d;
    private int dirX;

    void Start()
    {
        dirX = 0;
        rb2d = GetComponent<Rigidbody2D>();
        mainCamTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (mainCamTransform.position.x + offSet > transform.position.x
            && mainCamTransform.position.x > transform.position.x)
        {
            dirX = 1;
        }
        else if (mainCamTransform.position.x + offSet < transform.position.x 
            && mainCamTransform.position.x < transform.position.x)
        {
            dirX = -1;
        }
        if (mainCamTransform.position.x + offSet > transform.position.x 
            && (mainCamTransform.position.x + offSet) - transform.position.x > offSet)
        {
            dirX = 0;
        }*/
        if (mainCamTransform.position.x + offSet > transform.position.x)
        {
            dirX = 1;
        }
        else if (mainCamTransform.position.x + offSet < transform.position.x)
        {
            dirX = -1;
        }
        else if (mainCamTransform.position.x + offSet == transform.position.x) { dirX = 0; }
    }

    private void FixedUpdate()
    {
        rb2d.velocity = new Vector2(dirX * movementSpeed * Time.fixedDeltaTime, rb2d.velocity.y);
        /*transform.position = Vector3.Lerp(
            transform.position, 
            new Vector3(
                mainCamTransform.position.x + offSet,
                transform.position.y, 
                transform.position.z),
            Time.fixedDeltaTime
            );*/
    }
}
