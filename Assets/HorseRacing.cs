using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class HorseRacing : MonoBehaviour
{
    [Header("Cấu hình định danh")]
    public string horseId; // Đặt ID riêng cho mỗi con (Ngựa 1, Ngựa 2...)
    public string serverUrl = "http://localhost:8080/api/update";

    [Header("Thông số đường đua")]
    public float startX = -8f;
    public float finishX = 8f;

    [Header("Tốc độ")]
    public float minSpeed = 2f;
    public float maxSpeed = 5f;
    private float currentSpeed;

    private float timer = 0f;

    void Start()
    {
        // Mỗi con ngựa khi bắt đầu sẽ có một tốc độ ngẫu nhiên khác nhau
        currentSpeed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        // Thay đổi tốc độ nhẹ liên tục để tạo cảm giác đua kịch tính
        if (Random.value > 0.98f)
        {
            currentSpeed = Random.Range(minSpeed, maxSpeed);
        }

        // Di chuyển ngựa
        transform.Translate(Vector2.right * currentSpeed * Time.deltaTime);

        // Giới hạn không cho chạy quá đích
        if (transform.position.x >= finishX)
        {
            transform.position = new Vector3(finishX, transform.position.y, transform.position.z);
        }

        // Gửi dữ liệu lên web mỗi 0.3 giây (nhanh hơn cho mượt)
        timer += Time.deltaTime;
        if (timer >= 0.3f)
        {
            float totalDistance = finishX - startX;
            float currentDistance = transform.position.x - startX;
            float percentComplete = Mathf.Clamp((currentDistance / totalDistance) * 100f, 0f, 100f);

            StartCoroutine(SendDataToServer(horseId, percentComplete));
            timer = 0f;
        }
    }

    IEnumerator SendDataToServer(string id, float percent)
    {
        string jsonData = "{\"horseId\":\"" + id + "\", \"percent\":" + percent + "}";
        UnityWebRequest request = new UnityWebRequest(serverUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
    }
}