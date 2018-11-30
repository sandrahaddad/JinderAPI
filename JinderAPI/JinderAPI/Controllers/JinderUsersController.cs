using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using JinderAPI;

namespace JinderAPI.Controllers
{
    public class JinderUsersController : ApiController
    {
        private JinderDBEntities db = new JinderDBEntities();

        // GET: api/JinderUsers
        public IQueryable<JinderUser> GetJinderUsers()
        {
            return db.JinderUsers;
        }

        // GET: api/JinderUsers/5
        [ResponseType(typeof(JinderUser))]
        public IHttpActionResult GetJinderUser(int id)
        {
            JinderUser jinderUser = db.JinderUsers.Find(id);
            if (jinderUser == null)
            {
                return NotFound();
            }

            return Ok(jinderUser);
        }

        // PUT: api/JinderUsers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutJinderUser(int id, JinderUser jinderUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != jinderUser.JinderUserId)
            {
                return BadRequest();
            }

            db.Entry(jinderUser).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JinderUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/JinderUsers
        [ResponseType(typeof(JinderUser))]
        public IHttpActionResult PostJinderUser(JinderUser jinderUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.JinderUsers.Add(jinderUser);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = jinderUser.JinderUserId }, jinderUser);
        }

        // DELETE: api/JinderUsers/5
        [ResponseType(typeof(JinderUser))]
        public IHttpActionResult DeleteJinderUser(int id)
        {
            JinderUser jinderUser = db.JinderUsers.Find(id);
            if (jinderUser == null)
            {
                return NotFound();
            }

            db.JinderUsers.Remove(jinderUser);
            db.SaveChanges();

            return Ok(jinderUser);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool JinderUserExists(int id)
        {
            return db.JinderUsers.Count(e => e.JinderUserId == id) > 0;
        }
    }
}