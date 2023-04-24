using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PreviewSystem : MonoBehaviour
{
    private float _peviewYOffset = 0.05f;
    
    [SerializeField] GameObject cellindicator;
    GameObject _previewObject;

    [SerializeField] private Material previewMaterialPrefab;
    private Material _previewMaterialInstance;

    private Renderer _cellIndicatorRenderer;

    private void Awake()
    {
        _previewMaterialInstance = new Material(previewMaterialPrefab);
        _cellIndicatorRenderer = cellindicator.GetComponentInChildren<Renderer>();
        cellindicator.SetActive(false);
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        _previewObject = Instantiate(prefab);
        PreparePreview(_previewObject);
        PrepareCursor(size);
        cellindicator.SetActive(true);
    }

    public void StopShowingPlacementPreview()
    {
        cellindicator.SetActive(false);
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
            cellindicator.transform.localScale = new Vector3(size.x, 1, size.y);
            _cellIndicatorRenderer.material.mainTextureScale = size;
        }
    }

    public void UpdatePosition(Vector3 position, bool validity)
    {
        MovePreview(position);
        MoveCursor(position);
        ApplyFeedback(validity);
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
        cellindicator.transform.position = position;
    }

    private void MovePreview(Vector3 position)
    {
        _previewObject.transform.position = new Vector3(position.x,
            position.y + _peviewYOffset, position.z);
    }
}
