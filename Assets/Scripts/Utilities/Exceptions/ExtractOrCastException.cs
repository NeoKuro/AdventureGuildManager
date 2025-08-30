//-----------------------------\\
//      Project Idle AI
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractOrCastException : Exception
{
    public ExtractOrCastException()
    {

    }

    public ExtractOrCastException(string message) : base(message)
    {

    }

    public ExtractOrCastException(string message, Exception inner) : base(message, inner)
    {

    }
}