/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2019-03-20                        | 
'| Use: General                                         |
' \====================================================/
*/

using System;

namespace K2host.AI.Interfaces
{

    public interface IBookEntry : IDisposable
    {

        float[] Input { get; set; }

        float[] Output { get; set; }

    }

}
