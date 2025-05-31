using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target to Follow")]
    [SerializeField] private Transform player; // Посилання на гравця

    [Header("Camera Settings")]
    [SerializeField] private float zOffset = -10f; // Зміщення камери по осі Z відносно гравця
    [SerializeField] private float smoothSpeed = 500f; // Швидкість плавного переходу
    
    /// Використовується для оновлення позиції камери після того, як усі об'єкти переміщені (гарантує плавність).
    private void LateUpdate()
    {
        if (player != null)
        {
            // Цільова позиція — поточна позиція, але з новим значенням Z (гравець + відступ)
            Vector3 targetPosition = transform.position;
            targetPosition.z = player.position.z + zOffset;

            // Плавне переміщення камери до цільової позиції
            Vector3 currentPosition = transform.position;
            currentPosition.z = Mathf.Lerp(currentPosition.z, targetPosition.z, smoothSpeed * Time.deltaTime);

            transform.position = currentPosition;
        }
    }
}