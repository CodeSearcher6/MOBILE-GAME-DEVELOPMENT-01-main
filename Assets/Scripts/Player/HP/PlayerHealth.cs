using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [Header("HP Settings")]
    public int maxHP = 3;
    public int currentHP;

    public event Action OnDamaged;
    public event Action OnDied;
    public event Action OnRevived;

    private bool isFalling = false;
    private bool isAlive = true;

    private void Start()
    {
        currentHP = maxHP;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isAlive || isFalling) return;

        if (other.CompareTag("Obstacle"))
        {
            HandleDamage();
        }
    }

    public void HandleDamage()
    {
        OnDamaged?.Invoke();
        if (isAlive)
        {
            Death();
        }
    }

    private void Death()
    {
        isFalling = true;
        isAlive = false;
        OnDied?.Invoke();
    }

    public void Revive()
    {
        if (currentHP > 0)
        {
            currentHP--; 
            isAlive = true;
            isFalling = false;
            OnRevived?.Invoke();
        }
        else
        {
            // HP вже 0 — не можна ревайвити
            OnDied?.Invoke(); 
        }
    }



    public int GetCurrentHP() => currentHP;
}
