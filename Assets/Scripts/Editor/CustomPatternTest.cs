using System;
using UnityEngine;

public enum Elements {Water, Fire, Earth}

[CreateAssetMenu(fileName = "CustomPatternTest", menuName = "Scriptable Objects/CustomPatternTest")]
public class CustomPatternTest : ScriptableObject
{
    public Wrapper<Elements>[] grid;
    public const int size = 4;

    private void Awake()
    {
        Debug.Log("object");
        if (grid == null)
            ResetGrid();
    }

    public void ResetGrid()
    {
        grid = new Wrapper<Elements>[size];
        for (int i = 0; i < size; i++)
        {
            grid[i] = new Wrapper<Elements>();
            grid[i].values = new Elements[size];
        }
    }
}


[Serializable]
public class Wrapper<T>
{
    public T[] values;
}
