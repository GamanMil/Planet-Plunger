using UnityEngine;

public class SolarSystemGenerator : MonoBehaviour
{
    public GameObject[] planetPrefabs; // Array of planet assets
    public GameObject portalPrefab;    // Portal prefab
    public int minPlanets = 5;         // Minimum number of planets
    public int maxPlanets = 15;        // Maximum number of planets
    public float minDistance = 400f;  // Minimum distance between planets
    public float maxDistance = 600f;  // Maximum distance between planets
    public Vector3 startPosition = Vector3.zero; // Starting position for the solar system
    public Vector3 lineDirection = Vector3.right; // Direction of planet alignment
    public float verticalOffset = 50f;            // Max vertical offset for randomness
    public float horizontalOffset = 50f;          // Max horizontal offset for randomness

    private Vector3 currentPosition; // Tracks the position of the next planet

    void Start()
    {
        Debug.Log("Starting planet generation...");
        GeneratePlanetsInLine(); // Generate planets at the start
    }

    public void GeneratePlanetsInLine()
    {
        Debug.Log("Generating planets...");
        if (planetPrefabs.Length == 0)
        {
            Debug.LogWarning("No planet prefabs assigned!");
            return;
        }

        int planetCount = Random.Range(minPlanets, maxPlanets + 1);
        Debug.Log($"Planet count: {planetCount}");

        // Initialize position for the first planet
        currentPosition = startPosition + lineDirection.normalized * minDistance;

        GameObject lastPlanet = null;

        for (int i = 0; i < planetCount; i++)
        {
            // Choose a random planet prefab
            GameObject planetPrefab = planetPrefabs[Random.Range(0, planetPrefabs.Length)];

            // Instantiate the planet
            GameObject planet = Instantiate(planetPrefab);
            Debug.Log($"Planet {i + 1}/{planetCount} spawned at {currentPosition}");

            // Add random vertical and horizontal offsets
            Vector3 randomOffset = new Vector3(
                Random.Range(-horizontalOffset, horizontalOffset),
                Random.Range(-verticalOffset, verticalOffset),
                0
            );
            planet.transform.position = currentPosition + randomOffset;

            // Randomize the scale
            float scale = Random.Range(7000f, 11000f);
            planet.transform.localScale = Vector3.one * scale;

            // Save the last planet
            if (i == planetCount - 1)
            {
                lastPlanet = planet;
            }

            // Increment position for the next planet
            float distance = Random.Range(minDistance, maxDistance);
            currentPosition += lineDirection.normalized * distance;
        }

        // Add a portal to the last planet
        if (lastPlanet != null && portalPrefab != null)
        {
            GameObject portal = Instantiate(portalPrefab, lastPlanet.transform.position, Quaternion.identity);
            portal.transform.SetParent(lastPlanet.transform); // Attach portal to the last planet
            Debug.Log("Portal spawned at the last planet.");
        }
    }
}
