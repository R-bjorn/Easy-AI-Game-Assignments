using UnityEngine;

/// <summary>
/// Helper class to help with component validation.
/// </summary>
public static class ComponentValidator
{
    /// <summary>
    /// Check for a component to attach.
    /// </summary>
    /// <param name="gameObject">The game object the component is attached to.</param>
    /// <param name="component">The component to check and pass back out.</param>
    /// <typeparam name="T">The type of component.</typeparam>
    public static void Check<T>(GameObject gameObject, ref T component) where T : Component
    {
        // If the component is not null nothing to do.
        if (component != null)
        {
            return;
        }

        // Try to get an attached component.
        component = gameObject.GetComponent<T>();
        if (component != null)
        {
            return;
        }

        // Try to get a child component.
        component = gameObject.GetComponentInChildren<T>();
        if (component != null)
        {
            return;
        }

        // Try and find any object in the scene.
        component = Object.FindObjectOfType<T>();
    }
}