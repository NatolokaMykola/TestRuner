using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Панель з елементами інтерфейсу під час гри
    public GameObject GamePanel;

    // Панель паузи
    public GameObject PausePanel;
    
    /// Увімкнення режиму паузи: ховає ігрову панель, показує панель паузи і зупиняє гру.
    public void PauseOnHandler()
    {
        GamePanel.SetActive(false);     // Приховати ігровий інтерфейс
        PausePanel.SetActive(true);     // Показати меню паузи
        Time.timeScale = 0f;            // Зупинити час у грі
    }
    
    /// Перезапуск поточного рівня.
    public void RestartGameHandler()
    {
        Scene currentScene = SceneManager.GetActiveScene();       // Отримати поточну сцену
        SceneManager.LoadScene(currentScene.name);                // Перезавантажити її
        Time.timeScale = 1f;                                      // Відновити нормальний хід часу
    }

    
    /// Повернення до головного меню (сцена з індексом 0).
    public void MenuGameHandler()
    {
        SceneManager.LoadScene(0);      // Завантажити головне меню
        Time.timeScale = 1f;            // Відновити час (на випадок, якщо гра була на паузі)
    }
    
    /// Запуск гри з першого рівня (сцена з індексом 1).
    public void StartGameHandler()
    {
        SceneManager.LoadScene(1);      // Завантажити перший рівень гри
    }
    
    /// Вихід із гри 
    public void ExitGameHandler()
    {
        Application.Quit();             // Закрити застосунок
    }

    
    /// Вихід із паузи:
    public void PauseOfHandler()
    {
        GamePanel.SetActive(true);      // Показати ігровий інтерфейс
        PausePanel.SetActive(false);    // Сховати меню паузи
        Time.timeScale = 1f;            // Продовжити гру
    }
}
