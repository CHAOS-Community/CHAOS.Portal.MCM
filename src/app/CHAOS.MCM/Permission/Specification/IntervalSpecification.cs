using System;
using System.Threading;

namespace CHAOS.MCM.Permission.Specification
{
    public class IntervalSpecification : ISynchronizationSpecification
    {
        #region Fields

        private Timer _timer;

        #endregion

        public event EventHandler OnSynchronizationTrigger;

        #region Construction

        /// <summary>
        /// 
        /// </summary>
        /// <param name="period">the interval in milliseconds</param>
        public IntervalSpecification(int period)
        {
            _timer = new Timer(Callback, null, 0, period);
        }

        #endregion
        #region Business Logic

        private void Callback(object state)
        {
            if (OnSynchronizationTrigger != null)
                OnSynchronizationTrigger(this,new EventArgs());
        }

        #endregion
    }
}
