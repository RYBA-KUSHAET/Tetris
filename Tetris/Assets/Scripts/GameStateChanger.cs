using UnityEngine;

public class GameStateChanger : MonoBehaviour
{
    public GameField GameField;
    public ShapeMover ShapeMover;
    public ShapeSpawner ShapeSpawner;

    private void Start()
    {
        FirstStartGame();
    }

    private void FirstStartGame()
    {
        GameField.FillCellsPosition();
        SpawnNextShape();
    }

    public void SpawnNextShape()
    {
        Shape nextShape = ShapeSpawner.SpawnNextShape();

        ShapeMover.SetTargetShape(nextShape);
        ShapeMover.MoveShape(Vector2Int.right * (int)(GameField.FieldSize.x * 0.5f) + Vector2Int.up * (GameField.FieldSize.y - 3));
    }
}
