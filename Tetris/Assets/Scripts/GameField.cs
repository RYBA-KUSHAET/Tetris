using UnityEngine;

public class GameField : MonoBehaviour
{
    private GameFieldCell[,] _cells;

    public Vector2 CellSize;
    public Vector2Int FieldSize;

    public Transform FirstCellPosition;

    public void FillCellsPosition()
    {
        _cells = new GameFieldCell[FieldSize.x, FieldSize.y];
        {
            for(int i = 0; i < FieldSize.x; i++)
            {
                for(int j = 0; j < FieldSize.y; j++)
                {
                    Vector2 cellPosition = (Vector2)FirstCellPosition.position + Vector2.right * i * CellSize.x + Vector2.up * j * CellSize.y;

                    GameFieldCell newCell = new GameFieldCell(cellPosition);

                    _cells[i, j] = newCell;
                }
            }
        }
    }

    public Vector2 GetCellPosition(Vector2Int cellId)
    {
        GameFieldCell cell = GetCell(cellId.x, cellId.y);

        if(cell == null)
        {
            return Vector2.zero;
        }

        return cell.GetPosition();
    }

    private GameFieldCell GetCell(int x, int y)
    {
        if(x < 0 || y < 0 || x >= FieldSize.x || y >= FieldSize.y)
        {
            return null;
        }

        return _cells[x, y];  
    }

    public Vector2 GetCellPosition(int x, int y)
    {
        GameFieldCell cell = GetCell(x, y);

        if(cell == null)
        {
            return Vector2.zero;
        }

        return cell.GetPosition();
    }

    public Vector2Int GetNearestCellId(Vector2 position)
    {
        float resultDistance = float.MaxValue;
        int resultX = 0;
        int resultY = 0;
        for(int i = 0; i < FieldSize.x; i++)
        {
            for(int j = 0; j < FieldSize.y; j++)
            {
                Vector2 cellPosition = GetCellPosition(i, j);
                float distance = (cellPosition - position).magnitude;           
                
                if (distance < resultDistance)
                {
                    resultDistance = distance;
                    resultX = i;
                    resultY = j;
                }
            }
        }

        return new Vector2Int(resultX, resultY);                                                                                                                                                                                                                                                                                                          

    }

    public void SetSellEmpty(Vector2Int cellId, bool value)
    {
        GameFieldCell cell =  GetCell(cellId.x, cellId.y);

        if(cell == null)
        {
            return;
        }

        cell.SetIsEmpty(value);
    }

    public bool GetCellEmpty(Vector2Int cellId)
    {
        GameFieldCell cell = GetCell(cellId.x, cellId.y);

        if (cell == null)
        {
            return false;
        }

        return cell.GetIsEmpty();
    }

    public bool[] GetRowFillings()
    {
        bool[] rowFillings = new bool[FieldSize.y]; /////////////////
        bool isRowFilled;

        for(int i = 0; i < rowFillings.Length; i++)
        {
            isRowFilled = true;

            for(int j = 0; j < FieldSize.x; j++)
            {
                if (_cells[j, i].GetIsEmpty())
                {
                    isRowFilled = false;
                    break;
                }
            }

            rowFillings[i] = isRowFilled;
        }

        return rowFillings;
    }

}
