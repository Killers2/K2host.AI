/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2019-03-20                        | 
'| Use: General                                         |
' \====================================================/
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using K2host.Core;
using K2host.Threading.Classes;
using K2host.AI.Delegates;
using K2host.AI.Interfaces;
using Newtonsoft.Json;
using K2host.Threading.Interface;
using K2host.Threading.Extentions;

namespace K2host.AI.Classes
{
    //'--------------------------------------------------------------------

    //' Next i want to remove all layers, then creating a collection of neurons... (the layers are use for a 2D neural network model).
    //' This will help build a 3D Neuron collection based on a human brain

    //' To identify the input neurons and output neurons we can see which have dentrites mapped to and from 
    //' neurons with both we know are processing ones, Neurons which have dendrites only are outputs and
    //' neurons with only dendrite connected we know are inputs.

    //' Each Neuron is waiting (listening) for an value change from any dendrite.
    //' When Neuron Fires Every dendrite weight is calculated at the same time using theading !

    //' This gives the same action of a human brain.

    //' 1. Create Loads of neurons and connect them all in the 3D model..
    //' 2. Loop the number, input and remove the dendrite from this makes the number of neurons inputs
    //' 3. Do the same loop for the number of outputs and remove the dendrites to. this creates those neurons as outputs
    //' 4. Refference these in 2 lists "InPut Neurons" as "OutPut Neurons"
    //' 5. Now we have a list of inputs, a list of outputs and all the neurons are connected together in 3D like a human brain.
    //' 6. All neurons when created and connected start listening. waiting to fire !!!

    //'==========================================================================

    public class ONervousSystem : INervousSystem
    {

        public string File = string.Empty;
        public bool AutoDrop = false;
        public bool IsThinking = false;

        string _name = string.Empty;
        IThreadManager _threadManager = null;
        IMap _map;
        int _sensertivity = 1;
        Dictionary<int[], INerveOut> _outputs = null;
        Dictionary<string, INetwork> _networks = null;

        public INetwork this[int index]
        {
            get
            {

                int i = 0;

                foreach (INetwork n in _networks.Values)
                {
                    if (i == index)
                        return n;
                    i += 1;
                }

                return null;

            }
        }

        public IThreadManager ThreadManager
        {
            get
            {
                return _threadManager;
            }
            set
            {
                _threadManager = value;
            }
        }

        public IMap CurrentMap
        {
            get
            {
                return _map;
            }
            set
            {
                _map = value;
            }
        }

        public int Sensertivity
        {
            get
            {
                return _sensertivity;
            }
            set
            {
                _sensertivity = value;
            }
        }

        public Dictionary<int[], INerveOut> Outputs
        {
            get
            {
                return _outputs;
            }
        }

