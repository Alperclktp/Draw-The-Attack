using System.Collections.Generic;
using UnityEngine;
using AppsFlyerSDK;

public class AppsFlyerController : MonoBehaviour
{
    [SerializeField] private string devkey = "KEY";
    [SerializeField] private string appid = "APP_ID";

    void Start()
    {
        Dictionary<string, string> customData = new Dictionary<string, string>();

        customData.Add("af_prt", "example_partener_name");
        AppsFlyer.setAdditionalData(customData);

        AppsFlyer.initSDK(devkey, appid);

        AppsFlyer.startSDK();
    }
}
