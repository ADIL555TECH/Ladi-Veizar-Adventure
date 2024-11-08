using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    [SerializeField] GameObject map;
    [SerializeField] GameObject miniMap;
    bool isBigMap = false;
    private void OnEnable() {
        GetComponent<PlayerController>().onMap+= ToggleMap;
    }
    private void OnDisable() {
        GetComponent<PlayerController>().onMap-= ToggleMap;
    }
    public void EnableBigMap()
    {
        
        Debug.Log("activates");
        map.SetActive(true);
        miniMap.SetActive(false);
        isBigMap = true;
    }
    public void DisableBigMap()
    {
        map.SetActive(false);
        miniMap.SetActive(true);
        isBigMap = false;
    }

    public void ToggleMap()
    {
        
        if(isBigMap) DisableBigMap();
        else
        {
            if( GameManager.Instance.GameState!=GameState.playing) return;
            EnableBigMap();
        } 
    }
}
