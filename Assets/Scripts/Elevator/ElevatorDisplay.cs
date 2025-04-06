using TMPro;
using UnityEngine;

namespace MrLucy
{
    public class ElevatorDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI elevatorDisplayText;

        private int _floorNumber;

        public int FloorNumber
        {
            get => _floorNumber;
            set
            {
                _floorNumber = value;
                elevatorDisplayText.text = value.ToString();
            }
        }
    }
}