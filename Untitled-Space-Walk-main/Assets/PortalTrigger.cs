using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management
using System.Collections;

public class PortalTrigger : MonoBehaviour
{
    public bool isEntered = false; // Tracks if the player has entered the portal
    private bool canGenerate = true; // Controls when the next generation is allowed

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the portal and generation is allowed
        if (other.CompareTag("Player") && canGenerate)
        {
            Debug.Log("Player entered the portal!");
            RefreshSceneWithCooldown(); // Wait for 10 seconds before refreshing
        }
    }

    private void RefreshSceneWithCooldown()
    {
        canGenerate = false; // Disable further interactions
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
        Debug.Log("Scene reloaded.");
        canGenerate = true; // Re-enable generation after reload
    }
}
