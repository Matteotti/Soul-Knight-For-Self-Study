using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disappear : MonoBehaviour
{
    public void ObjectDisappear()
    {
        this.gameObject.SetActive(false);
    }
}
