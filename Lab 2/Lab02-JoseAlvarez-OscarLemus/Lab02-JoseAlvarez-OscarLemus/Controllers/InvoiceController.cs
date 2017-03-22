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
            string customer = Request.Form[2];
            string NIT = Request.Form[3];
            string date = Request.Form[4];
            string productCode = Request.Form[5];
            string total = Request.Form[6];

            Invoice InvoiceObj = new Invoice(serial, correlative, customer, NIT, date, productCode, total);

            // We send a delegate stating the criteria for search, 
            // which in this case is the ---------
            // Furthermore, we send the new info that will replace the existing one.
            InvoiceTree.Search(Invoice.compareInvoices, InvoiceObj, InvoiceObj);

            Session["InvoiceTree"] = InvoiceTree;
            return RedirectToAction("Index", Session["InvoiceTree"]);
        }




        [HttpPost] //Since user is retrieving data
        public ActionResult ReadInvoice(HttpPostedFileBase uploadedFile)
        {
            AVLTree<Invoice, Invoice> InvoiceTree;
            // If there's already a tree, we work on it. Otherwise, we create a new one.
            //if (Session["InvoiceTree"] != null)
            //InvoiceTree = (AVLTree<Invoice, Invoice>)Session["InvoiceTree"];
            //else
            InvoiceTree = new AVLTree<Invoice, Invoice>(Invoice.compareInvoices); // We thought it was better to always reset the structure every time we were going to read a file.

            try
            {
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
            }
            catch (Exception)
            {

                throw;
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

            try
            {
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
                            //Info para encontrar la factura correspondiente
                            string serial = fields[0];
                            string correlative = fields[1];
                            //Info a agregar a factura correspondiente
                            string productCode = fields[2];
                            string total = fields[3];

                            // We create an object with enough information to find the invoice
                            Invoice IncompleteInvoice = new Invoice(serial, correlative, null, null, null);


                            try
                            {
                                //We use this object to find the Invoice Of Interest with the missing information (details: productCode, total)
                                Invoice InvoiceOfInterest = InvoiceTree.SearchOnly(Invoice.compareInvoices, IncompleteInvoice);

                                //We add the missing information
                                InvoiceOfInterest.productCode = productCode;
                                InvoiceOfInterest.total = total;

                                //InvoiceTree.Search(Invoice.compareInvoices, InvoiceOfInterest, InvoiceOfInterest);

                                //If there are information in a productsTree to relate the invoices with, we do it.
                                if (Session["ProductsTree"] != null)
                                {

                                    Product productObj = new Product(productCode, null, null, null);


                                    productObj = ((BinaryTree<Product>)Session["ProductsTree"]).SearchOnly((Product x, Product y) => x.product_key.CompareTo(y.product_key), productObj);

                                    string purchasedProduct = productObj.product_description;

                                    InvoiceOfInterest.purchasedProduct = purchasedProduct;
                                }
                            }
                            //Since objects are passed by reference, we are done.
                            catch (Exception e)
                            {

                            }



                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
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

        public ActionResult Search(string serial, string correlative)
        {
            AVLTree<Invoice, Invoice> InvoiceTree = (AVLTree<Invoice, Invoice>)Session["InvoiceTree"];
            Invoice InvoiceObj = new Invoice(serial, correlative, "", "", "", "", "");

            Invoice NewInvoice = InvoiceTree.SearchOnly(delegate (Invoice x, Invoice y) { return (x.serial + x.correlative).CompareTo((y.serial + y.correlative)); }, InvoiceObj);

            AVLTree<Invoice, Invoice> temporalTree = new AVLTree<Invoice, Invoice>(Invoice.compareInvoices);
            temporalTree.Insert(NewInvoice, NewInvoice);
            Session["Filter"] = temporalTree;
            Session["InvoiceTree"] = InvoiceTree;
            return View("Index", Session["Filter"]);
        }






    }





}
