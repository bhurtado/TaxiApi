using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TaxiApi.Enteties;
using TaxiApi.Models;

namespace TaxiApi.Controllers
{

    public class MaintenanceController : ApiController
    {
        private TaxiDBEntities DBContext = new TaxiDBEntities();
        [Authorize]
        [HttpGet]
        [Route("api/maintenance/getmaintenance/{regnr}")]
        public List<OtherCostsModel> GetMaintenance(string regNr)
        {
            var list = new List<OtherCostsModel>();
            foreach (var maintenance in DBContext.Maintenance)
            {
                if (maintenance.Vehicle.RegNr == regNr)
                {
                    list.Add(new OtherCostsModel()
                    {
                        Id = maintenance.Id,
                        Vehicle_Id = maintenance.Vehicle.RegNr,
                        Cost = maintenance.MaintenanceCost,
                        MaintenanceType = maintenance.MaintenanceType,
                        ReportDate = maintenance.ReportDate
                    });
                }
            }
            return list;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/maintenance/addmaintenance/")]
        public IHttpActionResult AddMaintenance(OtherCostsModel maintenance)
        {
            try
            {
                var newMaintenance = new Maintenance()
                {
                    MaintenanceType = maintenance.MaintenanceType,
                    Vehicle = DBContext.Vehicle.FirstOrDefault(x => x.RegNr == maintenance.Vehicle_Id),
                    ReportDate = maintenance.ReportDate,
                    MaintenanceCost = maintenance.Cost
                };

                if (maintenance.Id != null)
                {
                    newMaintenance.Id = maintenance.Id.Value;
                }


                DBContext.Maintenance.AddOrUpdate(x => x.Id, newMaintenance);
                DBContext.SaveChanges();

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
           
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("api/maintenance/deletemaintenance/{id}")]
        public IHttpActionResult DeleteMaintenance(int id)
        {
            try
            {
                var maintenance = DBContext.Maintenance.FirstOrDefault(x => x.Id == id);

                if (maintenance != null)
                {
                    DBContext.Maintenance.Remove(maintenance);
                    DBContext.SaveChanges();
                }
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
          
        }
    }
}
