namespace QA.AutomatedMagic.MagicServer.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Models;
    using System.IO;

    public class MagicSolutionController : Controller
    {
        // GET: MagicSolution
        public ActionResult Index()
        {
            return View(SolutionManager.Storage.MagicSolutions);
        }

        // GET: MagicSolution/Details/5
        public ActionResult Details(string name)
        {
            var solution = SolutionManager.Storage.MagicSolutions.First(s => s.Name == name);
            return View(solution);
        }

        // GET: MagicSolution/Create
        public ActionResult Create()
        {
            var solution = new MagicSolution();
            return View(solution);
        }

        // POST: MagicSolution/Create
        [HttpPost]
        public ActionResult Create(MagicSolution magicSolution)
        {
            try
            {
                if (Directory.Exists(magicSolution.Path))
                {
                    SolutionManager.Add(magicSolution);
                    return RedirectToAction("PickProjects", new { name = magicSolution.Name });
                }
                return Error($"Couldn't find specified folder: {magicSolution.Path} for MagicSolution: {magicSolution.Name}");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
        }

        public ActionResult PickProjects(string name)
        {
            var solution = SolutionManager.Storage.MagicSolutions.First(s => s.Name == name);
            var magicProjectPicker = new MagicPicker { SolutionName = solution.Name };

            var di = new DirectoryInfo(solution.Path);
            var dirs = di.GetDirectories().ToList();
            dirs.ForEach(d => magicProjectPicker.Items.Add(new MagicPickerItem { NeedToAdd = false, Name = d.Name }));

            return View(magicProjectPicker);
        }

        [HttpPost]
        public ActionResult PickProjects(MagicPicker magicProjectPicker)
        {
            try
            {
                var solution = SolutionManager.Storage.MagicSolutions.First(s => s.Name == magicProjectPicker.SolutionName);
                magicProjectPicker.Items.Where(i => i.NeedToAdd).ToList()
                    .ForEach(i => solution.Projects.Add(new MagicProject { Name = i.Name, Solution = solution }));

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
        }


        public ActionResult PickReferences(string name)
        {
            var solution = SolutionManager.Storage.MagicSolutions.First(s => s.Name == name);
            var magicProjectPicker = new MagicPicker { SolutionName = solution.Name };

            var di = new DirectoryInfo(solution.Path);
            var dirs = di.GetDirectories().ToList();
            dirs.ForEach(d => magicProjectPicker.Items.Add(new MagicPickerItem { NeedToAdd = false, Name = d.Name }));

            return View(magicProjectPicker);
        }

        [HttpPost]
        public ActionResult PickReferences(MagicPicker magicProjectPicker)
        {
            try
            {
                var solution = SolutionManager.Storage.MagicSolutions.First(s => s.Name == magicProjectPicker.SolutionName);
                magicProjectPicker.Items.Where(i => i.NeedToAdd).ToList()
                    .ForEach(i => solution.Projects.Add(new MagicProject { Name = i.Name, Solution = solution }));

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
        }

        public ActionResult Error(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        // GET: MagicSolution/Edit/5
        public ActionResult Edit(string name)
        {
            var solution = SolutionManager.Storage.MagicSolutions.First(s => s.Name == name);
            return View(solution);
        }

        // POST: MagicSolution/Edit/5
        [HttpPost]
        public ActionResult Edit(string name, MagicSolution magicSolution)
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

        // GET: MagicSolution/Delete/5
        public ActionResult Delete(string name)
        {
            var solution = SolutionManager.Storage.MagicSolutions.First(s => s.Name == name);
            return View(solution);
        }

        // POST: MagicSolution/Delete/5
        [HttpPost]
        public ActionResult Delete(string name, MagicSolution magicSolution)
        {
            try
            {
                var solution = SolutionManager.Storage.MagicSolutions.First(s => s.Name == name);
                SolutionManager.Storage.MagicSolutions.Remove(solution);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
