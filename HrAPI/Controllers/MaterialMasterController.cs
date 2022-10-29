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
    public class MaterialMasterController : ApiController
    {
        private DbEntities db = new DbEntities();

        [HttpGet]
        public IQueryable<MATERAIL_MASTER> GetALL()
        {
            return db.MATERAIL_MASTER;
        }
        [HttpGet]
        public IHttpActionResult GetByID(int id)
        {
            MATERAIL_MASTER MATERAIL_MASTER = db.MATERAIL_MASTER.FirstOrDefault(f => f.MATERIAL_ID == id && f.IS_RECEIVED != "Y");
            if (MATERAIL_MASTER == null)
            {
                return NotFound();
            }
            return Ok(MATERAIL_MASTER);
        }
        [HttpPut]
        public IHttpActionResult Put(int id, MATERAIL_MASTER entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != entity.MATERIAL_ID)
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
        public IHttpActionResult Post([FromBody] MATERAIL_MASTER entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var list = db.MATERAIL_MASTER.ToList();
                var maxPK = FindMax(list);
                entity.MATERIAL_ID = maxPK;
                entity.IS_RECEIVED = "N";
                db.MATERAIL_MASTER.Add(entity);
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
        private int FindMax(List<MATERAIL_MASTER> list)
        {
            int maxId = int.MinValue;
            if (list.Count == 0)
            {
                maxId = 1;
            }
            else
            {
                var InOutId = list.Max(b => b.MATERIAL_ID);
                maxId = (int)InOutId + 1;
            }
            return maxId;
        }

    }
}
