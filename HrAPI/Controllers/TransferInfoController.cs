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
    public class TransferInfoController : ApiController
    {
        private DbEntities db = new DbEntities();

        [HttpGet]
        public IQueryable<TRANSFER_INFO> Get()
        {
            return db.TRANSFER_INFO.OrderBy(O => O.TRANSFER_ID);
        }
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            TRANSFER_INFO TRANSFER_INFO = db.TRANSFER_INFO.FirstOrDefault(f => f.TRANSFER_ID == id && f.IS_RECEIVED != "Y");
            if (TRANSFER_INFO == null)
            {
                return NotFound();
            }
            return Ok(TRANSFER_INFO);
        }
        [HttpGet]
        public IHttpActionResult GetByInvoiceNo(string invNo, string TOBRANCH_CODE)
        {
            var dataList = db.TRANSFER_INFO.Where(f => f.CHALLAN_NO == invNo && f.TO_BRANCH_CODE == TOBRANCH_CODE && f.IS_RECEIVED != "Y").ToList();
            if (dataList == null)
            {
                return BadRequest("Requested Invoice Not Found");
            }
            return Ok(dataList);
        }
        [HttpPut]
        public IHttpActionResult Put(string invNo, string FROM_BRANCH_CODE, List<TRANSFER_INFO> list)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (FROM_BRANCH_CODE == null)
                {
                    return BadRequest();
                }
                foreach (var item in list)
                {
                    var data = db.TRANSFER_INFO.Where(w => w.CHALLAN_NO == invNo && w.FROM_BRANCH_CODE == FROM_BRANCH_CODE && w.TRANSFER_ID == item.TRANSFER_ID).FirstOrDefault();
                    if (data == null)
                    {
                        return BadRequest("Requested Invoice Not Found");
                    }
                    else
                    {
                        data.TRANSFER_ID = item.TRANSFER_ID;
                        data.FROM_BRANCH_ID = item.FROM_BRANCH_ID;
                        data.FROM_BRANCH_CODE = item.FROM_BRANCH_CODE;
                        data.TO_BRANCH_ID = item.TO_BRANCH_ID;
                        data.TO_BRANCH_CODE = item.TO_BRANCH_CODE;
                        data.TRANSFER_TYPE = item.TRANSFER_TYPE;
                        data.CHALLAN_NO = item.CHALLAN_NO;
                        data.PRODUCT_CODE = item.PRODUCT_CODE;
                        data.PRODUCT_NAME = item.PRODUCT_NAME;
                        data.PRODUCT_UNIT = item.PRODUCT_UNIT;
                        data.PRODUCT_QTY = item.PRODUCT_QTY;
                        data.UNIT_PRICE = item.UNIT_PRICE;
                        data.PRODUCT_VALUE = item.PRODUCT_VALUE;
                        data.PRODTCT_VAT = item.PRODTCT_VAT;
                        data.TOTAL_PRICE = item.TOTAL_PRICE;
                        data.TRANSFER_DATE = item.TRANSFER_DATE;
                        data.TRANSFER_TIME = item.TRANSFER_TIME;
                        data.VEHICLE_INFO = item.VEHICLE_INFO;
                        data.IS_RECEIVED = item.IS_RECEIVED;
                        db.Entry(data).State = EntityState.Modified;
                    }
                }

                int rowcount = db.SaveChanges();
                if (rowcount > 0)
                {
                    return Ok("Update Sucessfully");
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
        [HttpPost]
        public IHttpActionResult Post([FromBody] List<TRANSFER_INFO> list)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var maxPK = 0;
                var lst = db.TRANSFER_INFO.ToList();
                maxPK = FindMax(lst);
                foreach (var item in list)
                {
                    item.TRANSFER_ID = maxPK;
                    item.IS_RECEIVED = "N";
                    db.TRANSFER_INFO.Add(item);
                    maxPK++;
                }
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
        private int FindMax(List<TRANSFER_INFO> list)
        {
            int maxId = int.MinValue;
            if (list.Count == 0)
            {
                maxId = 1;
            }
            else
            {
                var InOutId = list.Max(b => b.TRANSFER_ID);
                maxId = (int)InOutId + 1;
            }
            return maxId;
        }


    }
}
