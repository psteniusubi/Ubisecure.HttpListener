using System;
using System.Threading;

namespace SimpleHttpListener
{
    internal sealed class AtomicBool
    {
        private const Int32 TRUE = 1;
        private const Int32 FALSE = 0;
        private Int32 value;
        public AtomicBool() : this(false)
        {
        }
        public AtomicBool(bool value)
        {
            this.value = value ? TRUE : FALSE;
        }
        public bool Value { get { return value == TRUE; } }
        public bool GetAndSet(bool value)
        {
            Int32 t = value ? TRUE : FALSE;
            return Interlocked.Exchange(ref this.value, t) == TRUE;
        }
        public bool CompareAndSet(bool oldValue, bool newValue)
        {
            Int32 _old = oldValue ? TRUE : FALSE;
            Int32 _new = newValue ? TRUE : FALSE;
            return Interlocked.CompareExchange(ref this.value, _new, _old) == TRUE;
        }
        public static bool operator true(AtomicBool value)
        {
            return value.Value == true;
        }
        public static bool operator false(AtomicBool value)
        {
            return value.Value == false;
        }
    }
}
