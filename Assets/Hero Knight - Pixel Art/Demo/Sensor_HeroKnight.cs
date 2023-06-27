using UnityEngine;
using System.Collections;

public class Sensor_HeroKnight : MonoBehaviour {


    private float m_DisableTimer;

    public LayerMask groundLayer;

    public float groundCheckRadius = 0.1f;

    private void OnEnable()
    {
    }

    public bool State()
    {
        if (m_DisableTimer > 0)
            return false;
        return
            Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayer);
    }

    void Update()
    {
        m_DisableTimer -= Time.deltaTime;
    }

    public void Disable(float duration)
    {
        m_DisableTimer = duration;
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.DrawWireSphere(transform.position, groundCheckRadius);
    }
}
