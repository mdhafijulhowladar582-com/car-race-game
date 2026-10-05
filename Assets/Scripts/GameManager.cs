using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int startingHealth = 3;

    public int Coins { get; private set; }
    public int Health { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool RaceStarted { get; private set; }
    public bool RaceFinished { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Coins = 0;
        Health = Mathf.Max(1, startingHealth);
        IsGameOver = false;
        RaceStarted = false;
        RaceFinished = false;
    }

    public void StartRace()
    {
        if (IsGameOver || RaceFinished)
            return;

        RaceStarted = true;
        IsGameOver = false;
        MobileInput.ResetInput();
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0 || IsGameOver || RaceFinished)
            return;

        Coins += amount;
    }

    public void UpdateHealth(int currentHealth)
    {
        Health = Mathf.Max(0, currentHealth);

        if (Health <= 0)
            GameOver();
    }

    public void GameOver()
    {
        if (IsGameOver || RaceFinished)
            return;

        IsGameOver = true;
        RaceStarted = false;
        AudioManager.GetOrCreate().StopEngine();
        MobileInput.ResetInput();
    }

    public void FinishRace()
    {
        if (!RaceStarted || IsGameOver || RaceFinished)
            return;

        RaceFinished = true;
        RaceStarted = false;
        AudioManager.GetOrCreate().StopEngine();
        MobileInput.ResetInput();
    }

    public void RestartRace()
    {
        MobileInput.ResetInput();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
