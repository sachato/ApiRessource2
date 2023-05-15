using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiRessource2.Models;
using ApiRessource2.Services;
//using ApiRessource2.Migrations;
using ApiRessource2.Helpers;
using System.Reflection.Metadata.Ecma335;

namespace ApiRessource2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatistiqueController : ControllerBase
    {
        private readonly DataContext _context;
        private IUserService _userService;

        // GET: StatistiqueController
        public ActionResult Index()
        {
            return View();
        }

        // GET: StatistiqueController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StatistiqueController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StatistiqueController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StatistiqueController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StatistiqueController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StatistiqueController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StatistiqueController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
