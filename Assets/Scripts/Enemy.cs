 using UnityEngine;

 public class Enemy : MonoBehaviour
 {
     public float speed = 1f;
     public int health = 1;
     public int damage = 1;
     public Transform player;
     private Rigidbody2D rb;

     void Start()
     {
         player = GameObject.FindGameObjectWithTag("Player").transform;
         rb = GetComponent<Rigidbody2D>();
     }

     void Update()
     {
         if (player != null)
         {
             Vector2 direction = (player.position - transform.position).normalized;
             rb.linearVelocity = direction * speed;
         }
     }

     public void TakeDamage(int damage)
     {
         health -= damage;
         if (health <= 0)
         {
             Destroy(gameObject);
         }
     }

     void OnCollisionEnter2D(Collision2D collision)
     {
         if (collision.gameObject.CompareTag("Player"))
         {
             collision.gameObject.GetComponent<Player>().TakeDamage(damage);
             //Destroy(gameObject);
         }
     }
 }
