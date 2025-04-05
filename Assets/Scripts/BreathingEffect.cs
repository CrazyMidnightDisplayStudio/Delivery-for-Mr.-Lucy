using UnityEngine;

public class SmoothBreathing : MonoBehaviour
{
    [SerializeField] private float breathCycleTime = 3f;
    [SerializeField] private float intensity = 0.1f;
    [SerializeField] private Vector3 breathAxis = Vector3.up;

    private Vector3 _baseScale;
    private float _breathPhase;

    private void Awake()
    {
        _baseScale = transform.localScale;
        _breathPhase = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        _breathPhase += Time.deltaTime * (2f * Mathf.PI / breathCycleTime);
        
        float sineValue = Mathf.Sin(_breathPhase);
        float smoothFactor = sineValue * intensity;
        
        transform.localScale = _baseScale + breathAxis * smoothFactor;
    }
}
