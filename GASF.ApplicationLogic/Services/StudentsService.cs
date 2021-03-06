﻿using GASF.ApplicationLogic.Abstractions;
using GASF.ApplicationLogic.Data;
using GASF.ApplicationLogic.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GASF.ApplicationLogic.Services
{
   public class StudentsService : IStudentsService
    {
        private IStudentRepository studentRepository;
        public StudentsService(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return studentRepository.GetAll();
        }

        public Student GetStudentById(Guid id)
        {
            var student= studentRepository.GetStudentById(id);

            if(student==null)
            {
                throw new EntityNotFoundException(id);
            }
            return student;

        }
       
        public void AddStudent(Student student)
        {
            studentRepository.Add(student);
        }

        public void EditStudent(Student student)
        {
            studentRepository.Update(student);
        }

        public void DeleteStudent(Guid id)
        {
            if (id == null)
            {
                throw new EntityNotFoundException(id);
            }
            var student = studentRepository.GetStudentById(id);
            if (student == null)
            {
                throw new EntityNotFoundException(id);
            }
            studentRepository.Delete(student);
        }

        public IEnumerable<Student> GetStudentsByFirstName(String name)
        {
            if (name == null)
            {
                return studentRepository.GetAll();
            }

            var students = studentRepository.GetStudentByFirstName(name);

            return students;
        }
    }
}
