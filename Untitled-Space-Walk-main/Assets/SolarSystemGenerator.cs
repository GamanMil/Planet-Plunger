using UnityEngine;

public class SolarSystemGenerator : MonoBehaviour
{
    public GameObject[] planetPrefabs; 
    public int minPlanets = 5;         
    public int maxPlanets = 15;        
    public float minDistance = 400f;   
    public float maxDistance = 600f;   
    public Vector3 startPosition = Vector3.zero; 
    public Vector3 lineDirection = Vector3.right; 
    public float verticalOffset = 50f; 
    public float horizontalOffset = 50f;
    void Start()
    {
        GeneratePlanetsInLine();
    }

    void GeneratePlanetsInLine()
    {
        int planetCount = Random.Range(minPlanets, maxPlanets + 1);

        Vector3 currentPosition = startPosition;

        for (int i = 0; i < planetCount; i++)
        {
            // Choose a random planet prefab
            GameObject planetPrefab = planetPrefabs[Random.Range(0, planetPrefabs.Length)];

            // Instantiate the planet
            GameObject planet = Instantiate(planetPrefab);

            // Randomize position with slight vertical variation
            Vector3 verticalVariation = new Vector3(Random.Range(-horizontalOffset, horizontalOffset), Random.Range(-verticalOffset, verticalOffset), 0);
            planet.transform.position = currentPosition + verticalVariation;

            // Randomize scale for variety
            float scale = Random.Range(7000f, 11000f); // Adjust scale if needed
            planet.transform.localScale = Vector3.one * scale;

            // Increment position along the line direction with random spacing
            float distance = Random.Range(minDistance, maxDistance);
            currentPosition += lineDirection.normalized * distance + new Vector3(0, Mathf.Sin(i * 0.5f) * 50f, 0);


        }
    }
}
