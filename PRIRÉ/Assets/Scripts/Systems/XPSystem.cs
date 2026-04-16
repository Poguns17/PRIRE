using System;
using UnityEngine;

public class XPSystem : MonoBehaviour
{
    public static XPSystem Instance { get; private set; }

    [Header("Level Settings")]
    [SerializeField] private int baseXPThreshold = 100;
    [SerializeField] private float levelScaling = 1.5f;

    public event Action<int, int, int> OnXPChanged;
    public event Action<int> OnLevelUp;

    private int totalXP = 0;
    private int currentLevel = 1;
    private int xpToNextLevel;

    public int TotalXP => totalXP;
    public int CurrentLevel => currentLevel;
    public int XPToNextLevel => xpToNextLevel;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        xpToNextLevel = baseXPThreshold;
        OnXPChanged?.Invoke(totalXP, xpToNextLevel, currentLevel);
    }

    public void AddXP(int amount)
    {
        if (amount <= 0) return;
        totalXP += amount;
        Debug.Log($"[XP] +{amount} XP. Total: {totalXP} | Level: {currentLevel}");
        CheckLevelUp();
        OnXPChanged?.Invoke(totalXP, xpToNextLevel, currentLevel);
    }

    public bool SpendXP(int amount)
    {
        if (amount <= 0) return false;
        if (totalXP < amount) { Debug.Log($"[XP] Not enough XP. Have {totalXP}, need {amount}."); return false; }
        totalXP -= amount;
        OnXPChanged?.Invoke(totalXP, xpToNextLevel, currentLevel);
        Debug.Log($"[XP] Spent {amount}. Remaining: {totalXP}");
        return true;
    }

    public bool CanAfford(int amount) => totalXP >= amount;

    private void CheckLevelUp()
    {
        while (totalXP >= xpToNextLevel)
        {
            currentLevel++;
            xpToNextLevel = Mathf.RoundToInt(baseXPThreshold * Mathf.Pow(levelScaling, currentLevel - 1));
            Debug.Log($"[XP] Level up! Now level {currentLevel}. Next level at {xpToNextLevel} XP.");
            OnLevelUp?.Invoke(currentLevel);
        }
    }
}
