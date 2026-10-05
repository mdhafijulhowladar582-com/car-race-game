using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int Coins { get; private set; }
    public int Health { get; private set; } = 3;
    public bool IsGameOver { get; private set; }
    public bool RaceFinished { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddCoins(int amount)
    {
        if (IsGameOver || RaceFinished)
            return;

        Coins += Mathf.Max(0, amount);
    }

    public void UpdateHealth(int currentHealth)
    {
        Health = Mathf.Max(0, currentHealth);
    }

    public void GameOver()
    {
        if (RaceFinished)
            return;

        IsGameOver = true;
        Debug.Log("GAME OVER");
    }

    public void FinishRace()
    {
        if (IsGameOver)
            return;

        RaceFinished = true;
        Debug.Log("RACE FINISHED");
    }

    public void RestartRace()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }
}
