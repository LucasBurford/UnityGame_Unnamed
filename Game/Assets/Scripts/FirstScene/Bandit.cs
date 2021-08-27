using UnityEngine;
using System.Collections;

public class Bandit : MonoBehaviour {

    public GameManager gameManager;

    public Camera cam;

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_Bandit       m_groundSensor;
    private bool                m_grounded = false;
    private bool                m_combatIdle = false;
    private bool                m_isDead = false;

    // 1 = left, -1 = right
    public int facingDirection;

    public Vector3 mousePos;

    // Use this for initialization
    void Start () {

        facingDirection = 1;

        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
    }
	
	// Update is called once per frame
	void Update () {

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x > transform.position.x)
        {
            facingDirection = -1;
        }
        else if (mousePos.x < transform.position.x)
        {
            facingDirection = 1;
        }

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State()) {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if(m_grounded && !m_groundSensor.State()) {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            facingDirection = -1;
        }

        else if (inputX < 0)
        {
            facingDirection = 1;
        }
            
        if (facingDirection == -1)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else if (facingDirection == 1)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);

        // -- Handle Animations --
        //Death
        if (Input.GetKeyDown("'")) {
            if(!m_isDead)
                m_animator.SetTrigger("Death");
            else
                m_animator.SetTrigger("Recover");

            m_isDead = !m_isDead;
        }
            
        //Hurt
        else if (Input.GetKeyDown("q"))
            m_animator.SetTrigger("Hurt");

        //Change between idle and combat idle
        else if (Input.GetKeyDown("f"))
            m_combatIdle = !m_combatIdle;

        //Jump
        else if (Input.GetKeyDown("space") && m_grounded) {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon && FindObjectOfType<PlayerMovement>().CanMove)
            m_animator.SetInteger("AnimState", 2);

        else if (Mathf.Abs(inputY) > Mathf.Epsilon)
        {
            m_animator.SetInteger("AnimState", 2);
        }

        //Combat Idle
        else if (m_combatIdle)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);
    }

    public void AttackAnim()
    {
        m_animator.SetTrigger("Attack");
    }
}
