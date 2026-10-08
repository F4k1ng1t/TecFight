using System;
using UnityEngine;
using UnityEngine.UI;

public class KnockbackHandler : MonoBehaviour
{
    [Header("Variables")]
    public int totalHealth = 100;
    public int currentHealth;
   

    [Header("Componets")]
    public Slider healthSlider;


    [SerializeField]
    private float damage = 10f;

    Rigidbody rb;
    FighterStateMachine fsm;
    private void Start()
    {
        fsm = GetComponent<FighterStateMachine>();
        rb = gameObject.GetComponent<Rigidbody>();

        currentHealth = totalHealth;

        healthSlider.maxValue = totalHealth;
        healthSlider.value = currentHealth;
    }


    void Update()
    {
        healthSlider.value = currentHealth;

        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage((int)damage);
        }
    }

    public void OnHit(Vector3 knockback, float deltDamage)
    {
        if (!rb)
        {
            Debug.LogError("Rigidbody not found on KnockbackHandler");
            return;
        }
        rb.AddForce(knockback * damage, ForceMode.Impulse);
        damage += deltDamage;
        Debug.Log(damage);
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
