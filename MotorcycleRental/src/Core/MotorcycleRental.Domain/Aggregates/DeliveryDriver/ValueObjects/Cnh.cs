using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Domain.Aggregates.DeliveryDriver.ValueObjects
{
    public class Cnh
    {
        public string TypeCnh { get; set; }
        public string CnhNumber { get; set; }
        public Cnh(string typeCnh, string cnhNumber)
        {
            TypeCnh = typeCnh;
            CnhNumber = cnhNumber;
        }
    }
}
