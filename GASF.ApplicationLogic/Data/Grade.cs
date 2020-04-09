﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GASF.ApplicationLogic.Data
{
    class Grade
    {
        public Guid Id { get; set; }
        public int Score { get; set; }
        public Guid ExamId { get; set; }
        public Exam Exam { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
    }
}
