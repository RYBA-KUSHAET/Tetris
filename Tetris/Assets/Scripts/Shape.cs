using UnityEngine;

public class Shape : MonoBehaviour
{
    public int ExtraSpawnMove;

    public ShapePart[] Parts = new ShapePart[0];

    public virtual void Rotate() { }

    public Vector2Int[] GetPartCellId()
    {
        Vector2Int[] startCellIds = new Vector2Int[Parts.Length];

        for(int i = 0; i < Parts.Length; i++) 
        {
            startCellIds[i] = Parts[i].CellId;
        }

        return startCellIds;
    }
}
