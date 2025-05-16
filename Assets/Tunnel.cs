using UnityEngine;

public class Tunnel : MonoBehaviour
{
    public RoomClear previousRoom;
    public RoomClear nextRoom;
    public Collider2D entryTrigger;
    public Collider2D exitTrigger;
    public Collider2D cameraBorder; // Reference to the CameraBorder collider

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LevelGenerator levelGenerator = FindObjectOfType<LevelGenerator>();
            if (levelGenerator != null && cameraBorder != null)
            {
                levelGenerator.UpdateCameraConfiner(cameraBorder);
            }
        }
    }
}