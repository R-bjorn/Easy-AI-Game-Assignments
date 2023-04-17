using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

/// <summary>
/// Simple click to move.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(NavMeshAgent))]
public class ClickToMove : MonoBehaviour
{
    [Tooltip("The agent to control.")]
    [SerializeField]
    private NavMeshAgent agent;

    [Tooltip("Agent walking speed.")]
    [Min(float.Epsilon)]
    [SerializeField]
    private float walkSpeed = 5;

    [Tooltip("Agent crouching speed.")]
    [Min(float.Epsilon)]
    [SerializeField]
    private float crouchSpeed = 1;

    [Tooltip("The camera to get move commands from.")]
    [SerializeField]
    private Camera cam;

    private void Update()
    {
        // Raycast from the camera when right clicking with the mouse.
        if (Mouse.current.rightButton.wasPressedThisFrame && Physics.Raycast(cam.ScreenPointToRay(new(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), 0)), out RaycastHit hit, Mathf.Infinity))
        {
            // If the raycast hits, navigate to that position.
            agent.SetDestination(hit.point);
        }

        // Sample idea of how a crouching behaviour could be made.
        // In the navigation window, an area "Crouch" has been created on layer 3.
        // Links for crouching have been set to this layer, so check if on a link on that layer to detect if needing to crouch.
        if (agent.isOnOffMeshLink && agent.currentOffMeshLinkData.offMeshLink != null && agent.currentOffMeshLinkData.offMeshLink.area == 3)
        {
            Crouch();
        }
        else
        {
            Stand();
        }
    }

    /// <summary>
    /// Sample crouching.
    /// Only changing scale, but could expand with proper animations.
    /// </summary>
    private void Crouch()
    {
        agent.speed = crouchSpeed;
        // transform.localScale = new(1, 0.5f, 1);
    }
    
    /// <summary>
    /// Sample standing.
    /// Only changing scale, but could expand with proper animations.
    /// </summary>
    private void Stand()
    {
        agent.speed = walkSpeed;
        // transform.localScale = new(1, 1, 1);
    }

    private void OnValidate()
    {
        ComponentValidator.Check(gameObject, ref agent);
        ComponentValidator.Check(gameObject, ref cam);
    }
}