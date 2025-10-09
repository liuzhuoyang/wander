using UnityEngine;

public class MapColliderView : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(GameConfig.main!= null && GameConfig.debugToolRunTime == DebugTool.Off)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
