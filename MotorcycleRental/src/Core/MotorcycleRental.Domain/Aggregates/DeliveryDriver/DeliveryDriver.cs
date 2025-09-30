using MotorcycleRental.Domain.Aggregates.DeliveryDriver.ValueObjects;
using MotorcycleRental.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Domain.Aggregates.DeliveryDriver
{
    public class DeliveryDriver
    {
        private const int CNPJ_LENGHT = 14;
        private static readonly string[] AllowedCnhTypes = { "A", "B", "A+B" };

        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Cnpj { get; private set; }
        public DateTime BirthDate { get; private set; }
        public Cnh Cnh { get; private set; }


        public DeliveryDriver(string id, string name, string cnpj, DateTime birthDate, string cnhType, 
            string numberCnh, string cnhImagePath)
        {
            var sanitizedCnpj = SanitizeAndValidateCnpj(cnpj);

            if (AllowedCnhTypes.Contains(cnhType))
                throw new DomainException("CNH type not allowed");

            Id = id;
            Name = name;
            Cnpj = sanitizedCnpj;
            BirthDate = birthDate;
            Cnh = new Cnh(cnhType, numberCnh, cnhImagePath);
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

            Cnh = new Cnh(Cnh.CnhType, Cnh.CnhNumber, path);
        }

    }
}
