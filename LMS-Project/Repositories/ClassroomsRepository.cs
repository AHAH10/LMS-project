using LMS_Project.DataContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS_Project.Repositories
{
    public class ClassroomsRepository
    {
        private LMSDb db = new LMSDb();

        public int MyProperty { get; set; }
    }
}