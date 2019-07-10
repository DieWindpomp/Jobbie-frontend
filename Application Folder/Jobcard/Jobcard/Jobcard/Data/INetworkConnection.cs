using System;
using System.Collections.Generic;
using System.Text;

namespace Jobcard.Data
{
    public  interface INetworkConnection
    {
        bool IsConnected { get;}
        void CheckNetworkConnection();

    }
}
