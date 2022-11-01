using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    [SerializeField] List<SingletonBase> managers = new List<SingletonBase>();

    private void Awake() => managers.ForEach(m => m?.Inýt());

}
