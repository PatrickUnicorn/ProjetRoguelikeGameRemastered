using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public WaveData[] data;

    public int currentWaveIndex;
    public Camera referenceCamera;

    [Header("Spawner Attributes")]
    float spawnTimer;

    public static SpawnManager instance;

    void Start()
    {
        if(instance) Debug.LogWarning("Il y a plus d'un gestionnaire de spawn dans la scène ! Veuillez supprimer les extras.");
        instance = this;
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if(spawnTimer <= 0) {
            GameObject[] spawns = data[currentWaveIndex].GetPossibleSpawns();

            foreach(GameObject prefab in spawns)
            {
                Instantiate(prefab, GeneratePosition(), Quaternion.identity);
            }
            
            spawnTimer += data[currentWaveIndex].GetSpawnInterval();
        }
    }

    public static Vector3 GeneratePosition()
    {
        if(!instance.referenceCamera) instance.referenceCamera = Camera.main;

        if(!instance.referenceCamera.orthographic)
            Debug.LogWarning("La caméra de référence n'est pas orthographique ! Cela entraînera parfois l'apparition d'ennemis à l'intérieur des limites de la caméra !");

        float x = Random.Range(0f, 1f), y = Random.Range(0f, 1f);

        switch(Random.Range(0, 2)) {
            case 0: default:
                return instance.referenceCamera.ViewportToWorldPoint( new Vector3(Mathf.Round(x), y) );
            case 1:
                return instance.referenceCamera.ViewportToWorldPoint( new Vector3(x, Mathf.Round(y)) );
        }
    }
}