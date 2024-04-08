using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LoadRandomForest : MonoBehaviour
{
    public List<int> listNumbers = new List<int>();
    System.Random rand = new System.Random();
    void randomForest() {
        int number;
        for (int i = 0; i < 2; i++)
        {
          do {
             number = rand.Next(1,2);
          } while (listNumbers.Contains(number));
            listNumbers.Add(number);
        }
    }
}

