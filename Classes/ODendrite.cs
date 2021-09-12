/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2019-03-20                        | 
'| Use: General                                         |
' \====================================================/
*/

using System;

using Microsoft.VisualBasic;

using K2host.AI.Interfaces;

namespace K2host.AI.Classes
{

    public class ODendrite : IDendrite
    {

        string  _name;              // Dendrites weight value.
        float   _weight;            // The neuron this belongs to.
        readonly INeuron _parent;            // The neuron this is connected to.
        readonly INeuron _connected;

        public float Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
            }
        }

        public INeuron Parent
        {
            get
            {
                return _parent;
            }
        }

        public INeuron Connected
        {
            get
            {
                return _connected;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public ODendrite(string name)
        {

            _name = name;

            VBMath.Randomize(123);

            Weight = 2 - (2 * VBMath.Rnd() + 1);

        }

        public ODendrite(string name, ONeuron parent, ONeuron connected)
            : this(name)
        {
            _parent = parent;
            _connected = connected;
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
