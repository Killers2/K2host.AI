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

    public interface INeuron : IDisposable
    {

        IDendrite this[int index] { get; }

        public Dictionary<string, IDendrite> Dendrites { get; }

        public List<IDendrite> Synapsis { get; }

        ILayer Parent { get; set; }

        long Count { get; }

        string Name { get; set; }

        float ErrorTerm { get; set; }

        float Axon { get; set; }

        float SomaWeight { get; set; }

        void Think();

        void BackPropagate(float desiredvalue);

    }

}
