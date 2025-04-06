using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MrLucy
{
    public class ElevatorDownhillScenario : GameStateListener
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

        public void StopDownhill()
        {
            _chaosMode = false;
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
            
            elevatorLight.StartBlinking(5f);

            // State 2 - chaos
            _chaosMode = true;
            yield return StartCoroutine(ChaosMode());
        }

        private IEnumerator ChaosMode()
        {
            Debug.Log("ChaosMode");
            _chaosMode = true;
            
            while (_chaosMode)
            {
                int randomFloor = Random.Range(-999, -111);
                elevatorDisplay.FloorNumber = randomFloor;

                float chaosDelay = Random.Range(0.03f, 0.08f);
                yield return new WaitForSeconds(chaosDelay);
            }
        }

        protected override void OnGameStateChanged(GameState state)
        {
            if (state == GameState.Downhill)
            {
                StartDownhill();
            }
        }
    }
}