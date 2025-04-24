using UnityEngine;
public class NPCInteraction : MonoBehaviour
{
    public GameObject upgradePanel;
    private bool canInteract = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            upgradePanel.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
            upgradePanel.SetActive(false);
        }
    }
}
