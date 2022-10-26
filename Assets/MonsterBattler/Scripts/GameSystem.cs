using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum GameStates { player, enemy, win, lose } //game states
public enum EnemyStates { neutral, attacking, healing} //enemy states

public class GameSystem : MonoBehaviour
{
    #region Variables
    [Header("Action Texts")]
    //texts which tell the player which state the game and enemy has entered
    public Text enemyStatesText;
    public Text gameStatesText;

    [Header("Action Buttons")]
    //player's heal and attack buttons, will be disabled when its enemy's turn
    public Button healButton;
    public Button damageButton;
    public Button superDamageButton;

    [Header("Current States")]
    //tells the player which state the game and enemy has entered
    public GameStates gameState;
    public EnemyStates enemyState;

    //each fighter's classes
    public PlayerFighter _player;
    public EnemyFighter _enemy;
    #endregion

    public void Start()
    {
        //when starting, game is firstly player's turn, and enemy is not doing anything
        gameState = GameStates.player;
        enemyState = EnemyStates.neutral;

        //tells player the states for game and enemy
        enemyStatesText.text = "Enemy State: Neutral";
        gameStatesText.text = "Game State: Your Turn";
    }

    #region States
    public void NextGameState()
    {
        //player = player's turn
        //enemy = enemy's turn
        //win = player wins, enemy dies
        //lose = enemy wins, player dies
        switch (gameState)
        {
            case GameStates.player:
                StartCoroutine(PlayerTurn());
                break;
            case GameStates.enemy:
                StartCoroutine(EnemyTurn());
                break;
            case GameStates.win:
                StartCoroutine(Win());
                break;
            case GameStates.lose:
                StartCoroutine(Lose());
                break;
            default:
                break;
        }
    }

    public void NextEnemyState()
    {
        //neutral = not doing anything
        //attacking = attacks player
        //healing = heals self if health is low
        switch (enemyState)
        {
            case EnemyStates.neutral:
                StartCoroutine(Neutral());
                break;
            case EnemyStates.attacking:
                StartCoroutine(Attack());
                break;
            case EnemyStates.healing:
                StartCoroutine(Healng());
                break;
            default:
                break;
        }
    }

    #region Game States
    public IEnumerator PlayerTurn()
    {
        //if its player's turn
        if (gameState == GameStates.player)
        {
            //lets player know its player's turn
            gameStatesText.text = "Game State: Your Turn";
            gameStatesText.color = Color.red;

            yield return new WaitForSeconds(1f);

            gameStatesText.color = Color.white;

            //enables buttons, used for future turns after enemy's turns
            healButton.interactable = true;
            damageButton.interactable = true;
        }
        yield return null;
    }

    public IEnumerator EnemyTurn()
    {
        //if its enemy's turn
        if (gameState == GameStates.enemy)
        {
            //lets player know its enemy's turn
            gameStatesText.text = "Game State: Enemy's Turn";
            gameStatesText.color = Color.red;

            yield return new WaitForSeconds(1f);

            gameStatesText.color = Color.white;

            //enemy decides which action to take according to their health status
            DecideAction();
        }

        yield return null;
    }
    public IEnumerator Win()
    {
        //if player wins
        if (gameState == GameStates.win)
        {

        }
        yield return null;
    }

    public IEnumerator Lose()
    {
        //if player loses
        if (gameState == GameStates.lose)
        {

        }
        yield return null;
    }
    #endregion
    #region Enemy States
    public IEnumerator Neutral()
    {
        //does nothing, just here to let player know enemy is not attacking or healing (or another state in future)
        yield return null;
    }
    public IEnumerator Attack()
    {
        yield return new WaitForSeconds(1f);

        //checks if its enemy's turn
        if (gameState == GameStates.enemy)
        {
            //checks if health is high enough to prioritise attacking
            if (enemyState == EnemyStates.attacking)
            {
                if (_enemy.stamina >= 50)
                {
                    //if enemy's stamina is 50 or more, enemy will do super attack
                    enemyStatesText.text = "Enemy States: Super Attack";
                    enemyStatesText.color = Color.red;

                    yield return new WaitForSeconds(1f);

                    enemyStatesText.color = Color.white;

                    //attacks player with 30 damage
                    _player.Damage(30);
                }
                else
                {
                    //if enemy's stamina is less than 50, enemy will do normal attacks
                    enemyStatesText.text = "Enemy States: Attack";
                    enemyStatesText.color = Color.red;

                    yield return new WaitForSeconds(1f);

                    enemyStatesText.color = Color.white;

                    //attacks player with 10 damage
                    _player.Damage(10);
                }
            }
        }

        //after turn, enemy becomes neutral
        enemyState = EnemyStates.neutral;
        NextEnemyState();

        //switches to player's turn
        gameState = GameStates.player;
        NextGameState();        
    }

    public IEnumerator Healng()
    {
        yield return new WaitForSeconds(1f);

        //checks if its enemy's turn
        if (gameState == GameStates.enemy)
        {
            //checks if health is low and requires healing
            if (enemyState == EnemyStates.healing)
            {
                //lets player know enemy is healing
                enemyStatesText.text = "Enemy States: Healing";
                enemyStatesText.color = Color.red;

                yield return new WaitForSeconds(1f);

                enemyStatesText.color = Color.white;

                //heals enemy's health by 20
                _enemy.Heal(20);

                //increases enemy's stamina by 10
                _enemy.stamina += 10;
            }
        }

        //after turn, enemy becomes neutral
        enemyState = EnemyStates.neutral;
        NextEnemyState();

        //switches to player's turn
        gameState = GameStates.player;
        NextGameState();
    }

    public void DecideAction()
    {
        //checks enemy's health status
        //low = 50% or less of max health
        if (_enemy.isHealthLow())
        {
            //if health is low, requires healing
            enemyState = EnemyStates.healing;
            NextEnemyState();
        }
        else
        {
            //if health is still high, attacks player
            enemyState = EnemyStates.attacking;
            NextEnemyState();
        }
    }
    #endregion
    #endregion

    public void OnAttack()
    {
        //when either button is clicked, disabled both, prevents player from clicking buttons during enemy's turns
        healButton.interactable = false;
        damageButton.interactable = false;

        //switches to enemy's turn
        gameState = GameStates.enemy;
        NextGameState();
    }

    public void OnHeal()
    {
        healButton.interactable = false;
        damageButton.interactable = false;

        //increases player stamina after turn
        _player.stamina += 10;

        //switches to enemy's turn
        gameState = GameStates.enemy;
        NextGameState();
    }

    public void EndGame()
    { 
        Application.Quit();
    }
}
