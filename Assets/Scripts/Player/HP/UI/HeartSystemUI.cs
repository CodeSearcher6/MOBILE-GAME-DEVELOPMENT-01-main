using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPDisplay : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI HPCountText;

    [Header("HP Settings")]
    public int maxHP = 5;
    public int currentHP;

    void Start()
    {
        currentHP = maxHP;
        UpdateHPText();
    }

    void Update()
    {
        // Для демонстрації: натисни клавішу H щоб втратити 1 HP
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHP = Mathf.Clamp(currentHP - amount, 0, maxHP);
        UpdateHPText();
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Clamp(currentHP + amount, 0, maxHP);
        UpdateHPText();
    }

    void UpdateHPText()
    {
        HPCountText.text = $"HP: {currentHP}";
    }
}
