using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JinderAPI.Models
{
    public class PosterUserModel
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


        public int JobPostId { get; set; }
        public string RequiredSkills { get; set; }
        public string JobDescription { get; set; }
        public string SalaryRange { get; set; }
        public string OperationHours { get; set; }
        public string Location { get; set; }
        public Nullable<int> PosterId { get; set; }

    }
}