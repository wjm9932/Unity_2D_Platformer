using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : MonoBehaviour
{
    [SerializeField] private GameObject indicatorPrefab;

    private Indicator[] indicator;
    private int currentCount;
    private int maxCount;
   
    public void UpdateCount(int count)
    {
        if(count > maxCount)
        {
            return;
        }

        if(this.currentCount > count)
        {
            for(int i = this.currentCount - 1; i >= count; i--)
            {
                indicator[i].SetActiveTarget(false);
            }
        }
        else if (this.currentCount < count)
        {
            for(int i = this.currentCount; i < count; i++)
            {
                indicator[i].SetActiveTarget(true);
            }
        }
        currentCount = count;
    }

    public void SetStartCount(int count, int maxCount)
    {
        indicator = new Indicator[maxCount];
        currentCount = count;
        this.maxCount = maxCount;

        for(int i = 0; i < maxCount; i++)
        {
            indicator[i] = Instantiate(indicatorPrefab, this.transform).GetComponent<Indicator>();
        }
    }
}
