using UnityEngine;

public class CanvasGroupMonobehaviour : MonoBehaviour
{
    [SerializeField]
    protected CanvasGroup masterCanvasGroup;

    protected float speedDivider = 1.0f;
    protected float desiredAlpha = 0;
    protected float currentAlpha = 0;

    public float Alpha { get { return desiredAlpha; } }

    public virtual void Awake()
    {
        if (masterCanvasGroup != null)
        {
            desiredAlpha = masterCanvasGroup.alpha;
        }
    }

    public virtual void Start()
    {
        speedDivider = 1 / Singleton.Instance.GameInstance.Configuration.FadeSpeed;
    }

    public virtual void Update()
    {
        if (masterCanvasGroup != null && masterCanvasGroup.alpha != desiredAlpha)
        {

            //Debug.LogFormat("CVMB upd alpha {0}", masterCanvasGroup.alpha.ToString("0.000000"));
            masterCanvasGroup.alpha = Mathf.MoveTowards(masterCanvasGroup.alpha, desiredAlpha, speedDivider * Time.deltaTime);
        }
        else
        {

            //Debug.LogFormat("CVMB no upd alpha {0} des {1}", masterCanvasGroup.alpha.ToString("0.000000"), desiredAlpha.ToString("0.000000"));
        }
    }

    public void ShowImmediate()
    {
        if (masterCanvasGroup != null)
        {
            desiredAlpha = 1;
            masterCanvasGroup.alpha = 1;
        }
    }
    public void HideImmediate()
    {
        if (masterCanvasGroup != null)
        {
            desiredAlpha = 0;
            masterCanvasGroup.alpha = 0;
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
