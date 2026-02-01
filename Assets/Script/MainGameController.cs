using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainGameController : MonoBehaviour
{
    public static MainGameController Instance;

    public enum EntityType { Dog, Cat, Anomaly }

    [Header("Time")]
    public float maxTime = 2.5f;
    float currentTime;

    [Header("Spawn Points")]
    public Transform spawnPoint;
    public Transform decisionPoint;
    public Transform waitingPoint;
    public Transform leftPoint;
    public Transform rightPoint;
    public Transform downPoint;

    [Header("Prefabs")]
    public List<GameObject> valentinePrefabs;
    public List<GameObject> halloweenPrefabs;
    public List<GameObject> christmasPrefabs;
    List<GameObject> currentTheme;

    [Header("UI")]
    public Slider timeSlider;
    public Image timeFillImage;
    public List<TextMeshProUGUI> scoreText;
    public TextMeshProUGUI highScoreText;
    public GameObject gameOverPanel;

    [Header("Theme UI Groups")]
    public GameObject valentineUI;
    public GameObject halloweenUI;
    public GameObject christmasUI;

    CharacterController currentCharacter;
    CharacterController waitingCharacter;

    int score;
    int highScore;
    bool isGameOver;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = $"Highest : {highScore}";

        currentTime = maxTime;
        timeSlider.maxValue = maxTime;

        gameOverPanel.SetActive(false);
        SetTheme();
        SpawnCharacter();
        UpdateUI();
    }

    void Update()
    {
        if (isGameOver || currentCharacter == null) return;

        currentTime -= Time.deltaTime;
        currentTime = Mathf.Max(0, currentTime);
        timeSlider.value = currentTime;
        timeFillImage.color = currentTime <= 0.8f ? Color.red : Color.white;

        if (currentTime <= 0)
            GameOver();
    }

    void SpawnCharacter()
    {
        if (waitingCharacter != null) return;

        GameObject prefab = currentTheme[Random.Range(0, currentTheme.Count)];
        GameObject obj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        waitingCharacter = obj.GetComponent<CharacterController>();
        waitingCharacter.EnableSwipe(false);
        waitingCharacter.MoveTo(waitingPoint.position);

        TryMoveWaitingToDecision();
    }

    void TryMoveWaitingToDecision()
    {
        if (currentCharacter != null) return;
        if (waitingCharacter == null) return;

        currentCharacter = waitingCharacter;
        waitingCharacter = null;

        currentCharacter.EnableSwipe(true);
        currentCharacter.MoveTo(decisionPoint.position);
    }

    public void OnSwipe(EntityType input)
    {
        if (isGameOver) return;
        if (currentCharacter == null) return;
        if (!currentCharacter.canSwipe) return;

        Vector3 target =
            input == EntityType.Dog ? leftPoint.position :
            input == EntityType.Cat ? rightPoint.position :
            downPoint.position;

        bool isCorrect = input == currentCharacter.type;

        var character = currentCharacter;
        character.EnableSwipe(false);
        currentCharacter = null;

        character.PlaySwipe(input, target, () =>
        {
            Destroy(character.gameObject);

            if (!isCorrect)
            {
                GameOver();
                return;
            }

            score++;
            currentTime = Mathf.Min(currentTime + 1f, maxTime);

            SetTheme();
            UpdateUI();

            TryMoveWaitingToDecision();
            SpawnCharacter();
        });
    }

    void SetTheme()
    {
        int index = (score / 15) % 3;

        valentineUI.SetActive(false);
        halloweenUI.SetActive(false);
        christmasUI.SetActive(false);

        if (index == 0)
        {
            currentTheme = valentinePrefabs;
            valentineUI.SetActive(true);
        }
        else if (index == 1)
        {
            currentTheme = halloweenPrefabs;
            halloweenUI.SetActive(true);
        }
        else
        {
            currentTheme = christmasPrefabs;
            christmasUI.SetActive(true);
        }
    }

    void UpdateUI()
    {
        foreach (var t in scoreText)
            t.text = $"Score : {score}";

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            highScoreText.text = $"Highest : {highScore}";
        }
    }

    void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        gameOverPanel.SetActive(true);
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
