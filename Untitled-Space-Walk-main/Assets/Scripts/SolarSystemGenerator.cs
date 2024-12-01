using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SolarSystemGenerator : MonoBehaviour
{
    public GameObject[] planetPrefabs; 
    public GameObject portalPrefab;    
    public int minPlanets = 5;         
    public int maxPlanets = 15;       
    public float minDistance = 400f;  
    public float maxDistance = 600f;  
    public Vector3 startPosition = Vector3.zero; 
    public Vector3 lineDirection = Vector3.right;
    public float verticalOffset = 50f;           
    public float horizontalOffset = 50f;       

    private Vector3 currentPosition; 
    private List<GameObject> spawnedObjects = new List<GameObject>(); 

    void Start()
    {
        GeneratePlanetsInLine(); 
    }

    public void GeneratePlanetsInLine()
    {
        currentPosition = startPosition + lineDirection.normalized * minDistance;

        GameObject lastPlanet = null;

        int planetCount = Random.Range(minPlanets, maxPlanets + 1);
        Debug.Log($"Generating {planetCount} planets.");

        for (int i = 0; i < planetCount; i++)
        {
    
            GameObject planetPrefab = planetPrefabs[Random.Range(0, planetPrefabs.Length)];

            GameObject planet = Instantiate(planetPrefab);

      
            Vector3 randomOffset = new Vector3(
                Random.Range(-horizontalOffset, horizontalOffset),
                Random.Range(-verticalOffset, verticalOffset),
                0
            );
            planet.transform.position = currentPosition + randomOffset;

     
            float scale = Random.Range(7000f, 11000f);
            planet.transform.localScale = Vector3.one * scale;

            //add planet to the list of spawned objects
            spawnedObjects.Add(planet);

            //save the last planet
            if (i == planetCount - 1)
            {
                lastPlanet = planet;
            }

            //increment position for the next planet
            float distance = Random.Range(minDistance, maxDistance);
            currentPosition += lineDirection.normalized * distance;
        }

        //spawn a portal on the last planet
        if (lastPlanet != null && portalPrefab != null)
        {
            GameObject portal = Instantiate(portalPrefab, lastPlanet.transform.position, Quaternion.identity);
            portal.transform.SetParent(lastPlanet.transform); 
            spawnedObjects.Add(portal);
            Debug.Log("Portal spawned at the last planet.");
        }
    }

    public void ClearPreviousSystem()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        spawnedObjects.Clear();
        Debug.Log("Previous solar system cleared.");
    }

    public void RefreshScene()
    {
        ClearPreviousSystem(); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
}
