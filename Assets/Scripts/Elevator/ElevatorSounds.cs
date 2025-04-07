using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

namespace MrLucy
{
    [RequireComponent(typeof(AudioSource))]
    public class ElevatorSounds : MonoBehaviour
    {
        [SerializeField] private AudioClip _elevatorStart;
        [SerializeField] private AudioClip _downfall;
        
        [SerializeField] private AudioClip _chaosStart;
        [SerializeField] private AudioClip _chaosLoop;
        [SerializeField] private AudioClip _chaosEnd;

        private AudioSource _mainSource;
        private AudioSource _secondarySource;

        private void Awake()
        {
            // Основной источник
            _mainSource = GetComponent<AudioSource>();
            _mainSource.playOnAwake = false;

            // Дополнительный источник
            _secondarySource = gameObject.GetComponentInChildren<AudioSource>();
            _secondarySource.playOnAwake = false;
        }

        public void PlayChaosSequence()
        {
            StopAllCoroutines();
            StartCoroutine(PlayChaosRoutine());
        }

        public void StopChaosSequence()
        {
            StopAllCoroutines();
            StartCoroutine(StopChaosRoutine());
        }

        private IEnumerator PlayChaosRoutine()
        {
            _mainSource.loop = false;
            _mainSource.clip = _chaosStart;
            _mainSource.Play();

            yield return new WaitForSeconds(_chaosStart.length);

            _mainSource.loop = true;
            _mainSource.clip = _chaosLoop;
            _mainSource.Play();
        }

        private IEnumerator StopChaosRoutine()
        {
            _mainSource.loop = false;
            _mainSource.clip = _chaosEnd;
            _mainSource.Play();

            yield return new WaitForSeconds(_chaosEnd.length);
            _mainSource.Stop();
        }

        public void PlayElevatorFall()
        {
            _mainSource.clip = _elevatorStart;
            _mainSource.Play();

            _secondarySource.clip = _downfall;
            _secondarySource.Play();
        }
    }
}
