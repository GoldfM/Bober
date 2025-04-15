 using UnityEngine;

 public class Enemy : MonoBehaviour
 {
     public float speed = 1f;
     public int health = 1;
     public int damage = 1;
     private Rigidbody2D rb;

     

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
