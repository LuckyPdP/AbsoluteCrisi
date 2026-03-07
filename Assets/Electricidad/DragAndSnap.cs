using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class DragAndSnapMulti : MonoBehaviour
{
    public SnapTarget[] targetPoints;
    public float snapDistance = 0.5f;

    private bool isDragging = false;
    private Vector3 offset;
    public SnapTarget currentTarget = null;


    public UnityEvent CuandoArrastras;
    public UnityEvent CuandoSueltas;




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
        CuandoSueltas?.Invoke();

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

                    GetComponent<FastBoolChain>()?.NotificarMovimiento();
                }
            }
        }
    }

    void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - GetMouseWorldPosition();

        if (currentTarget != null)
        {
            currentTarget.Release();
            currentTarget = null;
            GetComponent<FastBoolChain>()?.NotificarMovimiento();
        }

        CuandoArrastras?.Invoke();
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