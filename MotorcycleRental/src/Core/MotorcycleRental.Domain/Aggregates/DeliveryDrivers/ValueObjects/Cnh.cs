using MotorcycleRental.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MotorcycleRental.Domain.Aggregates.DeliveryDrivers.ValueObjects
{
    public class Cnh
    {
        private static readonly string[] AllowedCnhTypes = { "A", "B", "A+B" };

        public string Type { get; set; }
        public string Number { get; set; }
        public string ImagePath { get; private set; }
        public Cnh(string type, string number, string imagePath)
        {
            if (string.IsNullOrWhiteSpace(type) || !AllowedCnhTypes.Contains(type))
            {
                throw new DomainException("CNH type is invalid or not allowed.");
            }

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new DomainException("CNH number cannot be empty.");
            }

            Type = type;
            Number = number;
            ImagePath = imagePath;
        }


    }
}
