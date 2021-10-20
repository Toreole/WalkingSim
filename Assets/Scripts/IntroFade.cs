using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroFade : MonoBehaviour
{
    [SerializeField]
    Behaviour[] playerComponents;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        var text = GetComponentInChildren<Text>();
        Color color = text.color;
        for(float t = 0; t < 1; t += Time.deltaTime)
        {
            color.a = t;
            text.color = color;
            yield return null;
        }
        yield return new WaitForSeconds(5);

        var cg = GetComponent<CanvasGroup>();
        for(float t = 1; t > 0; t -= Time.deltaTime)
        {
            cg.alpha = t;
            yield return null;
        }
        gameObject.SetActive(false);
        foreach(var c in playerComponents)
            c.enabled = true;
    }
}
