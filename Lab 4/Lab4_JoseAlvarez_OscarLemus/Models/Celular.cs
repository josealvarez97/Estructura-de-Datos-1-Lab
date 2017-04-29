using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lab4_JoseAlvarez_OscarLemus.Models
{
    public class Celular
    {

        [Key]
        [Required]
        [DisplayName("Número de teléfono")]
        public string phoneNumber { get; set; }

        [Required]
        [DisplayName("Marca del celular")]
        public string phoneBrand { get; set; }

        [Required]
        [DisplayName("Modelo del celular")]
        public string phoneModel { get; set; }

        [Required]
        [DisplayName("Sistema operativo")]
        public string phoneOS { get; set; }

        [Required]
        [DisplayName("Capacidad memoria interna (GB)")]
        public string phoneCapacity { get; set; }


        [Required]
        [DisplayName("RAM (GB)")]
        public string phoneRAM { get; set; }

        public Celular(string phoneNumber, string phoneBrand, string phoneModel, string phoneOS, string phoneCapacity, string phoneRAM)
        {
            this.phoneNumber = phoneNumber;
            this.phoneBrand = phoneBrand;
            this.phoneModel = phoneModel;
            this.phoneOS = phoneOS;
            this.phoneCapacity = phoneCapacity;
            this.phoneRAM = phoneRAM;
        }



    }
}