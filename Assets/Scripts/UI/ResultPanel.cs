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
            _textStatusCharacter.text = "Victory! All enemies are defeated";
        }
        else if(stateEndGame == StateEndGame.AllFriendsFell)
        {
            _textStatusCharacter.text = "Nooooo! We accidentally pushed our friends down :(";
            _buttonNext.gameObject.SetActive(false);
        }             
    }

    public void SetTextStatusShell()
    {
        _textStatusShell.gameObject.SetActive(true);
        _textStatusShell.text = "Shells run out";
        _buttonNext.gameObject.SetActive(false);
    }

    private void PressButtonRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void PressButtonNext()
    {
        DataGame.LevelUp();        
        SceneManager.LoadScene("Level" + DataGame.currentLevel);
    }
}
