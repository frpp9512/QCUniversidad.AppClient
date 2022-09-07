using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.AppClient.PlataformServices
{
    public interface ITimersHandler
    {
        Guid CreateTimer(Action<DateTime> callback, double interval, bool start = false);
        void StartTimer(Guid id);
        void StopTimer(Guid id);
        void RemoveTimer(Guid id);
    }
}
