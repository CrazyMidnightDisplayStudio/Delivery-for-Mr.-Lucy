using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MrLucy
{
    public class ElevatorDownhillScenario : MonoBehaviour
    {
        [SerializeField] ElevatorDisplay elevatorDisplay;
        [SerializeField] ElevatorLight elevatorLight;

        [SerializeField] private float initialDelay = 3f;
        [SerializeField] private float acceleration = 0.85f;

        private Coroutine _sequence;
        private bool _chaosMode = false;
        private float _currentFallSpeed = 0f;

        public void StartDownhill()
        {
            _sequence = StartCoroutine(DownhillSequence());
        }

        public void StartChaoticDownhill()
        {
            _sequence = StartCoroutine(ChaosDownhillSequence());
        }

        public void StopDownhill()
        {
            _chaosMode = false;
            StopCoroutine(_sequence);
            _sequence = null;
            _currentFallSpeed = 0f;
        }

        private IEnumerator DownhillSequence()
        {
            float delay = initialDelay;
            float chaosTimer = 0f;

            // State 1 - downhill
            while (elevatorDisplay.FloorNumber > -100)
            {
                yield return new WaitForSeconds(delay);

                elevatorDisplay.FloorNumber -= 1;

                delay *= acceleration;
                _currentFallSpeed = 1f / delay;
            }

            GameManager.Instance.SetState(GameState.ChaoticFall);
        }

        private IEnumerator ChaosDownhillSequence()
        {
            _chaosMode = true;

            while (_chaosMode)
            {
                int randomFloor = Random.Range(-999, -111);
                elevatorDisplay.FloorNumber = randomFloor;

                float chaosDelay = Random.Range(0.03f, 0.08f);
                yield return new WaitForSeconds(chaosDelay);
            }
        }
    }
}