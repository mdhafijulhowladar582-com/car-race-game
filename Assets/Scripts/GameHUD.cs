using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private Text coinText;
    [SerializeField] private Text healthText;
    [SerializeField] private GameObject gameOverPanel;

    private void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        Refresh();
    }

    private void Update()
    {
        Refresh();

        if (GameManager.Instance != null && GameManager.Instance.IsGameOver && gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    private void Refresh()
    {
        if (GameManager.Instance == null)
            return;

        if (coinText != null)
            coinText.text = "COINS  " + GameManager.Instance.Coins;

        if (healthText != null)
            healthText.text = "HP  " + GameManager.Instance.Health + "/3";
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
