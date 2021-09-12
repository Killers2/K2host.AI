/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2019-03-20                        | 
'| Use: General                                         |
' \====================================================/
*/

using K2host.AI.Delegates;

namespace K2host.AI.Interfaces
{

    public interface INerveOut
    {

        string Name { get; }

        int[] Trigger { get; }

        INetwork NeuralNet { get; }

        bool Load(string Name, INetwork NeuralNet, int[] trigger, OnNerveOutCompleteCallBack CallBack);

        void ExecuteRoutine(INerveIn input);

    }

}
