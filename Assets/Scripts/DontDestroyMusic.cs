using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMusic : MonoBehaviour
{
    public static GameObject musicObj = null;

    void Start()
    {
        if(DontDestroyMusic.musicObj == null)
        {
            DontDestroyOnLoad(this.gameObject);
            DontDestroyMusic.musicObj = this.gameObject;
        }
        else
            Destroy(this.gameObject);
    }

}
