using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JinderAPI.Models
{
    public class SeekerUserModel
    {

        public int JinderUserId { get; set; }
        public string FullName { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string ProfilePicture { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string UserType { get; set; }
        public string username { get; set; }
        public string password { get; set; }


        public int SeekerProfileId { get; set; }
        //public Nullable<int> JinderUserId { get; set; }
        public string Education { get; set; }
        public string Experience { get; set; }
        public string Skills { get; set; }
        public string Certification { get; set; }

        //public virtual JinderUser JinderUser { get; set; }

    }
}