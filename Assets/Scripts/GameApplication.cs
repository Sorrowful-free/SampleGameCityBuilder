using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Assets.Scripts.Grid;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameApplication : MonoBehaviour
    {
        [SerializeField]
        private WorldGrid _grid;

        [SerializeField]
        private GameUI _gameUI;

        [SerializeField]
        private List<GameObject> _buildings;

        private Building _currentBuilding;
        private Plane _worldGridPlane;
        
        private void Awake()
        {
            _worldGridPlane = new Plane(Vector3.up, Vector3.zero);
            _grid.Initialize(100,0.1f);
            _gameUI.BuildingList.OnBuildingSelected += OnBuildingSelected;
            _gameUI.BuildingInfo.OnDeleteBuilding+=OnBuildingDelete;
            _gameUI.OnClick += OnClickWorldGrid;
        }
        
        private void OnDestroy()
        {
            _gameUI.BuildingList.OnBuildingSelected -= OnBuildingSelected;
            _gameUI.BuildingInfo.OnDeleteBuilding -= OnBuildingDelete;
            _gameUI.OnClick -= OnClickWorldGrid;
        }
        
        private Vector3 GetCursorWorldPosition()
        {
            var camera = Camera.main;
            var cameraRay = camera.ScreenPointToRay(Input.mousePosition);
            var distance = 0f;
            _worldGridPlane.Raycast(cameraRay, out distance);
            var rayPoint = cameraRay.origin + (cameraRay.direction*distance);
            return rayPoint;
        }

        private void Update()
        {
            if (_currentBuilding != null)
            {
                var cell = _grid.GetCell(GetCursorWorldPosition());
                if(cell != null && _grid.CanBuildOnCurrentCell(_currentBuilding,cell))
                    _currentBuilding.transform.position = _grid.GetCellPosition(cell);
            }
        }

        private void OnClickWorldGrid()
        {
            var cell = _grid.GetCell(GetCursorWorldPosition());
            if (_currentBuilding != null && _grid.BuildBuilding(_currentBuilding, cell))
            {
                _currentBuilding.OnBuildingClick += OnBuildingClick;
                _currentBuilding = null;
                _gameUI.HideClickPlace();
            }
        }
        
        private void OnBuildingDelete(Building building)
        {
            building.OnBuildingClick -= OnBuildingClick;
            _grid.DestroyBuilding(building);
        }

        private void OnBuildingClick(Building building)
        {
            if(!_gameUI.SomeMenuIsVisible)
                _gameUI.BuildingInfo.Show(building);
        }

        private void OnBuildingSelected(int index)
        {
            _currentBuilding = Instantiate(_buildings[index]).GetComponent<Building>();
            _currentBuilding.transform.localScale = Vector3.one * _grid.CellSize;
            _gameUI.ShowClickPlace();
        }
    }
}
