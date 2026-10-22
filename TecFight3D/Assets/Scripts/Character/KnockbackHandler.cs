using UnityEngine;
using UnityEngine.UI;

public class KnockbackHandler : MonoBehaviour
{
    const float knockbackDecay = 0.6f;
    [SerializeField] private float totalHealth = 100;
    [SerializeField] private float damage = 0;
    private Image healthSlider;
    private float currentHealth;

    FighterStateMachine fsm;
    FighterECB ecb;
    Vector3 knockbackDirection;
    float knockbackStrength;
    private void Start()
    {
        fsm = GetComponent<FighterStateMachine>();
        ecb = GetComponent<FighterECB>();
    }

    private void FixedUpdate()
    {
        if(ecb && knockbackStrength > 0.001f)
        {
            //include some semblance of fsm.charobj.weight here
            ecb.Move(knockbackDirection * knockbackStrength);
            knockbackStrength *= knockbackDecay;
        }
    }

    public void SetHealthBar(Image healthBar)
    {
        healthSlider = healthBar;
        healthSlider.fillAmount = damage / totalHealth;
    }

    public void OnHit(Vector3 knockback, float deltDamage)
    {
        if (fsm.isDodging)
        {
            //damage is dodged
            return;
        }
        if (fsm.isSheilding)
        {
            //take sheild damage
            fsm.sheildHealth -= deltDamage;
            return;
        }
        damage += deltDamage;
        knockbackDirection = knockback.normalized;
        knockbackStrength = knockback.magnitude * damage;
        healthSlider.fillAmount = damage / totalHealth;
    }

    public void ResetDamage()
    {
        damage = 0;
        healthSlider.fillAmount = damage / totalHealth;
    }


    void TakeDamage(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;

            if (currentHealth < 0)
            {
                currentHealth = 0;
            }


            Debug.Log("Player Health: " + currentHealth);

            if (currentHealth == 0)
            {
                Die();
            }
        }
    }


    void Die()
    {
        Debug.Log("Player has died!");
    }
}
