using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public GameObject statusIconPrefabs;
    public List<Effect> effectDetailList;

    private void Awake()
    {
        UpdateStatusIconWithList();
    }

    
    public void AddStatusEffect(EffectDetail_SO newData, int newStatusCount)
    {
        // Loop effect List to find have same status
        bool isSame = false;
        foreach (Effect data in effectDetailList)
        {
            if (data.effectData == newData)
            {
                // Same status
                if (newData.isAccumulate)
                {
                    data.effectCount += newStatusCount;
                }

                isSame = true;
                break;
            }
        }

        // The input status is a new status
        if (!isSame)
        {
            effectDetailList.Add(new Effect(Character.Player, newData, newStatusCount));
        }

        UpdateStatusIconWithList();
    }

    private void UpdateStatusIconWithList()
    {
        // Destroy all children
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Inst new status icon object
        foreach (Effect data in effectDetailList)
        {
            GameObject newObj = Instantiate(statusIconPrefabs, transform) as GameObject;
            StatusEffect objCs = newObj.GetComponent<StatusEffect>();

            objCs.effectDetail = data.effectData;
            objCs.ReloadStatusCountText(data.effectCount);
        }
    }

    //TODO: Remove status effect

    //Test
    public void TestButtonAddStatus(EffectDetail_SO newData)
    {
        AddStatusEffect(newData, 1);
    }
}
