using MotorcycleRental.Domain.Aggregates.DeliveryDrivers.ValueObjects;
using MotorcycleRental.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Domain.Aggregates.DeliveryDrivers
{
    public class DeliveryDriver
    {
        private const int CNPJ_LENGHT = 14;

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Cnpj { get; private set; }
        public DateTime BirthDate { get; private set; }
        public Cnh Cnh { get; private set; }

        private DeliveryDriver() { }

        public DeliveryDriver(string id, string name, string cnpj, DateTime birthDate, Cnh cnh)
        {
            var sanitizedCnpj = SanitizeAndValidateCnpj(cnpj);

            Id = id;
            Name = name;
            Cnpj = sanitizedCnpj;
            BirthDate = birthDate;
            Cnh = cnh;
        }

        private string SanitizeAndValidateCnpj(string cnpj)
        {
            cnpj = cnpj.Replace(".", "").Replace("-", "");

            if(cnpj.Length != CNPJ_LENGHT)
            {
                throw new DomainException($"CNPJ must have exacly {CNPJ_LENGHT} digits");
            }
            //TODO: Implement complete cnpj validation
            
            return cnpj;
        }

        public void UpdateCnhImage(string path) 
        {
            if (string.IsNullOrEmpty(path))
                throw new DomainException("CNH image path cannot be null or empty");

            Cnh = new Cnh(Cnh.Type, Cnh.Number, path);
        }

    }
}
