using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigerGenerator.Logic.Helpers
{
    public interface INotifyChanges
    {
        event EventHandler<string> SendNotification;
    }
}
