using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace DotNet.Template.Business.UnitTest.TestFixtures
{
    internal static class SequentialNumber
    {
        private static int _counter;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static int GetAndIncrement()
        {
            int local = _counter;
            _counter++;
            return local;
        }
    }
}