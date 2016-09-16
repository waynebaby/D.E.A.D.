using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.Logging
{
    public interface IStandardLogger : ILogger<StandardLevels> 
    {
        IChannel<StandardLevels> Critical { get; }
        IChannel<StandardLevels> Important { get; }
        IChannel<StandardLevels> Warning { get; }
        IChannel<StandardLevels> Infomation { get; }
        IChannel<StandardLevels> Debug { get; }
    }
}
