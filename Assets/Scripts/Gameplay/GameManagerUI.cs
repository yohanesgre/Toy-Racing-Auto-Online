using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerUI : MonoBehaviour
{
    public static GameManagerUI Instance;
    public Text TxtDebugger1;
    public Text TxtDebugger2;

    public GameObject PanelCountdownStartGame;
    public Text TxtCountdownStartGame;
    public Text TxtGameOver;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonFlipScreen()
    {
        //if (Screen.orientation == ScreenOrientation.LandscapeLeft)
        //    Screen.orientation = ScreenOrientation.LandscapeRight;
        //if (Screen.orientation == ScreenOrientation.LandscapeRight)
        //    Screen.orientation = ScreenOrientation.LandscapeLeft;
        LapManager.Instance.CurrentLap = 4;
    }

    public void ButtonThrottleDown()
    {
        GameManager.Instance.FactoryPlayer.Get(0).IsThrottle = true;
    }

    public void ButtonThrottleUp()
    {
        GameManager.Instance.FactoryPlayer.Get(0).IsThrottle = false;
    }

    public void ButtonReverseDown()
    {
        GameManager.Instance.FactoryPlayer.Get(0).IsReverse = true;
    }

    public void ButtonReverseUp()
    {
        GameManager.Instance.FactoryPlayer.Get(0).IsReverse = false;
    }

}
