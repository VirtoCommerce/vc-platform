using System;
using System.Diagnostics.Tracing;
using VirtoCommerce.Slab.Contexts;
using VirtoCommerce.Slab.EventSources;

namespace VirtoCommerce.Slab
{
    public class SlabInvoker : SlabInvoker<BaseSlabContext>
    {
    }

    public class SlabInvoker<T> where T : BaseSlabContext
    {
        public SlabInvoker()
        {
            this.Context = Activator.CreateInstance<T>();
        }

        public delegate void SlabMethod(T context);
        public delegate void WrappedMethod(T context);
        public T Context { get; set; }

        public static SlabInvoker<T> Execute(WrappedMethod codeBody)
        {
            var slabInvoker = new SlabInvoker<T>();

            try
            {
                slabInvoker.Context.Start();
                codeBody.Invoke(slabInvoker.Context);  
            }
            catch (Exception ex)
            {
                slabInvoker.Context.Error = SlabExtendedException.Create(ex);
            }
            finally
            {
                slabInvoker.Context.Stop();
            }

            return slabInvoker;
        }

        public SlabInvoker<T> OnSuccess(SlabMethod codeBody)
        {
            if (!this.Context.HasError)
            {
                codeBody.Invoke(this.Context);
            }

            return this;
        }

		public SlabInvoker<T> OnSuccess(EventSource eventSource, int eventCode)
		{
			if (!this.Context.HasError)
			{
				eventSource.Write(eventCode, this.Context);
			}
			return this;
		}

        public SlabInvoker<T> OnError(SlabMethod codeBody)
        {
            if (this.Context.HasError)
            {
                codeBody.Invoke(this.Context);
            }

            return this;
        }

		public SlabInvoker<T> OnError(EventSource eventSource, int eventCode)
		{
			if (this.Context.HasError)
			{
				eventSource.Write(eventCode, this.Context);
			}

			return this;
		}

        public SlabInvoker<T> OnFinished(SlabMethod codeBody)
        {
            codeBody.Invoke(this.Context);
            return this;
        }

		public SlabInvoker<T> OnFinished(EventSource eventSource, int eventCode)
		{
			eventSource.Write(eventCode, this.Context);
			return this;
		}
    }
}