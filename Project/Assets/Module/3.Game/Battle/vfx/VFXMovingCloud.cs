using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VFXMovingCloud : MonoBehaviour
{
    [SerializeField] private Vector2 cloudRange = Vector2.zero;
    [SerializeField] private Vector2 rescaleRange = Vector2.one;
    [SerializeField] private float speed;
    private Transform[] clouds;
    void Start()
    {
        var _tempClouds = new HashSet<Transform>(GetComponentsInChildren<Transform>());
        _tempClouds.Remove(this.transform);
        clouds = _tempClouds.ToArray();
    }
    void Update()
    {
        foreach(var cloud in clouds)
        {
            cloud.localPosition += Vector3.right * speed * Time.deltaTime;
            if(cloud.localPosition.x<-cloudRange.x*0.5f)
            {
                cloud.localPosition = new Vector3(cloudRange.x*0.5f, cloud.localPosition.y, 0);
                cloud.localScale = Vector3.one*Random.Range(rescaleRange.x, rescaleRange.y);
            }
            else if(cloud.localPosition.x>cloudRange.x*0.5f)
            {
                cloud.localPosition = new Vector3(-cloudRange.x*0.5f, cloud.localPosition.y, 0);
                cloud.localScale = Vector3.one*Random.Range(rescaleRange.x, rescaleRange.y);
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(cloudRange.x, cloudRange.y, 0.01f));
    }
}