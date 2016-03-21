using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QA.AutomatedMagic.MagicServer.Controllers.Vs
{
    public class VsMagicController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Details(string name)
        {
            return View();
        }

        // GET: VsMagic/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VsMagic/Create
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

        // GET: VsMagic/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VsMagic/Edit/5
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

        // GET: VsMagic/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VsMagic/Delete/5
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
