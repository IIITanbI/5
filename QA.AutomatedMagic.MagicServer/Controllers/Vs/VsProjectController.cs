using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QA.AutomatedMagic.MagicServer.Controllers.Vs
{
    public class VsProjectController : Controller
    {
        // GET: VsProject
        public ActionResult Index()
        {
            return View();
        }

        // GET: VsProject/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VsProject/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VsProject/Create
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

        // GET: VsProject/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VsProject/Edit/5
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

        // GET: VsProject/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VsProject/Delete/5
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
