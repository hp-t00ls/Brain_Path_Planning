using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point_initialization : MonoBehaviour
{
      public GameObject startingPointPrefab;
      public GameObject targetPointPrefab;

      private GameObject startingPoint;
      private GameObject targetPoint;
    // Start is called before the first frame update
    void Start()
    { 
        InitializeStartingPoint();
        InitializeTargetPoint();
    }

    // Update is called once per frame
    void InitializeStartingPoint()
    {
        Vector3 startingPosition = new Vector3(1, -14.7f, -74.1f);
        startingPoint = Instantiate(startingPointPrefab, startingPosition, Quaternion.identity);
        startingPoint.name = "Starting Point";
        Debug.Log("Starting Point initialized at: " + startingPosition);
    }

    void InitializeTargetPoint()
    {
        // Remplacez cette position par la position réelle de votre cible (tumeur)
        Vector3 targetPosition = new Vector3(-1.2f, 32.9f, -58.3f); // Exemple de position de la tumeur
        targetPoint = Instantiate(targetPointPrefab, targetPosition, Quaternion.identity);
        targetPoint.name = "Tumor";
        Debug.Log("Tumor initialized at: " + targetPosition);
    }
}
