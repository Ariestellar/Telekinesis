using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private ResultPanel _resultPanel;

    public void ShowResultPanel()
    {
        _resultPanel.gameObject.SetActive(true);
    }
}
