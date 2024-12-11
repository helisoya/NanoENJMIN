using System.Collections.Generic;

[System.Serializable]
public class SpawnSetup
{
    public int               spawnerIndex;
    public List<EnemyTypeSO> enemies;
    public float             spawnRate;
}