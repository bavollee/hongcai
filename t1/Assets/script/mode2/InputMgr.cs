using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputMgr : MonoBehaviour
{
    public GameObject player;
    public KeyCode keyCode = KeyCode.None;
    public bool keyboardMode = true;

    private AutoRot _autoRotCom;
    private RunForward _runForwardCom;
    private bool _bRun = false;


    void Awake()
    {
#if UNITY_ANDROID
        keyboardMode = false;
#endif
    }

    void Start()
    {
        _autoRotCom = player.GetComponent<AutoRot>();
        _runForwardCom = player.GetComponent<RunForward>();

        UIEventListener.Get(gameObject).onPress += onPress;
    }

    void Update()
    {
        if (keyboardMode)
        {
            if (KeyCode.None != keyCode)
            {
                _bRun = Input.GetKey(keyCode);
            }
        }

        if (_bRun)
        {
            _runForwardCom.Run();
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void onPress(GameObject go, bool state)
    {
        if (state)
        {
            _bRun = true;
            _autoRotCom.enabled = false;
        }
        else
        {
            _bRun = false;

            _autoRotCom.ReverseRot();
            _autoRotCom.enabled = true;
        }
    }
}
