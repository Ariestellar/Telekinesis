using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [Header("Установить ссылки со сцены:")]
    [Tooltip("ссылка на UI")]
    [SerializeField] private UI _ui;
    [Tooltip("ссылки на всех персонажей со сцены")]
    [SerializeField] private List<CharacterBehavior> _characterBehavior;

    [Header("Установить колличество на уровне:")]
    [Tooltip("Количество врагов на уровне")]
    [SerializeField] private int _countEnemy;
    [Tooltip("Количество друзей на уровне")]
    [SerializeField] private int _countFrend;
    [Tooltip("Количество снарядов на уровне")]
    [SerializeField] private int _countProjectile;

    //Для автоматической покраски и добавления
    //[SerializeField] private CharacterBehavior[] _characterBehavior;    
    //[SerializeField] private Material _enemy;
    //[SerializeField] private Material _frend;

    private void Awake()
    {
        _characterBehavior.ForEach(i => i.Died += IncreaseNumerDiedCharacter);
    }

#region Проверить затратность автоматического добавления и расскраски персонажей по типу:
    //Проверить затратность автоматического добавления и расскраски персонажей по типу:
    /*private void Start()
    {        
        _characterBehavior = FindObjectsOfType<CharacterBehavior>(); 
        foreach (var character in _characterBehavior)
        {            
            if (character.GetTypeCharacter() == TypeCharacter.Enemy)
            {
                _countEnemy += 1;
                character.ChangeAppearance(_enemy);                
            }
            else if(character.GetTypeCharacter() == TypeCharacter.Frend)
            {
                _countFrend += 1;
                character.ChangeAppearance(_frend);                
            }
            else
            {
                Debug.Log("Несуществующий тип персонажа");
            }
            character.Died += IncreaseNumerDiedCharacter; //подписываем всеx персонажей на сцене на событие
        }
    }*/

#endregion

    public void IncreaseNumerProjetilesFired()
    {
        _countProjectile -= 1;
        if (_countProjectile == 0)
        {
            //После последнего запуска ждем 3 секунды и показываем
            StartCoroutine(ShowResult(StateEndGame.ShellsRunOut));
        }
    }

    public void IncreaseNumerDiedCharacter(TypeCharacter typeCharacter)
    {
        StateEndGame stateEndGame = 0;
        if (typeCharacter == TypeCharacter.Enemy)
        {
            _countEnemy -= 1;
            if (_countEnemy == 0)
            {
                stateEndGame = StateEndGame.AllEnemiesFell;
            }
        }
        else
        {
            _countFrend -= 1;
            if (_countFrend == 0)
            {
                stateEndGame = StateEndGame.AllFriendsFell;
            }
        }

        if (_countEnemy == 0 || _countFrend == 0)
        {
            StartCoroutine(ShowResult(stateEndGame));
        }
    }

    private IEnumerator ShowResult(StateEndGame stateEndGame)
    {
        yield return new WaitForSeconds(3);
        _ui.ShowResultPanel(stateEndGame);
    }
}
