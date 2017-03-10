using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Lab02_JoseAlvarez_OscarLemus.Extras;
using Lab02_JoseAlvarez_OscarLemus.Models;

namespace Lab02_JoseAlvarez_OscarLemus.Controllers
{
    public class ProductController : Controller
    {
        BinaryTree<Product> ProductsTree = new BinaryTree<Product>();

        // GET: Product
        public ActionResult Index()
        {
            // Si ya hay un arbol se trabaja en el, sino se crea uno nuevo
            if (Session["ProductsTree"] != null)
                ProductsTree = (BinaryTree<Product>)Session["ProductsTree"];
            else
                ProductsTree = new BinaryTree<Product>();

            // Esto se hizo para no tener problemas con el GetEnumerator()
            if (ProductsTree.Size() == 0)
                Session["ProductsTree"] = null;
            else
                Session["ProductsTree"] = ProductsTree;



            return View(Session["ProductsTree"]);
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
              
            return View();
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            // El id ya es el codigo del producto

            ProductsTree = (BinaryTree<Product>)Session["ProductsTree"];


            string product_description = Request.Form[2];
            int product_price = int.Parse(Request.Form[3]);
            int quantity_of_product = int.Parse(Request.Form[4]);

            Product ProductObj = new Product(id, product_description, product_price, quantity_of_product);

            // Se le manda un delegado diciendole el criterio que tome para buscar, que en este caso es por codigo de producto y le mando la nueva info para reemplazarla
            ProductsTree.Search(delegate (Product x, Product y) { return x.product_key.CompareTo(y.product_key); }, ProductObj);
       


            Session["ProductsTree"] = ProductsTree;
            return RedirectToAction("Index", Session["ProductsTree"]);

        }


        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost] // Porque obtiene datos 
        public ActionResult LeerArchivo(HttpPostedFileBase ArchivoCargado)
        {
            // Si ya hay un arbol trabajo sobre ese, sino creo uno
            if (Session["ProductsTree"] != null)
                ProductsTree = (BinaryTree<Product>)Session["ProductsTree"];
            else
                ProductsTree = new BinaryTree<Product>();

            if (ArchivoCargado == null)
            {
                return View("Index");
            }
            StreamReader reader = new StreamReader(ArchivoCargado.InputStream);
            string linea = "";
            //StreamReader reader = new StreamReader(ArchivoCargado.InputStream);
            if (ArchivoCargado != null && ArchivoCargado.ContentLength > 0)
            {
                while ((linea = reader.ReadLine()) != null)
                {
                    var informacion = linea.Split(',');

                    Product ProductObj = new Product(int.Parse(informacion[0]), informacion[1], int.Parse(informacion[2]), int.Parse(informacion[3]));

                    ProductsTree.Insert(ProductObj, delegate (Product x, Product y) { return x.product_key.CompareTo(y.product_key); });

                }

            }

            Session["ProductsTree"] = ProductsTree;
            return View("Index", Session["ProductsTree"]);
        }

        public ActionResult Add(string product_key, string product_description, string product_price, string quantity_of_product)
        {
            // Si ya hay un arbol trabajo sobre ese, sino creo uno.
            if (Session["ProductsTree"] != null)
                ProductsTree = (BinaryTree<Product>)Session["ProductsTree"];
            else
                ProductsTree = new BinaryTree<Product>();


            Product ProductObj = new Product(int.Parse(product_key), product_description, int.Parse(product_price), int.Parse(quantity_of_product));

            ProductsTree.Insert(ProductObj, compararProductos);

            Session["ProductsTree"] = ProductsTree;
            return View("Index", Session["ProductsTree"]);
        }

        public ActionResult Search(int product_key)
        {
            ProductsTree = (BinaryTree<Product>)Session["ProductsTree"];
            Product ProductObj = new Product(product_key, null, -1, -1);

            Product product = ProductsTree.SearchOnly(compararProductos, ProductObj);

            BinaryTree<Product> temporalTree = new BinaryTree<Product>();
            temporalTree.Insert(product, compararProductos); 
            Session["Filter"] = temporalTree;
            Session["ProductsTree"] = ProductsTree;
            return View("Index", Session["Filter"]);
        }

        public int compararProductos(Product x, Product y)
        {
            if (y == null)
                return 1;
            else if (x.product_key > y.product_key)
                return 1;
            else if (x.product_key < y.product_key)
                return -1;
            return 0;
        }
    }
}
