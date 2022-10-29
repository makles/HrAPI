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
    public class CustomerMasterController : ApiController
    {
        private DbEntities db = new DbEntities();

        [HttpGet]
        public IQueryable<CUSTOMER_MASTER> GetALL()
        {
            return db.CUSTOMER_MASTER;
        }
        [HttpGet]
        public IHttpActionResult GetByID(int id)
        {
            CUSTOMER_MASTER CUSTOMER_MASTER = db.CUSTOMER_MASTER.FirstOrDefault(f => f.CUSTOMER_ID == id && f.IS_RECEIVED != "Y");
            if (CUSTOMER_MASTER == null)
            {
                return NotFound();
            }
            return Ok(CUSTOMER_MASTER);
        }
        [HttpPut]
        public IHttpActionResult Put(int id, CUSTOMER_MASTER entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != entity.CUSTOMER_ID)
                {
                    return BadRequest();
                }

                db.Entry(entity).State = EntityState.Modified;
                int rowcount = db.SaveChanges();
                if (rowcount > 0)
                {
                    return Ok("Update Sucessfully");
                }
                else
                {
                    return Ok("Update Failed");
                }
            }
            catch (Exception ex)
            {
                var retResponse = ex.InnerException.ToString().Contains("cannot insert NULL into") ? "Please check all required field" : ex.Message;
                return BadRequest(retResponse);
            }
        }
        [HttpPost]
        public IHttpActionResult Post([FromBody] CUSTOMER_MASTER entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var list = db.CUSTOMER_MASTER.ToList();
                var maxPK = FindMax(list);
                entity.CUSTOMER_ID = maxPK;
                entity.IS_RECEIVED = "N";
                db.CUSTOMER_MASTER.Add(entity);
                int rowcount = db.SaveChanges();
                if (rowcount > 0)
                {
                    return Ok("Save Sucessfully");
                }
                else
                {
                    return Ok("Save Failed");
                }
            }
            catch (Exception ex)
            {
                var retResponse = ex.InnerException.ToString().Contains("cannot insert NULL into") ? "Please check all required field" : ex.Message;
                return BadRequest(retResponse);
            }

        }
        private int FindMax(List<CUSTOMER_MASTER> list)
        {
            int maxId = int.MinValue;
            if (list.Count == 0)
            {
                maxId = 1;
            }
            else
            {
                var InOutId = list.Max(b => b.CUSTOMER_ID);
                maxId = (int)InOutId + 1;
            }
            return maxId;
        }
    }
}
