using MotorcycleRental.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Domain.Aggregates.Motorcycles
{
    public class Motorcycle
    {
        public string Id { get; private set; }
        public int Year { get; set; }
        public string Model { get; private set; }
        public string LicensePlate { get; private set; }

        public Motorcycle(string id, int year, string model, string licensePlate)
        {
            if(year > DateTime.Now.Year)
            {
                throw new DomainException("The provided year cannot be greater than the current year.");
            }

            Id = id;
            Year = year;
            Model = model;
            LicensePlate = licensePlate;
        }

        public void UpdateLicensePlate(string licensePlate)
        {
            if (string.IsNullOrEmpty(licensePlate))
                throw new DomainException("The provided plate cannot be null or empty");

            LicensePlate = licensePlate;
        }
    }
}
