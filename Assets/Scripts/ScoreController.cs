using System.Collections;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI score;
    public TextMeshProUGUI highScore;

    [Header("Score Settings")]
    public static int scoreCounter, highScoreCounter;
    public float time = 0.2f; // Інтервал між автоматичним збільшенням очок

    private float timeStart;
    
    /// Завантаження рекордного рахунку з PlayerPrefs.
    private void Awake()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScoreCounter = PlayerPrefs.GetInt("HighScore");
        }
    }
    
    /// Початкове встановлення таймерів і обнулення рахунку.
    private void Start()
    {
        timeStart = time;
        scoreCounter = 0;
    }
    
    /// Оновлення рахунку кожен кадр та відображення його на UI.
    private void Update()
    {
        AddScore();

        if (score && score.gameObject.activeInHierarchy)
        {
            score.text = scoreCounter.ToString();
        }

        if (highScore && highScore.gameObject.activeInHierarchy)
        {
            highScore.text = highScoreCounter.ToString();
        }
    }
    
    /// Додає очки щотаймерно. Збільшує загальний рахунок.
    private void AddScore()
    {
        time -= Time.deltaTime;

        if (time <= 0f)
        {
            scoreCounter += 1;
            time = timeStart;

            HighScore();
        }
    }
    
    /// Оновлює рекорд, якщо поточний рахунок більший за збережений.
    private void HighScore()
    {
        if (scoreCounter > highScoreCounter)
        {
            highScoreCounter = scoreCounter;
            PlayerPrefs.SetInt("HighScore", highScoreCounter);
        }
    }
}