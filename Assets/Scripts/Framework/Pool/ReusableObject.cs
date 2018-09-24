using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ReusableObject : MonoBehaviour, IReusable
{
    public abstract void OnSpawn();
    public abstract void OnUnspawn();
}
