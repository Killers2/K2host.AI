/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2019-03-20                        | 
'| Use: General                                         |
' \====================================================/
*/

using System;
using System.Collections.Generic;

using K2host.Threading.Interface;

namespace K2host.AI.Interfaces
{

    public interface INervousSystem : IDisposable
    {

        INetwork this[int index] { get; }

        IThreadManager ThreadManager { get; set; }

        IMap CurrentMap { get; set; }

        int Sensertivity { get; set; }

        Dictionary<int[], INerveOut> Outputs { get; }

        Dictionary<string, INetwork> Networks { get; set; }

        string Name { get; set; }

        bool Thinking { get; set; }

    }

}
