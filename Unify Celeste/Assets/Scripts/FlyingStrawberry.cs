using UnityEngine;

public class FlyingStrawberry : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject scoreUI;

    private PlayerController player;
    private AudioSource audioSource;
    private SinOscillator sinOscillator;
    private LinearMovement linearMovement;

    public bool flying = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        sinOscillator = GetComponent<SinOscillator>();
        linearMovement = GetComponent<LinearMovement>();
    }

    private void Update()
    {
        if (player == null)
        {
            player = FindFirstObjectByType<PlayerController>();
            return;
        }

        if (player.isDashing && !flying)
        {
            StartFlying();
        }
    }

    private void StartFlying()
    {
        flying = true;

        AudioClip fleeClip = Resources.Load<AudioClip>("StrawberryFlee");
        if (fleeClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(fleeClip);
        }

        if (sinOscillator != null)
        {
            sinOscillator.enabled = false;
        }

        if (linearMovement != null)
        {
            linearMovement.ySpeed = speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        PlayerController playerController = c.GetComponent<PlayerController>();

        if (playerController == null)
        {
            return;
        }

        if (LevelStorage.instance != null)
        {
            LevelStorage.instance.totalStrawberries++;

            AudioClip collectClip = Resources.Load<AudioClip>("CollectStrawberry");
            LevelStorage.instance.PlaySFX(collectClip);
        }

        if (scoreUI != null)
        {
            Instantiate(scoreUI, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}