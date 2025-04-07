using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MrLucy
{
    public class ElevatorDownhillScenario : MonoBehaviour
    {
        private ElevatorDisplay _elevatorDisplay;
        private ElevatorLight _elevatorLight;
        private ElevatorSounds _elevatorSounds;

        [SerializeField] private float initialDelay = 3f;
        [SerializeField] private float acceleration = 0.85f;


        private Coroutine _sequence;
        private bool _chaosMode = false;
        private float _currentFallSpeed = 0f;

        private void Awake()
        {
            _elevatorDisplay = GetComponent<ElevatorDisplay>();
            _elevatorLight = GetComponent<ElevatorLight>();
            _elevatorSounds = GetComponent<ElevatorSounds>();
        }

        public void StartDownhill()
        {
            _sequence = StartCoroutine(DownhillSequence());
            _elevatorSounds.PlayElevatorFall();
        }

        public void StartChaoticDownhill()
        {
            _sequence = StartCoroutine(ChaosDownhillSequence());
        }

        public void StartChaotic666()
        {
            _sequence = StartCoroutine(ChaosSequence666());
        }

        public void StopDownhill()
        {
            _chaosMode = false;
            StopCoroutine(_sequence);
            _sequence = null;
            _currentFallSpeed = 0f;
            _elevatorSounds.StopChaosSequence();
        }

        private IEnumerator DownhillSequence()
        {
            float delay = initialDelay;

            // State 1 - downhill
            while (_elevatorDisplay.FloorNumber > -100)
            {
                yield return new WaitForSeconds(delay);

                _elevatorDisplay.FloorNumber -= 1;

                delay = Mathf.Max(0.05f, delay * acceleration); // защита от слишком маленьких значений
                _currentFallSpeed = 1f / delay;


                if (_elevatorDisplay.FloorNumber == -85)
                {
                    _elevatorSounds.PlayChaosSequence();
                }
            }

            GameManager.Instance.SetState(GameState.ChaoticFall);
        }

        private IEnumerator ChaosDownhillSequence()
        {
            _chaosMode = true;

            while (_chaosMode)
            {
                int randomFloor = Random.Range(-999, -111);
                _elevatorDisplay.FloorNumber = randomFloor;

                float chaosDelay = Random.Range(0.03f, 0.08f);
                yield return new WaitForSeconds(chaosDelay);
            }
        }

        private IEnumerator ChaosSequence666()
        {
            _chaosMode = true;
            bool switcher = true;

            while (_chaosMode)
            {
                _elevatorDisplay.FloorNumber = switcher ? -666 : -667;
                switcher = !switcher;

                float chaosDelay = Random.Range(0.1f, 1f);
                yield return new WaitForSeconds(chaosDelay);
            }
        }
    }
}