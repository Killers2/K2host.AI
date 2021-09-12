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

    public interface INerveIn
    {

        int[] Trigger { get; }

        INetwork NeuralNet { get; }

        object StateObj { get; }

        void Pulse(object state);

    }

}
