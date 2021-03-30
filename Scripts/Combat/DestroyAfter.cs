using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{

    [SerializeField] private float timeToDestroy = 1f;

    private void Start()
    {
        Invoke(nameof(destoryAfter) , timeToDestroy);
    }

    private void destoryAfter()
    {
        Destroy(gameObject);
    }

}
