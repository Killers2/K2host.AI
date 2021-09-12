/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2019-03-20                        | 
'| Use: General                                         |
' \====================================================/
*/

using K2host.AI.Interfaces;

namespace K2host.AI.Delegates
{

    public delegate void OnNerveInCompleteCallBack(INerveIn input, INerveOut[] output);

    public delegate void OnNerveOutCompleteCallBack(object e, INerveOut output);

    public delegate void OnCompletedBookCallBack(IBook e);


}
