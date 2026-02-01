using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainGameController : MonoBehaviour
{
    public static MainGameController Instance;

    public enum EntityType { Dog, Cat, Anomaly }

    [Header("BGM")]
    public AudioClip mainBGM;

    [Header("Time")]
    public float maxTime = 2.5f;
    float currentTime;

    [Header("Spawn")]
    public Transform spawnPoint;
    public Transform decisionPoint;

    CharacterController currentCharacter;

    [Header("Swipe Points")]
    public Transform leftPoint;
    public Transform rightPoint;
    public Transform downPoint;

    [Header("Theme Prefabs")]
    public List<GameObject> valentinePrefabs;
    public List<GameObject> halloweenPrefabs;
    public List<GameObject> christmasPrefabs;
    List<GameObject> currentTheme;

    [Header("Theme UI")]
    public GameObject valentineUI;
    public GameObject halloweenUI;
    public GameObject christmasUI;
    GameObject currentThemeUI;

    [Header("UI")]
    public Slider timeSlider;
    public Image timeFillImage;
    public List<TextMeshProUGUI> scoreText;
    public TextMeshProUGUI highScoreText;
    public GameObject gameOverPanel;

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

        currentTime = maxTime;
        timeSlider.maxValue = maxTime;
        timeSlider.value = maxTime;

        gameOverPanel.SetActive(false);

        SetTheme();
        SpawnCharacter();
        UpdateUI();

        SoundManager.Instance?.PlayMainBGM(mainBGM);
    }

    void Update()
    {
        if (isGameOver) return;

        currentTime -= Time.deltaTime;
        currentTime = Mathf.Max(currentTime, 0f);
        timeSlider.value = currentTime;

        timeFillImage.color = currentTime <= 0.8f ? Color.red : Color.white;

        if (currentTime <= 0f)
            GameOver();
    }

    void SpawnCharacter()
    {
        if (currentCharacter != null)
            Destroy(currentCharacter.gameObject);

        GameObject prefab = currentTheme[Random.Range(0, currentTheme.Count)];
        GameObject obj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        currentCharacter = obj.GetComponent<CharacterController>();
        currentCharacter.Init(decisionPoint.position);
    }

    public void OnSwipe(EntityType input)
    {
        if (currentCharacter == null || isGameOver) return;

        Transform target =
            input == EntityType.Dog ? leftPoint :
            input == EntityType.Cat ? rightPoint :
            downPoint;

        currentCharacter.PlaySwipe(input, target.position);
        CheckAnswer(input);
    }

    void CheckAnswer(EntityType input)
    {
        if (input == currentCharacter.type)
        {
            score++;
            AddTime(1f);
            SetTheme();
            UpdateUI();
            SpawnCharacter();
        }
        else
        {
            GameOver();
        }
    }

    void AddTime(float v)
    {
        currentTime = Mathf.Min(currentTime + v, maxTime);
    }

    void SetTheme()
    {
        int index = (score / 15) % 3;

        if (index == 0)
        {
            currentTheme = valentinePrefabs;
            SwitchThemeUI(valentineUI);
        }
        else if (index == 1)
        {
            currentTheme = halloweenPrefabs;
            SwitchThemeUI(halloweenUI);
        }
        else
        {
            currentTheme = christmasPrefabs;
            SwitchThemeUI(christmasUI);
        }
    }

    void SwitchThemeUI(GameObject ui)
    {
        if (currentThemeUI != null)
            currentThemeUI.SetActive(false);

        currentThemeUI = ui;
        currentThemeUI.SetActive(true);
    }

    void UpdateUI()
    {
        foreach (var t in scoreText)
            t.text = $"Score : {score}";

        highScoreText.text = $"Highest : {highScore}";
    }

    void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        SoundManager.Instance?.StopBGM();
        gameOverPanel.SetActive(true);

        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
