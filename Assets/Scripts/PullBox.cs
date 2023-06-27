using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullBox : MonoBehaviour
{

    //推箱子
    public float distance = 1f;
    public LayerMask boxMask;
    private int m_facingDirection = 1;
    public float height;

    GameObject box;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<SpriteRenderer>().flipX)
        {
            m_facingDirection = -1;
        }
        else
        {
            m_facingDirection = 1;
        }

        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0f, height, 0f), Vector2.right * m_facingDirection, distance, boxMask);

        if (hit.collider != null && Input.GetButton("Pull"))
        {
            box = hit.collider.gameObject;

            box.GetComponent<FixedJoint2D>().enabled = true;
            box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
        }
        else if (Input.GetButtonUp("Pull"))
        {
            box.GetComponent<FixedJoint2D>().enabled = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position + new Vector3(0f, height, 0f), (Vector2)(transform.position + new Vector3(0f, height, 0f)) + Vector2.right * m_facingDirection * distance);
    }
}
