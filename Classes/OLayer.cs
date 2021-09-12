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

namespace K2host.AI.Classes
{

    public class OLayer : ILayer
    {

        string _name;
        readonly Dictionary<string, INeuron> _neurons;
        readonly INetwork _parent;

        public INeuron this[int index]
        {
            get
            {

                if (!_neurons.ContainsKey(Name + ".N" + index))
                    return null;

                return _neurons[Name + ".N" + index];

            }
        }

        public Dictionary<string, INeuron> Neurons
        {
            get
            {
                return _neurons;
            }
        }

        public long Count
        {
            get
            {
                return _neurons.Count;
            }
        }

        public INetwork Parent
        {
            get
            {
                return _parent;
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

        public OLayer(string name, ONetwork parent)
        {
            _neurons = new Dictionary<string, INeuron>();
            _name = name;
            _parent = parent;
        }

        public OLayer(string name, ONetwork parent, int numberOfNeurons)
            : this(name, parent)
        {

            string np;
            int prevLayerId = Convert.ToInt32(name.Remove(0, 1)) - 1;

            for (int i = 0; i <= numberOfNeurons - 1; i++)
            {

                np = name + ".N" + i.ToString();

                if (prevLayerId > -1)
                    Neurons.Add(np, new ONeuron(np, this, (OLayer)parent.Layers["L" + prevLayerId]));
                else
                    Neurons.Add(np, new ONeuron(np, this));

            }

        }

        public void SetInput(float[] input)
        {

            for (int j = 0; j <= Count - 1; j++)
                this[j].Axon = input[j];

        }

        public void Think()
        {

            foreach (INeuron n in _neurons.Values)
                n.Think();

        }

        public float[] GetOutput()
        {

            float[] output = new float[Count];
            int j = 0;

            foreach (INeuron n in _neurons.Values)
            {
                output[j] = n.Axon;
                j += 1;
            }

            return output;

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
