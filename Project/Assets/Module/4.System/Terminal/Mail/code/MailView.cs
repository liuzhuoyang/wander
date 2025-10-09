using System.Collections.Generic;
using UnityEngine;

public class MailView : MonoBehaviour
{
    [SerializeField] GameObject prefabSlot;
    [SerializeField] Transform container;
    [SerializeField] GameObject objEmpty;

    public void Init(List<MailArgs> listMailUIArgs)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
        if (listMailUIArgs.Count == 0)
        {
            objEmpty.SetActive(true);
        }
        else
        {
            objEmpty.SetActive(false);
            foreach (MailArgs viewArgs in listMailUIArgs)
            {
                GameObject go = Instantiate(prefabSlot, container);
                //判断是否是功能邮件
                if (viewArgs.isGM)
                {
                    go.GetComponent<MailSlot>().InitGMMail(viewArgs);
                }
                else
                {
                    go.GetComponent<MailSlot>().Init(viewArgs);
                }
            }
        }
    }
}
