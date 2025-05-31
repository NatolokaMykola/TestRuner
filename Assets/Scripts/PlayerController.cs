using System.Collections.Generic;
using UnityEngine;

namespace Player.Controll
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float jumpForce = 4f;
        [SerializeField] private float minJumpInterval = 0.25f;

        [Header("Components")]
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody rigidBody;

        [Header("UI Panels")]
        public GameObject GameOverPanel;
        public GameObject GameFinishPanel;
        public GameObject GamePanel;

        [Header("Movement Limits")]
        public float minX = 0f;
        public float maxX = 6f;

        private float moveDirection = 0f;
        private float jumpTimeStamp = 0f;
        private bool jumpInput = false;
        private bool isGrounded = false;
        private List<Collider> collisions = new List<Collider>();
        
        /// Ініціалізація компонентів Animator і Rigidbody, якщо не задані вручну.
        private void Awake()
        {
            if (!animator) animator = GetComponent<Animator>();
            if (!rigidBody) rigidBody = GetComponent<Rigidbody>();
        }
        
        /// Основна логіка руху, керування та стрибків.
        private void Update()
        {
            HandleForwardMovement();     // Рух вперед
            HandleHorizontalInput();     // Керування вліво/вправо
            HandleAnimation();           // Анімація
            JumpingAndLanding();         // Обробка стрибка
        }
        
        /// Оновлення фізики — анімація "на землі", скидання прапора стрибка.
        private void FixedUpdate()
        {
            animator.SetBool("Grounded", isGrounded);
            jumpInput = false;
        }
        
        /// Метод, що викликається з UI для виконання стрибка.
        public void JumpHandler()
        {
            if (isGrounded && !jumpInput)
            {
                jumpInput = true;
            }
        }
        
        /// Рух гравця вперед автоматично.
        private void HandleForwardMovement()
        {
            float moveDistance = moveSpeed * Time.deltaTime;
            transform.position += new Vector3(0f, 0f, moveDistance);
            moveDirection = moveDistance / Time.deltaTime;
        }
        
        /// Горизонтальне керування гравцем (A/D або ←/→).
        private void HandleHorizontalInput()
        {
            float moveInput = 0f;

            if (Input.GetKey(KeyCode.A)) moveInput = -1f;
            else if (Input.GetKey(KeyCode.D)) moveInput = 1f;

            float newX = transform.position.x + moveInput * moveSpeed * Time.deltaTime;
            newX = Mathf.Clamp(newX, minX, maxX);

            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
        
        /// Оновлення параметрів анімації руху.
        private void HandleAnimation()
        {
            animator.SetFloat("MoveSpeed", moveDirection);
        }
        
        /// Виконує стрибок, якщо гравець на землі і минула пауза між стрибками.
        private void JumpingAndLanding()
        {
            bool jumpCooldownOver = (Time.time - jumpTimeStamp) >= minJumpInterval;

            if (jumpCooldownOver && isGrounded && jumpInput)
            {
                jumpTimeStamp = Time.time;
                rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
        
        /// Обробка зіткнення з підлогою (визначення "на землі").
        private void OnCollisionEnter(Collision collision)
        {
            CheckGroundedState(collision);
        }
        
        /// Продовження контакту з підлогою.
        private void OnCollisionStay(Collision collision)
        {
            CheckGroundedState(collision);
        }
        
        /// Завершення контакту з підлогою.
        private void OnCollisionExit(Collision collision)
        {
            if (collisions.Contains(collision.collider))
                collisions.Remove(collision.collider);

            if (collisions.Count == 0)
                isGrounded = false;
        }
        
        /// Перевірка чи є колайдер валідною поверхнею для стояння.
        private void CheckGroundedState(Collision collision)
        {
            ContactPoint[] contactPoints = collision.contacts;
            bool validSurfaceNormal = false;

            for (int i = 0; i < contactPoints.Length; i++)
            {
                if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
                {
                    validSurfaceNormal = true;
                    break;
                }
            }

            if (validSurfaceNormal)
            {
                isGrounded = true;
                if (!collisions.Contains(collision.collider))
                    collisions.Add(collision.collider);
            }
            else
            {
                if (collisions.Contains(collision.collider))
                    collisions.Remove(collision.collider);
                if (collisions.Count == 0)
                    isGrounded = false;
            }
        }
        
        /// Обробка входу в тригери — кінець рівня або смерть.
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                GameOverPanel.SetActive(true);
                GamePanel.SetActive(false);
                Time.timeScale = 0f;
            }
            else if (other.CompareTag("Finish"))
            {
                GameFinishPanel.SetActive(true);
                GamePanel.SetActive(false);
                Time.timeScale = 0f;
            }
        }
    }
}
