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
            if (jinderuser != null)
            {
                message.StatusCode = HttpStatusCode.BadRequest;
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
                message.StatusCode = HttpStatusCode.BadRequest;
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


        [HttpPost]
        [Route("PostLoginInfo")]
        public HttpResponseMessage PostLoginInfo(UserLogin userLogin)
        {
            JinderDBEntities dbContext = new JinderDBEntities();
            var usersTable = dbContext.JinderUsers;

            HttpResponseMessage message = new HttpResponseMessage();


            JinderUser jinderuser = (from user in usersTable
                                     where user.username == userLogin.username && user.password == userLogin.password
                                     select user).FirstOrDefault<JinderUser>();

            if (jinderuser == null)
            {
                message.StatusCode = HttpStatusCode.NotFound;
                return message;
            }
            else
            {
                message.StatusCode = HttpStatusCode.OK;

                return message;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetSeekers")]
        public List<JinderSeeker> GetSeekers(String Username, [FromUri] int count)
        {
            JinderDBEntities dbContext = new JinderDBEntities();
            var usersTable = dbContext.JinderUsers;

            JinderUser jinderuser = (from user in usersTable
                                     where user.username == Username
                                     select user).FirstOrDefault<JinderUser>();

            if (jinderuser.UserType != "poster")
                throw new Exception(" JinderUser with username: " + jinderuser.username + " is not a poster");

            var viewedSeekers = from log in dbContext.ExpressionLogs
                                where log.SourceUserId == jinderuser.JinderUserId
                                select log.TargetUserId;

            //var viewedIds = viewedSeekers.ToList();

            var notViewedSeekers = (from seeker in usersTable
                                    where seeker.UserType == "seeker" && !viewedSeekers.Contains(seeker.JinderUserId)
                                    select seeker).Take<JinderUser>(count);

            List<JinderSeeker> jinderSeekers = new List<JinderSeeker>();

            foreach (JinderUser seeker in notViewedSeekers)
            {
                JinderSeeker js = new JinderSeeker();
                js.username = seeker.username;
                jinderSeekers.Add(js);
            }
            return jinderSeekers;
        }

        [Authorize]
        [HttpGet]
        [Route("GetPosters")]
        public List<JinderPoster> GetPosters(String Username, [FromUri] int count)
        {
            JinderDBEntities dbContext = new JinderDBEntities();
            var usersTable = dbContext.JinderUsers;

            JinderUser jinderuser = (from user in usersTable
                                     where user.username == Username
                                     select user).FirstOrDefault<JinderUser>();

            if (jinderuser.UserType != "seeker")
                throw new Exception(" JinderUser with username: " + jinderuser.username + " is not a seeker");

            var viewedJobPost = from log in dbContext.ExpressionLogs
                                where log.SourceUserId == jinderuser.JinderUserId
                                select log.TargetUserId;

            var notViewedJobs = (from jobpost in dbContext.JobPosts
                                 where !viewedJobPost.Contains(jobpost.JobPostId)
                                 select jobpost).Take<JobPost>(count);

            List<JinderPoster> jobPosts = new List<JinderPoster>();

            foreach (JobPost post in notViewedJobs)
            {
                JinderPoster jp = new JinderPoster();
                jp.JobDescription = post.JobDescription;
                jobPosts.Add(jp);
            }
            return jobPosts;
        }


        [Authorize]
        [HttpPost]
        [Route("PostSwipeAction")]
        public HttpResponseMessage PostSwipeAction(SwipeActionDTO swipeAction)
        {
            JinderDBEntities dbContext = new JinderDBEntities();
            var actionsTable = dbContext.ExpressionLogs;
            var usersTable = dbContext.JinderUsers;

            HttpResponseMessage message = new HttpResponseMessage();


            JinderUser sourcejinderuser = (from user in usersTable
                                           where user.JinderUserId == swipeAction.SourceUserId
                                           select user).FirstOrDefault<JinderUser>();

            JinderUser targetjinderuser = (from user in usersTable
                                           where user.JinderUserId == swipeAction.TargetUserId
                                           select user).FirstOrDefault<JinderUser>();

            if (swipeAction.SourceUserId == null || swipeAction.TargetUserId == null || swipeAction.IsInterested == null)
            {
                message.StatusCode = HttpStatusCode.BadRequest;
                return message;
            }
            else
            if (sourcejinderuser == null || targetjinderuser == null)
            {
                message.StatusCode = HttpStatusCode.NotFound;
                return message;
            }
            else
            {


                ExpressionLog action = new ExpressionLog();

                action.IsInterested = swipeAction.IsInterested;
                action.SourceUserId = swipeAction.SourceUserId;
                action.TargetUserId = swipeAction.TargetUserId;


                actionsTable.Add(action);
                dbContext.SaveChanges();

                message.StatusCode = HttpStatusCode.OK;

                return message;
            }

        }


        [Authorize]
        [HttpGet]
        [Route("GetMatchedCandidate")]
        public List<MatchedCandidate> GetMatchedCandidate(int sourceId, int count)
        {
            JinderDBEntities dbContext = new JinderDBEntities();
            var actionsTable = dbContext.ExpressionLogs;
            var usersTable = dbContext.JinderUsers;

            HttpResponseMessage message = new HttpResponseMessage();


            var targetjinderuser = from user in actionsTable
                                   where user.SourceUserId == sourceId && user.IsInterested == true
                                   select user.TargetUserId;
            var targetsResult = targetjinderuser.ToList();


            var matchedCandidate = (from user in actionsTable
                                    where targetsResult.Contains(user.SourceUserId) &&
                                    user.TargetUserId == sourceId && user.IsInterested == true
                                    select user).Take<ExpressionLog>(count);

            List<MatchedCandidate> candidates = new List<MatchedCandidate>();

            foreach (ExpressionLog log in matchedCandidate)
            {
                MatchedCandidate c = new MatchedCandidate();
                c.TargetUserId = log.TargetUserId;
                candidates.Add(c);
            }
            return candidates;

        }
    
    }
}
