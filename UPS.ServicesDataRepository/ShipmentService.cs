using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using UPS.DataObjects.Shipment;
using UPS.DataObjects.UserData;
using UPS.ServicesAsyncActions;
using UPS.ServicesDataRepository.DataContext;

namespace UPS.ServicesDataRepository
{
    public class ShipmentService : IShipmentAsync
    {
        private string connectionString { get; set; }

        private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;

        public int CreateShipment(ShipmentDataRequest shipmentData)
        {
            optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(optionsBuilder.Options))
            {
                ShipmentDataRequest shipmentDataRequest = new ShipmentDataRequest();
                shipmentDataRequest.SHP_ADR_TE = shipmentData.SHP_ADR_TE;
                context.shipmentDataRequests.Add(shipmentDataRequest);
                context.Entry(shipmentDataRequest).State = EntityState.Added;
                context.SaveChanges();
                //context.shipmentDataRequests.Add(new ShipmentDataRequest()
                //{
                //    SHP_ADR_TE = shipmentData.SHP_ADR_TE
                //});
            }

            return 0;
        }

        public int CreateShipmentList(List<ShipmentDataRequest> shipmentData)
        {
            throw new NotImplementedException();
        }

        public int DeleteShipment(ShipmentDataRequest shipmentData)
        {
            throw new NotImplementedException();
        }

        public int DeleteShipmentList(List<ShipmentDataRequest> shipmentData)
        {
            throw new NotImplementedException();
        }

        public ShipmentDataResponse GetShipmentData()
        {
            throw new NotImplementedException();
        }

        public List<ShipmentDataResponse> GetShipmentDataList()
        {
            throw new NotImplementedException();
        }

        public ShipmentDataResponse UpdateShipment(ShipmentDataRequest shipmentData)
        {
            throw new NotImplementedException();
        }

        public List<ShipmentDataResponse> UpdateShipmentList(List<ShipmentDataRequest> shipmentData)
        {
            throw new NotImplementedException();
        }
    }
}
