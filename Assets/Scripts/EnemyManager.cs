using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    Dictionary<int, List<EnemyAIController>> m_enemyMap = new Dictionary<int, List<EnemyAIController>>();

    private float m_freezeTimer = 0f;
    private bool m_isPaused = false;

    private void Update()
    {
        if(m_isPaused)
        {
            PauseAllEnemy();
            return;
        }

        m_freezeTimer -= Time.deltaTime;
        if (m_freezeTimer < 0f)
        {
            UnPauseAllEnemy();
        }
        else
        {
            PauseAllEnemy();
        }
    }

    public void AddEnemy(int roomIndex, EnemyAIController enemy)
    {
        // Check if the room index already exist
        if(m_enemyMap.ContainsKey(roomIndex))
        {
            // If it already exist, we can just get the list and add it
            List<EnemyAIController> enemys = m_enemyMap[roomIndex];
            enemys.Add(enemy);
        }
        else
        {
            // If it does not exist, we create the list and add it
            List<EnemyAIController> enemysList = new List<EnemyAIController>();
            enemysList.Add(enemy);
            m_enemyMap.Add(roomIndex, enemysList);
        }
    }

    public int GetEnemyAliveAtRoom(int roomIndex)
    {
        List<EnemyAIController> enemys = m_enemyMap[roomIndex];
        int enemyAliveCounter = 0;

        foreach(EnemyAIController enemy in enemys)
        {
            if(enemy.GetIsAlive())
                enemyAliveCounter++;
        }

        return enemyAliveCounter;
    }

    public void ResetEnemys()
    {
        foreach (KeyValuePair<int, List<EnemyAIController>> enemyList in m_enemyMap)
        {
            foreach(EnemyAIController enemy in enemyList.Value)
            {
                enemy.ResetToDefault();
            }
        }
    }

    public void FreezeAll(float freezeTime)
    {
        m_freezeTimer = freezeTime;
    }

    public void SetPaused(bool isPaused) 
    { 
        m_isPaused = isPaused;
    }

    void PauseAllEnemy()
    {
        foreach (KeyValuePair<int, List<EnemyAIController>> enemyList in m_enemyMap)
        {
            foreach (EnemyAIController enemy in enemyList.Value)
            {
                enemy.SetIsPaused(true);
            }
        }
    }

    void UnPauseAllEnemy()
    {
        foreach (KeyValuePair<int, List<EnemyAIController>> enemyList in m_enemyMap)
        {
            foreach (EnemyAIController enemy in enemyList.Value)
            {
                enemy.SetIsPaused(false);
            }
        }
    }
}
