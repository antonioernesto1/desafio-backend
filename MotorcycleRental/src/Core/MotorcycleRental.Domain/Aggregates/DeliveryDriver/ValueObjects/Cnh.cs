using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Domain.Aggregates.DeliveryDriver.ValueObjects
{
    public class Cnh
    {
        public string CnhType { get; set; }
        public string CnhNumber { get; set; }
        public string CnhImagePath { get; private set; }
        public Cnh(string cnhType, string cnhNumber, string cnhImagePath)
        {
            CnhType = cnhType;
            CnhNumber = cnhNumber;
            CnhImagePath = cnhImagePath;
        }
    }
}
