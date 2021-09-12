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

    public interface INetwork : IDisposable
    {

        ILayer this[int index] { get;  }

        ILayer InputLayer { get; }

        ILayer OutputLayer { get; }

        Dictionary<string, ILayer> Layers { get; }

        INervousSystem Parent { get; }

        int Count { get; }

        float LearningRate { get; set; }

        string Name { get; set; }

        void SetParent(INervousSystem e);

        void Add(int index, ILayer layer);

    }

}
