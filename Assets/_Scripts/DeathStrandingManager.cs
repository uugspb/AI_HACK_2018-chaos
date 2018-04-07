using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// кодзима гений
public class DeathStrandingManager : Singleton<DeathStrandingManager>
{
    public float predictionTime = 2f; // время появления modifier до времени убийства (предсказание появления агента)
    public float stayingTime = 3f; // сколько держится modifieer
    public GameObject modifierPrefab;
    public NavMeshSurface foxSurface;

    private float startingTime;
    private List<KillingPoint> killingPoints;
    
    private void Start()
    {
        startingTime = Time.time;
    }

    public void SetKillingPoint(Vector3 position)
    {
        killingPoints.Add(new KillingPoint(Time.time - startingTime, position));
        startingTime = Time.time;
        StopAllCoroutines();
        

    }

    IEnumerator AppearingModifierCoroutine()
    {
        while (true)
        {
            foreach (var killingPoint in killingPoints)
            {
                if (Time.time - startingTime - predictionTime >= killingPoint.time && !killingPoint.created)
                {
                    killingPoint.created = true;
                    killingPoint.modifier = Instantiate(modifierPrefab, killingPoint.position, Quaternion.identity);
                    foxSurface.BuildNavMesh();
                }
                
                if (Time.time - startingTime - predictionTime >= killingPoint.time && !killingPoint.created)
                {
                    killingPoint.created = true;
                    killingPoint.modifier = Instantiate(modifierPrefab, killingPoint.position, Quaternion.identity);
                    foxSurface.BuildNavMesh();
                }
            }
            
            

            yield return null;
        }
    }
}

public class KillingPoint
{
    public float time;
    public Vector3 position;
    public bool created;
    public bool destoyed;
    public GameObject modifier;
    
    public KillingPoint(float time, Vector3 position)
    {
        this.time = time;
        this.position = position;
    }
    
}