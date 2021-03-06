﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GASF.ApplicationLogic.Data;
using Microsoft.AspNetCore.Identity;
using GASF.ApplicationLogic.Services;
using Microsoft.AspNetCore.Authorization;

namespace GASF.Controllers
{
    [Authorize(Policy = "Secretary")]
    public class TeachersController : Controller
    {
        

        private readonly UserManager<IdentityUser> userManager;
        private readonly ITeachersService teachersService;
        private readonly ICertificateForTeachersService certificateForTeachersService;
        private readonly ISecretaryService secretaryService;
        private readonly UserService userService;

        public TeachersController(UserManager<IdentityUser> userManager,
            ITeachersService teachersService,
            ICertificateForTeachersService certificateForTeachersService,
            ISecretaryService secretaryService,
            UserService userService
        ) {
            this.userManager = userManager;
            this.teachersService = teachersService;
            this.certificateForTeachersService = certificateForTeachersService;
            this.secretaryService = secretaryService;
            this.userService = userService;
        }


        // GET: Teachers
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Teacher> teachers = teachersService.GetAllTeachers();
            return View(teachers);
        }


        // GET: Teachers/Details/5
        [HttpGet]
        public IActionResult Details(Guid id)
        {
            var teacher = teachersService.GetTeacherById(id);

            return View(teacher);
        }

        // GET: Teachers/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm]Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var teacherUser = userManager.Users
                .AsEnumerable()
                .Where(
                    user => Guid.Parse(user.Id) == teacher.UserId
                )
                .Single();

            teacher.Email = teacherUser.Email;
            teacher.Phone = teacherUser.PhoneNumber;

            teachersService.AddTeacher(teacher);
            return Redirect(Url.Action("Index", "Teachers"));
        }


        // GET: Teachers/Edit/5
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var teacher = teachersService.GetTeacherById(id);
            return View(teacher);
        }


        // POST: Teachers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,FirstName,LastName,Email,Phone,BirthDate")] Teacher teacher)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            teachersService.EditTeacher(id, teacher);
            return RedirectToAction("Index");
        }

        // GET: Teachers/Delete/5
        public IActionResult Delete(Guid id)
        {
            var teacher = teachersService.GetTeacherById(id);
            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            teachersService.DeleteTeacher(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Teachers/CreateCertificate
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult CreateCertificate(Guid Id, [Bind("Title,Message,Date")] CertificateForTeacher certificateForTeacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var userId = userManager.GetUserId(User);
            var secretaryId = secretaryService.GetSecretaryByUserId(userId);

            certificateForTeachersService.createCertificateForTeacher(certificateForTeacher, Id, secretaryId.Id);
            //
            return Redirect(Url.Action("Index", "Teachers"));
        }


        [HttpGet]
        public IActionResult CreateCertificate()
        {
            return View();
        }
        // GET: Teachers/Certification//Details/5
        [HttpGet]
        public IActionResult CertificateDetails(Guid id)
        {
            var certificate = certificateForTeachersService.GetCertificateById(id);
            var teacher = teachersService.GetTeacherById(certificate.IdTeacher);
            var secretary = secretaryService.GetSecretaryById(certificate.IdSecretary);
            CertificateForTeacherView certificateForTeacherView = new CertificateForTeacherView()
            {
                CertificateForTeacher = certificate,
                Teacher = teacher,
                secretary = secretary
            };
            return View(certificateForTeacherView);
        }

        // GET: Certifications
        [HttpGet]
        public IActionResult AllCertificates(Guid id)
        {

            IEnumerable<CertificateForTeacher> certificates = certificateForTeachersService.GetCertificatesForTeacher(id);
            return View(certificates);
        }

    }
}