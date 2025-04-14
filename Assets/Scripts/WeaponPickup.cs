using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weaponPrefab;
    private Transform player;
    private PlayerInventory inventory;
    public float pickupRange = 1f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        inventory = player.GetComponent<PlayerInventory>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= pickupRange && Input.GetKeyDown(KeyCode.E))
        {
            Pickup();
        }
    }

    void Pickup()
    {
        if (weaponPrefab != null && inventory != null)
        {
            inventory.AddWeapon(weaponPrefab);
            Destroy(gameObject); // Уничтожаем объект оружия на земле
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
