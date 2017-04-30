using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab4_JoseAlvarez_OscarLemus.Models;

namespace Lab4_JoseAlvarez_OscarLemus.Controllers
{
    public class CelularController : Controller
    {
        Dictionary<string, Celular> dictionary;
        IList<Celular> phoneList;



        // GET: Celular
        public ActionResult Index()
        {

            if (Session["dictionary"] != null)
            {
                dictionary = (Dictionary<string, Celular>)Session["dictionary"];
                phoneList = dictionary.Values.ToList();
            }

            else
                dictionary = new Dictionary<string, Celular>();




            return View(phoneList);
        }


        // GET: Celular/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Celular/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            // El id ya es el codigo del producto
            dictionary = (Dictionary<string, Celular>)Session["dictionary"];
            phoneList = dictionary.Values.ToList();


            string phoneBrand = Request.Form[2];
            string phoneModel = Request.Form[3];
            string phoneOS = Request.Form[4];
            string phoneCapacity = Request.Form[5];
            string phoneRam = Request.Form[6];

            Celular objCelular = new Celular(id.ToString(), phoneBrand, phoneModel, phoneOS, phoneCapacity, phoneRam);

            dictionary[id.ToString()] = objCelular;

            //Actualizo la lista que se muestra
            phoneList = dictionary.Values.ToList();

            //Actualizo la info del diccionario en la session
            Session["dictionary"] = dictionary;

            return RedirectToAction("Index", phoneList);
        }

        // GET: Celular/Delete/5

        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here

                dictionary = (Dictionary<string, Celular>)Session["dictionary"];

                // Remuevo el celular con el telefono
                dictionary.Remove(id.ToString());

                //Actualizo la lista que se muestra
                phoneList = dictionary.Values.ToList();

                //Actualizo la info del diccionario en la session
                Session["dictionary"] = dictionary;

                return View("Index", phoneList);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult AddPhone(string phoneNumber, string phoneBrand, string phoneModel, string phoneOS, string phoneCapacity, string phoneRAM)
        {
            // Si tiene algo trabajar sobre ese
            if (Session["dictionary"] != null)
            {
                dictionary = (Dictionary<string, Celular>)Session["dictionary"];
                phoneList = dictionary.Values.ToList();
            }

            // Si no, crear uno
            else
                dictionary = new Dictionary<string, Celular>();



            Celular celularObj = new Celular(phoneNumber, phoneBrand, phoneModel, phoneOS, phoneCapacity, phoneRAM);

            // Inserto en el diccionario
            dictionary.Add(celularObj.phoneNumber, celularObj);
            //Actualizo la lista que muestra en pantalla
            phoneList = dictionary.Values.ToList();

            //Actualizo la info del diccionario en la sesion
            Session["dictionary"] = dictionary;

            return View("Index", phoneList);
        }

        public ActionResult Search(string phoneNumber)
        {
            // Fijo entra al search cuando ya tiene algo el diccionario
            dictionary = (Dictionary<string, Celular>)Session["dictionary"];

            // Actualizo lista que se muestra
            phoneList = dictionary.Values.ToList();

            //Limpio la lista porque solo quiero mostrar el elemento que busco :v, esta tonto este movimiento pero si solo hacia phoneList.Add(number) no me dejaba, saber porque...
            phoneList.Clear();


            Celular celularObj = dictionary[phoneNumber];

            //Agregar el elemento que el usuario quiere buscar
            phoneList.Add(celularObj);

            return View("Index", phoneList);
        }
    }
}
