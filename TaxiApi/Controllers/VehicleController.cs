using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TaxiApi.Enteties;
using TaxiApi.Models;

namespace TaxiApi.Controllers
{
    public class VehicleController : ApiController
    {
        private readonly TaxiDBEntities DBContext = new TaxiDBEntities();

        [Authorize]
        [HttpGet]
        [Route("api/vehicle/getvehicles")]
        public List<VehicleModel> GetVehicle()
        {
            var list = new List<VehicleModel>();
            foreach (var foundvehicle in DBContext.Vehicle)
            {
                var vehicle = new VehicleModel();
                vehicle.RegNr = foundvehicle.RegNr;
                vehicle.Color_ID = foundvehicle.Color_Id.Value;
                vehicle.Manufacturer_ID = foundvehicle.Manufact_Id;
                vehicle.Model_ID = foundvehicle.Model_Id;
                vehicle.TripMeter = foundvehicle.TripMeter;
                vehicle.YearModel = foundvehicle.YearModel.Value;

                foreach (var report in DBContext.Report.Where(report => foundvehicle.RegNr == report.Vehicle_Id))
                {
                    vehicle.TripMeter += report.Distance.Value;
                }
                foreach (var item in DBContext.VehicleFuel)
                {
                    if (item.Vehicle_Id == vehicle.RegNr)
                    {
                        if (vehicle.PrimaryFuel_ID > 0)
                            vehicle.PrimaryFuel_ID = item.Fuel_Id;
                        else
                        {
                            vehicle.SecondaryFuel_ID = item.Fuel_Id;
                        }

                    }
                }
                list.Add(vehicle);
            }
            return list;
        }

        [Authorize]
        [HttpGet]
        [Route("api/vehicle/gettripmeter/{regnr}")]
        public int GetVehicle(string regnr)
        {
            var vehicle = DBContext.Vehicle.FirstOrDefault(x => x.RegNr == regnr);

            if (vehicle != null) return vehicle.TripMeter;
            return 0;
        }

       
        [Authorize]
        [HttpGet]
        [Route("api/vehicles/getvehicles/{userid}")]
        public List<VehicleModel>GetUsedVehicles(int userid)
        {
            var list = new List<VehicleModel>();
            foreach (var report in DBContext.Report)
            {
                if (report.Employee_Id == userid)
                {
                    foreach (var vehicle in DBContext.Vehicle)
                    {
                        if (vehicle.RegNr == report.Vehicle_Id)
                        {
                            list.Add(new VehicleModel()
                            {
                                RegNr = vehicle.RegNr,
                                TripMeter = vehicle.TripMeter,
                                Color_ID = vehicle.Color_Id.Value,
                                Manufacturer_ID = vehicle.Manufact_Id,
                                Model_ID = vehicle.Model_Id,
                                YearModel = vehicle.YearModel.Value,
                                PrimaryFuel_ID = report.PrimaryFuel_ID,
                                SecondaryFuel_ID = report.SecondaryFuel_ID
                            });
                        }
                    }
                }
            }
            var distinctList = list.GroupBy(x => x.RegNr).Select(x => x.First());
            return distinctList.ToList();
        }


        [Authorize]
        [HttpGet]
        [Route("api/vehicle/getforviewvehicles")]
        public List<VehicleModelForView> GetVehicles()
        {
            var list = new List<VehicleModelForView>();
            foreach (var vehicle in DBContext.Vehicle)
            {
                var addVehicle = new VehicleModelForView()
                {
                    RegNr = vehicle.RegNr,
                    Color = vehicle.Color.ColorName,
                    Model = vehicle.Model.ModelType,
                    Manufacturer = vehicle.Manufacturer.ManufactName,
                    Yearmodel = vehicle.YearModel.Value
                };

                foreach (var report in DBContext.Report)
                {
                    if (report.Vehicle_Id == vehicle.RegNr)
                    {
                        addVehicle.Tripmeter += report.Distance.Value;
                    }
                }
                foreach (var vehicleFuel in DBContext.VehicleFuel)
                {
                    if (addVehicle.PrimaryFuel == null)
                    {
                        if (vehicleFuel.Vehicle_Id == vehicle.RegNr)
                        {
                            addVehicle.PrimaryFuel = vehicleFuel.Fuel.FuelType;
                        }
                    }
                    else
                    {
                        if (vehicleFuel.Vehicle_Id == vehicle.RegNr)
                        {
                            addVehicle.SecondaryFuel = vehicleFuel.Fuel.FuelType;
                        }
                    }

                }
                list.Add(addVehicle);
            }
            return list;
        }

        [Authorize]
        [HttpGet]
        [Route("api/vehicle/getvehicleinfo/{RegNr}")]
        public List<FuelModel> GetFuels(string regnr)
        {
            var fuellist = new List<FuelModel>();
            foreach (var findfuel in DBContext.VehicleFuel)
            {
                if (regnr == findfuel.Vehicle.RegNr)
                {
                    var fuel = new FuelModel()
                    {
                        Id = findfuel.Fuel.Id,
                        FuelType = findfuel.Fuel.FuelType
                    };
                    fuellist.Add(fuel);
                }
            }
            return fuellist;
        }

