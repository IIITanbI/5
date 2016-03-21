using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QA.AutomatedMagic.MagicServer.Controllers
{
    public class MagicProjectController : Controller
    {
        // GET: MagicProject
        public ActionResult Index()
        {
            return View();
        }

        // GET: MagicProject/Details/5
        public ActionResult Details(string name)
        {
            return View();
        }

        // GET: MagicProject/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MagicProject/Create
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

        // GET: MagicProject/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MagicProject/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MagicProject/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MagicProject/Delete/5
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
    }
}
