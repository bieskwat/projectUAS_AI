using UnityEngine;

public class VaseSpawner : MonoBehaviour
{
    public GameObject vasePrefab;
    public Transform[] spawnPoints;

    public void SpawnVase()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);

        Instantiate(
            vasePrefab,
            spawnPoints[randomIndex].position,
            Quaternion.identity);
    }
}