        public Dictionary<string, INetwork> Networks
        {
            get
            {
                return _networks;
            }
            set
            {
                _networks = value;
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

        public bool Thinking
        {
            get
            {
                return IsThinking;
            }
            set
            {
                IsThinking = value;
            }
        }

        public ONervousSystem(string name)
        {
            _name = name;
            _networks = new Dictionary<string, INetwork>();
            _outputs = new Dictionary<int[], INerveOut>();
        }

        public void SetThreadManager(IThreadManager e)
        {

            _threadManager = e;

            foreach (INetwork n in _networks.Values)
                n.SetParent(this);

        }

        public bool Create(INetwork network, int[] layers)
        {

            IsThinking = true;

            //check to see the networks list has not been set to nothing
            if (_networks == null)
                _networks = new Dictionary<string, INetwork>();

            //check the layers, there should be 3 min, input at least 1 hidden and 1 output
            if (layers.Length <= 2)
                return false;

            //create a new network to give birth to layers neurons
            Networks.Add(network.Name, network);

            //check the hidden layers, make sure theres more the 2 neurons, and add the layers with mapping
            for (int i = 0; i <= layers.Length - 1; i++)
            {

                if (i > 0 && i < layers.Length - 1)
                {

                    if (layers[i] <= 2)
                    {
                        _networks.Remove(network.Name);
                        network.Layers.Clear();
                        network.Dispose();
                        return false;
                    }

                }

                _networks[network.Name].Add(i, new OLayer("L" + i, (ONetwork)network, layers[i]));

            }

            //map the synapsis
            for (int i = 0; i <= layers.Length - 2; i++)
            {

                foreach (INeuron pNeuron in _networks[network.Name][i].Neurons.Values)
                {

                    foreach (INeuron nNeuron in _networks[network.Name][i + 1].Neurons.Values)
                    {

                        foreach (IDendrite nNeuronDendrite in nNeuron.Dendrites.Values)
                        {
                            if (nNeuronDendrite.Connected == pNeuron)
                                pNeuron.Synapsis.Add(nNeuronDendrite);

                        }

                    }

                }

            }

            IsThinking = false;

            return true;

        }

        public void AddOutPutNerve(INerveOut e, string Name, INetwork NeuralNet, int[] trigger, OnNerveOutCompleteCallBack CallBack)
        {
            if (_outputs == null)
                _outputs = new Dictionary<int[], INerveOut>();

            e.Load(Name, NeuralNet, trigger, CallBack);

            Outputs.Add(e.Trigger, e);

        }

        public INerveOut GetOutPutNerve(string Name)
        {

            foreach (INerveOut r in Outputs.Values)
            {
                if (r.Name == Name)
                    return r;
            }

            return null;

        }

        public void ThinkAsync(INerveIn e, OnNerveInCompleteCallBack CallBack)
        {
            if (IsThinking)
                return;

            IsThinking = true;

            _threadManager.Add(new OThread(new ParameterizedThreadStart(WaitThink))).Start(new object[] { e, CallBack });

        }

        private void WaitThink(object e)
        {

            object[] ab = (object[])e;

            INerveIn a = (INerveIn)ab[0];
            OnNerveInCompleteCallBack b = (OnNerveInCompleteCallBack)ab[1];

            if (b != null)
                b.Invoke(a, ((ONervousSystem)a.NeuralNet.Parent).Think(a));

            IsThinking = false;

        }

        public INerveOut[] Think(INerveIn e)
        {

            IsThinking = true;

            float[] o;
            float[] b   = new float[e.Trigger.Length];
            int[]   c   = new int[e.NeuralNet.OutputLayer.Count];

            for (int i = 0; i <= e.Trigger.Length - 1; i++)
                b[i] = e.Trigger[i].Map(CurrentMap.Min, CurrentMap.Max, 0f, 1f);

            o = Think(e.NeuralNet.Name, b);

            for (int i = 0; i <= o.Length - 1; i++)
                c[i] = o[i].Map(0f, 1f, CurrentMap.Min, CurrentMap.Max);

            return GetScore(c).ToArray();

        }

        public INerveOut[] GetScore(int[] rawOutPut)
        {

            List<INerveOut> l = new();
            Dictionary<int, INerveOut> t = new();
            int score;

            foreach (int[] Sense in Outputs.Keys)
            {

                score = 0;

                for (int j = 0; j <= Sense.Length - 1; j++)
                {
                    if (Sense[j] == rawOutPut[j])
                        score += 1;
                }


                if (score == 0)
                {
                    foreach (int val in rawOutPut)
                    {
                        if (Sense.Contains(val))
                            score += 1;
                    }
                }


                if (score > ((rawOutPut.Length - 1) - _sensertivity))
                {

                    if (t.ContainsKey(score))
                        t[score] = GetBestSens(Sense, t[score], Outputs[Sense]);
                    else
                        t.Add(score, Outputs[Sense]);

                }

            }

            int[] scores = t.Keys.ToArray();

            Array.Sort(scores);
            Array.Reverse(scores);

            foreach (int key in scores)
                l.Add(t[key]);

            t.Clear();

            Array.Clear(scores, 0, scores.Length);

            return l.ToArray();

        }

        private static INerveOut GetBestSens(int[] values, INerveOut already_exists, INerveOut one_to_check)
        {

            int vCount1 = 0;
            int vCount2 = 0;

            foreach (int value in values)
            {
                if (one_to_check.Trigger.Contains(value))
                    vCount1 += 1;

                if (already_exists.Trigger.Contains(value))
                    vCount2 += 1;
            }

            INerveOut t = one_to_check;

            if (vCount1 > vCount2)
                t = one_to_check;

            if (vCount1 < vCount2)
                t = already_exists;

            return t;

        }

        public float[] Think(string Network, float[] input)
        {

            IsThinking = true;

            if (input.Length != _networks[Network].Layers["L0"].Count)
                return null;

            foreach (ILayer l in _networks[Network].Layers.Values)
            {
                if (l.Name == "L0")
                    l.SetInput(input);
                else
                    l.Think();

            }

            float[] ret = _networks[Network][(Networks[Network].Count - 1)].GetOutput();

            IsThinking = false;

            return ret;

        }

        public void Learn(INerveIn input, INerveOut output, int iterations, OnCompletedBookCallBack callback)
        {
            if (IsThinking)
                return;

            IsThinking = true;

            OBook a = new(input.NeuralNet.Name, iterations);

            float[] b = new float[input.Trigger.Length];
            float[] c = new float[output.Trigger.Length];

            for (int i = 0; i <= input.Trigger.Length - 1; i++)
                b[i] = input.Trigger[i].Map(input.NeuralNet.Parent.CurrentMap.Min, input.NeuralNet.Parent.CurrentMap.Max, 0f, 1f);

            for (int i = 0; i <= output.Trigger.Length - 1; i++)
                c[i] = output.Trigger[i].Map(input.NeuralNet.Parent.CurrentMap.Min, input.NeuralNet.Parent.CurrentMap.Max, 0f, 1f);

            a.Add(new OBookEntry(b, c));
            a.OnCompletedBook = callback;

            IsThinking = false;

            ((ONervousSystem)input.NeuralNet.Parent).Learn(a);

        }

        public void Learn(IBook e)
        {
            if (IsThinking)
                return;

            IsThinking = true;

            //_threadManager.add(new bzi_thread_manager.thread(new ParameterizedThreadStart(AddressOf learn))).start(e);
            _threadManager.Add(new OThread(new ParameterizedThreadStart(LearnTest))).Start(e);

        }

        public void Learn(object e)
        {

            //  ERROR: Not supported in C#: OnErrorStatement

            OBook book = (OBook)e;

            for (int g = 0; g <= book.Iterations - 1; g++)
            {

                foreach (OBookEntry entry in book.Entries)
                {

                    if (entry.Input.Length != _networks[book.Network][0].Count)
                        return;

                    if (entry.Output.Length != _networks[book.Network][Networks[book.Network].Count - 1].Count)
                        return;

                    Think(book.Network, entry.Input);

                    int i = 0;
                    int j = 0;
                    int k = 0;

                    INetwork n = Networks[book.Network];
                    ILayer[] l = Networks[book.Network].Layers.Values.ToArray();

                    for (i = 0; i <= l[^1].Count - 1; i++)
                    {

                        l[^1][i].ErrorTerm = l[^1][i].Axon * (1 - l[^1][i].Axon) * (entry.Output[i] - l[^1][i].Axon);

                        for (j = (l.Length - 1); j >= 1; j += -1)
                            l[j][k].ErrorTerm = l[j][k].Axon * (1 - l[j][k].Axon) * l[j + 1][i][k].Weight * l[j + 1][i].ErrorTerm;

                    }


                    for (i = (l.Length - 1); i >= 1; i += -1)
                    {

                        for (j = 0; j <= l[i].Count - 1; j++)
                        {

                            l[i][j].SomaWeight += (n.LearningRate * 1 * l[i][j].ErrorTerm);

                            for (k = 0; k <= l[i][j].Count - 1; k++)
                                l[i][j][k].Weight += (n.LearningRate * l[i - 1][k].Axon * l[i][j].ErrorTerm);

                        }

                    }

                    Array.Clear(l, 0, l.Length);
                    l = null;

                }

            }

            IsThinking = false;

            book.Complete();

        }

        public void LearnTest(object e)
        {

            //  ERROR: Not supported in C#: OnErrorStatement

            OBook book = (OBook)e;

            for (int g = 0; g < book.Iterations; g++)
            {

                foreach (OBookEntry entry in book.Entries)
                {

                    if (entry.Input.Length != _networks[book.Network][0].Count)
                        return;

                    if (entry.Output.Length != _networks[book.Network][Networks[book.Network].Count - 1].Count)
                        return;

                    Think(book.Network, entry.Input);

                    for (var i = (Networks[book.Network].Layers.Count - 1); i >= 1; i += -1)
                    {

                        for (var j = 0; j <= _networks[book.Network][i].Neurons.Count - 1; j++)
                        {

                            if (i == (Networks[book.Network].Layers.Count - 1))
                            {
                                _networks[book.Network][i][j].BackPropagate(entry.Output[j]);
                            }
                            else
                            {
                                _networks[book.Network][i][j].BackPropagate(Networks[book.Network][i][j].Synapsis[0].Parent.Axon);
                            }

                        }

                    }

                }

            }

            IsThinking = false;

            book.Complete();

        }

        public bool Sleep()
        {


            try
            {

                AutoDrop = false;

                if (!IsThinking)
                {

                    if (System.IO.File.Exists(File))
                        System.IO.File.Delete(File);

                    _outputs.Clear();
                    _outputs = null;

                    _threadManager.Dispose();
                    _threadManager = null;

                    foreach (INetwork n in this.Networks.Values)
                        n.SetParent(null);

                    System.IO.File.WriteAllText(
                        File, 
                        JsonConvert.SerializeObject(this.Networks)
                    );

                }

                return true;

            }
            catch
            {

                return false;

            }

        }

        public void AutoSave()
        {

            AutoDrop = true;

            _threadManager.Add(new OThread(new ThreadStart(ExecuteAutoDrop))).Start(null);

        }

        private void ExecuteAutoDrop()
        {

            while (AutoDrop)
            {

                Thread.Sleep(300000);

                //5 mins

                if (!IsThinking)
                {
                    IsThinking = true;

                    foreach (INetwork n in this.Networks.Values)
                        n.SetParent(null);

                    if (System.IO.File.Exists(File))
                        System.IO.File.Delete(File);

                    System.IO.File.WriteAllText(
                        File,
                        JsonConvert.SerializeObject(this.Networks)
                    );

                    foreach (INetwork n in this.Networks.Values)
                        n.SetParent(this);

                    IsThinking = false;

                }

            }

        }

        public static Dictionary<string, INetwork> WakeUp(string filename)
        {


            try
            {

                if (!System.IO.File.Exists(filename))
                    return null;

                Dictionary<string, INetwork> t = JsonConvert.DeserializeObject<Dictionary<string, INetwork>>(System.IO.File.ReadAllText(filename));

                return t;

            }
            catch
            {
                return null;

            }

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
