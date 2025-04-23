using UnityEngine;

public class TouchManager : MonoBehaviour
{
    [SerializeField] Transform selectedBlock;
    const float positionMultiplier = 1.28f;
    Vector3 touchOffset = Vector3.zero;
    private float dragSmoothSpeed = 10f;

    private void Start()
    {
        Application.targetFrameRate = 60;
    }
    private void Update()
    {
        if (Input.touchCount == 1)
        {
            HandleTouch();
        }
    }

    void HandleTouch()
    {

        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Block"))
                {
                    selectedBlock = hit.transform;
                    selectedBlock.GetComponent<Rigidbody>().isKinematic = true;
                    Vector3 worldTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.WorldToScreenPoint(selectedBlock.position).z));
                    touchOffset = selectedBlock.position - worldTouchPos;
                }
                else return;
            }
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            if (selectedBlock)
            {

                Vector3 worldTouch = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.WorldToScreenPoint(selectedBlock.position).z));
                Vector3 proposedPosition = worldTouch + touchOffset;
                Vector3 moveDir = proposedPosition - selectedBlock.position;

                if (moveDir != Vector3.zero)
                {
                    float distance = moveDir.magnitude;
                    Vector3 direction = moveDir.normalized;
                    Vector3 boxExtents = selectedBlock.GetComponent<Collider>().bounds.extents;

                    // Slightly increase extents to be more reliable
                    Vector3 adjustedExtents = boxExtents * 0.95f;

                    RaycastHit[] hits = Physics.BoxCastAll(selectedBlock.position, adjustedExtents, direction, selectedBlock.rotation, distance);
                    bool isBlocked = false;

                    foreach (var hit in hits)
                    {
                        if (hit.transform != selectedBlock && hit.collider.CompareTag("Block"))
                        {
                            isBlocked = true;
                            break;
                        }
                    }

                    if (!isBlocked)
                    {
                        selectedBlock.position = proposedPosition;
                    }
                }
            }
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            if (selectedBlock)
            {
                selectedBlock.position = SnapToGrid(selectedBlock.position);
                selectedBlock = null;
            }
        }
    }

    Vector3 SnapToGrid(Vector3 position)
    {
        float x = Mathf.Round(position.x / positionMultiplier) * positionMultiplier;
        float y = Mathf.Round(position.y / positionMultiplier) * positionMultiplier;
        float z = Mathf.Round(position.z / positionMultiplier) * positionMultiplier;
        return new Vector3(x, y, z);
    }

}
