using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class LightningScript : MonoBehaviour
{
    public float destroyDelay = 0.2f;
    void Start()
    {
        Destroy(gameObject, destroyDelay);
    }
}
