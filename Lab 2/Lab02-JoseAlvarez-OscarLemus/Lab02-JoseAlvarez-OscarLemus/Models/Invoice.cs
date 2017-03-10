using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lab02_JoseAlvarez_OscarLemus.Models
{
    public class Invoice
    {
        //Random number
        [Required]
        [DisplayName("Serie")]
        public string serial { get; set; }

        [Required]
        [DisplayName("Correlativo")]
        public string correlative { get; set; }

        [Required]
        [DisplayName("Cliente")]
        public string customer { get; set; }

        [Required]
        [DisplayName("NIT")]
        public string NIT { get; set; }

        [Required]
        [DisplayName("Fecha")]
        public string date { get; set; }

        //The products must exist
        [Required]
        [DisplayName("Descripión de compra")]
        public string purchaseDescription { get; set; }

        [Required]
        [DisplayName("Total")]
        public string total { get; set; }


        // With Random Serial
        public Invoice(string correlative, string customer, string NIT, string date, string purchaseDescription, string total)
        {
            this.serial = (new Random().Next(1, 1000)).ToString();
            this.correlative = correlative;
            this.customer = customer;
            this.NIT = NIT;
            this.date = date;
            this.purchaseDescription = purchaseDescription;
            this.total = total;
        }


        // With Not Random Serial
        public Invoice(string serial, string correlative, string customer, string NIT, string date, string purchaseDescription, string total)
        {
            this.serial = serial;
            this.correlative = correlative;
            this.customer = customer;
            this.NIT = NIT;
            this.date = date;
            this.purchaseDescription = purchaseDescription;
            this.total = total;
        }


        public static int compareInvoices(Invoice x, Invoice y)
        {
            //if (y == null)
            //    return 1;
            //else if (int.Parse(x.serial + x.correlative) > int.Parse(y.serial + y.correlative).Normalize())
            //    return 1;
            //else if (int.Parse(x.serial + x.correlative) < int.y.serial + y.correlative)
            //    return -1;
            //else
            //    return 0;
            return (x.serial + x.correlative).CompareTo(y.serial + y.correlative);
        }
    }
}