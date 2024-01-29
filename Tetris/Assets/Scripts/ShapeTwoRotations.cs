using UnityEngine;

public class ShapeTwoRotations : Shape
{
    bool _rotate = false;
    public override void Rotate()
    {
        float rotateMultiplier = _rotate ? -1 : 1;

        Vector2 rotatePosition = Parts[0].transform.position;

        for(int i = 0; i < Parts.Length; i++)
        {
            Parts[i].transform.RotateAround(rotatePosition, Vector3.forward, 90f * rotateMultiplier);
            Parts[i].transform.Rotate(Vector3.forward, -90f * rotateMultiplier);
        }

        _rotate = !_rotate;
    }
}
