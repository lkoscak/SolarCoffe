using System;
using System.ComponentModel.DataAnnotations;

namespace SolarCoffe.Data.Models
{
    public class CustomerAddress
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        [MaxLength(200)]
        public string AddressLine1 { get; set; }
        [MaxLength(200)]
        public string AddressLine2 { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(20)]
        public string PostalCode { get; set; }
        [MaxLength(50)]
        public string Country { get; set; }

    }
}
