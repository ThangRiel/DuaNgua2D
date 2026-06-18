using UnityEngine;
using System.Collections.Generic;
public class RaceManager : MonoBehaviour
{
    public static RaceManager Instance; // Để các con ngựa dễ dàng kiểm tra trạng thái
    public bool isRacing = false; // Trạng thái cuộc đua
    public bool isFinished = false; // Trạng thái kết thúc cuộc đua
 private List<string> rankings = new List<string>(); // Danh sách chốt sổ

    void Awake()
    {
        Instance = this;
    }

    // Hàm này sẽ gọi khi bấm nút Start
    public void StartRace()
    {
        isRacing = true;
        
    }
    public int GetRank(string horseId){
        if (!rankings.Contains(horseId))
        {
            rankings.Add(horseId);
        }
        return rankings.IndexOf(horseId) + 1; // Trả về 1, 2, 3...
    }
}