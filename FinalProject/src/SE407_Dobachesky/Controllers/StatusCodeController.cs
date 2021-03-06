﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SE407_Dobachesky.Controllers
{
    public class StatusCodeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            StatusCodeViewModel StatusCodeVm = new StatusCodeViewModel();
            using (var db = new StatusCodeDBContext())
            {
                StatusCodeVm.StatusCodesList = db.StatusCodes.ToList();
                StatusCodeVm.NewStatusCode = new StatusCode();
            }

            return View(StatusCodeVm);
        }

        // Post action
        [HttpPost]
        public IActionResult Index(StatusCodeViewModel StatusCodeVm)
        {
            if (ModelState.IsValid)
            {
                using (var db = new StatusCodeDBContext())
                {
                    db.StatusCodes.Add(StatusCodeVm.NewStatusCode);
                    db.SaveChanges();
                }
            }
            // Redirect to index GET method
            return RedirectToAction("Index");
        }

        // Delete action
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            StatusCodeViewModel StatusCodeVm = new StatusCodeViewModel();
            using (StatusCodeDBContext db = new StatusCodeDBContext())
            {
                using (var dbCheck = new BridgeDBContext())
                {
                    BridgeViewModel viewModel = new BridgeViewModel();
                    viewModel.BridgesList = dbCheck.Bridges.ToList();
                    viewModel.NewBridge = dbCheck.Bridges.Where(x => x.StatusId == id).FirstOrDefault();

                    // Instantiate object from view model
                    if (viewModel.NewBridge == null)
                    {
                        StatusCodeVm.NewStatusCode = new StatusCode();
                        // Get primary key from route data
                        StatusCodeVm.NewStatusCode.StatusCodeId = Guid.Parse(RouteData.Values["id"].ToString());
                        // Mark record as modified
                        db.Entry(StatusCodeVm.NewStatusCode).State = EntityState.Deleted;
                        // Persist changes
                        db.SaveChanges();
                        TempData["ResultMessage"] = "Delete Successful";
                    }
                    else
                    {
                        TempData["ResultMessage"] = "Delete Failed (Is the record being referenced in another table?)";
                    }
                }
            }

            return RedirectToAction("Index");
        }

        // Get action for edit page
        public IActionResult Edit(Guid id)
        {
            StatusCodeViewModel ViewModel = new StatusCodeViewModel();
            using (StatusCodeDBContext db = new StatusCodeDBContext())
            {
                ViewModel.NewStatusCode = db.StatusCodes.Where(item => item.StatusCodeId == id).SingleOrDefault();
                return View(ViewModel);
            }
        }

        // Post action for edit page
        [HttpPost]
        public IActionResult Edit(StatusCodeViewModel obj)
        {
            if (ModelState.IsValid)
            {
                using (StatusCodeDBContext db = new StatusCodeDBContext())
                {
                    StatusCode item = obj.NewStatusCode;
                    item.StatusCodeId = Guid.Parse(RouteData.Values["id"].ToString());
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }


        // Error page
        public IActionResult Error()
        {
            ViewData["Title"] = "Status Codes";
            ViewData["Message"] = "Error loading Status Codes";

            return View();
        }
    }
}