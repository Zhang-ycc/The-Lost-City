using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Enemy
{
    private Rigidbody2D rb;

    public float speed;

    private bool Faceleft;

    public Transform leftPos;
    public Transform rightPos;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        transform.DetachChildren();

        Faceleft = transform.localScale.x < 0;
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
            rb.velocity = new Vector2( - speed, rb.velocity.y);
            if (transform.position.x < leftPos.position.x)
            {
                float x = transform.localScale.x;
                float y = transform.localScale.y;
                transform.localScale = new Vector3( - x, y, 1);
                Faceleft = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            if (transform.position.x > rightPos.position.x)
            {
                float x = transform.localScale.x;
                float y = transform.localScale.y;
                transform.localScale = new Vector3(-x, y, 1);
                Faceleft = true;
            }
        }
    }
}
