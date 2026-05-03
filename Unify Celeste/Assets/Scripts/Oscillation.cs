using UnityEngine;

public class SinOscillator : MonoBehaviour
{
    [Header("Oscillation")]
    [SerializeField] private Vector3 direction = Vector3.up;
    [SerializeField] private float amplitude = 0.2f;
    [SerializeField] private float frequency = 0.2f;

    [Header("Options")]
    [SerializeField] private bool useLocalPosition = true;
    [SerializeField] private bool randomizePhase = false;

    private Vector3 startPosition;
    private float phaseOffset;

    private void Start()
    {
        startPosition = useLocalPosition ? transform.localPosition : transform.position;

        if (randomizePhase)
        {
            phaseOffset = Random.Range(0f, Mathf.PI * 2f);
        }
    }

    private void Update()
    {
        float sine = Mathf.Sin((Time.time * frequency * Mathf.PI * 2f) + phaseOffset);
        Vector3 offset = direction.normalized * sine * amplitude;

        if (useLocalPosition)
        {
            transform.localPosition = startPosition + offset;
        }
        else
        {
            transform.position = startPosition + offset;
        }
    }
}