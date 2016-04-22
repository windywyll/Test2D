using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour {

    bool isGrounded, jumping, topJump, fall;
    float sizeJump, heightStartJump, timeZeroGrav, timeStartZG;
    BasicMovement mov;
    Vector2 jumpDir;

    [SerializeField]
    float jumpSpeed;

    void Awake()
    {
        mov = gameObject.GetComponent<BasicMovement>();
    }

	// Use this for initialization
	void Start () {
        jumping = false;
        topJump = false;
        sizeJump = 4.0f * mov.getHeight();
        timeZeroGrav = 1.0f;
        jumpDir = Vector2.zero;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        isGrounded = mov.getIsGroundedState();
        float axisTrigger = Input.GetAxis("TriggersL_0");

        if (isGrounded)
            fall = false;

        if((Input.GetButton("LB_0") || axisTrigger > 0.03) && isGrounded && !jumping)
        {
            jumping = true;
            mov.modifyGravity(0.0f);
            heightStartJump = transform.position.y;
        }

        if(jumping && !fall)
        {
            if (transform.position.y < heightStartJump + sizeJump)
                jumpDir.y = jumpSpeed * Time.deltaTime;
            else
            {
                if (!topJump)
                {
                    timeStartZG = Time.time;
                    topJump = true;
                }

                if (timeStartZG + timeZeroGrav > Time.time)
                    jumpDir = Vector2.zero;
                else
                {
                    mov.modifyGravity(1.0f);
                    fall = true;
                }
            }
        }

        if(Input.GetButtonUp("LB_0") || axisTrigger < 0.03)
        {
            jumpDir = Vector2.zero;
            topJump = false;
            jumping = false;
            mov.modifyGravity(1.0f);
        }

        transform.Translate(jumpDir);
	}
}
