using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class DetectionZone : MonoBehaviour
{
    public List<Collider2D> detectedCols = new List<Collider2D> ();
    Collider2D col;
    public UnityEvent unityEventNoCol;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectedCols.Add (collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedCols.Remove(collision);

        if (detectedCols.Count <= 0)
        {
            unityEventNoCol.Invoke();
        }
    }
}
