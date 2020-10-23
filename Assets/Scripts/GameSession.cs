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
    [Tooltip("ссылка на аниматор главного персонажа со сцены")]
    //[SerializeField] private Animator _characterMain;    
    [SerializeField] private AnimatorMainCharacter _characterMain;    

    [Header("Установить колличество на уровне:")]
    [Tooltip("Количество врагов на уровне")]
    [SerializeField] private int _countEnemy;
    [Tooltip("Количество друзей на уровне")]
    [SerializeField] private int _countFrend;
    [Tooltip("Количество снарядов на уровне")]
    [SerializeField] private int _countProjectile;

    private Animator _mainCamera;
    //Для автоматической покраски и добавления
    //[SerializeField] private CharacterBehavior[] _characterBehavior;    
    //[SerializeField] private Material _enemy;
    //[SerializeField] private Material _frend;

    private void Awake()
    {
        _characterBehavior.ForEach(i => i.Died += IncreaseNumerDiedCharacter);
        _mainCamera = Camera.main.GetComponent<Animator>();
    }

    private void Start()
    {
        DataGame.isMainMenu = false;
        if (DataGame.isMainMenu)
        {
            _mainCamera.SetTrigger("PositionMainMenu");            
            _characterMain.FlyingMainMenu();
            StartCoroutine(TimerShowMainMenu());
        }
        else
        {
            StartPlayGame();
        }
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

    public List<CharacterBehavior> GetListCharacter()
    {
        return _characterBehavior;
    }

    public void StartPlayGame()//Установленн на кнопку Play в главном меню
    {
        if (DataGame.isMainMenu)//При запуске с кнопки деалем проверку
        {
            _mainCamera.SetTrigger("ChangePositionCameraForGame");
            DataGame.isMainMenu = false;
        }
        else
        {
            _mainCamera.SetTrigger("PositionPlayGame");
        }        
        _characterMain.PlayIdleCharacter();        
        _ui.HideMainMenu();        
    }


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
                StartCoroutine(ShowResult(stateEndGame));
            }
        }
        else
        {
            _countFrend -= 1;
            if (_countFrend == 0)
            {
                stateEndGame = StateEndGame.AllFriendsFell;
                StartCoroutine(ShowResult(stateEndGame));
            }
        }
    }

    private IEnumerator ShowResult(StateEndGame stateEndGame)
    {
        yield return new WaitForSeconds(3);
        _ui.ShowResultPanel(stateEndGame);
    }

    //Показать меню после прилета героя, анимация прилета занимает 2 сек
    private IEnumerator TimerShowMainMenu()
    {
        yield return new WaitForSeconds(2);
        _ui.ShowMainMenu();
    }
}
