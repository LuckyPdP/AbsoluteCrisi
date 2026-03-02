using UnityEngine;

public class DragAndSnapMulti : MonoBehaviour
{
    public SnapTarget[] targetPoints;
    public float snapDistance = 0.5f;

    private bool isDragging = false;
    private Vector3 offset;
    private SnapTarget currentTarget = null;

    void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - GetMouseWorldPosition();

        // Si estaba ocupando un target, lo liberamos
        if (currentTarget != null)
        {
            currentTarget.Release();
            currentTarget = null;
        }
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;

        SnapTarget closestTarget = GetClosestAvailableTarget();

        if (closestTarget != null)
        {
            float distance = Vector3.Distance(transform.position, closestTarget.transform.position);

            if (distance < snapDistance)
            {
                if (closestTarget.TrySnap(this))
                {
                    transform.position = closestTarget.transform.position;
                    currentTarget = closestTarget;
                }
            }
        }
    }

    SnapTarget GetClosestAvailableTarget()
    {
        SnapTarget closest = null;
        float minDistance = Mathf.Infinity;

        foreach (SnapTarget target in targetPoints)
        {
            if (target.IsOccupied) continue;

            float distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = target;
            }
        }

        return closest;
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}