using UnityEngine;

public class ChangeLayerScript : MonoBehaviour
{
    public string layerName;

    public void ChangeLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(layerName);
    }
}
