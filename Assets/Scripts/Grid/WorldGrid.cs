using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Grid
{
    public class WorldGrid : MonoBehaviour
    {

        [Header("Debug"), SerializeField]
        private bool EnableDebug = false;
        
        [SerializeField]
        private float _cellSize = 0.1f;
        public float CellSize
        {
            get { return _cellSize; }
        }

        [SerializeField]
        private GameObject _obstaclePrefab;


        private int _size;
        private Cell[,] _cells;
        
        private Cell _selectedCell;
        private Dictionary<Building, Cell> _buildings;

        public Cell[,] Cells
        {
            get { return _cells; }
        }
        
        /// <summary>
        /// метод инициализации мировой сетки14:20
        /// </summary>
        /// <param name="size">размер одной стороны сетки в количестве ячеек</param>
        /// <param name="fill">насколько заполнена сетка занятыми ячейками</param>
        public void Initialize(int size,float fill)
        {
            _size = size;
            _cells = new Cell[_size,_size];
            _buildings = new Dictionary<Building, Cell>();
            transform.localScale = Vector3.one*_size*_cellSize;
            for (int i = 0; i < _size*_size; i++)
            {
                var rowIndex = i / _size;
                var columnIndex = i % _size;
                var isEmpty = Random.value > fill;
                var cell = new Cell(columnIndex, rowIndex) { IsEmpty = isEmpty };
                _cells[columnIndex, rowIndex] = cell;
                if (!isEmpty)
                {
                    var obstacle = Instantiate(_obstaclePrefab);
                    obstacle.transform.position = GetCellPosition(cell);
                    obstacle.transform.localScale = Vector3.one*_cellSize;
                    obstacle.transform.SetParent(transform);
                }
            }
        }
        
        private void OnDrawGizmos()
        {
            if(_cells == null || !EnableDebug)
                return;
            foreach (var cell in _cells)
            {
                var cellPosition = GetCellPosition(cell);
                var cellSize = Vector3.one*_cellSize;
                if (!cell.IsEmpty)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(cellPosition, cellSize);
                }
                else
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(cellPosition, cellSize);
                }
                if (cell.IsSelected)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(cellPosition, _cellSize);
                }
            }
        }
        public bool CanBuildOnCurrentPosition(Building building, Vector3 position)
        {
            var cell = GetCell(position);
            return CanBuildOnCurrentCell(building, cell);
        }

        public bool CanBuildOnCurrentCell(Building building, Cell cell)
        {
            if (!cell.IsEmpty)
                return false;

            foreach (var usedCell in building.UsedCells)
            {
                var columnIndex = cell.X + (int) usedCell.x;
                var rowIndex = cell.Y + (int)usedCell.y;
                if (columnIndex < 0 || columnIndex >= _size || rowIndex < 0 || rowIndex >= _size || !_cells[columnIndex, rowIndex].IsEmpty)
                    return false;
            }
            return true;
        }

        public Cell GetCell(Vector3 position)
        {
            position = position + new Vector3(_cellSize * _size / 2, 0, _cellSize * _size / 2);
            var columnIndex = (int)(position.x / (_cellSize));
            var rowIndex = (int)(position.z / (_cellSize));
            var cell = (rowIndex >= 0 && rowIndex < _size && columnIndex >= 0 && columnIndex < _size) ? _cells[columnIndex, rowIndex] : null;
            if (EnableDebug)
            {
                if (_selectedCell != null)
                    _selectedCell.IsSelected = false;
                if (cell != null)
                {
                    cell.IsSelected = true;
                    _selectedCell = cell;
                }

            }
            return cell;
        }

        public bool BuildBuilding(Building building, Cell cell)
        {
            if (CanBuildOnCurrentCell(building, cell))
            {
                building.transform.SetParent(transform);
                _buildings.Add(building, cell);
                foreach (var usedCell in building.UsedCells)
                {
                    var columnIndex = cell.X + (int)usedCell.x;
                    var rowIndex = cell.Y + (int)usedCell.y;
                    if (columnIndex >= 0 && columnIndex < _size && rowIndex >= 0 && rowIndex < _size)
                        _cells[columnIndex, rowIndex].IsEmpty = false;
                }
                return true;
            }
            return false;
        }
        
        public void DestroyBuilding(Building building)
        {
            var cell = default(Cell);
            if (_buildings.TryGetValue(building, out cell))
            {
                foreach (var usedCell in building.UsedCells)
                {
                    var columnIndex = cell.X + (int)usedCell.x;
                    var rowIndex = cell.Y + (int)usedCell.y;
                    if (columnIndex >= 0 && columnIndex < _size && rowIndex >= 0 && rowIndex < _size)
                        _cells[columnIndex, rowIndex].IsEmpty = true;
                }
                _buildings.Remove(building);
                Destroy(building.gameObject);
            }
        }

        

        public Vector3 GetCellPosition(Cell cell)
        {
            var offset = new Vector3(_size * _cellSize, 0, _size * _cellSize) / 2;
            var cellPosition = new Vector3(cell.X, 0, cell.Y) * _cellSize;
            return cellPosition - offset; 
        }
    }
}
