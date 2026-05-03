using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class EightWayParticleBurst : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 1f;
    public float particleSize = 0.2f;

    private ParticleSystem ps;

    private static readonly Vector3[] Directions =
    {
        Vector3.up,
        (Vector3.up + Vector3.right).normalized,
        Vector3.right,
        (Vector3.down + Vector3.right).normalized,
        Vector3.down,
        (Vector3.down + Vector3.left).normalized,
        Vector3.left,
        (Vector3.up + Vector3.left).normalized
    };

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        Burst();
        Destroy(gameObject, 4f);
    }

    public void Burst()
    {
        ps.Clear();

        foreach (Vector3 dir in Directions)
        {
            ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
            {
                position = Vector3.zero,
                velocity = dir * speed,
                startLifetime = lifetime,
                startSize = particleSize
            };

            ps.Emit(emitParams, 1);
        }
    }
}