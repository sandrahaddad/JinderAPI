using JinderAPI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace JinderAPI.Controllers
{
    public class LoginController : ApiController
    {

        [HttpPost]
        [Route("PostNewSeekerUser")]
        public HttpResponseMessage PostNewSeekerUser(SeekerUserModel jinderSeekerUser)
        {
            JinderDBEntities dbContext = new JinderDBEntities();
            var usersTable = dbContext.JinderUsers;
            var SeekersTable = dbContext.SeekerProfiles;

            HttpResponseMessage message = new HttpResponseMessage();

            JinderUser jinderuser = (from user in usersTable
                                     where user.username == jinderSeekerUser.username
                                     select user).FirstOrDefault<JinderUser>();
            if(jinderuser != null)
            {
                message.StatusCode = HttpStatusCode.Conflict;
                return message;
            }
            else
            {
                JinderUser user = new JinderUser();

                user.FullName = jinderSeekerUser.FullName;
                user.DateOfBirth = jinderSeekerUser.DateOfBirth;
                user.Gender = jinderSeekerUser.Gender;
                user.Address = jinderSeekerUser.Address;
                user.UserType = jinderSeekerUser.UserType;
                user.username = jinderSeekerUser.username;
                user.password = jinderSeekerUser.password;


                usersTable.Add(user);
                dbContext.SaveChanges();

                SeekerProfile seekerUser = new SeekerProfile();

                seekerUser.JinderUserId = user.JinderUserId;
                seekerUser.Certification = jinderSeekerUser.Certification;
                seekerUser.Education = jinderSeekerUser.Education;
                seekerUser.Experience = jinderSeekerUser.Experience;
                seekerUser.Skills = jinderSeekerUser.Skills;

                SeekersTable.Add(seekerUser);
                dbContext.SaveChanges();


                message.StatusCode = HttpStatusCode.OK;

                return message;
            }
        }


        [HttpPost]
        [Route("PostNewPosterUser")]
        public HttpResponseMessage PostNewPosterUser(PosterUserModel jinderPosterUser)
        {
            JinderDBEntities dbContext = new JinderDBEntities();
            var usersTable = dbContext.JinderUsers;
            var PosterTable = dbContext.JobPosts;

            HttpResponseMessage message = new HttpResponseMessage();


            JinderUser jinderuser = (from user in usersTable
                                     where user.username == jinderPosterUser.username
                                     select user).FirstOrDefault<JinderUser>();
            if (jinderuser != null)
            {
                message.StatusCode = HttpStatusCode.Conflict;
                return message;
            }
            else
            {
                JinderUser user = new JinderUser();

                user.FullName = jinderPosterUser.FullName;
                user.DateOfBirth = jinderPosterUser.DateOfBirth;
                user.Gender = jinderPosterUser.Gender;
                user.Address = jinderPosterUser.Address;
                user.UserType = jinderPosterUser.UserType;
                user.username = jinderPosterUser.username;
                user.password = jinderPosterUser.password;


                usersTable.Add(user);
                dbContext.SaveChanges();

                JobPost posterUser = new JobPost();

                posterUser.PosterId = user.JinderUserId;
                posterUser.Location = jinderPosterUser.Location;
                posterUser.JobDescription = jinderPosterUser.JobDescription;
                posterUser.RequiredSkills = jinderPosterUser.RequiredSkills;
                posterUser.SalaryRange = jinderPosterUser.SalaryRange;
                posterUser.OperationHours = jinderPosterUser.OperationHours;

                PosterTable.Add(posterUser);
                dbContext.SaveChanges();

                message.StatusCode = HttpStatusCode.OK;

                return message;
            }
        }
    }
}
