using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Framework.Common
{
    public sealed class Retries
    {
        public const int LessRetries = 1;
        public const int StandardRetries = 2;
        public const int MoreRetries = 3;
        public const int LotsOfRetries = 4;

        private Retries()
        {
        }
    }
}
