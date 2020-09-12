using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField] private List<CharacterBehavior> _characterBehavior;       

    [SerializeField] private int _projectileFired;
    [SerializeField] private int _countDeadFrends;
    [SerializeField] private int _countDeadEnemy;
    [SerializeField] private UI _ui;

    private void Awake()
    {
        _characterBehavior.ForEach(i => i.Died += IncreaseNumerDiedCharacter);//подписываем все персонажей на сцене на событие
    }

    public void IncreaseNumerProjetilesFired()
    {
        _projectileFired += 1;
        if (_projectileFired == 2)
        {
            //После последнего запуска ждем 3 секунды и показываем
            StartCoroutine(ShowResult());
        }
    }

    public void IncreaseNumerDiedCharacter(TypeCharacter typeCharacter)
    {
        if (typeCharacter == TypeCharacter.Enemy)
        {
            _countDeadEnemy += 1;
        }
        else
        {
            _countDeadFrends += 1;
        }

        if (_countDeadEnemy == 4 || _countDeadFrends == 2)
        {
            StartCoroutine(ShowResult());
        }
    }

    private IEnumerator ShowResult()
    {
        yield return new WaitForSeconds(3);
        _ui.ShowResultPanel();
    }
}
