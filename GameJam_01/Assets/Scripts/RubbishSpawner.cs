using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbishSpawner : MonoBehaviour
{

    [SerializeField]
    [Tooltip("How many seconds between each burst")]
    private float spawnRate = 30;

    [SerializeField]
    private int quantitySpawned = 10;

    [SerializeField]
    private GameObject[] RubbishPrefabs;

    private void Start()
    {
        InvokeRepeating("SpawnRubbish", 0.0f, spawnRate);
    }

    public void SpawnRubbish()
    {
        for (int i = 0; i < quantitySpawned; i++)
        {
            Rigidbody rubbish = Instantiate(RubbishPrefabs[Random.Range(0, RubbishPrefabs.Length)], transform.position + Random.onUnitSphere * Random.Range(0, 3), Quaternion.identity).GetComponent<Rigidbody>();

            rubbish.AddForce(Random.onUnitSphere * 15.0f * Random.Range(0, 1), ForceMode.Impulse);
        }
    }
}