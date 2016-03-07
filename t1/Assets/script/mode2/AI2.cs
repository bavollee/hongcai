using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Role2))]
public class AI2 : MonoBehaviour
{
    public float runTimeMin = 0.5f;
    public float runTimeMax = 1f;

    public float thinkTimeMin = 0.5f;
    public float thinkTimeMax = 1f;

    private Role2 _player;
    private InputMgr _input;

    private float _runTime = 0f;
    private Coroutine _actCor = null;


    void Start()
    {
        _player = gameObject.GetComponent<Role2>();
        _input = _player.input;
    }

    public void StartAI()
    {
        _actCor = StartCoroutine(OnAction());
    }

    public void StopAI()
    {
        if (null != _actCor)
            StopCoroutine(_actCor);
    }

    private IEnumerator OnAction()
    {
        while (Main2.isStartGame)
        {
            _runTime = Random.Range(runTimeMin, runTimeMax);
            while (_runTime > 0)
            {
                _input.OnHandleInput(true);
                _runTime -= Time.deltaTime;
                yield return null;
            }

            _input.OnHandleInput(false);
            float thinkTime = Random.Range(thinkTimeMin, thinkTimeMax);
            yield return new WaitForSeconds(thinkTime);
        }

        yield return true;
    }
}
