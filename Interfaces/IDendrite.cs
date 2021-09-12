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

    public interface IDendrite : IDisposable
    {

        float Weight { get; set; }

        INeuron Parent { get; }

        INeuron Connected { get; }

        string Name { get; set; }

    }

}
