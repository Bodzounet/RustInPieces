using UnityEngine;
using System.Collections;

public class MarkBehaviour : MonoBehaviour
{
    Coroutine routine;

    void OnMouseEnterCustom()
    {
        if (routine == null)
        {
            routine = StartCoroutine(Co_TargetHoovered());
        }
    }

    void OnMouseExitCustom()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
            transform.localScale = Vector3.one;
        }
    }

    private IEnumerator Co_TargetHoovered()
    {
        float t = 0;

        while (true)
        {
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 2, Mathf.PingPong(t, 1));
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;

        }
    }
}
