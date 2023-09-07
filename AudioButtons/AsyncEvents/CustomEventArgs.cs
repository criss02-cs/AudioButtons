using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioButtons.AsyncEvents
{
    public delegate Task EventHandlerAsync(object sender, CustomEventArgs<bool> e);
    public class CustomEventArgs<T> : EventArgs
    {
        public T Value { get; set; }
    }
}
