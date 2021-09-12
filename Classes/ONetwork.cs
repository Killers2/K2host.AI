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
using System.Collections.Generic;

namespace K2host.AI.Classes
{

    public class ONetwork : INetwork
    {

        string _name = string.Empty;
        ILayer _inputLayer = null;
        ILayer _outputLayer = null;
        readonly Dictionary<string, ILayer> _layers;
        INervousSystem _parent = null;
        float _learningRate = 1.5f;

        public ILayer this[int index]
        {
            get
            {
                if (!_layers.ContainsKey("L" + index))
                    return null;

                return _layers["L" + index];

            }
        }

        public ILayer InputLayer
        {
            get
            {
                return _inputLayer;
            }
        }

        public ILayer OutputLayer
        {
            get
            {
                return _outputLayer;
            }
        }

        public Dictionary<string, ILayer> Layers
        {
            get
            {
                return _layers;
            }
        }

        public INervousSystem Parent
        {
            get
            {
                return _parent;
            }
        }

        public int Count
        {
            get
            {
                return _layers.Count;
            }
        }

        public float LearningRate
        {
            get
            {
                return _learningRate;
            }
            set
            {
                _learningRate = value;
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

        public ONetwork(string name, float learningRate, ONervousSystem parent)
        {
            _name = name;
            _learningRate = learningRate;
            _layers = new Dictionary<string, ILayer>();
            _parent = parent;
        }

        public void SetParent(INervousSystem e)
        {
            _parent = e;
        }

        public void Add(int index, ILayer layer)
        {
            _layers.Add(layer.Name, layer);

            _outputLayer = _layers[layer.Name];

            if (index == 0)
                _inputLayer = _layers[layer.Name];

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
