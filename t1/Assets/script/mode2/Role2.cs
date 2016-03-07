using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AutoRot))]
[RequireComponent(typeof(RunForward))]
[RequireComponent(typeof(AI2))]
public class Role2 : MonoBehaviour
{
    public delegate void ScoreUpdatedCallback(int newestScore);
    public ScoreUpdatedCallback scoreUpdatedCallback;

    private Transform _bombBindPoint;
    private AutoRot _autoRot;
    private RunForward _runForward;
    private AI2 _ai;
    private InputMgr _input;
    public InputMgr input
    {
        get { return _input; }
    }

    private int _score = 0;
    public int score
    {
        get { return _score; }
    }

    private bool _isActivated = false;
    public bool isActivated
    {
        get { return _isActivated; }
    }


    void Awake()
    {
        _bombBindPoint = transform.Find("bp_bomb");
        _autoRot = gameObject.GetComponent<AutoRot>();
        _runForward = gameObject.GetComponent<RunForward>();
        _ai = gameObject.GetComponent<AI2>();
    }

    public void SetInput(InputMgr input)
    {
        _input = input;
    }

    public void SetIsActivated(bool value)
    {
        _isActivated = value;

        _autoRot.enabled = value;
        _runForward.enabled = value;

        if (value && !input.isSelected)
            _ai.StartAI();
        else
            _ai.StopAI();
    }

    public void BindBomb(Bomb bomb)
    {
        Transform tran = bomb.transform;
        tran.parent = _bombBindPoint;
        tran.localPosition = Vector3.zero;
        tran.localRotation = Quaternion.identity;
        tran.localScale = Vector3.one;

        bomb.BindPlayer(this);
    }

    public void AddScore(int add)
    {
        _score += add;
        if (null != scoreUpdatedCallback)
            scoreUpdatedCallback(_score);
    }
}
