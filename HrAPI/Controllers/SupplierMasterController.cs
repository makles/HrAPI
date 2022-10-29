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
    public class SupplierMasterController : ApiController
    {
        private DbEntities db = new DbEntities();

        [HttpGet]
        public IQueryable<SUPPLIER_MASTER> GetALL()
        {
            return db.SUPPLIER_MASTER;
        }
        [HttpGet]
        public IHttpActionResult GetByID(int id)
        {
            SUPPLIER_MASTER SUPPLIER_MASTER = db.SUPPLIER_MASTER.FirstOrDefault(f => f.SUPPLIER_ID == id && f.IS_RECEIVED != "Y");
            if (SUPPLIER_MASTER == null)
            {
                return NotFound();
            }
            return Ok(SUPPLIER_MASTER);
        }
        [HttpPut]
        public IHttpActionResult Put(int id, SUPPLIER_MASTER entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != entity.SUPPLIER_ID)
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
        public IHttpActionResult Post([FromBody] SUPPLIER_MASTER entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var list = db.SUPPLIER_MASTER.ToList();
                var maxPK = FindMax(list);
                entity.SUPPLIER_ID = maxPK;
                entity.IS_RECEIVED = "N";
                db.SUPPLIER_MASTER.Add(entity);
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
        private int FindMax(List<SUPPLIER_MASTER> list)
        {
            int maxId = int.MinValue;
            if (list.Count == 0)
            {
                maxId = 1;
            }
            else
            {
                var InOutId = list.Max(b => b.SUPPLIER_ID);
                maxId = (int)InOutId + 1;
            }
            return maxId;
        }
    }
}
