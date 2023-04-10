using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject _mouseIndicator;
    [SerializeField] private GameObject _cellIndicator;
    [SerializeField] private Grid _grid;
    [SerializeField] private PlacementSystemInputHolder _inputHolder;

    [SerializeField] private ObjectsDatabaseSO _database;
    [SerializeField] private GameObject _gridVisualisation;
    private int _selectedObjectIndex = -1;

    private GridData _floorData;
    private GridData _furnitureData;

    private Renderer _previewRenderer;

    private List<GameObject> _placedGameObjects = new();

    private void Start()
    {
        StopPlacement();
        _floorData = new();
        _furnitureData = new();
        _previewRenderer = _cellIndicator.GetComponentInChildren<Renderer>();
    }

    private void Update()
    {
        if (_selectedObjectIndex < 0) return;

        Vector3 mousePosition = _inputHolder.GetSelectedMapPosition();
        Vector3Int gridPosition = _grid.WorldToCell(mousePosition);

        _previewRenderer.material.color = CheckPlacementValidity(gridPosition, _selectedObjectIndex) ? Color.white : Color.red;

        _mouseIndicator.transform.position = mousePosition;
        _cellIndicator.transform.position = _grid.CellToWorld(gridPosition) + new Vector3(_grid.cellSize.x / 2, 0f, _grid.cellSize.z / 2);
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        _selectedObjectIndex = _database.ObjectsData.FindIndex(data => data.ID == ID);

        if (_selectedObjectIndex < 0)
        {
            Debug.Log($"No ID found {ID}");
            return;
        }

        _gridVisualisation.SetActive(true);
        _cellIndicator.SetActive(true);
        _inputHolder.OnClicked += PlaceStructure;
        _inputHolder.OnEscape += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (_inputHolder.IsPointerOverUI()) return;

        Vector3 mousePosition = _inputHolder.GetSelectedMapPosition();
        Vector3Int gridPosition = _grid.WorldToCell(mousePosition);

        if (!CheckPlacementValidity(gridPosition, _selectedObjectIndex)) return;

        GameObject newObject = Instantiate(_database.ObjectsData[_selectedObjectIndex].Prefab);
        newObject.transform.position = _grid.CellToWorld(gridPosition);
            //+ new Vector3(_grid.cellSize.x / 2, 0f, _grid.cellSize.z / 2);
        _placedGameObjects.Add(newObject);
        GridData selectedData = _database.ObjectsData[_selectedObjectIndex].ID == 0 ? _floorData : _furnitureData;
        selectedData.AddObjectAt(gridPosition, _database.ObjectsData[_selectedObjectIndex].Size, _database.ObjectsData[_selectedObjectIndex].ID, _placedGameObjects.Count - 1);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = _database.ObjectsData[selectedObjectIndex].ID == 0 ? _floorData : _furnitureData;
        return selectedData.CanPlaceObjectAt(gridPosition, _database.ObjectsData[selectedObjectIndex].Size);
    }

    private void StopPlacement()
    {
        _selectedObjectIndex = -1;
        _gridVisualisation.SetActive(false);
        _cellIndicator.SetActive(false);
        _inputHolder.OnClicked -= PlaceStructure;
        _inputHolder.OnEscape -= StopPlacement;
    }
}
