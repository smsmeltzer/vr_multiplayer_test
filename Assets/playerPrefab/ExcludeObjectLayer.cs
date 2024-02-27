using UnityEngine;

public class ExcludeObjectLayer : MonoBehaviour
{
    public Camera cameraToConfigure;
    public GameObject objectToExclude;

    void Start()
    {
        if (cameraToConfigure == null)
        {
            Debug.LogError("Camera to configure is not assigned!");
            return;
        }

        if (objectToExclude == null)
        {
            Debug.LogError("Object to exclude is not assigned!");
            return;
        }

        // Get the layer of the object to exclude
        int layerToExclude = objectToExclude.layer;

        // Remove the layer from the camera's culling mask
        cameraToConfigure.cullingMask &= ~(1 << layerToExclude);

        Debug.Log("Excluded layer " + LayerMask.LayerToName(layerToExclude) + " from camera " + cameraToConfigure.name);
    }
}

