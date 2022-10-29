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
    public class ProductMasterController : ApiController
    {
        private DbEntities db = new DbEntities();

        [HttpGet]
        public IQueryable<PRODUCT_MASTER> GetAll()
        {
            return db.PRODUCT_MASTER;
        }

        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            PRODUCT_MASTER entity = db.PRODUCT_MASTER.FirstOrDefault(C => C.PRODUCT_ID == id && C.IS_RECEIVED != "Y");
            if (entity == null)
            {
                return BadRequest();
            }
            return Ok(entity);
        }
        [HttpPut]
        public IHttpActionResult Put(int id, PRODUCT_MASTER entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != entity.PRODUCT_ID)
                {
                    return BadRequest(ModelState);
                }
                db.Entry(entity).State = EntityState.Modified;
                int rowCount = db.SaveChanges();
                if (rowCount > 0)
                {
                    return Ok("Update Successfully");
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
        public IHttpActionResult Post([FromBody] PRODUCT_MASTER entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var list = db.PRODUCT_MASTER.ToList();
                var maxPK = FindMax(list);
                entity.PRODUCT_ID = maxPK;
                entity.IS_RECEIVED = "N";
                db.PRODUCT_MASTER.Add(entity);
                int rowCount = db.SaveChanges();
                if (rowCount > 0)
                {
                    return Ok("Save Successfully");
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
        private int FindMax(List<PRODUCT_MASTER> list)
        {
            int maxId = int.MinValue;
            if (list.Count == 0)
            {
                maxId = 1;
            }
            else
            {
                var InOutId = list.Max(b => b.PRODUCT_ID);
                maxId = (int)InOutId + 1;
            }
            return maxId;
        }
    }
}
