using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    public float maxspeed = 4;
    public float jumpforce = 0.1f;
    public float gravity = 1;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    [HideInInspector]
    public bool lookingRight = true;

    private Rigidbody2D rb2d;
    private bool isGrounded = false;
    // Use this for initialization
    void Awake()
    {
        groundCheck = transform.Find("GroundCheck");
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void FixedUpdate()
    {
        float hor = Input.GetAxis("Horizontal");

        rb2d.velocity = new Vector2(hor * maxspeed, rb2d.velocity.y);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpforce * maxspeed);
        }

        if(!isGrounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y - (gravity * Time.deltaTime));
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.15F, whatIsGround);

        if ((hor > 0 && !lookingRight) || (hor < 0 && lookingRight))
            Flip();


    }

    public void Flip()
    {
        lookingRight = !lookingRight;
        Vector3 myScale = transform.localScale;
        myScale.x *= -1;
        transform.localScale = myScale;
    }
} 
