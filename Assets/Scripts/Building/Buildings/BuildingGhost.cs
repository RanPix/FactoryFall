using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    private Transform visual;

    [SerializeField] private LayerMask ghostLM;
    //[SerializeField] private Material canPlaceBlockMat;
    //[SerializeField] private Material cantPlaceBlockMat;

    private void Start()
    {
        RefreshVisual(null);

        PlayerBuilding.Instance.OnSelectedBlockChanged += RefreshVisual;
        PlayerBuilding.Instance.OnBuildingDisabled += RemoveVisual;
    }

    private void LateUpdate()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        Vector3 targetPosition = PlayerBuilding.Instance.GetWorldSnappedBuildingPosition();
        Quaternion targetRotation = PlayerBuilding.Instance.GetBuildingRotation();

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 30f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 30f);
    }

    private void RefreshVisual(PlacedBlockType blockType)
    {
        if (visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }

        if (blockType != null)
        {
            visual = Instantiate(blockType.Visual, Vector3.zero, Quaternion.identity);
            visual.parent = transform;
            visual.localPosition = Vector3.zero;
            visual.localEulerAngles = Vector3.zero;
            SetLayerRecursive(visual.gameObject, 11);
        }
    }

    private void RemoveVisual() => visual = null;

    private void SetLayerRecursive(GameObject targetGameObject, LayerMask layer)
    {
        targetGameObject.layer = layer;

        foreach (Transform child in targetGameObject.transform)
            SetLayerRecursive(child.gameObject, layer);
    }
}
