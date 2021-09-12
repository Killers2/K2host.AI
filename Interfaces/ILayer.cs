/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2019-03-20                        | 
'| Use: General                                         |
' \====================================================/
*/

using System;
using System.Collections.Generic;

namespace K2host.AI.Interfaces
{

    public interface ILayer : IDisposable
    {

        INeuron this[int index] { get; }

        Dictionary<string, INeuron> Neurons { get; }

        long Count { get; }

        INetwork Parent { get; }

        string Name { get; set; }

        void SetInput(float[] input);

        void Think();

        float[] GetOutput();

    }

}
