using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] float      m_rollForce = 6.0f;
    [SerializeField] bool       m_noBlood = false;
    [SerializeField] GameObject m_slideDust;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    //传感器
    private Sensor_HeroKnight   m_groundSensor_1;
    private Sensor_HeroKnight   m_groundSensor_2;
    private Sensor_HeroKnight   m_groundSensor_3;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;

    private bool                m_isWallSliding = false;
    private bool                m_grounded = false;
    private bool                m_rolling = false;
    private int                 m_facingDirection = 1;
    private float               m_delayToIdle = 0.0f;
    private float               m_rollDuration = 16.0f / 14.0f;
    private float               m_rollCurrentTime;

    private bool isDie = false;

    [Space]

    //二段跳
    public int                  maxJumpCount = 1;
    private int                 jumpCount = 0;
    //二段跳系数
    public float                jumpBouce = 0.7f;

    [Space]
    public Collider2D DisColl;

    [Space]
    public AudioSource jumpAudio;
    public AudioSource landAudio;
    public AudioSource hurtAudio;
    public AudioSource dieAudio;


    // Use this for initialization
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor_1 = transform.Find("GroundSensor_1").GetComponent<Sensor_HeroKnight>();
        m_groundSensor_2 = transform.Find("GroundSensor_2").GetComponent<Sensor_HeroKnight>();
        m_groundSensor_3 = transform.Find("GroundSensor_3").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (!isDie)
        {
            movement();
        }
    }

    void movement()
    {

        // Increase timer that checks roll duration
        if (m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if (m_rollCurrentTime > m_rollDuration)
        {
            m_rollCurrentTime = 0f;
            m_rolling = false;
            DisColl.enabled = true;
        }

        //Check if character just landed on the ground
        if (!m_grounded && (m_groundSensor_1.State() || m_groundSensor_2.State() || m_groundSensor_3.State()))
        {
            landAudio.Play();
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
            jumpCount = 0;
        }

        //Check if character just started falling
        if (m_grounded && !(m_groundSensor_1.State() || m_groundSensor_2.State() || m_groundSensor_3.State()))
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }

        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        // Move
        if (!m_rolling)
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --
        //Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);


        // Roll
        if (Input.GetButtonDown("Roll") && !m_rolling && !m_isWallSliding)
        {
            DisColl.enabled = false;
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
        }


        //Jump
        else if (Input.GetButtonDown("Jump") && !m_rolling)
        {
            if (m_grounded)
            {
                jumpAudio.Play();
                m_animator.SetTrigger("Jump");
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                m_groundSensor_1.Disable(0.2f);
                m_groundSensor_2.Disable(0.2f);
                m_groundSensor_3.Disable(0.2f);
            }
            // 如果玩家不在地面上，但还有二段跳次数，可以进行二段跳
            else if (jumpCount < maxJumpCount)
            {
                jumpAudio.Play();
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce * jumpBouce);
                m_groundSensor_1.Disable(0.2f);
                m_groundSensor_2.Disable(0.2f);
                m_groundSensor_3.Disable(0.2f);
                jumpCount++;
            }
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }

    void Restart()
    {
        GameManager.Drop();
    }

    public void TakeDamage(int damage)
    {
        if (!isDie)
        {
            HealthSystem.Instance.TakeDamage(damage);

            hurtAudio.Play();
            m_animator.SetTrigger("Hurt");
        }
    }

    public void Die()
    {
        m_animator.SetBool("noBlood", m_noBlood);
        m_animator.SetTrigger("Death");
        isDie = true;
        dieAudio.Play();
        Invoke("GameOver", 1f);
    }

    void GameOver()
    {
        GameManager.GameOver(isDie);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("spike")){
            TakeDamage(2);
        }
        if (collision.gameObject.CompareTag("deadline"))
        {
            dieAudio.Play();
            TakeDamage(5);
            if (!isDie)
            {
                Invoke("Restart", 0.5f);
            }
        }
    }
}
