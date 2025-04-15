using UnityEngine;

public class EnemyMeleeMove : MonoBehaviour
{
    public Transform player;
     private Rigidbody2D rb;
    public float speed = 1f;
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
}
