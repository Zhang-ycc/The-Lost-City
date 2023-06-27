using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public int damage;

    private Animator m_animator;

    public AudioSource damageAudio;
    public AudioSource attackAudio;

    [Space]
    public float flashTime;

    private SpriteRenderer sr;
    private Color originalColor;

    public bool isDie = false;
    private float deathTime;

    // Start is called before the first frame update
    protected void Start()
    {
        m_animator = GetComponent<Animator>();

        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;

        var clips = m_animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (clip.name == "Death")
            {
                deathTime = clip.length;
            }
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        if (health <= 0 && !isDie)
        { 
            isDie = true;
            m_animator.SetTrigger("die");

            if (gameObject.layer != LayerMask.NameToLayer("Box"))
            {
                Destroy(gameObject, deathTime);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        damageAudio.Play();
        FlashColor(flashTime);
    }

    void FlashColor(float time)
    {
        sr.color = Color.red;
        Invoke("ResetColor", time);
    }

    void ResetColor()
    {
        sr.color = originalColor;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && !isDie)
        {
            m_animator.SetBool("attack", true);
            attackAudio.Play();
            collision.gameObject.GetComponent<HeroKnight>().TakeDamage(damage);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && !isDie)
        {
            m_animator.SetBool("attack", false);
        }
    }
}
