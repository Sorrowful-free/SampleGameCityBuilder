using System;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class BuildingListUI : MonoBehaviour
    {
        public event Action<int> OnBuildingSelected;
        public void OnSelectBuilding(int index)
        {
            if (OnBuildingSelected != null)
                OnBuildingSelected(index);
        }
    }
}