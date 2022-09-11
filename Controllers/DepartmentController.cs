using AutoMapper;
using DemoTest.BL.InterFace;
using DemoTest.BL.Model;
using DemoTest.DL.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoTest.Controllers
{
    public class DepartmentController : Controller
    {

        #region fild
        private readonly IDepartmentRepo department;
        private readonly IMapper mapper;
        #endregion


        #region ctor
        public DepartmentController(IDepartmentRepo department, IMapper mapper)
        {
            this.department = department;
            this.mapper = mapper;
        }

        #endregion


        #region action

        public IActionResult Index()
        {
            var data = department.Get();
            var model = mapper.Map<IEnumerable<DepartmentVM>>(data);

            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Create(DepartmentVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = mapper.Map<Department>(model);
                    department.Create(data);

                    return RedirectToAction("Index");
                }
                return View(model);

            }
            catch (Exception ex)
            {

                return View(model);

            }
        }

        [HttpGet]
        public IActionResult Detalis (int Id)
        {
            var data = department.GetById(Id);
            var model = mapper.Map<DepartmentVM>(data);
            return View(model);

        }
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var data = department.GetById(Id);
            var model = mapper.Map<DepartmentVM>(data);
            return View(model);

        }
        [HttpPost]
        public IActionResult Delete(DepartmentVM model)
        {

            var data = mapper.Map<Department>(model);
               department.Delete(data);
            return RedirectToAction("Index");

        }


    [HttpGet]
    public IActionResult Edit(int Id)
        {

            var data = department.GetById(Id);
            var model = mapper.Map<DepartmentVM>(data);
            return View(model);
        }

        [HttpPost] 
        public IActionResult Edit (DepartmentVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = mapper.Map<Department>(model);

                    department.Eidt(data);

                    return RedirectToAction("Index");
                }
                return View (model);

            }
            catch(Exception ex)
            {

                return View(model);


            }


        }
        #endregion

    }
}
