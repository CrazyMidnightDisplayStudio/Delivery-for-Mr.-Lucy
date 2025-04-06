using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MrLucy
{
    public class ElevatorDownhillScenario : MonoBehaviour
    {
        [SerializeField] ElevatorDisplay elevatorDisplay;
        [SerializeField] ElevatorLight elevatorLight;
        
        [SerializeField] private float initialDelay = 1f;
        [SerializeField] private float chaosThreshold = 0.2f;
        [SerializeField] private float acceleration = 0.92f;

        private Coroutine _sequence;
        private bool _chaosMode = false;

        public void Downhill()
        {
            _sequence = StartCoroutine(DownhillSequence());
        }

        public void Stop()
        {
            _chaosMode = false;
        }

        private IEnumerator DownhillSequence()
        {
            float delay = initialDelay;

            // ЭТАП 1 - обычный спуск с ускорением
            while (delay > chaosThreshold)
            {
                elevatorDisplay.FloorNumber -= 1;
                yield return new WaitForSeconds(delay);
                delay *= acceleration;
            }
            
            // ЭТАП 2 - хаотичное переключение
            yield return StartCoroutine(ChaosMode());
        }
        
        private IEnumerator ChaosMode()
        {
            _chaosMode = true;
            while (_chaosMode)
            {
                int randomFloor = Random.Range(-999, 1000);
                elevatorDisplay.FloorNumber = randomFloor;
        
                float chaosDelay = Random.Range(0.03f, 0.08f);
                yield return new WaitForSeconds(chaosDelay);
            }
        }
    }
}