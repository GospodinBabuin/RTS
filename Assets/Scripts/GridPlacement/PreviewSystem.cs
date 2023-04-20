using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] private float _previewYOffset = 0.06f;

    [SerializeField] private GameObject _cellIndicator;
    private GameObject _previewObject;

    [SerializeField] private Material _previewMaterialsPrefab;
    private Material _previewMaterialInstance;

    private Renderer _cellIndicatorRenderer;

    private void Start()
    {
        _previewMaterialInstance = new Material(_previewMaterialsPrefab);

        _cellIndicator.SetActive(false);

        _cellIndicatorRenderer = _cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        _previewObject = Instantiate(prefab);
        PreparePreview(_previewObject);
        PrepareCursor(size);
        _cellIndicator.SetActive(true);
    }

    private void PrepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            //_cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            _cellIndicatorRenderer.material.mainTextureScale = size;
        }
    }

    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = _previewMaterialInstance;
            }
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.materials = materials;
        }
    }

    public void StopShowingPreview()
    {
        _cellIndicator.SetActive(false);
        if (_previewObject != null)
            Destroy(_previewObject);
    }

    public void UpdatePosition(Vector3 position, bool validity)
    {
        if (_previewObject != null)
        {
            MovePreview(position);
            ApplyFeedback(validity);
        }
        MoveCursor(position);
    }

    private void ApplyFeedback(bool validity)
    {
        Color color = validity ? Color.white : Color.red;
        _cellIndicatorRenderer.material.color = color;
        color.a = 0.5f;
        _previewMaterialInstance.color = color;
    }

    private void MoveCursor(Vector3 position)
    {
        _cellIndicator.transform.position = position;
    }

    private void MovePreview(Vector3 position)
    {
        _previewObject.transform.position = new Vector3(position.x, position.y + _previewYOffset, position.z);
    }

    internal void StartShowingRemovePreview()
    {
        _cellIndicator.SetActive(true);
        PrepareCursor(Vector2Int.one);
        ApplyFeedback(false);
    }
}