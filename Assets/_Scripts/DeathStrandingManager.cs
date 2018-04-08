using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// кодзима гелий
public class DeathStrandingManager : Singleton<DeathStrandingManager> {
    public float predictionTime = 2f; // время появления modifier до времени убийства (предсказание появления агента)
    public float stayingTime = 3f; // сколько держится modifieer
    public GameObject modifierPrefab;
    public NavMeshSurface foxSurface;
    int levelsCount = 10;

    private float startingTime;
    private List<KillingPoint> killingPoints = new List<KillingPoint> ();

    public bool IsCloseToKillingPoint (Vector3 position) {
        for (int i = 0; i < killingPoints.Count; i++) {
            if (Vector3.Distance (killingPoints[i].position, position) < 4 &&
                killingPoints[i].created) {
                return true;
            }
        }
        return false;
    }

    public void StartPlayMode () {
        killingPoints = new List<KillingPoint> ();
        startingTime = Time.time;
    }

    public void StopPlayMode () {
        StopAllCoroutines ();
        ClearAllModiriers ();
    }

    public void SetKillingPoint (Vector3 position) {
        int level = 0;
        for (int i = killingPoints.Count - 1; i >= 0; i--) {
            var point = killingPoints[i];
            if (Vector3.Distance (point.position, position) < 1f && point.modifier != null) {
                level = Mathf.Max (level, Mathf.Min (levelsCount, point.level + 1));
                killingPoints.RemoveAt (i);
            }
        }
        float descreetDeltaTime = 0.25f;
        float descreetedTime = Mathf.RoundToInt ((Time.time - startingTime) / descreetDeltaTime) * descreetDeltaTime;
        killingPoints.Add (new KillingPoint (descreetedTime, position, level));
        startingTime = Time.time;
        StopAllCoroutines ();
        ClearAllModiriers ();
        StartCoroutine (AppearingModifierCoroutine ());
    }

    IEnumerator AppearingModifierCoroutine () {
        while (true) {
            bool needRebuild = false;
            foreach (var killingPoint in killingPoints) {
                if (Time.time - startingTime >= killingPoint.time - predictionTime && !killingPoint.created) {
                    killingPoint.created = true;
                    killingPoint.modifier = Instantiate (modifierPrefab, killingPoint.position, Quaternion.identity);
                    var modifier = killingPoint.modifier.GetComponent<NavMeshModifierVolume> ();
                    modifier.area += killingPoint.level;
                    // foxSurface.BuildNavMesh ();
                    // yield return null;
                    // Fox.instance.ResetDestination ();
                    needRebuild = true;
                }

                if (Time.time - startingTime >= killingPoint.time - predictionTime + stayingTime &&
                    !killingPoint.destoyed) {
                    killingPoint.destoyed = true;
                    Destroy (killingPoint.modifier);
                    // foxSurface.BuildNavMesh ();
                    // yield return null;
                    // Fox.instance.ResetDestination ();
                    needRebuild = true;
                    //
                }
            }
            if (needRebuild) {
                foxSurface.BuildNavMesh ();
                yield return null;
                Fox.instance.ResetDestination ();
            }

            yield return null;
        }
    }

    void ClearAllModiriers () {
        foreach (var killingPoint in killingPoints) {
            killingPoint.created = false;
            killingPoint.destoyed = false;
            Destroy (killingPoint.modifier);
            //Fox.instance.ResetDestination();

        }
        foxSurface.BuildNavMesh ();
    }
}

public class KillingPoint {
    public float time;
    public Vector3 position;
    public bool created;
    public bool destoyed;
    public GameObject modifier;
    public int level;

    public KillingPoint (float time, Vector3 position, int level) {
        this.time = time;
        this.position = position;
        this.level = level;
    }
}