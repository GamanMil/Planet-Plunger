using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    public SolarSystemGenerator solarSystemGenerator; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the portal!");
            solarSystemGenerator.RefreshScene(); 
        }
    }
}
