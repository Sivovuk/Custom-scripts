using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    [SerializeField] private float animationSpeed = 0.5f;

    public static PanelController instance;

    private void Awake()
    {
        instance = this;
    }

    public void OpenPanel(GameObject panel)
    {
        if (panel.GetComponent<PanelElements>().GetAnimationType == PanelElements.AnimationType.animateByPosition)
        {
            PanelAnimationMove(panel, panel.GetComponent<PanelElements>().GetEndPosition);
        }
        else if (panel.GetComponent<PanelElements>().GetAnimationType == PanelElements.AnimationType.animateByScale)
        {
            PanelAnimationScale(panel, panel.GetComponent<PanelElements>().GetEndPosition);
        }

        panel.GetComponent<PanelElements>().StartTimer();
    }

    public void ClosePanel(GameObject panel) 
    {
        if (panel.GetComponent<PanelElements>().GetAnimationType == PanelElements.AnimationType.animateByPosition)
        {
            PanelAnimationMove(panel, panel.GetComponent<PanelElements>().GetStartPosition);
        }
        else if (panel.GetComponent<PanelElements>().GetAnimationType == PanelElements.AnimationType.animateByScale)
        {
            PanelAnimationScale(panel, panel.GetComponent<PanelElements>().GetStartPosition);
        }

    }

    public void PanelAnimationMove(GameObject panel, Vector3 newPosition) 
    {
        LeanTween.moveLocal(panel, newPosition, animationSpeed).setOnComplete(panel.GetComponent<PanelElements>().ActivateFirstChild);
    }

    public void PanelAnimationScale(GameObject panel, Vector3 newPosition)
    {
        panel.transform.localScale = new Vector3(0, 0, 0);
        panel.transform.position = new Vector3(0, 0, 0);

        LeanTween.scale(panel, newPosition, animationSpeed).setOnComplete(panel.GetComponent<PanelElements>().ActivateFirstChild);
    }

}
