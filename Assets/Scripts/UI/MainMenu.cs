using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _buttonPlay;
    [SerializeField] private GameObject _buttonTeam;
    [SerializeField] private GameObject _nameGame;

    public void ShowMainMenu()
    {
        _nameGame.gameObject.SetActive(true);
        _buttonTeam.gameObject.SetActive(true);
        StartCoroutine(TimerPlay());
    }

    private IEnumerator TimerPlay()
    {
        yield return new WaitForSeconds(0);
        _buttonPlay.SetActive(true);
    }   
}

