using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public WaveData[] data;

    public int currentWaveIndex; //The index of the current wave [Remember, a list starts from 0]
    public Camera referenceCamera;

    [Header("Spawner Attributes")]
    float spawnTimer; //Timer used to determine when to spawn the next enemy

    public static SpawnManager instance;

    void Start()
    {
        if(instance) Debug.LogWarning("There is more tha 1 Spawn Manager in the Scene! Please remove the extras");
        instance = this;
    }

    void Update()
    {
        // Updates the spawn timer at every frame.
        spawnTimer -= Time.deltaTime;
        if(spawnTimer <= 0) {
            // Get the array of enemies that we are spawning for this tick.
            GameObject[] spawns = data[currentWaveIndex].GetPossibleSpawns();

            // Loop through and spawn all the prefabs.
            foreach(GameObject prefab in spawns)
            {
                Instantiate(prefab, GeneratePosition(), Quaternion.identity);
            }
            
            // Regenerates the spawn timer.
            spawnTimer += data[currentWaveIndex].GetSpawnInterval();
        }
    }

    // Creates a new location where we can place the enemy at.
    public static Vector3 GeneratePosition()
    {
        // If there is no reference camera, then get one.
        if(!instance.referenceCamera) instance.referenceCamera = Camera.main;

        // Give a warning if the camera is not orthographic.
        if(!instance.referenceCamera.orthographic)
            Debug.LogWarning("The reference camera is not orthographic! This will cause enemy spawns to sometimes appear within camera boundaries!");

        // Generate a position outside of camera boundaries using 2 random numbers.
        float x = Random.Range(0f, 1f), y = Random.Range(0f, 1f);

        // Then, randomly choose whether we want to round the x or the y value.
        switch(Random.Range(0, 2)) {
            case 0: default:
                return instance.referenceCamera.ViewportToWorldPoint( new Vector3(Mathf.Round(x), y) );
            case 1:
                return instance.referenceCamera.ViewportToWorldPoint( new Vector3(x, Mathf.Round(y)) );
        }
    }
}