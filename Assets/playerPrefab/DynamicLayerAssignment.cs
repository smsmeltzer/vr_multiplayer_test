using UnityEngine;

public class DynamicLayerAssignment : MonoBehaviour
{
    // The name prefix for the dynamically created layer
    public string layerNamePrefix = "DynamicLayer";

    void Start()
    {
        // Generate a unique layer name based on the prefix and current time
        string uniqueLayerName = layerNamePrefix + "_" + Time.time.ToString();

        // Create a new layer with the generated name
        int newLayerIndex = LayerMask.NameToLayer(uniqueLayerName);
        if (newLayerIndex == -1)
        {
            // Layer doesn't exist, so create it
            newLayerIndex = LayerMask.NameToLayer(uniqueLayerName);
            if (newLayerIndex == -1)
            {
                Debug.LogError("Failed to create layer: " + uniqueLayerName);
                return;
            }
        }

        // Assign the new layer to the object's layer
        gameObject.layer = newLayerIndex;

        Debug.Log("Assigned layer " + uniqueLayerName + " to object: " + gameObject.name);
    }
}

