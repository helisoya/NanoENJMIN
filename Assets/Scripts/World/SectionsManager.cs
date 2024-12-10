using UnityEngine;

/// <summary>
/// Handles the scrolling system
/// </summary>
public class SectionsManager : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float destroyAtThreshold;
    [SerializeField] private float sectionSize = 100.12f;
    private Vector3 direction = Vector3.left;

    /// <summary>
    /// Gets the size of a section
    /// </summary>
    /// <returns>A section's size</returns>
    public float GetSectionSize()
    {
        return sectionSize;
    }

    void Update()
    {
        if (!GameManager.instance.InGame) return;

        foreach (Transform child in transform)
        {
            child.Translate(direction * speed * Time.deltaTime);
            if (child.position.x <= destroyAtThreshold) Destroy(child.gameObject);
        }
    }
}
