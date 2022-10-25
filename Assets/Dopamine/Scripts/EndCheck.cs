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
            GameManager.Instance.enemyList.RemoveAt(0);

            GameManager.Instance.FailLevel();

            for (int i = 0; i < GameManager.Instance.enemyList.Count; i++)
            {
                Destroy(GameManager.Instance.enemyList[i].gameObject);
            }
        }   
    }
}
