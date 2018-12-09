using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HardwareOrder.ApiController.Models
{
    [Serializable]
    public class AddressModel
    {
        [Required]
        public string AttentionTo { get; set; }
        [Required]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        [Required]
        public string City { get; set; }
        public string Suburb { get; set; }
        [Required]
        public string PostCode { get; set; }
        public string Country { get; set; }
    }
}
