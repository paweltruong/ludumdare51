using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    CanvasGroupMonobehaviour[] TutorialStages;

    public void ShowStage(int index)
    {
        var selectedStage = TutorialStages[index];
        selectedStage.FadeIn();

        foreach (var stage in TutorialStages)
        {
            if (stage != selectedStage)
            {
                stage.FadeOut();
            }
        }
    }

}
