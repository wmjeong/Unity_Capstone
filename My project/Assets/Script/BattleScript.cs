using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleScript : MonoBehaviour
{
    // 플레이어의 공격 수행 함수
    // 플레이어 공격 종료 후 EnemyAttack() 자동 진행
    // 적 처치시 승리처리 포함
    public void PlayerAttack()
    {
        float damage = GameManager.playerDamage;
        if(damage <= 0)
        { damage = 0; }

        // 방어력이 존재할 경우 방어력 우선 감소
        // 방어가 깨지면(0미만으로 감소) 그때부터 hp 감소
        if (GameManager.enemyStatus.defence > 0)
        {
            GameManager.enemyStatus.defence -= damage;
            if (GameManager.enemyStatus.defence < 0)
            {
                // 방어를 초과한 데미지만금 hp 감소, defence는 0으로 초기화
                GameManager.enemyStatus.hp += GameManager.enemyStatus.defence;
                GameManager.enemyStatus.defence = 2;
            }
        }

        else
        { GameManager.enemyStatus.hp -= damage; }

        if (GameManager.enemyStatus.hp < 0)
        { GameManager.enemyStatus.hp = 0; }

        if (GameManager.enemyStatus.hp == 0)
        {
            Victory();
        }

        EnemyAttack();
        GameManager.turnStart = true;        
    }

    // 방어력 증가 함수
    // 방어력 증가 후 EnemyAttack() 자동 진행
    public void GetDefence()
    {
        float defence = GameManager.playerDamage;
        GameManager.playerStatus.defence = defence;
        EnemyAttack();

        GameManager.turnStart = true;
    }

    // 적의 공격 수행 함수
    // 적의 공격으로 플레이어 사망시 처리하는 기능 포함
    public void EnemyAttack()
    {
        float damage = GameManager.enemyDamage;
        if(damage <= 0)
        { damage = 0; }

        // 방어력이 존재할 경우 방어력 우선 감소
        // 방어가 깨지면(0미만으로 감소) 그때부터 hp 감소
        if(GameManager.playerStatus.defence > 0)
        {
            GameManager.playerStatus.defence -= damage;
            if(GameManager.playerStatus.defence < 0)
            {
                // 방어를 초과한 데미지만금 hp 감소, defence는 0으로 초기화
                GameManager.playerStatus.hp += GameManager.playerStatus.defence;
                GameManager.playerStatus.defence = 0;
            }
        }

        else
        { GameManager.playerStatus.hp -= damage; }
        
        if(GameManager.playerStatus.hp < 0)
        { GameManager.playerStatus.hp = 0; }

        if(GameManager.playerStatus.hp == 0)
        { 
            Debug.Log("패배...");
            GameManager.inBattle = false;
            SceneManager.LoadScene("0.MainScene");
            GameManager.cameraSelect = CAMERA_TYPE.MAIN;            
        }
    }


    void Victory()
    {
        Debug.Log("승리!");
        GameManager.inBattle = false;
        GameManager.cameraSelect = CAMERA_TYPE.MAIN;

        switch (GameManager.difficulty)
        {
            case DIFFICULTY.EASY:
                int eRand = Random.Range(5, 25);
                GameManager.playerGold += eRand;
                break;
            case DIFFICULTY.NORMAL:
                int nRand = Random.Range(10, 30);
                GameManager.playerGold += nRand;
                break;
            case DIFFICULTY.HARD:
                int hRand = Random.Range(15, 40);
                GameManager.playerGold += hRand;
                break;
        }
    }
    
}
