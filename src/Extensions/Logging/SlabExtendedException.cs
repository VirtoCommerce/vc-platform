using System;
using System.Text;
using System.IO;

using Newtonsoft.Json;

namespace VirtoCommerce.Slab
{
    public class SlabExtendedException : Exception
    {
        private readonly Exception _exception;
        public SlabExtendedException(Exception error) : base(error.Message, error)
        {
            _exception = error;
        }

        public static SlabExtendedException Create(Exception ex)
        {
            return new SlabExtendedException(ex);
        }

        public override string ToString()
        {
            if (_exception == null)
            {
                return string.Empty;
            }
            
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                try
                {
                    JsonSerializer.Create().Serialize(sw, _exception);
                }
                catch (Exception ex)
                {
                    sb.AppendLine(
                        string.Format(
                            "Failed to serialize exception type {0}: {1}",
                            _exception.GetType().Name,
                            ex));
                    sb.AppendLine("------------- original exception -------------");
                    sb.AppendLine(_exception.ToString());
                }
            }

            return sb.ToString();
        }
    }
}
