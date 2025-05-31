using System.Collections;
using UnityEngine;

public class PedestrianSpawner : MonoBehaviour
{
    private LineRenderer road;

    private Vector3 spawnPoint;
    public float minSpawnDuration = 1;
    public float maxSpawnDuration = 2;
    public GameObject pedestrian;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        road = GetComponent<LineRenderer>();
        spawnPoint = road.GetPosition(0);

        StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnDuration, maxSpawnDuration));
            Pedestrian newPedestrian = Instantiate(pedestrian, spawnPoint, Quaternion.identity).GetComponent<Pedestrian>();
            newPedestrian.currentRoad = road;
        }
    }
}
