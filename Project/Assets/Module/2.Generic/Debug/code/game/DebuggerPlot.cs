using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebuggerPlot : MonoBehaviour
{
    public TMP_Dropdown dropdownTut;
    public TMP_Dropdown dropdownPlot;
    public TMP_Dropdown dropdownPlotSeq;
    public Toggle triggerToggle;
    public Toggle animationToggle;

    public void Init()
    {
        dropdownTut.options.Clear();
        foreach (var tutKey in AllTutorial.dictData.Keys)
        {
            dropdownTut.options.Add(new TMP_Dropdown.OptionData(tutKey));
        }

        dropdownPlot.options.Clear();
        foreach (var plotKey in AllPlot.dictData.Keys)
        {
            dropdownPlot.options.Add(new TMP_Dropdown.OptionData(plotKey));
        }

        dropdownPlotSeq.options.Clear();
        foreach (var plotSeqKey in AllPlotSequence.dictData.Keys)
        {
            dropdownPlotSeq.options.Add(new TMP_Dropdown.OptionData(plotSeqKey));
        }
    }

    public void OnDebugTut()
    {
        int currentIndex = dropdownTut.value;
        string currentText = dropdownTut.options[currentIndex].text;

        TutSystem.Instance.OnTriggerTut(currentText, null, true);
        Debugger.Instance.OnCloseDebug();
    }

    public void OnDebugPlot()
    {
        int currentIndex = dropdownPlot.value;
        string currentText = dropdownPlot.options[currentIndex].text;

        PlotSystem.Instance.OnTriggerPlot(currentText);
        Debugger.Instance.OnCloseDebug();
    }

    public void OnDebugPlotSeq()
    {
        int currentIndex = dropdownPlotSeq.value;
        string currentText = dropdownPlotSeq.options[currentIndex].text;

        SequenceTaskSystem.Instance.AddPlotSeq(currentText);
    }

    public void OnDebugEnterHome()
    {
        Debugger.Instance.OnCloseDebug();
        Game.Instance.OnChangeState(GameStates.Pause);
        Game.Instance.OnChangeState(GameStates.Home);
    }

    public void OnToggleTrigger()
    {
        //UnitBase.ForceColliderToTrigger = triggerToggle.isOn;
    }

    public void OnToggleAnimation()
    {
        //UnitBase.ForceNoAnimation = animationToggle.isOn;
    }
}
