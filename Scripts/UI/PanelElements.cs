using System.Collections;
using UnityEngine;


public class PanelElements : MonoBehaviour
{
    public enum AnimationType
    {
        animateByPosition,
        animateByScale
    }

    [SerializeField] private Vector3 startPosition;
    [SerializeField] private bool startPositionSet = false;
    [SerializeField] private Vector3 endPosition;

    [Space(10)]
    [SerializeField] private AnimationType animationType;

    [Space(10)]
    [SerializeField] private bool closeOnTimer;

    [Space(10)]
    [SerializeField] private bool activateFirstChild;

    public Vector3 GetStartPosition { get => startPosition; set => startPosition = value; }
    public Vector3 GetEndPosition { get => endPosition; set => endPosition = value; }

    public AnimationType GetAnimationType { get => animationType; set => animationType = value; }
    

    private void Start()
    {
        if (!startPositionSet)
        {
            if (animationType == AnimationType.animateByPosition)
            {
                startPosition = transform.localPosition;
            }
            else if (animationType == AnimationType.animateByScale)
            {
                startPosition = transform.localScale;
            }
        }

    }

    public void StartTimer() 
    {
        if (closeOnTimer)
        {
            StartCoroutine(CloseOnTimer());
        }
    }

    IEnumerator CloseOnTimer() 
    {
        yield return new WaitForSeconds(2f);

        PanelController.instance.ClosePanel(gameObject);
    }

    public void ActivateFirstChild() 
    {
        if (activateFirstChild && !transform.GetChild(0).gameObject.activeSelf) 
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (activateFirstChild && transform.GetChild(0).gameObject.activeSelf)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

}
