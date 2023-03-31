using System.Collections.Generic;
using UnityEngine;
using AppsFlyerSDK;

public class AppsFlyerController : MonoBehaviour
{
    [SerializeField] protected private string devkey = "KEY";
    [SerializeField] protected private string appid = "APP_ID";

    void Start()
    {
        Dictionary<string, string> customData = new Dictionary<string, string>();

        customData.Add("af_prt", "example_partener_name");
        AppsFlyer.setAdditionalData(customData);

        AppsFlyer.initSDK(devkey, appid);

        AppsFlyer.startSDK();

        if(devkey == "KEY" && appid == "APP_ID")
        {
            Debug.LogError("You entered invalid key and ID for AppsFlyer SDK, please check again.");
        }
        else
        {
            Debug.Log("AppsFlyer SDK activated successfully!");
        }
    }
}
