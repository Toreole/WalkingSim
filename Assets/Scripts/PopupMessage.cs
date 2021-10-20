using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMessage : MonoBehaviour
{
    private Coroutine routine;
    [SerializeField]
    private UnityEngine.UI.Text textElement;

    private static PopupMessage instance;

    public static void ShowMessage(string msg)
    {
        if(instance.routine != null)
            instance.StopCoroutine(instance.routine);
        instance.routine = instance.StartCoroutine(instance.DoShowMessage(msg));
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Color color = textElement.color;
        color.a = 0;
        textElement.color = color;
    }
    
    IEnumerator DoShowMessage(string message)
    {
        textElement.text = message;
        Color color = textElement.color;
        color.a = 1;
        textElement.color = color;
        yield return new WaitForSeconds(1.5f);
        for(float t = 1; t > 0; t -= Time.deltaTime)
        {
            color.a = t;
            textElement.color = color;
            yield return null;
        }
        color.a = 0;
        textElement.color = color;
    }
}
