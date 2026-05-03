using UnityEngine;

[RequireComponent(typeof(Camera))]
public class EndingCameraShrink : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 centerPoint;

    [Header("Trigger")]
    [SerializeField] private float shrinkStartDistance = 6f;
    [SerializeField] private float shrinkEndDistance = 0.5f;

    [Header("Viewport Size")]
    [SerializeField] private Vector2 fullSize = new Vector2(1f, 1f);
    [SerializeField] private Vector2 smallSize = new Vector2(0.35f, 0.35f);

    [Header("Motion")]
    [SerializeField] private float followSpeed = 6f;
    [SerializeField] private float shrinkSpeed = 4f;

    private Camera cam;
    private bool activeSequence;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        cam.rect = new Rect(0f, 0f, fullSize.x, fullSize.y);
    }

    public void StartEndingSequence()
    {
        activeSequence = true;
    }

    private void LateUpdate()
    {
        if(player == null)
        {
            player = FindFirstObjectByType<PlayerController>().gameObject.transform;
        }
        if(!player) return;
        if (!activeSequence)
            return;

        Vector3 desiredPos = new Vector3(centerPoint.x, centerPoint.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);

        float dist = Vector2.Distance(player.position, centerPoint);
        float t = Mathf.InverseLerp(shrinkStartDistance, shrinkEndDistance, dist);
        t = Mathf.Clamp01(t);

        float targetWidth = Mathf.Lerp(fullSize.x, smallSize.x, t);
        float targetHeight = Mathf.Lerp(fullSize.y, smallSize.y, t);

        float targetX = (1f - targetWidth) * 0.5f;
        float targetY = (1f - targetHeight) * 0.5f;

        Rect targetRect = new Rect(targetX, targetY, targetWidth, targetHeight);
        cam.rect = new Rect(
            Mathf.Lerp(cam.rect.x, targetRect.x, shrinkSpeed * Time.deltaTime),
            Mathf.Lerp(cam.rect.y, targetRect.y, shrinkSpeed * Time.deltaTime),
            Mathf.Lerp(cam.rect.width, targetRect.width, shrinkSpeed * Time.deltaTime),
            Mathf.Lerp(cam.rect.height, targetRect.height, shrinkSpeed * Time.deltaTime)
        );
    }
}