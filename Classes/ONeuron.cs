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
using Microsoft.VisualBasic;

namespace K2host.AI.Classes
{

    public class ONeuron : INeuron
    {

        string _name;
        readonly List<IDendrite> _synapsis;                      // Dendrites connected to this neuron (farwards).
        readonly Dictionary<string, IDendrite> _dendrites;       // Dendrites with mapped neuron (backwards).
        float _axon;                                    // Activation value.
        float _soma_weight;                             // Neuron's own weight value.
        float _error_term;                              // Used in the learning process.
        ILayer _parent;                                 // The Layer this neuron belongs to.
        //float _original_axon = 0;

        public IDendrite this[int index]
        {
            get
            {

                if (!_dendrites.ContainsKey(Name + ".D" + index))
                    return null;

                return _dendrites[Name + ".D" + index];

            }
        }

        public Dictionary<string, IDendrite> Dendrites
        {
            get
            {
                return _dendrites;
            }
        }

        public List<IDendrite> Synapsis
        {
            get
            {
                return _synapsis;
            }
        }

        public ILayer Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }

        public long Count
        {
            get
            {
                return _dendrites.Count;
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

        public float ErrorTerm
        {
            get
            {
                return _error_term;
            }
            set
            {
                _error_term = value;
            }
        }

        public float Axon
        {
            get
            {
                return _axon;
            }
            set
            {
                _axon = value;
            }
        }

        public float SomaWeight
        {
            get
            {
                return _soma_weight;
            }
            set
            {
                _soma_weight = value;
            }
        }

        public ONeuron(string name)
        {

            _name = name;
            _dendrites = new Dictionary<string, IDendrite>();
            _synapsis = new List<IDendrite>();

            VBMath.Randomize(123);

            _soma_weight = 2 - (2 * VBMath.Rnd() + 1);

        }

        public ONeuron(string name, OLayer parent)
            : this(name)
        {
            _parent = parent;
        }

        public ONeuron(string name, OLayer parent, OLayer previousLayer)
            : this(name, parent)
        {

            int i = 0;
            string np;

            foreach (ONeuron mappedNeuron in previousLayer.Neurons.Values)
            {

                np = name + ".D" + i.ToString();

                _dendrites.Add(np, new ODendrite(np, this, mappedNeuron));

                i += 1;

            }

        }

        public void Think()
        {
            //                1
            // f(x) = ------------------
            //        1 + exp(-alpha * x)


            //           alpha * exp(-alpha * x )
            // f(x) = ---------------------------- = alpha * f(x) * (1 - f(x))
            //           (1 + exp(-alpha * x)) ^2


            float sum = 0;

            //_original_axon = Axon;

            foreach (IDendrite d in Dendrites.Values)
                sum += (d.Connected.Axon * d.Weight);

            _axon = (1.0F / (1.0F + (float)Math.Exp((float)-Math.E * sum)));


        }

        public void BackPropagate(float desiredvalue)
        {

            _error_term = desiredvalue - Axon;

            foreach (IDendrite d in _dendrites.Values)
                d.Weight += d.Connected.Axon * _error_term * Parent.Parent.LearningRate;

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
