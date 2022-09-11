using AutoMapper;
using DemoTest.BL.Helper;
using DemoTest.BL.InterFace;
using DemoTest.BL.Model;
using DemoTest.DL.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DemoTest.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ICityRepo city;
        private readonly IDistrictRepo district;


        #region fild
        private readonly IEmployeeRepo employee;
        private readonly IDepartmentRepo department;
        private readonly IMapper mapper;


        #endregion



        #region ctor
        public EmployeeController(ICityRepo city,IDistrictRepo district, IEmployeeRepo employee,IDepartmentRepo department, IMapper mapper)
        {
            this.city = city;
            this.district = district;
            this.employee = employee;
            this.department = department;
            this.mapper = mapper;
        }

        #endregion


        #region
        public IActionResult Index()
        {

            {
                var data = employee.Get();
                var model = mapper.Map<IEnumerable<EmployeeVM>>(data);
                return View(model);

            }
          
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.DepartmentList = new SelectList(department.Get(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    model.PhotoName = FileUploader.UploadFile("/wwwroot/File/imags", model.Photo);
                    model.CvName = FileUploader.UploadFile("/wwwroot/File/Docs", model.Cv);

                    var data = mapper.Map<Employee>(model);
                    employee.Create(data);
                   return RedirectToAction("Index");

                }
                ViewBag.DepartmentList = new SelectList(department.Get(), "Id", "Name");

                return View(model);

            }
            catch (Exception ex)
            {
                ViewBag.DepartmentList = new SelectList(department.Get(), "Id", "Name");

                return View(model);
            }



        }
        [HttpGet]
        public IActionResult Detalis(int Id)
        {
            var data = employee.GetById(Id);
            var model = mapper.Map<EmployeeVM>(data);
            ViewBag.DepartmentList = new SelectList(department.Get(), "Id", "Name",model.DepartmentId);

            return View(model);
        }
        [HttpGet]

        public IActionResult Edit(int id)
        {
            var data = employee.GetById(id);
            var model = mapper.Map<EmployeeVM>(data);
            ViewBag.DepartmentList = new SelectList(department.Get(), "Id", "Name", model.DepartmentId);

            return View(model);
        }
        [HttpPost]

        public IActionResult Edit(EmployeeVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.DepartmentList = new SelectList(department.Get(), "Id", "Name", model.DepartmentId);

                    var data = mapper.Map<Employee>(model);
                    employee.Eidt(data);
                    return RedirectToAction("Index");
                }
                ViewBag.DepartmentList = new SelectList(department.Get(), "Id", "Name", model.DepartmentId);

                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.DepartmentList = new SelectList(department.Get(), "Id", "Name", model.DepartmentId);
                return View(model);

            }
        }
        [HttpGet]
        public IActionResult Delete(int Id)
        {

            var data = employee.GetById(Id);
            var model = mapper.Map<EmployeeVM>(data);
            ViewBag.DepartmentList = new SelectList(department.Get(), "Id", "Name", model.DepartmentId);

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(EmployeeVM model)
        {
            try
            {
                var data = mapper.Map<Employee>(model);
                employee.Delete(data);
                FileUploader.RemoveFile("/wwwroot/File/imags/", model.PhotoName);
                FileUploader.RemoveFile("/wwwroot/File/Docs/", model.CvName);
                return RedirectToAction("Index");


            }
            catch(Exception ex)
            {

                ViewBag.DepartmentList = new SelectList(department.Get(), "Id", "Name", model.DepartmentId);
                return View(model);


            }




        }

        #endregion


        [HttpPost]
        public JsonResult GetCityDataByCountryId(int CtryId)
        {
            var data = city.Get(a => a.CountryId == CtryId);
            var model = mapper.Map<IEnumerable<CityVM>>(data);
            return Json(model);
        }


        [HttpPost]
        public JsonResult GetDistrictDataByCityId(int CtyId)
        {
            var data = district.Get(a => a.CityId == CtyId);
            var model = mapper.Map<IEnumerable<DistrictVM>>(data);
            return Json(model);
        }




    }
}
