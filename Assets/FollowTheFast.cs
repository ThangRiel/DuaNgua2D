using UnityEngine;

public class FollowTheFast : MonoBehaviour
{
    public GameObject[] gameObjects;
    public float xOffset = 0f; // Khoảng cách bù theo trục X nếu cần
    public float yOffset = 0f; // Khoảng cách bù theo trục Y nếu cần

    private float initialZ;

    void Start()
    {
        // Giữ nguyên vị trí Z ban đầu của Camera (thường là -10 trong game 2D)
        initialZ = transform.position.z;
    }

    void Update()
    {
        if (gameObjects == null || gameObjects.Length == 0) return;

        GameObject farthestTarget = null;
        float maxX = float.MinValue;

        // Tìm thằng có tọa độ X lớn nhất (xa nhất về bên phải)
        foreach (GameObject go in gameObjects)
        {
            if (go != null && go.transform.position.x > maxX)
            {
                maxX = go.transform.position.x;
                farthestTarget = go;
            }
        }

        // Nếu tìm thấy thằng xa nhất, cập nhật vị trí Camera
        if (farthestTarget != null)
        {
            Vector3 targetPosition = farthestTarget.transform.position;
            
            // Di chuyển X, Y theo target, giữ nguyên Z của Camera 2D
            transform.position = new Vector3(targetPosition.x + xOffset, transform.position.y, initialZ);
        }
    }
}