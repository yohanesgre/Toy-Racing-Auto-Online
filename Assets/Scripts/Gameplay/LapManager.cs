using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapManager : MonoBehaviour
{
    public static LapManager Instance;
    public float CurrentLapTime
    {
        get
        {
            if (isLapStarted == false)
            {
                return 0f;
            }
            return Time.realtimeSinceStartup - currentLapStartTime; 
        }
    }

    public float LastLapTime { get; private set; }
    public float BestLapTime { get; private set; }
    public float TotalLapTime { get; private set; }
    public int CurrentLap { get => currentLap; set => currentLap = value; }

    private bool isLapStarted = false;
    private float currentLapStartTime;
    int lastLapLineIndex = 0;    
    int highestLapLine;
    int currentLap;

    void Start()
    {
        Instance = this;
        highestLapLine = GetHighestLapLine();
    }

    int GetHighestLapLine()
    {
        LapLine[] lapLines = GetComponentsInChildren<LapLine>();
        int _highestLapLine = 0;
        for (int i = 0; i < lapLines.Length; i++)
        {
            _highestLapLine = Mathf.Max(_highestLapLine, lapLines[i].Index);
        }
        return _highestLapLine;
    }

    public void OnLapLinePassed(int index)
    {
        if (index == 0)
        {
            if (isLapStarted == false || lastLapLineIndex == highestLapLine)
            {
                OnFinishLinePassed();
            }
        }
        else
        {
            if (index == lastLapLineIndex + 1)
            {
                lastLapLineIndex = index;
            }
        }
    }

    void OnFinishLinePassed()
    {
        if (isLapStarted == true)
        {
            LastLapTime = Time.realtimeSinceStartup - currentLapStartTime;
            TotalLapTime += LastLapTime;

            if (LastLapTime < BestLapTime || BestLapTime == 0f)
            {
                BestLapTime = LastLapTime;
            }
        }
        isLapStarted = true;
        currentLapStartTime = Time.realtimeSinceStartup;
        CurrentLap++;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
