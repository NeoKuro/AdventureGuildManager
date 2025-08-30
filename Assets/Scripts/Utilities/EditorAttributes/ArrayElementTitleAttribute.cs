//     Project:  RWI MMO    
//       Author: NeoKuro   
//     Twitch.tv/Neokuro 


using UnityEngine;

public class ArrayElementTitleAttribute : PropertyAttribute
{
    public string Varname;
    public ArrayElementTitleAttribute(string ElementTitleVar)
    {
        Varname = ElementTitleVar;
    }
}