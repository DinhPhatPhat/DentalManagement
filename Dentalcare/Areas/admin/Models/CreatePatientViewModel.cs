﻿using Dentalcare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dentalcare.Areas.admin.Models
{
    public class CreatePatientViewModel
    {
        public Patient patient { get; set; }  // Thông tin liên quan đến Patient
        public Person person { get; set; }    // Thông tin cá nhân liên quan đến Person
        public Account account { get; set; }  // Thông tin tài khoản liên quan đến Account
    }
}