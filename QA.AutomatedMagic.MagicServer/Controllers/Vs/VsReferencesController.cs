using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QA.AutomatedMagic.MagicServer.Controllers.Vs
{
    public class VsReferencesController : Controller
    {
        // GET: VsReferences
        public ActionResult Index()
        {
            return View();
        }

        // GET: VsReferences/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VsReferences/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VsReferences/Create
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

        // GET: VsReferences/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VsReferences/Edit/5
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

        // GET: VsReferences/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VsReferences/Delete/5
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
