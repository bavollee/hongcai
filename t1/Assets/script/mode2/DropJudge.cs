using UnityEngine;
using System.Collections;

public class DropJudge : MonoBehaviour
{
    private GameObject[] _players;
    private int _droppedCount = 0;


    void Start()
    {
        _players = GameObject.FindGameObjectsWithTag("Player");
        _droppedCount = 0;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.SetActive(false);

            ++_droppedCount;
            if (_droppedCount >= _players.Length - 1)
            {
                Time.timeScale = 0;
                _droppedCount = 0;
                Debug.LogWarning("Game over.");
            }
        }
    }
}
