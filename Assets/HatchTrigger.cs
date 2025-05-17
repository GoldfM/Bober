using UnityEngine;
using UnityEngine.UI;

public class HatchTrigger : MonoBehaviour
{
    public GameObject startButton; // Ссылка на кнопку "Начать"
    private bool playerIsNear = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsNear = true;
            UpdateStartButtonVisibility();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsNear = false;
            UpdateStartButtonVisibility();
        }
    }

    private void UpdateStartButtonVisibility()
    {
        startButton.SetActive(playerIsNear);
    }
}