using UnityEngine;
using UnityEngine.Rendering;

namespace GridPlacement
{
    public class PreviewSystem : MonoBehaviour
    {
        private readonly float _previewYOffset = 0.05f;
    
        [SerializeField] private GameObject cellIndicator;
        private GameObject _previewObject;

        [SerializeField] private Material previewMaterialPrefab;
        private Material _previewMaterialInstance;

        private Renderer _cellIndicatorRenderer;

        private void Awake()
        {
            _previewMaterialInstance = new Material(previewMaterialPrefab);
            _cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
            cellIndicator.SetActive(false);
        }

        public void StartShowingPreview(GameObject prefab, Vector2Int size)
        {
            _previewObject = Instantiate(prefab);
            PreparePreview(_previewObject);
            PrepareCursor(size);
            cellIndicator.SetActive(true);
        }

        public void StopShowingPreview()
        {
            cellIndicator.SetActive(false);
            
            if (_previewObject != null)
                Destroy(_previewObject);
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

                renderer.materials = materials;
                renderer.shadowCastingMode = ShadowCastingMode.Off;
            }
        }
    
        private void PrepareCursor(Vector2Int size)
        {
            if (size.x > 0 || size.y > 0)
            {
                cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
                _cellIndicatorRenderer.material.mainTextureScale = size;
            }
        }

        public void UpdatePosition(Vector3 position, bool validity)
        {
            if (_previewObject != null)
            {
                MovePreview(position);
                ApplyFeedbackToPreview(validity);
            }
            
            MoveCursor(position);
            ApplyFeedbackToCursor(validity);
        }

        private void ApplyFeedbackToPreview(bool validity)
        {
            Color color = validity ? Color.white : Color.red;
            
            color.a = 0.5f;
            _previewMaterialInstance.color = color;
        }
        private void ApplyFeedbackToCursor(bool validity)
        {
            Color color = validity ? Color.white : Color.red;
            color.a = 0.5f;
            _cellIndicatorRenderer.material.color = color;
        }

        private void MoveCursor(Vector3 position)
        {
            cellIndicator.transform.position = position;
        }

        private void MovePreview(Vector3 position)
        {
            _previewObject.transform.position = new Vector3(position.x,
                position.y + _previewYOffset, position.z);
        }

        public void StartShowingRemovePreview()
        {
            cellIndicator.SetActive(true);
            PrepareCursor(Vector2Int.one);
            ApplyFeedbackToCursor(false);
        }
    }
}
