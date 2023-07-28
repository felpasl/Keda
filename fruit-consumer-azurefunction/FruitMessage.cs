using Avro.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Consumer.AzureFunction
{
    public class FruitMessage
    {
        public Date now;
        public Fruit fruit;
    }
}
