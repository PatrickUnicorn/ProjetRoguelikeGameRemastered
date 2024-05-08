using UnityEngine;

[CreateAssetMenu(fileName = "Wave Data", menuName = "Rogue-like/Wave Data")]
public class WaveData : ScriptableObject
{

    [Tooltip("Une liste de tous les ennemis possibles qui peuvent appara�tre pendant cette vague.")]
    public GameObject[] possibleSpawnPrefabs = new GameObject[1];

    [Header("Spawn Settings")]
    [Tooltip("Temps entre chaque apparition (en secondes). Prendra un nombre al�atoire entre X et Y.")]
    public Vector2 spawnInterval = new Vector2(2, 3);

    [Tooltip("Combien d'ennemis sont g�n�r�s par intervalle ?")]
    public Vector2Int spawnsPerTick = new Vector2Int(1, 1);

    [Tooltip("Combien de temps (en secondes) cette vague va-t-elle g�n�rer des ennemis ?")]
    [Min(0.1f)] public float waveDuration = 60;

    [Tooltip("Si la valeur n'est pas nulle, alors la vague ne peut pas se terminer tant que le nombre minimum d'apparitions n'est pas atteint.")]
    [Min(0)] public uint minSpawns = 0;

    [Tooltip("Combien d'ennemis cette vague peut-elle g�n�rer au maximum ?")]
    [Min(1)] public uint maxSpawns = uint.MaxValue;

    [System.Flags] public enum ExitCondition { waveDuration = 1, maxSpawns = 2 }

    [Header("Exit Conditions")]
    [Tooltip("D�finissez les �l�ments qui peuvent d�clencher la fin de cette vague.")]
    public ExitCondition conditions = (ExitCondition)~0;
    
    [Tooltip("Tous les ennemis doivent �tre �limin�s pour que la vague puisse avancer.")]
    public bool mustKillAll = false;

    [HideInInspector] public uint spawnCount;  


    public GameObject[] GetPossibleSpawns()
    {
   
        int count = Random.Range(spawnsPerTick.x, spawnsPerTick.y);

        GameObject[] result = new GameObject[count];
        for(int i = 0; i < count; i++)
        {
            result[i] = possibleSpawnPrefabs[Random.Range(0, possibleSpawnPrefabs.Length)];
        }

        return result;
    }

    public float GetSpawnInterval()
    {
        return Random.Range(spawnInterval.x, spawnInterval.y);
    }
}