        [Authorize]
        [HttpGet]
        [Route("api/vehicle/getvehicleinfo/")]
        public AddVehicleInfoModel GetVehicleInfo()
        {
            var avim = new AddVehicleInfoModel();
            var colorList = new List<ColorModel>();
            var modelList = new List<ModelsModel>();
            var fuelList = new List<FuelModel>();
            var manufactList = new List<ManufacturerModel>();

            foreach (var color in DBContext.Color)
            {
                var c = new ColorModel();
                c.Id = color.Id;
                c.ColorName = color.ColorName;
                c.Metallic = color.Metalic;
                colorList.Add(c);
            }

            avim.Colors = colorList;

            foreach (var model in DBContext.Model)
            {
                var m = new ModelsModel();

                m.Id = model.Id;
                m.ModelType = model.ModelType;
                modelList.Add(m);
            }
            avim.Models = modelList;

            foreach (var fuel in DBContext.Fuel)
            {
                var f = new FuelModel();
                f.Id = fuel.Id;
                f.FuelType = fuel.FuelType;
                fuelList.Add(f);
            }
            avim.Fuels = fuelList;

            foreach (var manufacturer in DBContext.Manufacturer)
            {
                var manu = new ManufacturerModel();
                manu.Id = manufacturer.Id;
                manu.ManufactName = manufacturer.ManufactName;
                manufactList.Add(manu);
            }
            avim.Manufacturers = manufactList;

            return avim;
        }

        [Authorize]
        [HttpPost]
        [Route("api/vehicle/addvehicle")]
        public HttpResponseMessage AddVehicle(VehicleModel vehicle)
        {
            var newVehicle = new Vehicle();

            newVehicle.RegNr = vehicle.RegNr;
            newVehicle.Color_Id = vehicle.Color_ID;
            newVehicle.Manufact_Id = vehicle.Manufacturer_ID;
            newVehicle.Model_Id = vehicle.Model_ID;
            newVehicle.TripMeter = vehicle.TripMeter;
            newVehicle.YearModel = vehicle.YearModel;

            var list = new List<VehicleFuel>();
            var fuel = new VehicleFuel()
            {
                Vehicle_Id = vehicle.RegNr,
                Fuel_Id = vehicle.PrimaryFuel_ID
            };
            list.Add(fuel);
            if (vehicle.SecondaryFuel_ID != null)
            {
                var hybridFuel = new VehicleFuel()
                {
                    Vehicle_Id = vehicle.RegNr,
                    Fuel_Id = vehicle.SecondaryFuel_ID.Value
                };
                list.Add(hybridFuel);
            }
            try
            {
                DBContext.Vehicle.Add(newVehicle);
                DBContext.VehicleFuel.AddRange(list);
                DBContext.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.Created);
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.RequestTimeout);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/vehicle/addcolor")]
        public HttpResponseMessage AddColor(ColorModel newcolor)
        {
            foreach (var findcolor in DBContext.Color)
            {
                if (newcolor.ColorName.ToLower() == findcolor.ColorName.ToLower())
                {
                    foreach (var ismetalic in DBContext.Color)
                    {
                        if (newcolor.ColorName.ToLower() == ismetalic.ColorName.ToLower())
                        {
                            if (newcolor.Metallic == ismetalic.Metalic)
                            {
                                return new HttpResponseMessage(HttpStatusCode.Conflict);
                            }
                        }
                    }
                }
            }

            var color = new Color();
            color.ColorName = newcolor.ColorName;
            color.Metalic = newcolor.Metallic;
            try
            {
                DBContext.Color.Add(color);
                DBContext.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.Created);
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/vehicle/addfuel")]
        public HttpResponseMessage AddFuel(FuelModel fuel)
        {
            var fuelType = new Fuel() { FuelType = fuel.FuelType };

            try
            {
                foreach (var checkfuel in DBContext.Fuel)
                {
                    if (checkfuel.FuelType == fuel.FuelType)
                    {
                        return new HttpResponseMessage(HttpStatusCode.Conflict);
                    }
                }
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            DBContext.Fuel.Add(fuelType);
            DBContext.SaveChanges();
            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        [Authorize]
        [HttpPost]
        [Route("api/vehicle/addmodel")]
        public HttpResponseMessage AddModel(ModelsModel model)
        {
            var modeltype = new Model() { ModelType = model.ModelType };
            try
            {
                foreach (var checkmodel in DBContext.Model)
                {
                    if (checkmodel.ModelType == model.ModelType)
                    {
                        return new HttpResponseMessage(HttpStatusCode.Conflict);
                    }
                }
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            DBContext.Model.Add(modeltype);
            DBContext.SaveChanges();
            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        [Authorize]
        [Route("api/vehicle/addmanufacturer")]
        public HttpResponseMessage AddManufacturer(ManufacturerModel manufacturer)
        {
            var newManufaturer = new Manufacturer() { ManufactName = manufacturer.ManufactName };
            try
            {
                foreach (var checkManufacturer in DBContext.Manufacturer)
                {
                    if (checkManufacturer.ManufactName == newManufaturer.ManufactName)
                    {
                        return new HttpResponseMessage(HttpStatusCode.Conflict);
                    }
                }
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            DBContext.Manufacturer.Add(newManufaturer);
            DBContext.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

    }
}
