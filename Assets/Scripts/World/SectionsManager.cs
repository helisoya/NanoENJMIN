using UnityEngine;

public class SectionsManager : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float destroyAtThreshold;
    private Vector3 direction = Vector3.left;

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
