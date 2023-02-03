using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimator : MonoBehaviour
{
    Animator anim;

    [SerializeField] int characterIndex = 0; //TODO: Check Player Index to change Var

    private void Awake()
    {
        anim = GetComponent<Animator>();

        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation()
    {
        anim.SetTrigger("isPlay");
        
        yield return new WaitForSeconds(2);

        Destroy(gameObject);
    }
}
