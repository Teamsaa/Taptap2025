using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2;
    public Animator anim; 
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 targetPos;
    private bool isMoving = false;
    private float isRight = 1;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPos = transform.position;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos = new Vector2(mouseWorld.x, transform.position.y);
            float offset = targetPos.x - transform.position.x;

            if (Mathf.Abs(offset) > 0.1)
            {
                isMoving = true;
                if (offset > 0)
                {
                    isRight = 1;
                    sr.flipX = true;

                }
                else
                {
                    isRight = -1;
                    sr.flipX = false;
                }
            }
        }
        if (Mathf.Abs(targetPos.x - transform.position.x) < 0.1f)
        {
            isMoving = false;
            rb.velocity = Vector2.zero;
            anim.SetFloat("running", 0);
        }
        if (isMoving)
        {
            Vector2 Velocity = rb.velocity;
            Velocity.x =  isRight * moveSpeed;
            rb.velocity = Velocity;
            anim.SetFloat("running", Mathf.Abs(targetPos.x - transform.position.x));
        }
        
    }
}
