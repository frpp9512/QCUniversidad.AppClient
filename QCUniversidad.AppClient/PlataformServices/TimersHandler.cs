using QCUniversidad.AppClient.Services.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace QCUniversidad.AppClient.PlataformServices
{
    public class TimersHandler : ITimersHandler
    {
        private readonly Dictionary<Guid, Timer> _timers;

        public TimersHandler(IUserManager userManager)
        {
            _timers = new Dictionary<Guid, Timer>();
            userManager.AuthenticationEvent += (authEvent, sender) => 
            {
                if (authEvent == AuthenticationEvent.Logout)
                {
                    RemoveAllTimers();
                }
            };
        }

        private void RemoveAllTimers()
        {
            foreach (var timer in _timers)
            {
                RemoveTimer(timer.Key);
            }
        }

        public Guid CreateTimer(Action<DateTime> callback, double interval, bool start = false)
        {
            var timer = new Timer(interval);
            timer.Elapsed += (sender, args) => callback(args.SignalTime);
            var id = Guid.NewGuid();
            _timers.Add(id, timer);
            if (start)
            {
                StartTimer(id);
            }
            return id;
        }

        public void RemoveTimer(Guid id)
        {
            if (_timers.ContainsKey(id))
            {
                StopTimer(id);
                var timer = _timers[id];
                timer.Dispose();
                _timers.Remove(id);
            }
        }

        public void StartTimer(Guid id)
        {
            if (_timers.ContainsKey(id))
            {
                var timer = _timers[id];
                if (!timer.Enabled)
                {
                    timer.Start();
                }
            }
        }

        public void StopTimer(Guid id)
        {
            if (_timers.ContainsKey(id))
            {
                var timer = _timers[id];
                if (timer.Enabled)
                {
                    timer.Stop();
                }
            }
        }

        public bool ExistTimer(Guid id)
        {
            return _timers.ContainsKey(id);
        }

        public bool IsTimerRunning(Guid id)
        {
            if (_timers.ContainsKey(id))
            {
                var timer = _timers[id];
                return timer.Enabled;
            }
            return false;
        }
    }
}
