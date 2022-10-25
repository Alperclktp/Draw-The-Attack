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
            GameManager.Instance.FailLevel();

            for (int i = 0; i < GameManager.Instance.enemyList.Count; i++)
            {
                GameManager.Instance.enemyList[i].GetComponent<NavMeshAgent>().speed = 0f;
                GameManager.Instance.enemyList[i].GetComponentInChildren<Animator>().enabled = false;
            }
        }   
    }
}
