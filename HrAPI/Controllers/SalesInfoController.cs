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
using HaierDbEntity;

namespace HrAPI.Controllers
{
    public class SalesInfoController : ApiController
    {
        private DbEntities db = new DbEntities();

        [HttpGet]
        public IQueryable<SALES_INFO> Get()
        {
            return db.SALES_INFO.OrderBy(O => O.SALES_ID);
        }
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            SALES_INFO SALES_INFO = db.SALES_INFO.FirstOrDefault(f => f.SALES_ID == id && f.IS_RECEIVED != "Y");
            if (SALES_INFO == null)
            {
                return NotFound();
            }
            return Ok(SALES_INFO);
        }
        [HttpGet]
        public IHttpActionResult GetByInvoiceNo(string invNo)
        {
            var dataList = db.SALES_INFO.Where(f => f.CHALLAN_NO == invNo && f.IS_RECEIVED != "Y").ToList();
            if (dataList == null)
            {
                return BadRequest("Requested Invoice Not Found");
            }
            return Ok(dataList);
        }
        [HttpPut]
        public IHttpActionResult Put(string invNo, List<SALES_INFO> list)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (invNo == null)
                {
                    return BadRequest();
                }
                foreach (var item in list)
                {
                    var data = db.SALES_INFO.Where(w => w.CHALLAN_NO == invNo && w.SALES_ID == item.SALES_ID).FirstOrDefault();
                    if (data == null)
                    {
                        return BadRequest("Requested Invoice Not Found");
                    }
                    else
                     {
                        data.SALES_ID = item.SALES_ID;
                        data.CUSTOMER_CODE = item.CUSTOMER_CODE;
                        data.CUSTOMER_NAME = item.CUSTOMER_NAME;
                        data.CUSTOMER_ADDRESS= item.CUSTOMER_ADDRESS;
                        data.CUSTOMER_VATREGNO = item.CUSTOMER_VATREGNO;
                        data.FINAL_DESTINATION = item.FINAL_DESTINATION;
                        data.VEICHLE_INFO = item.VEICHLE_INFO;
                        data.CHALLAN_NO = item.CHALLAN_NO;
                        data.CHALLAN_DATE = item.CHALLAN_DATE;
                        data.PRODUCT_CODE = item.PRODUCT_CODE;
                        data.PRODUCT_NAME = item.PRODUCT_NAME;
                        data.PRODUCT_UNIT = item.PRODUCT_UNIT;
                        data.PRODUCT_QTY = item.PRODUCT_QTY;
                        data.PRODUCT_UNITPRICE = item.PRODUCT_UNITPRICE;
                        data.VATBASED_VALUE = item.VATBASED_VALUE;
                        data.PRODUCT_SD_APPLICABLE_PRICE = item.PRODUCT_SD_APPLICABLE_PRICE;
                        data.PRODUCT_SD_AMOUNT = item.PRODUCT_SD_AMOUNT;
                        data.PRODUCT_VAT_RATE = item.PRODUCT_VAT_RATE;
                        data.TOTAL_AMOUNT = item.TOTAL_AMOUNT;
                        data.PRODUCT_VAT_AMOUNT = item.PRODUCT_VAT_AMOUNT;
                        data.BRANCH_ID = item.BRANCH_ID;
                        data.BRANCH_CODE = item.BRANCH_CODE;
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
        public IHttpActionResult Post([FromBody] List<SALES_INFO> list)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var maxPK = 0;
                var lst = db.SALES_INFO.ToList();
                maxPK = FindMax(lst);
                foreach (var item in list)
                {
                    item.SALES_ID = maxPK;
                    item.IS_RECEIVED = "N";
                    db.SALES_INFO.Add(item);
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
        private int FindMax(List<SALES_INFO> list)
        {
            int maxId = int.MinValue;
            if (list.Count == 0)
            {
                maxId = 1;
            }
            else
            {
                var InOutId = list.Max(b => b.SALES_ID);
                maxId = (int)InOutId + 1;
            }
            return maxId;
        }


    }
}