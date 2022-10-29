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
    public class PurchaseInfoController : ApiController
    {
        private DbEntities db = new DbEntities();

        [HttpGet]
        public IQueryable<PURCHASE_INFO> GetAll()
        {
            return db.PURCHASE_INFO;
        }

        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            PURCHASE_INFO entity = db.PURCHASE_INFO.FirstOrDefault(C => C.PURCHASE_ID == id && C.IS_RECEIVED != "Y");
            if (entity == null)
            {
                return BadRequest();
            }
            return Ok(entity);
        }

        [HttpGet]
        public IHttpActionResult GetByInvoiceNo(string invNo)
        {
            var dataList = db.PURCHASE_INFO.Where(f => f.CHALLAN_NO == invNo && f.IS_RECEIVED != "Y").ToList();
            if (dataList == null)
            {
                return NotFound();
            }
            return Ok(dataList);
        }
        [HttpPut]
        public IHttpActionResult Put(string invNo, List<PURCHASE_INFO> list)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (invNo == null)
                {
                    return BadRequest(ModelState);
                }
                foreach (var item in list)
                {
                    var data = db.PURCHASE_INFO.Where(w => w.CHALLAN_NO == invNo && w.PURCHASE_ID == item.PURCHASE_ID).FirstOrDefault();
                    if (data == null)
                    {
                        return BadRequest("Requested Invoice Not Found");
                    }
                    else
                    {
                        data.PURCHASE_ID = item.PURCHASE_ID;
                        data.SUPPLIER_CODE = item.SUPPLIER_CODE;
                        data.SUPPLIER_ADDRESS = item.SUPPLIER_ADDRESS;
                        data.SUPPLIER_BIN = item.SUPPLIER_BIN;
                        data.CHALLAN_NO = item.CHALLAN_NO;
                        data.CHALLAN_DATE = item.CHALLAN_DATE;
                        data.LCNO = item.LCNO;
                        data.LCDATE = item.LCDATE;
                        data.PONO = item.PONO;
                        data.PODATE = item.PODATE;
                        data.CUSTOMSID = item.CUSTOMSID;
                        data.ITEMCODE = item.ITEMCODE;
                        data.ITEMNAME = item.ITEMNAME;
                        data.UNITCODE = item.UNITCODE;
                        data.RECEIVEDQTY = item.RECEIVEDQTY;
                        data.UNIT_PRICE = item.UNIT_PRICE;
                        data.VALUE = item.VALUE;
                        data.CD = item.CD;
                        data.RD = item.RD;
                        data.SD = item.SD;
                        data.VAT = item.VAT;
                        data.AIT = item.AIT;
                        data.AT = item.AT;
                        data.REBATE = item.REBATE;
                        data.VATPERCENT = item.VATPERCENT;
                        data.VATABLEVALUE = item.VATABLEVALUE;
                        data.HSCODE = item.HSCODE;
                        data.SPTYPE = item.SPTYPE;
                        data.EQUIPMENT_SPAREPARTS = item.EQUIPMENT_SPAREPARTS;
                        data.PURCHASETYPE = item.PURCHASETYPE;
                        data.TDATE = item.TDATE;
                        data.BRANCH_ID = item.BRANCH_ID;
                        data.BRANCH_CODE = item.BRANCH_CODE;
                        data.IS_RECEIVED = item.IS_RECEIVED;
                        db.Entry(data).State = EntityState.Modified;
                    }
                }

                int rowCount = db.SaveChanges();
                if (rowCount > 0)
                {
                    return Ok("Update Successfully");
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
        public IHttpActionResult Post(List<PURCHASE_INFO> list)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var maxPK = 0;
            var auto = db.PURCHASE_INFO.ToList();
            maxPK = FindMax(auto);
            foreach (var item in list)
            {
                item.PURCHASE_ID = maxPK;
                item.IS_RECEIVED = "N";
                db.PURCHASE_INFO.Add(item);
                maxPK++;
            }

            int rowCount = db.SaveChanges();
            if (rowCount > 0)
            {
                return Ok("Save Successfully");
            }
            return BadRequest("Save Failed");
        }
        private int FindMax(List<PURCHASE_INFO> list)
        {
            int maxId = int.MinValue;
            if (list.Count == 0)
            {
                maxId = 1;
            }
            else
            {
                var InOutId = list.Max(b => b.PURCHASE_ID);
                maxId = (int)InOutId + 1;
            }
            return maxId;
        }
    }
}
