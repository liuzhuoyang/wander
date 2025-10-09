/*
using System;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

public class UnityServicesControl : MonoBehaviour
{
    public string environment = "production";

    public async void Init()
    {
        try
        {
            if (!CheckUnityServicesInitialization())
            {
                var options = new InitializationOptions()
               .SetEnvironmentName(environment);

                await UnityServices.InitializeAsync(options);
                Debug.Log("=== UnityServicesControl: init succeed ===");

                UnityLevelPlay.Instance.InitIronsource();
            }
        }
        catch (Exception exception)
        {
            Debug.Log(exception);
            // An error occurred during services initialization.
        }
    }

    bool CheckUnityServicesInitialization()
    {
        switch (UnityServices.State)
        {
            case ServicesInitializationState.Uninitialized:
                
                break;
            case ServicesInitializationState.Initializing:
                Debug.Log("=== UnityServicesControl: Unity Services are currently initializing. ===");
                break;
            case ServicesInitializationState.Initialized:
                Debug.Log("=== UnityServicesControl: trying to init Unity Services, but have laready been initialized. ===");
                return true;
        }

        return false;
    }
}
*/