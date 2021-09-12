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

    public interface IBook : IDisposable
    {

        List<IBookEntry> Entries { get; set; }

        string Network { get; }

        int Iterations { get; }

        void Add(float[] input, float[] output);

        void Add(IBookEntry e);

        void Complete();

    }

}
