using MotorcycleRental.Domain.Exceptions;
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
        public DateTime? ReturnDate { get; private set; }
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
            ReturnDate = null;
        }

        public decimal GetDailyRate()
        {
            switch (Plan)
            {
                case 7:
                    return 30;
                case 15:
                    return 28;
                case 30:
                    return 22;
                case 45:
                    return 20;
                case 50:
                    return 18;
                default:
                    throw new DomainException("Plan doesn't have daily rate");
            }
        }

        public void ReturnMotorcycle(DateTime returnDate)
        {
            if (returnDate < InitialDate)
                throw new DomainException("Return date cannot be before the initial date");

            ReturnDate = returnDate;
        }
    }
}
