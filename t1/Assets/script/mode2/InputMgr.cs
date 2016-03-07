using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputMgr : MonoBehaviour
{
    public string playerName;
    public GameObject playerGO;
    public KeyCode keyCode = KeyCode.None;
    public bool keyboardMode = true;
    public Color color;

    private AutoRot _autoRotCom;
    private RunForward _runForwardCom;
    private bool _bRun = false;

    private UILabel _score;
    private Role2 _player;


    void Awake()
    {
#if UNITY_ANDROID
        keyboardMode = false;
#endif
    }

    void Start()
    {
        _autoRotCom = playerGO.GetComponent<AutoRot>();
        _runForwardCom = playerGO.GetComponent<RunForward>();

        _score = transform.Find("Label").gameObject.GetComponent<UILabel>();
        _player = playerGO.GetComponent<Role2>();
        _player.scoreUpdatedCallback += OnScoreUpdated;

        UIEventListener.Get(gameObject).onPress += onPress;
    }

    private void OnScoreUpdated(int newestScore)
    {
        _score.text = newestScore.ToString();
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
