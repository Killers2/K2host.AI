/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2019-03-20                        | 
'| Use: General                                         |
' \====================================================/
*/

using System;
using System.Collections.Generic;

using K2host.AI.Interfaces;
using K2host.AI.Delegates;

namespace K2host.AI.Classes
{

    public class OBook : IBook
    {

        public OnCompletedBookCallBack OnCompletedBook;

        readonly string _network = string.Empty;
        List<IBookEntry> _list = null;
        readonly int _iterations = 5;

        public List<IBookEntry> Entries
        {
            get
            {
                return _list;
            }
            set
            {
                _list = value;
            }
        }

        public string Network
        {
            get
            {
                return _network;
            }
        }

        public int Iterations
        {
            get
            {
                return _iterations;
            }
        }

        public OBook(string network, int iterations)
        {
            _network = network;
            _list = new List<IBookEntry>();
            _iterations = iterations;
        }

        public OBook(string network, int iterations, OBookEntry[] entries)
            :this(network, iterations)
        {
            _list.AddRange(entries);

        }

        public void Add(float[] input, float[] output)
        {
            _list.Add(new OBookEntry(input, output));
        }

        public void Add(IBookEntry e)
        {
            _list.Add(e);

        }

        public void Complete()
        {
            OnCompletedBook?.Invoke(this);
        }

        bool IsDisposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            if (disposing)
            {

            }

            IsDisposed = true;
        }

    }

}
