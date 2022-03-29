using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObject : MonoBehaviour
{
    public void HideMe()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
    }
}
