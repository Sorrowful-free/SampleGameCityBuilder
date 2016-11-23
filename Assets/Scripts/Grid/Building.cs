using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Assets.Scripts.Grid
{
    public class Building : MonoBehaviour
    {
        [SerializeField]
        private List<Vector2> _usedCells;

        [SerializeField,TextArea(2,5)]
        private string _buildingInfo;
        public string BuildingInfo
        {
            get { return _buildingInfo; }
        }

        public event Action<Building> OnBuildingClick;
        public ReadOnlyCollection<Vector2> UsedCells
        {
            get { return _usedCells.AsReadOnly(); }
        }

        public void OnMouseUp()
        {
            if (OnBuildingClick != null)
                OnBuildingClick(this);
        }
    }
}