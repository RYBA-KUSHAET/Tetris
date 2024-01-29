using Unity.VisualScripting;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour
{
    public Shape[] ShapePrefabs = new Shape[0];

    public Shape SpawnNextShape()
    {
        Shape randomPrefab = ShapePrefabs[Random.Range(0, ShapePrefabs.Length)];

        return Instantiate(randomPrefab);
    }
}
