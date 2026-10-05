using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RaceHUD : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private Button startButton;
    [SerializeField] private Button winRestartButton;

    private bool raceStarted;

    private void Start()
    {
        raceStarted = false;

        if (startPanel != null)
            startPanel.SetActive(true);

        if (winPanel != null)
            winPanel.SetActive(false);

        if (startButton != null)
            startButton.onClick.AddListener(StartRace);

        if (winRestartButton != null)
            winRestartButton.onClick.AddListener(RestartRace);
    }

    private void Update()
    {
        if (GameManager.Instance == null)
            return;

        if (GameManager.Instance.RaceFinished && winPanel != null)
            winPanel.SetActive(true);
    }

    public void StartRace()
    {
        raceStarted = true;

        if (startPanel != null)
            startPanel.SetActive(false);
    }

    public void RestartRace()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
