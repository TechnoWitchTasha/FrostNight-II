using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This class lists all current Settings variables and their base values.
public class SettingGlobals : MonoBehaviour
{
    public static SettingGlobals Singleton { get; private set;}
    private void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogError("There is more than one SettingGlobals instance");
        }
        Singleton = this;
    }
    public float sensX, sensY;
    public float baseSensX, baseSensY;
    private void Start(){
        this.baseSensX = 4f;
        this.baseSensY = 4f;

        this.sensX = this.baseSensX;
        this.sensY = this.baseSensY;
    }
}
