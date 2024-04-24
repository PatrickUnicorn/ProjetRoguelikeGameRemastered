using UnityEngine;

[CreateAssetMenu(fileName = "Wave Data", menuName = "2D Top-down Rogue-like/Wave Data")]
public class WaveData : ScriptableObject
{

    [Tooltip("A list of all possible enemies that can spawn in this wave.")]
    public GameObject[] possibleSpawnPrefabs = new GameObject[1];

    [Header("Spawn Settings")]
    [Tooltip("Time between each spawn (in seconds). Will take a random number between X and Y.")]
    public Vector2 spawnInterval = new Vector2(2, 3);

    [Tooltip("How many enemies are spawned per interval?")]
    public Vector2Int spawnsPerTick = new Vector2Int(1, 1);

    [Tooltip("How long (in seconds) this wave will spawn enemies for.")]
    [Min(0.1f)] public float waveDuration = 60;

    [Tooltip("If value is non-zero, then wave cannot exit until minimum spawns are met.")]
    [Min(0)] public uint minSpawns = 0;

    [Tooltip("How many enemies can this wave spawn at maximum?")]
    [Min(1)] public uint maxSpawns = uint.MaxValue;

    [System.Flags] public enum ExitCondition { waveDuration = 1, maxSpawns = 2 }

    [Header("Exit Conditions")]
    [Tooltip("Set the things that can trigger the end of this wave")]
    public ExitCondition conditions = (ExitCondition)~0;
    
    [Tooltip("All enemies must be dead for the wave to advance.")]
    public bool mustKillAll = false;

    [HideInInspector] public uint spawnCount;  //The number of enemies already spawned in this wave

    // Returns an array of prefabs that this wave can spawn.
    public GameObject[] GetPossibleSpawns()
    {
        // Determine how many enemies to spawn.
        int count = Random.Range(spawnsPerTick.x, spawnsPerTick.y);

        // Generate the result.
        GameObject[] result = new GameObject[count];
        for(int i = 0; i < count; i++)
        {
            // Randomly picks one of the possible spawns and inserts it
            // into the result array.
            result[i] = possibleSpawnPrefabs[Random.Range(0, possibleSpawnPrefabs.Length)];
        }

        return result;
    }

    // Get a random spawn interval between the min and max values.
    public float GetSpawnInterval()
    {
        return Random.Range(spawnInterval.x, spawnInterval.y);
    }
}
