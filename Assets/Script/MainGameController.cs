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
    private float currentTime;

    [Header("Spawn")]
    public Transform spawnPoint;
    public Transform decisionPoint;

    private CharacterController currentCharacter;

    [Header("Theme Prefabs")]
    public List<GameObject> valentinePrefabs;
    public List<GameObject> halloweenPrefabs;
    public List<GameObject> christmasPrefabs;
    private List<GameObject> currentTheme;

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

    private GameObject currentThemeUI;

    private int score;
    private int highScore;
    private bool isGameOver;

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
        currentTime -= Time.deltaTime;
        currentTime = Mathf.Max(currentTime, 0f);

        timeSlider.value = currentTime;

        if (currentTime <= 0.8f)
            timeFillImage.color = Color.red;
        else
            timeFillImage.color = Color.white;

        if (currentTime <= 0f)
        {
            GameOver();
        }

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    void SpawnCharacter()
    {
        if (currentCharacter != null)
            Destroy(currentCharacter.gameObject);

        GameObject prefab = GetRandomPrefab();
        GameObject obj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        currentCharacter = obj.GetComponent<CharacterController>();
        currentCharacter.MoveTo(decisionPoint.position);
    }

    public void CheckAnswer(EntityType input)
    {
        if (isGameOver || currentCharacter == null) return;

        if (input == currentCharacter.type)
        {
            SoundManager.Instance?.PlayCorrect();
            Destroy(currentCharacter.gameObject);
            currentCharacter = null;

            score++;
            AddTime(1.0f);
            SetTheme();
            UpdateUI();

            SpawnCharacter();
        }
        else
        {
            SoundManager.Instance?.PlayWrong();
            GameOver();
        }
    }

    void AddTime(float value)
    {
        currentTime = Mathf.Min(currentTime + value, maxTime);
        timeSlider.value = currentTime;
    }

    void SetTheme()
    {
        if (score >= 30)
        {
            currentTheme = christmasPrefabs;
            SwitchThemeUI(christmasUI);
        }
        else if (score >= 15)
        {
            currentTheme = halloweenPrefabs;
            SwitchThemeUI(halloweenUI);
        }
        else
        {
            currentTheme = valentinePrefabs;
            SwitchThemeUI(valentineUI);
        }
    }


    void SwitchThemeUI(GameObject newTheme)
    {
        if (currentThemeUI == newTheme) return;

        if (currentThemeUI != null)
            currentThemeUI.SetActive(false);

        currentThemeUI = newTheme;
        currentThemeUI.SetActive(true);
    }



    GameObject GetRandomPrefab()
    {
        return currentTheme[Random.Range(0, currentTheme.Count)];
    }

    void UpdateUI()
    {
        foreach (var txt in scoreText)
        {
            txt.text = $"Score : {score}";
        }

        highScoreText.text = $"Highest : {highScore}";
    }

    void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        SoundManager.Instance?.PlayWrong();
        SoundManager.Instance?.StopBGM();

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        gameOverPanel.SetActive(true);
    }
}
