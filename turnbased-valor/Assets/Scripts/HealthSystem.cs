using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthSystem : MonoBehaviour
{

    public event EventHandler OnDead;
    public event EventHandler OnDamaged;
    
    [SerializeField] private int health = 100;

    private int healthMax;

    private void Awake()
    {
        healthMax = health;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        OnDamaged?.Invoke(this, EventArgs.Empty);
        
        if ( health < 0)
        {
            health = 0;
        }


        if (health == 0)
        {
            Die();
        }

        Debug.Log("Current Health: " + health);
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float)health / healthMax;
    }

    public void GetDamage(int damageAmount)
    {
        Damage(damageAmount);
    }
}
