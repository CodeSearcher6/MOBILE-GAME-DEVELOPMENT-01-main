using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HPDisplay : MonoBehaviour
{
    [Header("UI")]

    [Header("HP Settings")]
    public int maxHP = 5;
    public int currentHP;
    [SerializeField] private PlayerHealth health;
    [SerializeField] public TextMeshProUGUI HPCountText;
    [SerializeField] public GameObject reviveScreen;


    void Start()
    {
        currentHP = maxHP;
        UpdateHPText();
    }

    private void Awake()
    {
        health.OnDamaged += UpdateHPText;
        health.OnDied += () => reviveScreen?.SetActive(true);
        health.OnRevived += () => reviveScreen?.SetActive(true);
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
