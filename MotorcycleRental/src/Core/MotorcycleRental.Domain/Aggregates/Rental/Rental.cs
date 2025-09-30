using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Domain.Aggregates.Rental
{
    public class Rental
    {
        public Guid Id { get; private set; }
        public string DeliveryDriverId { get; private set; }
        public string MotorcycleId { get; private set; }
        public DateTime InitialDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public DateTime ExpectedEndDate { get; private set; }
        public int Plan { get; private set; }

        public Rental(string deliveryDriverId, string motorcycleId, DateTime initialDate, DateTime endDate, 
            DateTime expectedEndDate, int plan)
        {
            Id = Guid.NewGuid();
            DeliveryDriverId = deliveryDriverId;
            MotorcycleId = motorcycleId;
            InitialDate = initialDate;
            EndDate = endDate;
            ExpectedEndDate = expectedEndDate;
            Plan = plan;
        }
    }
}
