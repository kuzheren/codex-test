using UnityEngine;

public class SimpleArea : MonoBehaviour
{
    [Header("Floor Bounds")]
    public Vector2 size = new Vector2(10f, 10f); // x -> width, y -> depth (z)

    public Vector3 GetRandomPointOnFloor(float y = 0f)
    {
        float halfX = size.x * 0.5f;
        float halfZ = size.y * 0.5f;
        float x = Random.Range(-halfX, halfX);
        float z = Random.Range(-halfZ, halfZ);
        return new Vector3(x, y, z) + transform.localPosition;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = transform.position;
        Vector3 cubeSize = new Vector3(size.x, 0.05f, size.y);
        Gizmos.DrawWireCube(center, cubeSize);
    }
}