using UnityEngine;
using System;

public class CloudControl : MonoBehaviour
{
    public void Init(bool isUnknownUser, Action<bool> onSuccess, Action onFailure, Action onTimeout)
    {
        gameObject.AddComponent<CloudVersion>();
        gameObject.AddComponent<CloudMail>();
        gameObject.AddComponent<CloudProgress>();
        gameObject.AddComponent<CloudGroupAB>();
        gameObject.AddComponent<CloudAccess>();
        gameObject.AddComponent<CloudAccount>().Init(isUnknownUser,
                    (isNewRegisterUser) =>
                    {
                        onSuccess?.Invoke(isNewRegisterUser);
                    },
                    () =>
                    {
                        onFailure?.Invoke();
                    },
                    () =>
                    {
                        onTimeout?.Invoke();
                    });
    }
}