using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapManagerUI : MonoBehaviour
{
    public static LapManagerUI Instance;
    public Text TxtLapTimeInfo;

    LapManager lapManager;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        lapManager = GetComponent<LapManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLapTimeInfoText();
    }

    void UpdateLapTimeInfoText()
    {
        int currentLap = 0;
        if (lapManager.CurrentLap >= GameManager.Instance.TotalLaps)
        {
            currentLap = GameManager.Instance.TotalLaps;
        }
        TxtLapTimeInfo.text = "Laps: " + currentLap + "/" + GameManager.Instance.TotalLaps + "\n" +
            "Current: " +SecondsToTime(lapManager.CurrentLapTime) + "\n" +
            "Last: " + SecondsToTime(lapManager.LastLapTime) + "\n" +
            "Best: " + SecondsToTime(lapManager.BestLapTime) + "";
    }

    public string SecondsToTime(float seconds)
    {
        int displayMinutes = Mathf.FloorToInt(seconds / 60f);
        int displaySeconds = Mathf.FloorToInt(seconds % 60);
        int displayTenthSeconds = Mathf.FloorToInt((seconds - displaySeconds) * 10f);

        return displayMinutes + ":" + displaySeconds.ToString("00") + ":" + displayTenthSeconds;
    }
}
