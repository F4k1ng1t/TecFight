using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Variables")]
    public int totalHealth = 100;
    public int currentHealth;
    public int damageAmount = 10;

    [Header("Componets")]
    public Slider healthSlider;


    void Start()
    {
        currentHealth = totalHealth;

        healthSlider.maxValue = totalHealth;
        healthSlider.value = currentHealth;

    }


    void Update()
    {
        healthSlider.value = currentHealth;

        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(damageAmount);
        }
    }
    

    void TakeDamage(int damageAmount)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damageAmount;

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
