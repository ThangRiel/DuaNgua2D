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
    public float randomnessFactor = 0.5f; // Yếu tố ngẫu nhiên để thay đổi tốc độ liên tục
    private float currentSpeed;

    private float timer = 0f;
    private bool isStarted = false;
    private Animator animator;
    private bool isFinished = false;
    private int rank = 0;

    void Start()
    {
        // Mỗi con ngựa khi bắt đầu sẽ có một tốc độ ngẫu nhiên khác nhau
        currentSpeed = Random.Range(minSpeed, maxSpeed);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Nếu chưa bấm bắt đầu thì không làm gì cả, đứng im xếp hàng
        if (RaceManager.Instance == null || !RaceManager.Instance.isRacing) return;
        isStarted = true;

        if(animator!=null) animator.SetBool("isRunning", true);

        // Thay đổi tốc độ nhẹ liên tục để tạo cảm giác đua kịch tính
        if (Random.value > 1f - randomnessFactor)
        {
            currentSpeed = Random.Range(minSpeed, maxSpeed);
        }

        // Di chuyển ngựa
        transform.Translate(Vector2.right * currentSpeed * Time.deltaTime);

        // Giới hạn không cho chạy quá đích
        if (transform.position.x >= finishX)
        {
            transform.position = new Vector3(finishX, transform.position.y, transform.position.z);
            if(animator!=null) animator.SetBool("isRunning", false);
        }

        // Gửi dữ liệu lên web mỗi 0.3 giây (nhanh hơn cho mượt)
        timer += Time.deltaTime;
        if (timer >= 0.3f)
        {
            float totalDistance = finishX - startX;
            float currentDistance = transform.position.x - startX;
            float percentComplete = Mathf.Clamp((currentDistance / totalDistance) * 100f, 0f, 100f);
            if (percentComplete >= 100f && !isFinished)
            {
                isFinished = true;
                rank = RaceManager.Instance.GetRank(horseId);
            }
            StartCoroutine(SendDataToServer(horseId, percentComplete,rank));
            timer = 0f;
        }
    }

    IEnumerator SendDataToServer(string id, float percent, int currentRank)
    {
        string jsonData = "{\"horseId\":\"" + id + "\", \"percent\":" + percent + ", \"rank\":" + currentRank + "}";

        UnityWebRequest request = new UnityWebRequest(serverUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
    }
}