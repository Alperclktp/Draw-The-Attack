using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: Singleton<T>
{
    public static T Instance;   
    public static T _instance { get { return Instance;} }
    public Singleton()
    {
        if(Instance == null)
        {
            Instance = this as T;         
        }
        else
        {
            Debug.LogWarning("There cannot be more than one of this object in the scene!");

            //Destroy the newly copied manager
        }
    }
}
