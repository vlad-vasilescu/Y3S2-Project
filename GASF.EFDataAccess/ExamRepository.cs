using System;
using System.Collections.Generic;
using System.Linq;
using GASF.ApplicationLogic.Abstractions;
using GASF.ApplicationLogic.Data;
using Microsoft.EntityFrameworkCore;

namespace GASF.EFDataAccess
{
    public class ExamRepository : IExamRepository
    {
        private readonly StudentRecordDbContext dbContext;

        public ExamRepository(StudentRecordDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Exam Add(Exam itemToAdd)
        {
            dbContext.Exams.Add(itemToAdd);
            dbContext.SaveChanges();

            return itemToAdd;
        }

        public bool Delete(Exam itemToDelete)
        {
            dbContext.Exams.Remove(itemToDelete);
            dbContext.SaveChanges();

            return true;
        }

        public Exam Delete(Guid id)
        {
            var itemToDelete = dbContext.Exams.Find(id);
            dbContext.Exams.Remove(itemToDelete);
            dbContext.SaveChanges();

            return itemToDelete;
        }

        public IEnumerable<Exam> GetAll()
        {
            return dbContext.Exams.Include(exam => exam.Course);
        }

        public Exam GetById(Guid id)
        {
            return dbContext.Exams.Find(id);
        }

        public Exam GetExamByCourseName(string courseName)
        {
            return dbContext
                .Exams
                .Include(e => e.Course)
                .Where(e => e.Course.Name == courseName)
                .FirstOrDefault();
        }

        public ICollection<Exam> GetExamByDate(DateTime date)
        {
            return dbContext.Exams.Where(exam => exam.Date == date).ToList();
        }

        public ICollection<Exam> GetGroupExams(Guid groupId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Exam> GetTeacherExams(Guid teacherId)
        {
            return dbContext.Exams.Include(exam => exam.Course).Where(exam => exam.Course.Teacher.Id == teacherId);
        }

        public Exam Update(Exam itemToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}