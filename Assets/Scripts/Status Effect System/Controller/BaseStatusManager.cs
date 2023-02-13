using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStatusManager : MonoBehaviour
{
    public GameObject statusIconPrefabs;
    public List<Effect> currentEffectList;

    protected void Awake()
    {
        UpdateStatusIconWithList();
    }

    
    public void AddStatusEffect(EffectDetail_SO newData, int newStatusCount)
    {
        // Loop effect List to find have same status
        bool isSame = false;
        foreach (Effect data in currentEffectList)
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
            currentEffectList.Add(new Effect(Character.Player, newData, newStatusCount));
        }

        UpdateStatusIconWithList();
    }

    public void RemoveStatusEffect(EffectDetail_SO targetData, int newStatusCount)
    {
        foreach (Effect status in currentEffectList)
        {
            if (status.effectData == targetData)
            {
                // Same status
                if (targetData.isAccumulate)
                {
                    status.effectCount -= newStatusCount;
                }

                if(status.effectCount == 0)
                {
                    currentEffectList.Remove(status);
                }
                break;
            }
        }


        UpdateStatusIconWithList();
    }

    public bool HaveStatus(EffectType targetStatusType)
    {
        foreach (Effect status in currentEffectList)
        {
            if(status.effectData.effectType == targetStatusType) return true;
        }

        return false;
    }

    private void UpdateStatusIconWithList()
    {
        // Destroy all children
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Inst new status icon object
        foreach (Effect data in currentEffectList)
        {
            GameObject newObj = Instantiate(statusIconPrefabs, transform) as GameObject;
            StatusEffect objCs = newObj.GetComponent<StatusEffect>();

            objCs.effectDetail = data.effectData;
            objCs.ReloadStatusCountText(data.effectCount);
        }
    }

    protected virtual void UpdataCurrentHurtStatusToGameManger()
    {
        foreach (Effect status in currentEffectList)
        {
            if(status.effectData.logicTrigger.isHurtLogic)
            {
                // hurt status on settlement step will hurt self
                // Add in gameManager list wait to hurt 
                GameManager.Instance.SettlementHurtStatusEffectActionList.Add(status);
            }
        }
    }




    //Test
    public void TestButtonAddStatus(EffectDetail_SO newData)
    {
        AddStatusEffect(newData, 1);
    }
}
