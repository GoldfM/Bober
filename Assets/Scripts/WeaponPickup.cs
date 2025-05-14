using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weaponPrefab;
    private PlayerInventory inventory;
    private bool canPickup = false;

    void Start()
    {
        // Assuming the PlayerInventory is on the same GameObject as the Player script
        // This is a more robust way to get the PlayerInventory
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            inventory = player.GetComponent<PlayerInventory>();
            if (inventory == null)
            {
                Debug.LogError("PlayerInventory not found on the Player GameObject!");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found with tag 'Player'!");
        }
    }

    void Update()
    {
        if (canPickup && Input.GetKeyDown(KeyCode.E))
        {
            Pickup();
        }
    }

    void Pickup()
    {
        if (weaponPrefab != null && inventory != null)
        {
            inventory.AddWeapon(weaponPrefab);
            Destroy(gameObject); // Destroy the weapon object on the ground
        }
    }

    // Use OnTriggerEnter2D/OnTriggerExit2D if your pickup is a trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canPickup = true;
            Debug.Log("Press 'E' to pick up the weapon."); // Optional feedback
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canPickup = false;
            Debug.Log("You are no longer near the weapon."); // Optional feedback
        }
    }

    //Or use OnCollisionEnter2D/OnCollisionExit2D if it's not a trigger
    /*
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canPickup = true;
            Debug.Log("Press 'E' to pick up the weapon."); // Optional feedback
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canPickup = false;
            Debug.Log("You are no longer near the weapon."); // Optional feedback
        }
    }
    */
}