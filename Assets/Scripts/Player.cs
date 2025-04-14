using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1f;
     public int health = 10;

     private Rigidbody2D rb;

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
}
