using UnityEngine;
using System.Collections;

public class Driver : MonoBehaviour
{
    public GameObject modeBtn1;
    public GameObject modeBtn2;


    void Awake()
    {
        UIEventListener.Get(modeBtn1).onClick = OnStartMode1;
        UIEventListener.Get(modeBtn2).onClick = OnStartMode2;
    }

    private void OnStartMode1(GameObject go)
    {
        Application.LoadLevel("game");
    }

    private void OnStartMode2(GameObject go)
    {
//         Application.LoadLevel("game2");
        Debug.LogWarning("开发中，请稍候~");
    }
}
