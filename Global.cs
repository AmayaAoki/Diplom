﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomAE
{
    internal class Global
    {
        //для пк колледжа
        //public static string connectionstring = @"Data Source=310-SQL\STUDENT;Initial Catalog=2YP0101RA;Integrated Security=True";
        // для домашнего пк 
        //public static string connectionstring = @"Data Source = DESKTOP-VD7C8RL\SQLEXPRESS; Initial Catalog = dimplomfilled; Integrated Security = True; Encrypt = False";
        public static string connectionstring = @"Data Source = ADMIN\sqlexpress;Initial Catalog=diplomITR; Integrated Security = True; Encrypt = False";

        public static string login = "";
        public static string password = "";
        public static string FIO = "Неавторизованный пользователь";
        public static string photo = "photo0";
        public static string status = "Не авторизован";
        public static bool Astatus = false;

        
        public static string AppointmentID = null;
        //public static DateTime DateOfBirth;

        public static string EmployeeID = null;
        public static string ClientID = null;
        public static string ServiseID = null;
        public static DateTime date = DateTime.MinValue;
        public static TimeSpan time1 = new TimeSpan(9, 0, 0);
        public static string StatusS = null;

        public static int ADMINID = 1;

        public static int procedureid;
        public static string oldprocedure;
        public static string olddescription;
        public static string oldcost;
        public static string oldphoto;
        public static string oldcategory;



        public static int Code = 0;
        public static int OrderCode = 0;
        public static string PickUpLocation = "";
        public static string ProductName = "";

        public static List<string> productNamesList = new List<string>();
        public static int PID = 0;
        public static int PCount = 0;
        public static double PCost = 0;
        public static double PAmount = 0;
        public static double PCostAndAmount = 0;


        public static int OrderID = 0;
        //public static DateTime OrderDate;
        public static string OrderStatus = "";
        public static int PickUPLocationID = 0;
        public static double ResultCost = 0;


        public static int oldValue = 1;
    }
}
