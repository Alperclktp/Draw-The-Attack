using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EndCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            TowerManager.Instance.canSpawn = false;

            GameManager.Instance.enemyList.RemoveAt(0);

            GameManager.Instance.FailLevel();

            foreach (var item in GameManager.Instance.enemyList.Where(_ => _ != null && !_.name.Contains("Tower")))
                Destroy(item);

            for (int i = 0; i < GameManager.Instance.soldierList.Count; i++)
            {
                Destroy(GameManager.Instance.soldierList[i].gameObject);
            }
        }   
    }
}
