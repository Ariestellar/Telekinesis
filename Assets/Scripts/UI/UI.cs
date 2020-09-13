using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private ResultPanel _resultPanel;

    public void ShowResultPanel(StateEndGame stateEndGame)
    {
        _resultPanel.gameObject.SetActive(true);
        if (stateEndGame == StateEndGame.ShellsRunOut)
        {
            _resultPanel.SetTextStatusShell();            
        }
        else
        {
            _resultPanel.SetTextStatusCharacter(stateEndGame);
        }
        
    }
}
