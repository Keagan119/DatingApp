using System.Security.AccessControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }

        public string URL { get; set; }

        public bool IsMain { get; set; }

        public string PublicID { get; set; }

        public int AppUserID { get; set; }



    }
}