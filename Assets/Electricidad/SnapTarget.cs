using UnityEngine;

public class SnapTarget : MonoBehaviour
{
    public bool IsOccupied { get; private set; } = false;
    private DragAndSnapMulti currentPiece;

    public bool TrySnap(DragAndSnapMulti piece)
    {
        if (IsOccupied) return false;

        IsOccupied = true;
        currentPiece = piece;
        return true;
    }

    public void Release()
    {
        IsOccupied = false;
        currentPiece = null;
    }
}