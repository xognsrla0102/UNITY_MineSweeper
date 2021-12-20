using UnityEngine;
using System.Diagnostics;

public class Effect : MonoBehaviour
{
    [SerializeField] private Stopwatch stopWatch = new Stopwatch();
    [SerializeField] private long lifeTimeMilliSec;

    public void Awake()
    {
        stopWatch.Start();
    }

    
    void Update()
    {
        if (stopWatch.ElapsedMilliseconds >= lifeTimeMilliSec)
        {
            stopWatch.Stop();
            Destroy(gameObject);
        }
    }
}
