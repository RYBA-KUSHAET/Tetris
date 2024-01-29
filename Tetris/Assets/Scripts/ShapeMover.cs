using UnityEngine;

public class ShapeMover : MonoBehaviour
{
    private float _moveDownTimer = 0;

    public float MoveDownDelay = 0.8f;

    public GameStateChanger GameStateChanger;
    public GameField GameField;

    private Shape TargetShape;


    public void MoveShape(Vector2Int deltaMove)
    {
        if (!CheckMovePossible(deltaMove))
        {
            return;
        }

        for (int i = 0; i < TargetShape.Parts.Length; i++)
        {
            Vector2Int newPartCellId = TargetShape.Parts[i].CellId + deltaMove;
            Vector2 newPartPosition = GameField.GetCellPosition(newPartCellId);

            TargetShape.Parts[i].CellId = newPartCellId;
            TargetShape.Parts[i].SetPosition(newPartPosition);
        }
    }

    private void Update()
    {
        SetShapePartEmpty(true);
        HorizontalMove();
        VerticalMove();

        Rotate();

        bool reachBottom = CheckBottom();
        bool reachOtherShape = CheckOtherShape();

        SetShapePartEmpty(false);

        if (reachBottom || reachOtherShape)
        {
            GameStateChanger.SpawnNextShape();
        }
    }

    private void HorizontalMove()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveShape(Vector2Int.left);
        }

        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveShape(Vector2Int.right);
        }
    }

    private void VerticalMove()
    {
        _moveDownTimer += Time.deltaTime;

        if (_moveDownTimer >= MoveDownDelay || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            _moveDownTimer = 0;
            MoveShape(Vector2Int.down);
        }
    }

    private bool CheckMovePossible(Vector2Int deltaMove)
    {
        for (int i = 0; i < TargetShape.Parts.Length; i++)
        {
            Vector2Int newPartCellId = TargetShape.Parts[i].CellId + deltaMove;

            if (newPartCellId.x < 0 || newPartCellId.y < 0 || newPartCellId.x >= GameField.FieldSize.x || newPartCellId.x >= GameField.FieldSize.y)
            {
                return false;
            }

            else if(!GameField.GetCellEmpty(newPartCellId))
            {
                return false;
            }
        }

        return true;
    }

    public void SetTargetShape(Shape targetShape)
    {
        TargetShape = targetShape;
    }

    private bool CheckBottom()
    {
        for (int i = 0; i < TargetShape.Parts.Length; i++)
        {
            if (TargetShape.Parts[i].CellId.y == 0)
            {
                return true;
            }
        }

        return false;
    }

    public void Rotate()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector2Int[] startCellIds = TargetShape.GetPartCellId();

            TargetShape.Rotate();

            UpdateByWalls();
            UpdateByBottom();
            bool shapeSetted = SetShapeInCells();

            if(!shapeSetted)
            {
                MoveShapeToCellIds(TargetShape, startCellIds);
            }
        }
    }

    public bool SetShapeInCells()
    {
        for(int i = 0; i < TargetShape.Parts.Length; i++)
        {
            Vector2 shapePartPosition = TargetShape.Parts[i].transform.position;
            Vector2Int newPartCellId = GameField.GetNearestCellId(shapePartPosition);

            if(!GameField.GetCellEmpty(newPartCellId))
            {
                return false;
            }

            Vector2 newPartPosition = GameField.GetCellPosition(newPartCellId);

            TargetShape.Parts[i].CellId = newPartCellId;
            TargetShape.Parts[i].SetPosition(newPartPosition);
        }

        return true;
    }

    public void UpdateByWalls()
    {
        UpdateByWall(true);
        UpdateByWall(false);
    }

    private void UpdateByWall(bool right)
    {
        for (int i = 0; i < TargetShape.Parts.Length; i++)
        {
            if (CheckWallOver(TargetShape.Parts[i], right))
            {
                for(int j = 0; j < TargetShape.Parts.Length; j++)
                {
                    TargetShape.Parts[j].transform.position += (right ? -1 : 1) * Vector3.right * GameField.CellSize.x;
                }
            }
        }
    }

    private bool CheckWallOver(ShapePart part, bool right)
    {
        float wallDistance = 0;

        if(right)
        {
            wallDistance = part.transform.position.x - (GameField.FirstCellPosition.position.x + (GameField.FieldSize.x - 1) * GameField.CellSize.x);

            wallDistance = GetRoundWallDistance(wallDistance);

            if(wallDistance != 0 && wallDistance > 0)
            {
                return true;
            }
        }

        else
        {
            wallDistance = part.transform.position.x - GameField.FirstCellPosition.position.x;

            wallDistance = GetRoundWallDistance(wallDistance);

            if(wallDistance != 0 && wallDistance < 0)
            {
                return true;
            }
        }

        return false;
    }
    
    private float GetRoundWallDistance(float distance)
    {
        int roundValue = 100;

        distance = Mathf.Round(distance * roundValue);

        return distance;
    }

    private void UpdateByBottom()
    {
        for (int i = 0; i < TargetShape.Parts.Length; i++)
        {
            if (CheckBottomOver(TargetShape.Parts[i]))
            {
                for (int j = 0; j < TargetShape.Parts.Length; j++)
                {
                    TargetShape.Parts[j].transform.position = Vector3.up * GameField.CellSize.y;
                }
            }
        }
    }

    private bool CheckBottomOver(ShapePart part)
    {
        float wallDistance = part.transform.position.y - GameField.FirstCellPosition.position.y;

        wallDistance = GetRoundWallDistance(wallDistance);

        if(wallDistance != 0 && wallDistance < 0)
        {
            return true;
        }

        return false;
    }

    private bool CheckOtherShape()
    {
        for(int i = 0; i < TargetShape.Parts.Length; i++)
        {
            if (!GameField.GetCellEmpty(TargetShape.Parts[i].CellId + Vector2Int.down))
            {
                return true;
            }
        }
        return false;
    }

    private void SetShapePartEmpty(bool value)
    {
        for (int i = 0; i < TargetShape.Parts.Length; i++)
        {
            GameField.SetSellEmpty(TargetShape.Parts[i].CellId, value);
        }
    }

    private void MoveShapeToCellIds(Shape shape, Vector2Int[] cellIds)
    {
        for (int i = 0; i < TargetShape.Parts.Length; i++)
        {
            MoveShapePartToCellId(shape.Parts[i], cellIds[i]);
        }
    }

    private void MoveShapePartToCellId(ShapePart part, Vector2Int cellId)
    {
        Vector2 newPartPosition = GameField.GetCellPosition(cellId);
        part.CellId = cellId;
        part.SetPosition(newPartPosition);
    }
}