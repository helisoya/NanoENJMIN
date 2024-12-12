using UnityEngine;

/// <summary>
/// Handles the scrolling system
/// </summary>
public class SectionsManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float speed;
    [SerializeField] private float destroyAtThreshold;
    [SerializeField] private float _stopScrollingAt;
    private Vector3 direction = Vector3.left;
    private bool _isScrolling = true;
    private float _distance;

    /// <summary>
    /// Sets the scroll speed
    /// </summary>
    /// <param name="speed">The new scroll speed</param>
    public void SetScrollSpeed(float speed)
    {
        this.speed = speed;
    }

    void Update()
    {
        if (!GameManager.instance.InGame) return;
        if (!_isScrolling) return;
        
        foreach (Transform child in transform)
        {
            child.Translate(direction * speed * Time.deltaTime);
            if (child.position.x <= destroyAtThreshold) Destroy(child.gameObject);
        }

        _distance += speed * Time.deltaTime;
        if (_distance >= _stopScrollingAt)
        {
            _isScrolling = false;
        }
    }
}
