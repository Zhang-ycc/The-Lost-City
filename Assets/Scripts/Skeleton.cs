using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    private Rigidbody2D rb;

    public float speed;

    private bool Faceleft = false;

    public Transform leftPos;
    public Transform rightPos;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        transform.DetachChildren();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (!base.isDie)
        {
            Movement();
        }
    }

    void Movement()
    {
        if (Faceleft)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            if (transform.position.x < leftPos.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                Faceleft = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            if (transform.position.x > rightPos.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                Faceleft = true;
            }
        }
    }
}
