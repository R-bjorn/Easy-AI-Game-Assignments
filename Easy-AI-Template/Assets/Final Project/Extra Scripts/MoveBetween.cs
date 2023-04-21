using UnityEngine;

/// <summary>
/// Move an object between positions.
/// </summary>
[DisallowMultipleComponent]
public class MoveBetween : MonoBehaviour
{
    [Tooltip("How fast in meters to move between positions.")]
    [Min(0)]
    [SerializeField]
    private float speed = 5;
    
    [Tooltip("The positions to move between.")]
    [SerializeField]
    private Transform[] positions;

    /// <summary>
    /// The starting position.
    /// </summary>
    private Vector3 _start;

    /// <summary>
    /// The current index to move to.
    /// </summary>
    private int _index;

    private void Start()
    {
        // Store the starting position.
        _start = transform.position;
    }

    private void Update()
    {
        // If completed all moves, return to the starting position.
        if (_index >= positions.Length)
        {
            _index = -1;
        }

        // Set the target.
        Vector3 target = _index == -1 ? _start : positions[_index].position;
        
        // Move towards the target.
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Move to the next target once reached.
        if (transform.position == target)
        {
            _index++;
        }
    }
}