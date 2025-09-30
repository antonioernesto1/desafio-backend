using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Domain.Aggregates.Motorcycle
{
    public class Motorcycle
    {
        public string Id { get; private set; }
        public string Code { get; private set; }
        public int Year { get; set; }
        public string Model { get; private set; }
        public string LicensePlate { get; private set; }

        public Motorcycle(string id, string code, int year, string model, string licensePlate)
        {
            Id = id;
            Code = code;
            Year = year;
            Model = model;
            LicensePlate = licensePlate;
        }
    }
}
