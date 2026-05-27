using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 50;

    public int currentHealth { get; private set; }
    public bool isDead { get; private set; }

    // Subscribe to these from UI, score, VFX, etc.
    public event Action<int, int> OnHealthChanged;  // (current, max)
    public event Action OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0); // Ensure health doesn't go negative

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
