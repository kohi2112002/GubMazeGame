using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePath : MonoBehaviour
{
    IEnumerator WaitAndDestroy(int secs)
    {
        yield return new WaitForSeconds(secs);
        Destroy(gameObject);
    }

    public void Delete()
    {
        StartCoroutine(WaitAndDestroy(5));
    }
}
