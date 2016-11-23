using System;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField]
        private BuildingListUI _buildingList;
        public BuildingListUI BuildingList
        {
            get { return _buildingList; }
        }

        [SerializeField]
        private BuildindInfoUI _buildingInfo;
        public BuildindInfoUI BuildingInfo
        {
            get { return _buildingInfo; }
        }

        [SerializeField]
        private GameObject _clickPlace;
        public void ShowClickPlace()
        {
            _clickPlace.SetActive(true);
        }

        public void HideClickPlace()
        {
            _clickPlace.SetActive(false);
        }

        public event Action OnClick;
        public void OnClickHandler()
        {
            if (OnClick != null)
                OnClick();
        }

        public bool SomeMenuIsVisible { get
        {
            return _buildingList.gameObject.activeSelf || _buildingInfo.gameObject.activeSelf;
        } }
    }
}
