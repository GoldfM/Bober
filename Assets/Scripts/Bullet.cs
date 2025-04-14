 using UnityEngine;

 public class Bullet : MonoBehaviour
 {
     public float speed = 10f;
     public float lifeTime = 2f;
     public int damage = 1;
     private Rigidbody2D rb;

     void Start()
     {
         rb = GetComponent<Rigidbody2D>();
         rb.linearVelocity = transform.right * speed;
         Destroy(gameObject, lifeTime);
     }

      void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

 }
