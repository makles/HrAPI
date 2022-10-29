using HaierDbEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HrAPI.Controllers
{
    public class BranchMasterController : ApiController
    {
        private DbEntities db = new DbEntities();

        [HttpGet]
        public IQueryable<BRANCH_MASTER> GetALL()
        {
            return db.BRANCH_MASTER;
        }
        [HttpGet]
        public IHttpActionResult GetByID(int id)
        {
            BRANCH_MASTER BRANCH_MASTER = db.BRANCH_MASTER.FirstOrDefault(f => f.BRANCH_ID == id && f.IS_RECEIVED != "Y");
            if (BRANCH_MASTER == null)
            {
                return NotFound();
            }
            return Ok(BRANCH_MASTER);
        }
        [HttpPut]
        public IHttpActionResult Put(int id, BRANCH_MASTER entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != entity.BRANCH_ID)
                {
                    return BadRequest();
                }
                db.Entry(entity).State = EntityState.Modified;
                int rowcount = db.SaveChanges();
                if (rowcount > 0)
                {
                    return Ok("update Sucessfully");
                }
                else
                {
                    return Ok("update Failed");
                }
            }
            catch (Exception ex)
            {
                var retResponse = ex.InnerException.ToString().Contains("cannot insert NULL into") ? "Please check all required field" : ex.Message;
                return BadRequest(retResponse);
            }
        }
        [HttpPost]
        public IHttpActionResult Post([FromBody] BRANCH_MASTER entity)
        {
            string retResponse = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var list = db.BRANCH_MASTER.ToList();
                var maxPK = FindMax(list);
                entity.BRANCH_ID = maxPK;
                entity.IS_RECEIVED = "N";
                db.BRANCH_MASTER.Add(entity);
                int rowcount = db.SaveChanges();
                if (rowcount > 0)
                {
                    retResponse = "Save Sucessfully";
                    return Ok(retResponse);
                }
                else
                {
                    retResponse = "Save Fail";
                }
            }
            catch (Exception ex)
            {
                retResponse = ex.InnerException.ToString().Contains("cannot insert NULL into") ? "Please check all required field" : ex.Message;
            }
            return BadRequest(retResponse);
        }
        private int FindMax(List<BRANCH_MASTER> list)
        {
            int maxId = int.MinValue;
            if (list.Count == 0)
            {
                maxId = 1;
            }
            else
            {
                var InOutId = list.Max(b => b.BRANCH_ID);
                maxId = (int)InOutId + 1;
            }
            return maxId;
        }
    }
}
