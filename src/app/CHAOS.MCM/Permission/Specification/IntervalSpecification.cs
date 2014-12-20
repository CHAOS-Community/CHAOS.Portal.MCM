using System;

namespace Chaos.Mcm.Permission.Specification
{
    using System.Timers;

    public class IntervalSpecification : ISynchronizationSpecification
    {
        #region Fields

        private readonly Timer _timer;

        #endregion

        public event EventHandler OnSynchronizationTrigger;

        #region Construction

        /// <summary>
        /// 
        /// </summary>
        /// <param name="period">the interval in milliseconds</param>
        public IntervalSpecification(int period)
        {
            _timer = new Timer(period);
            _timer.Elapsed += Callback;
            _timer.AutoReset = false;
            _timer.Enabled = true;
        }

        #endregion
        #region Business Logic

        void Callback(object sender, ElapsedEventArgs e)
        {
            if (OnSynchronizationTrigger != null)
                OnSynchronizationTrigger(this, new EventArgs());

            _timer.Enabled = true;
        }

        #endregion
    }
}
