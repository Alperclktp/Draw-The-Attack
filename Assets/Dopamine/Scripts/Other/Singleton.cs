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
            Debug.LogWarning("Cannot have singelton more than one on scene!");
            Debug.Log("There are multiple singletons on scene");
        }
    }
}
