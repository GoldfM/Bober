 using UnityEngine;

 public class Bullet : MonoBehaviour
 {
     public float speed = 10f;
     public float lifeTime = 2f;
     private Rigidbody2D rb;

     void Start()
     {
         rb = GetComponent<Rigidbody2D>();
         rb.linearVelocity = transform.right * speed;
         Destroy(gameObject, lifeTime);
     }
 }
