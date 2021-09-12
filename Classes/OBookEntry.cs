/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2019-03-20                        | 
'| Use: General                                         |
' \====================================================/
*/

using System;

using K2host.AI.Interfaces;

namespace K2host.AI.Classes
{

    public class OBookEntry : IBookEntry
    {

        public float[] Input { get; set; }

        public float[] Output { get; set; }

        public OBookEntry(float[] input, float[] output)
        {
            Input   = input;
            Output  = output;
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
