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
    public class ExpressionLogsController : ApiController
    {
        private JinderDBEntities db = new JinderDBEntities();

        // GET: api/ExpressionLogs
        public IQueryable<ExpressionLog> GetExpressionLogs()
        {
            return db.ExpressionLogs;
        }

        // GET: api/ExpressionLogs/5
        [ResponseType(typeof(ExpressionLog))]
        public IHttpActionResult GetExpressionLog(int id)
        {
            ExpressionLog expressionLog = db.ExpressionLogs.Find(id);
            if (expressionLog == null)
            {
                return NotFound();
            }

            return Ok(expressionLog);
        }

        // PUT: api/ExpressionLogs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutExpressionLog(int id, ExpressionLog expressionLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != expressionLog.ExpressionLogId)
            {
                return BadRequest();
            }

            db.Entry(expressionLog).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpressionLogExists(id))
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

        // POST: api/ExpressionLogs
        [ResponseType(typeof(ExpressionLog))]
        public IHttpActionResult PostExpressionLog(ExpressionLog expressionLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ExpressionLogs.Add(expressionLog);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = expressionLog.ExpressionLogId }, expressionLog);
        }

        // DELETE: api/ExpressionLogs/5
        [ResponseType(typeof(ExpressionLog))]
        public IHttpActionResult DeleteExpressionLog(int id)
        {
            ExpressionLog expressionLog = db.ExpressionLogs.Find(id);
            if (expressionLog == null)
            {
                return NotFound();
            }

            db.ExpressionLogs.Remove(expressionLog);
            db.SaveChanges();

            return Ok(expressionLog);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExpressionLogExists(int id)
        {
            return db.ExpressionLogs.Count(e => e.ExpressionLogId == id) > 0;
        }
    }
}