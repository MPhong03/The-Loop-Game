using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringToFront : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.SetAsLastSibling();
    }

}
