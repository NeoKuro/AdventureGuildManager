//-----------------------------\\
//      Project CyberTex
//    Author: Joshua Hughes
//      Twitch.tv/neokuro
//-----------------------------\\

using UnityEngine;
using System;
using System.Collections;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ConditionalHideAttribute : PropertyAttribute
{
    //The name of the bool field that will be in control
    public string ConditionalSourceField = "";
    //TRUE = Hide in inspector / FALSE = Disable in inspector 
    public bool HideInInspector = false;
    // TRUE = a 'true' result of the conditionalSourceField will result in property being hidden
    // FALSE = a 'false' result of the conditionalSourceField will result in property being SHOWN
    public bool InvertCondition = false;

    public ConditionalHideAttribute(string conditionalSourceField)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = false;
        this.InvertCondition = false;
    }

    public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = hideInInspector;
        this.InvertCondition = false;
    }

    public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector, bool invertCondition)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = hideInInspector;
        this.InvertCondition = invertCondition;
    }
}