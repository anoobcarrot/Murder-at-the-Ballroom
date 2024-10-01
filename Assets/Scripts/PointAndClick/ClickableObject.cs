using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObject : MonoBehaviour, IClickable
{
    public void OnClick()
    {
        Debug.Log("Object clicked: " + gameObject.name);
        // Add your click behavior here
    }
}
