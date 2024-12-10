using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Scriptable Objects/Wave")]
public class WaveSO : ScriptableObject
{
    public List<WaveSpawnerSetup> waveSpawnerSetups;
}

[Serializable]
public class WaveSpawnerSetup
{
    public int spawnerIndex;
    public EnemyPatternSO pattern;
}
