using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] private Text _textStatusCharacter;
    [SerializeField] private Text _textStatusShell;
    [SerializeField] private Button _buttonRestart;
    [SerializeField] private Button _buttonNext;

    private void Awake()
    {
        _buttonRestart.onClick.AddListener(PressButtonRestart);
        _buttonNext.onClick.AddListener(PressButtonNext);
    }

    public void SetTextStatusCharacter(StateEndGame stateEndGame)
    {
        _textStatusCharacter.gameObject.SetActive(true);
        if (stateEndGame == StateEndGame.AllEnemiesFell)
        {
            _textStatusCharacter.text = "All enemies fell";
        }
        else if(stateEndGame == StateEndGame.AllFriendsFell)
        {
            _textStatusCharacter.text = "All friends fell";
        }             
    }

    public void SetTextStatusShell()
    {
        _textStatusShell.gameObject.SetActive(true);
        _textStatusShell.text = "Shells run out";
    }

    private void PressButtonRestart()
    {
        SceneManager.LoadScene("Level1");
    }

    private void PressButtonNext()
    {
        SceneManager.LoadScene("Level1");
    }
}
