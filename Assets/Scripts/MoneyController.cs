using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyController : MonoBehaviour
{
    // Загальна кількість монет (глобальна, зберігається між сесіями)
    public static float Coin;

    // Кількість монет, зібраних на поточному рівні
    public static int LevelCoinCount;

    // UI елементи для відображення монет
    public TextMeshProUGUI CoinText;
    public TextMeshProUGUI LevelCoinText;
    public TextMeshProUGUI LevelCoinFinishText;

    // Звук збору монети
    public AudioSource CoinSelect;

    // Викликається при старті сцени
    public void Start()
    {
        // Якщо монети вже зберігались, завантажити їх із PlayerPrefs
        if (PlayerPrefs.HasKey("Coin"))
        {
            Coin = PlayerPrefs.GetFloat("Coin");
        }

        // Скинути лічильник монет поточного рівня
        LevelCoinCount = 0;

        // Оновити UI
        UpdateUI();
    }

    // Оновлення кожен кадр (може бути оптимізовано — див. нижче)
    public void Update()
    {
        UpdateUI(); // Необов'язково оновлювати щокадрово, якщо значення не змінюються
    }

    // Викликається, коли об'єкт з тригером входить у колайдер
    public void OnTriggerEnter(Collider other)
    {
        // Якщо зіткнення з об'єктом, який має тег "Coin"
        if (other.CompareTag("Coin"))
        {
            Coin += 1f;                // Збільшити загальну кількість монет
            LevelCoinCount += 1;      // Збільшити кількість монет на рівні

            CoinSelect.Play();        // Відтворити звук збору монети

            ScoreController.scoreCounter += 10; // Додати очки до рахунку

            PlayerPrefs.SetFloat("Coin", Coin); // Зберегти монети

            MoneyDelete(other.gameObject);      // Приховати зібрану монету

            UpdateUI();             // Оновити інтерфейс
        }
    }

    // Метод приховування монети після збору
    public void MoneyDelete(GameObject moneyObject)
    {
        moneyObject.SetActive(false); // Просто вимикає об’єкт
    }

    // Оновлення тексту UI
    private void UpdateUI()
    {
        CoinText.text = Coin.ToString();                          // Загальна кількість монет
        LevelCoinText.text = "Money: " + LevelCoinCount.ToString();       // Поточний рахунок у грі
        LevelCoinFinishText.text = "Money: " + LevelCoinCount.ToString(); // Підсумок рівня
    }
}
