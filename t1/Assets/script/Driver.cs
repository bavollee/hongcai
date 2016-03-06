using UnityEngine;
using System.Collections;

public class Driver : MonoBehaviour
{
    public GameObject modeBtn1;
    public GameObject modeBtn2;
    public GameMgr gameMgr;
    public static int mode = 0;

    void Awake()
    {
        if(GameMgr.instance != null)
        {
            DestroyImmediate(GameMgr.instance.gameObject);
        }
        GameMgr.instance = gameMgr.GetComponent<GameMgr>();
        DontDestroyOnLoad(gameMgr);
        UIEventListener.Get(modeBtn1).onClick = OnStartMode1;
        UIEventListener.Get(modeBtn2).onClick = OnStartMode2;
    }

    private void OnStartMode1(GameObject go)
    {
        mode = 1;
        Application.LoadLevel("game");
    }

    private void OnStartMode2(GameObject go)
    {
        mode = 2;
        Application.LoadLevel("game2");
    }
}
