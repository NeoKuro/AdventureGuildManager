//-----------------------------\\
//      Project RWI Commander
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//      Twitch.tv/neokuro
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Currency in bits
//      Idea is as the AI you will 'process' bits and as you become more advanced / smarter, you can process more bits
//          doesn't matter if its not technically accurate, most won't know or care
//      Another option is the currency is 'FLOPS' (FLoating-point Operations Per Second) which determines how much processing/computer power something actually has
//          Could just have this as a 'nifty' stat that is displayed (and can then compare to real world things like total human processing power etc)
//          (HINT: In 2007 the total global computer processing power in FLOPS had only just reach the total processing power of ONE human brain...there are now 7billion ofus >.> )

// The elements listed here are only the ones shown to represent their 'true' values (k = thousand, M = million, B = Billion, T = Trillion etc)
//      Anything after that we instead will use 'aa' notation (calculated automatically)
//      This way, if we decide to change it later (add in Qd, Qn etc) the system will automatically adjust it for us
public enum UnitCurrencyScale 
{
    NONE = 0,
    K    = 1,
    M    = 2,
    B    = 3,
    T    = 4,    
}