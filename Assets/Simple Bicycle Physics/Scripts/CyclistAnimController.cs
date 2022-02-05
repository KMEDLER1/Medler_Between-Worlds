using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CyclistAnimController : MonoBehaviour
{
    BicycleController bicycleController;
    Animator anim;
    string clipInfoCurrent, clipInfoLast;
    [HideInInspector]
    public float speed;

    public GameObject hipIK, chestIK, leftFootIK, leftFootIdleIK, headIK;
    void Start()
    {
        bicycleController = FindObjectOfType<BicycleController>();
        anim = GetComponent<Animator>();
        leftFootIK.GetComponent<TwoBoneIKConstraint>().weight = 0;
        chestIK.GetComponent<TwoBoneIKConstraint>().weight = 0;
        hipIK.GetComponent<MultiParentConstraint>().weight = 0;
        headIK.GetComponent<MultiAimConstraint>().weight = 0;
    }

    void Update()
    {
        speed = bicycleController.transform.InverseTransformDirection(bicycleController.GetComponent<Rigidbody>().velocity).z;
        anim.SetFloat("Speed", speed);
        clipInfoCurrent = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if (clipInfoCurrent == "IdleToStart" && clipInfoLast == "Idle")
            StartCoroutine(Lfik(0));
        if (clipInfoCurrent == "Idle" && clipInfoLast == "IdleToStart")
            StartCoroutine(Lfik(1));
        if(clipInfoCurrent == "Idle" && clipInfoLast == "Reverse")
            StartCoroutine(LfIdleik(0));
        if(clipInfoCurrent == "Reverse" && clipInfoLast == "Idle")
            StartCoroutine(LfIdleik(1));

        clipInfoLast = clipInfoCurrent;
        // var sources = hipIK.GetComponent<MultiParentConstraint>().data.sourceObjects;
        // sources.SetWeight(0, Mathf.Clamp((speed-(bicycleController.relaxedSpeed+0.5f))*0.5f,0,1f));
        // sources.SetWeight(1, chestIK.GetComponent<MultiParentConstraint>().weight);
        // hipIK.GetComponent<MultiParentConstraint>().data.sourceObjects = sources;
    }

    IEnumerator Lfik(int offset)
    {
        float t1 = 0f;
        while (t1 <= 1f)
        {
            t1 += Time.fixedDeltaTime;
            leftFootIK.GetComponent<TwoBoneIKConstraint>().weight = Mathf.Abs(offset - t1);
            leftFootIdleIK.GetComponent<TwoBoneIKConstraint>().weight = 1 - leftFootIK.GetComponent<TwoBoneIKConstraint>().weight;         
            yield return null;
        }

    }
    IEnumerator LfIdleik(int offset)
    {
        float t1 = 0f;
        while (t1 <= 1f)
        {
            t1 += Time.fixedDeltaTime;
            leftFootIdleIK.GetComponent<TwoBoneIKConstraint>().weight = Mathf.Abs(offset - t1);
            yield return null;
        }

    }
}
