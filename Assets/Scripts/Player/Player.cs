using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1f;
     public int health = 10;

     private Rigidbody2D rb;
     public static Player Instance { get; private set; }

     private void Awake()
     {
         Instance = this;
     }
     void Start()
     {
         rb = GetComponent<Rigidbody2D>();
     }


     public void TakeDamage(int damage)
     {
         health -= damage;
         if (health <= 0)
         {
             Destroy(gameObject);
         }
     }

     public Vector3 GetPlayerScreenPosition()
     {
         Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
         return playerScreenPosition;
     }
}
