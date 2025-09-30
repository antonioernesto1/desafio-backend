using MotorcycleRental.Domain.Aggregates.DeliveryDriver.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Domain.Aggregates.DeliveryDriver
{
    public class DeliveryDriver
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Cnpj { get; private set; }
        public DateTime BirthDate { get; private set; }
        public Cnh Cnh { get; private set; }
        public string CnhImage { get; private set; }

        public DeliveryDriver(string id, string name, string cnpj, DateTime birthDate, string typeCnh, 
            string numberCnh, string cnhImage)
        {
            Id = id;
            Name = name;
            Cnpj = cnpj;
            BirthDate = birthDate;
            Cnh = new Cnh(typeCnh, numberCnh);
            CnhImage = cnhImage;
        }
    }
}
