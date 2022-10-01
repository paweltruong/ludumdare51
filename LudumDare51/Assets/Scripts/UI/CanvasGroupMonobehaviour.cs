using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGroupMonobehaviour : MonoBehaviour
{
    [SerializeField]
    protected CanvasGroup masterCanvasGroup;

    protected float speedDivider = 1.0f;
    protected float desiredAlpha = 0;
    protected float currentAlpha = 0;

    public virtual void Start()
    {
        speedDivider = 1 / Singleton.Instance.GameInstance.GetConfiguration().FadeSpeed;
    }

    public virtual void Update()
    {
        if (masterCanvasGroup != null && masterCanvasGroup.alpha != desiredAlpha)
        {

            Debug.LogFormat("CVMB upd alpha {0}", masterCanvasGroup.alpha.ToString("0.000000"));
            masterCanvasGroup.alpha = Mathf.MoveTowards(masterCanvasGroup.alpha, desiredAlpha, speedDivider * Time.deltaTime);
        }
        else
        {

            Debug.LogFormat("CVMB no upd alpha {0} des {1}", masterCanvasGroup.alpha.ToString("0.000000"), desiredAlpha.ToString("0.000000"));
        }
    }

    public void FadeIn()
    {
        desiredAlpha = 1;
    }
    public void FadeOut()
    {
        desiredAlpha = 0;
    }
}
