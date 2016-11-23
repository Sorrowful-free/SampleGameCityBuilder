using System;
using Assets.Scripts.Grid;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class BuildindInfoUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject _buttonBlock;
        [SerializeField]
        private GameObject _infoBlock;

        [SerializeField]
        private Text _infoText;

        private Building _currentBuilding;

        public event Action<Building> OnDeleteBuilding;
        public void Show(Building building)
        {
            gameObject.SetActive(true);
            _infoBlock.SetActive(false);
            _buttonBlock.SetActive(true);
            _currentBuilding = building;
            _infoText.text = building.BuildingInfo;
        }

        public void OnDeleteClick()
        {
            if (OnDeleteBuilding != null)
                OnDeleteBuilding(_currentBuilding);
        }
    }
}