using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab02_JoseAlvarez_OscarLemus.Extras;
using Lab02_JoseAlvarez_OscarLemus.Models;
using System.IO;
using Microsoft.VisualBasic.FileIO; // A TREASURE TO READ FILES!!!!!!!!!!!!!!!!!!!!


namespace Lab02_JoseAlvarez_OscarLemus.Controllers
{
    public class InvoiceController : Controller
    {
        // GET: Invoice
        public ActionResult Index()
        {
            AVLTree<Invoice, Invoice> InvoiceTree;
            // If there's already a tree, we work on it. Otherwise, we create a new one.
            if (Session["InvoiceTree"] != null)
                InvoiceTree = (AVLTree<Invoice, Invoice>)Session["InvoiceTree"];
            else
                InvoiceTree = new AVLTree<Invoice, Invoice>(Invoice.compareInvoices);

            // Session["ProductsTree"] will be considered null if there's no item in the tree
            if (InvoiceTree.Size() == 0)
                Session["InvoiceTree"] = null;
            else
                Session["InvoiceTree"] = InvoiceTree;

            return View(Session["InvoiceTree"]);
        }


        // GET: Invoice/Edit/5
        public ActionResult Edit(string serial, string correlative)
        {
            return View();
        }

        // POST: Invoice/Edit/5
        [HttpPost]
        public ActionResult Edit(string serial, string correlative, FormCollection collection)
        {
            AVLTree<Invoice, Invoice> InvoiceTree = (AVLTree<Invoice, Invoice>)Session["InvoiceTree"];

            //string serial = Request.Form[0];
            //string correlative = Request.Form[1];
            string customer = Request.Form[0];
            string NIT = Request.Form[1];
            string date = Request.Form[2];
            string productCode = Request.Form[3];
            string total = Request.Form[4];

            Invoice InvoiceObj = new Invoice(serial, correlative, customer, NIT, date, productCode, total);

            // We send a delegate stating the criteria for search, 
            // which in this case is the ---------
            // Furthermore, we send the new info that will replace the existing one.
            InvoiceTree.Search(Invoice.compareInvoices, InvoiceObj);

            Session["InvoiceTree"] = InvoiceTree;
            return RedirectToAction("Index", Session["InvoiceTree"]);
        }




        [HttpPost] //Since user is retrieving data
        public ActionResult ReadInvoice(HttpPostedFileBase uploadedFile)
        {
            AVLTree<Invoice, Invoice> InvoiceTree;
            // If there's already a tree, we work on it. Otherwise, we create a new one.
            if (Session["InvoiceTree"] != null)
                InvoiceTree = (AVLTree<Invoice, Invoice>)Session["InvoiceTree"];
            else
                InvoiceTree = new AVLTree<Invoice, Invoice>(Invoice.compareInvoices);


            if (uploadedFile == null)
                return View("Index");



            if (uploadedFile != null && uploadedFile.ContentLength > 0)
                //if (uploadedFile.FileName.EndsWith(".csv"))
                {
                    Stream stream = uploadedFile.InputStream;
                    using (TextFieldParser csvParser = new TextFieldParser(stream))
                    {
                        csvParser.SetDelimiters(new string[] { "," });
                        csvParser.HasFieldsEnclosedInQuotes = true;

                        while (!csvParser.EndOfData)
                        {
                            string[] fields = csvParser.ReadFields();
                            string serial = fields[0];
                            string correlative = fields[1];
                            string customer = fields[2];
                            string NIT = fields[3];
                            string date = fields[4];

                            Invoice aNewInvoice = new Invoice(serial, correlative, customer, NIT, date);
                            InvoiceTree.Insert(aNewInvoice, aNewInvoice);

                        }
                    }
                }

            Session["InvoiceTree"] = InvoiceTree;
            return View("Index", Session["InvoiceTree"]);

        }


        [HttpPost] //Since user is retrieving data
        public ActionResult ReadInvoiceDetails(HttpPostedFileBase uploadedFile)
        {
            AVLTree<Invoice, Invoice> InvoiceTree;
            // If there's already a tree, we work on it. Otherwise, we create a new one.
            if (Session["InvoiceTree"] != null)
                InvoiceTree = (AVLTree<Invoice, Invoice>)Session["InvoiceTree"];
            else
                InvoiceTree = new AVLTree<Invoice, Invoice>(Invoice.compareInvoices);


            if (uploadedFile == null)
                return View("Index");


            if (uploadedFile != null && uploadedFile.ContentLength > 0)
                if (uploadedFile.FileName.EndsWith(".csv"))
                {
                    Stream stream = uploadedFile.InputStream;
                    using (TextFieldParser csvParser = new TextFieldParser(stream))
                    {
                        csvParser.SetDelimiters(new string[] { "," });
                        csvParser.HasFieldsEnclosedInQuotes = true;

                        while (!csvParser.EndOfData)
                        {
                            string[] fields = csvParser.ReadFields();
                            //Info para encontrar la factura correspondiente
                            string serial = fields[0];
                            string correlative = fields[1];
                            //Info a agregar a factura correspondiente
                            string productCode = fields[2];
                            string total = fields[3];

                            // We create an object with enough information to find the invoice
                            Invoice IncompleteInvoice = new Invoice(serial, correlative, null, null, null);

                            //We use this object to find the Invoice Of Interest with the missing information (details: productCode, total)
                            Invoice InvoiceOfInterest = InvoiceTree.SearchOnly(Invoice.compareInvoices, IncompleteInvoice);
                            //We add the missing information
                            InvoiceOfInterest.productCode = productCode;
                            InvoiceOfInterest.total = total;

                            //Since objects are passed by reference, we are done.

                        }
                    }
                }

            Session["InvoiceTree"] = InvoiceTree;
            return View("Index", Session["InvoiceTree"]);

        }

        public ActionResult Add(string serial, string correlative, string customer, string NIT, string date, string productCode, string total)
        {
            AVLTree<Invoice, Invoice> InvoiceTree;
            if (Session["InvoiceTree"] != null)
                InvoiceTree = (AVLTree<Invoice, Invoice>)Session["InvoiceTree"];
            else
                InvoiceTree = new AVLTree<Invoice, Invoice>(Invoice.compareInvoices);


            Invoice InvoiceObj = new Invoice(serial, correlative, customer, NIT, date, productCode, total);
            InvoiceTree.Insert(InvoiceObj, InvoiceObj/*, Invoice.compareInvoices*/);


            Session["InvoiceTree"] = InvoiceTree;
            return View("Index", Session["InvoiceTree"]);

        }



    }





}



     //var information = line.Split(',');

                            //Invoice InvoiceObj = null;
                            //if (information.Length == 7) //means there's specific serial in the parameters
                            //{
                            //    int total = -1;
                            //    //Data validation
                            //    try
                            //    {
                            //        total = int.Parse(information[6]);
                            //    }
                            //    catch (Exception)
                            //    {
                            //    }
                            //    InvoiceObj = new Invoice(information[0], information[1], information[2], information[3], information[4], information[5], information[6]);
                            //}
                            //else if (information.Length == 6) // means no serial in the parameteres, there will be a random one.
                            //{
                            //    int total = -1;
                            //    //Data validation
                            //    try
                            //    {
                            //        total = int.Parse(information[7]);
                            //    }
                            //    catch (Exception)
                            //    {
                            //    }
                            //    InvoiceObj = new Invoice(information[0], information[1], information[2], information[3], information[4], information[5]);
                            //}