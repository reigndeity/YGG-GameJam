using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineBehavior : MonoBehaviour
{
    // Dictionary to store outlines for each GameObject
    private Dictionary<GameObject, List<Outline>> trackedObjectsOutlines = new Dictionary<GameObject, List<Outline>>();

    void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the tag "Player"
        if (other.gameObject.CompareTag("Player") || other.gameObject.layer == 3)
        {
            // If this object is not already tracked, add it to the dictionary
            if (!trackedObjectsOutlines.ContainsKey(other.gameObject))
            {
                // Get all Outline components on the GameObject
                List<Outline> outlines = new List<Outline>(other.gameObject.GetComponents<Outline>());
                trackedObjectsOutlines.Add(other.gameObject, outlines);

                // Enable each Outline component
                foreach (Outline outline in outlines)
                {
                    outline.enabled = true;
                }

                Debug.Log($"Enabled Outline components for: {other.gameObject.name} (Layer: {LayerMask.LayerToName(other.gameObject.layer)})");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the exiting object is tracked
        if (trackedObjectsOutlines.ContainsKey(other.gameObject))
        {
            List<Outline> outlines = trackedObjectsOutlines[other.gameObject];

            // Disable each Outline component
            foreach (Outline outline in outlines)
            {
                outline.enabled = false;
            }

            // Remove the object from the dictionary
            trackedObjectsOutlines.Remove(other.gameObject);

            Debug.Log($"Disabled Outline components for: {other.gameObject.name} (Layer: {LayerMask.LayerToName(other.gameObject.layer)})");
        }
    }
}
