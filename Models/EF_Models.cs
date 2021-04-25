using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Assignment4.DataAccess;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity;
#nullable enable
namespace Assignment4.Models
{

    public class EF_Models
    {
        public class ORI
        {
            [Key] public string nameORI { get; set; }
            public string county { get; set; }
            public List<Result> results { get; set; }
        }
        public class Rootobject
        {
            public int ID { get; set; }
            //public Result[] results { get; set; } //Is it because we don't have it written as: 
            public List<Result> results { get; set; }
            //What is the differenc between those syntaxes?
            public Pagination pagination { get; set; }
        }
        public class Pagination
        {
            public int ID { get; set; }
            public int count { get; set; }
            public int page { get; set; }
            public int pages { get; set; }
            public int per_page { get; set; }
        }
        public class Result
        {
            [Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public int ID { get; set; }
            public string ori { get; set; }
            public int data_year { get; set; }
            public string offense { get; set; }
            public string state_abbr { get; set; }
            public int cleared { get; set; }
            public int actual { get; set; }
        }

    }
}

  

