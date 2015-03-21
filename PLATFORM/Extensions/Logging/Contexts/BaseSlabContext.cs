using System;

namespace VirtoCommerce.Slab.Contexts
{
    public class BaseSlabContext
    {
        public DateTime StartTime { get; set; }
		public string startTime { get { return StartTime.ToString("s"); } }

        public DateTime EndTime { get; set; }
		public string endTime { get { return EndTime.ToString("s"); } }

        public TimeSpan Duration 
        {
            get
            {
                return this.EndTime - this.StartTime;
            }
        }

		public double duration
		{
			get
			{
				return (this.EndTime - this.StartTime).TotalMilliseconds;
			}
		}

        public dynamic ReturnValue { get; set; }

        public Exception Error { get; set; }
		public string error { get { return Error != null ? Error.Message : string.Empty; } }
		public string exception { get { return Error != null ? Error.ToString() : string.Empty; } }

        public bool HasError
        {
            get
            {
                return this.Error != null;
            }
        }

        public DateTime Start()
        {
            return this.StartTime = DateTime.UtcNow;
        }

        public DateTime Stop()
        {
            return this.EndTime = DateTime.UtcNow;
        }
    }
}