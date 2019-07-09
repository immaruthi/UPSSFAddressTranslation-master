using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using UPS.DataObjects.Shipment;
using UPS.DataObjects.UserData;
using UPS.ServicesAsyncActions;
using UPS.ServicesDataRepository.Common;
using UPS.ServicesDataRepository.DataContext;

namespace UPS.ServicesDataRepository
{
    public class ShipmentService : IShipmentAsync
    {
        private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;
        private readonly ApplicationDbContext context;
        private IAddressBookAsync addressBookService;

        public ShipmentService(ApplicationDbContext applicationDbContext, IAddressBookAsync addressBookService)
        {
            this.context = applicationDbContext;
            this.addressBookService = addressBookService;
        }
        public List<ShipmentDataRequest> GetShipment(int workflowID)
        {
            optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            List<ShipmentDataRequest> shipmentDataRequests = null;
            try
            {
                optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    shipmentDataRequests = new List<ShipmentDataRequest>();
                    var anonymousList =
                        (
                            from s in context.shipmentDataRequests
                            where s.WFL_ID == workflowID 
                            && s.SMT_STA_NR != (int)Enums.ATStatus.Completed
                            && s.SMT_STA_NR != (int)Enums.ATStatus.Inactive
                            orderby s.ID
                            select new
                            {
                                s.ID,
                                s.WFL_ID,
                                s.ACY_TE,
                                s.BIL_TYP_TE,
                                s.CCY_VAL_TE,
                                s.COD_TE,
                                s.CON_NR,
                                s.CSG_CTC_TE,
                                s.DIM_WGT_DE,
                                s.DST_CTY_TE,
                                s.DST_PSL_TE,
                                s.EXP_SLC_CD,
                                s.EXP_TYP,
                                s.FST_INV_LN_DES_TE,
                                s.IMP_NR,
                                s.IMP_SLC_TE,
                                s.IN_FLG_TE,
                                s.ORG_CTY_TE,
                                s.ORG_PSL_CD,
                                s.OU_FLG_TE,
                                s.PCS_QTY_NR,
                                s.PH_NR,
                                s.PKG_NR_TE,
                                s.PKG_WGT_DE,
                                s.PK_UP_TM,
                                s.PYM_MTD,
                                s.PY_MT_TE,
                                s.QQS_TRA_LG_ID,
                                s.RCV_ADR_TE,
                                s.RCV_CPY_TE,
                                s.SF_TRA_LG_ID,
                                s.SHP_ADR_TE,
                                s.SHP_ADR_TR_TE,
                                s.SHP_CPY_NA,
                                s.SHP_CTC_TE,
                                s.SHP_DT,
                                s.SHP_NR,
                                s.SHP_PH_TE,
                                s.SMT_NR_TE,
                                s.SMT_STA_NR,
                                s.SMT_VAL_DE,
                                s.SMT_WGT_DE,
                                s.SPC_SLIC_NR,
                                s.SVL_NR,
                                s.WGT_UNT_TE,
                                s.POD_RTN_SVC
                            }).ToList();

                    foreach (var shipmentData in anonymousList)
                    {
                        ShipmentDataRequest shipmentDataRequest = new ShipmentDataRequest();
                        shipmentDataRequest.ID = shipmentData.ID;
                        shipmentDataRequest.BIL_TYP_TE = shipmentData.BIL_TYP_TE;
                        shipmentDataRequest.CCY_VAL_TE = shipmentData.CCY_VAL_TE;
                        shipmentDataRequest.COD_TE = shipmentData.COD_TE;
                        shipmentDataRequest.CSG_CTC_TE = shipmentData.CSG_CTC_TE;
                        shipmentDataRequest.DIM_WGT_DE = shipmentData.DIM_WGT_DE;
                        shipmentDataRequest.DST_CTY_TE = shipmentData.DST_CTY_TE;
                        shipmentDataRequest.DST_PSL_TE = shipmentData.DST_PSL_TE;
                        shipmentDataRequest.EXP_SLC_CD = shipmentData.EXP_SLC_CD;
                        shipmentDataRequest.EXP_TYP = shipmentData.EXP_TYP;
                        shipmentDataRequest.FST_INV_LN_DES_TE = shipmentData.FST_INV_LN_DES_TE;
                        shipmentDataRequest.IMP_NR = shipmentData.IMP_NR;
                        shipmentDataRequest.IMP_SLC_TE = shipmentData.IMP_SLC_TE;
                        shipmentDataRequest.IN_FLG_TE = shipmentData.IN_FLG_TE;
                        shipmentDataRequest.ORG_CTY_TE = shipmentData.ORG_CTY_TE;
                        shipmentDataRequest.ORG_PSL_CD = shipmentData.ORG_PSL_CD;
                        shipmentDataRequest.OU_FLG_TE = shipmentData.OU_FLG_TE;
                        shipmentDataRequest.PCS_QTY_NR = shipmentData.PCS_QTY_NR;
                        shipmentDataRequest.PH_NR = shipmentData.PH_NR;
                        shipmentDataRequest.PKG_NR_TE = shipmentData.PKG_NR_TE;
                        shipmentDataRequest.PKG_WGT_DE = shipmentData.PKG_WGT_DE;
                        shipmentDataRequest.PK_UP_TM = shipmentData.PK_UP_TM;
                        shipmentDataRequest.PYM_MTD = shipmentData.PYM_MTD;
                        shipmentDataRequest.PY_MT_TE = shipmentData.PY_MT_TE;
                        shipmentDataRequest.QQS_TRA_LG_ID = shipmentData.QQS_TRA_LG_ID;
                        shipmentDataRequest.RCV_ADR_TE = shipmentData.RCV_ADR_TE;
                        shipmentDataRequest.RCV_CPY_TE = shipmentData.RCV_CPY_TE;
                        shipmentDataRequest.SF_TRA_LG_ID = shipmentData.SF_TRA_LG_ID;
                        shipmentDataRequest.SHP_ADR_TE = shipmentData.SHP_ADR_TE;
                        shipmentDataRequest.SHP_ADR_TR_TE = shipmentData.SHP_ADR_TR_TE;
                        shipmentDataRequest.SHP_CPY_NA = shipmentData.SHP_CPY_NA;
                        shipmentDataRequest.SHP_CTC_TE = shipmentData.SHP_CTC_TE;
                        shipmentDataRequest.SHP_DT = shipmentData.SHP_DT;
                        shipmentDataRequest.SHP_NR = shipmentData.SHP_NR;
                        shipmentDataRequest.SHP_PH_TE = shipmentData.SHP_PH_TE;
                        shipmentDataRequest.SMT_NR_TE = shipmentData.SMT_NR_TE;
                        shipmentDataRequest.SMT_STA_NR = shipmentData.SMT_STA_NR;

                        switch (shipmentDataRequest.SMT_STA_NR)
                        {
                            case 0:
                                shipmentDataRequest.SMT_STA_TE = "Uploaded";
                                break;
                            case 1:
                                shipmentDataRequest.SMT_STA_TE = "Curated";
                                break;
                            case 2:
                                shipmentDataRequest.SMT_STA_TE = "Translated";
                                break;
                            case 3:
                                shipmentDataRequest.SMT_STA_TE = "Completed";
                                break;
                            case 4:
                                shipmentDataRequest.SMT_STA_TE = "Inactive";
                                break;
                            default:
                                shipmentDataRequest.SMT_STA_TE = "Uploaded";
                                break;
                        }

                        shipmentDataRequest.SMT_VAL_DE = shipmentData.SMT_VAL_DE;
                        shipmentDataRequest.SMT_WGT_DE = shipmentData.SMT_WGT_DE;
                        shipmentDataRequest.SVL_NR = shipmentData.SVL_NR;
                        shipmentDataRequest.WFL_ID = shipmentData.WFL_ID;
                        shipmentDataRequest.WGT_UNT_TE = shipmentData.WGT_UNT_TE;
                        shipmentDataRequest.ACY_TE = shipmentData.ACY_TE;
                        shipmentDataRequest.CON_NR = shipmentData.CON_NR;
                        shipmentDataRequest.SPC_SLIC_NR = shipmentData.SPC_SLIC_NR;
                        shipmentDataRequest.POD_RTN_SVC = shipmentData.POD_RTN_SVC;

                        shipmentDataRequests.Add(shipmentDataRequest);
                    }
                }
            }
            catch
            {

            }

            return shipmentDataRequests;
        }

        public int CreateShipment(ShipmentDataRequest shipmentData)
        {
            optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(optionsBuilder.Options))
            {
                ShipmentDataRequest shipmentDataRequest = new ShipmentDataRequest();
                shipmentDataRequest.BIL_TYP_TE = shipmentData.BIL_TYP_TE;
                shipmentDataRequest.CCY_VAL_TE = shipmentData.CCY_VAL_TE;
                shipmentDataRequest.COD_TE = shipmentData.COD_TE;
                shipmentDataRequest.CSG_CTC_TE = shipmentData.CSG_CTC_TE;
                shipmentDataRequest.DIM_WGT_DE = shipmentData.DIM_WGT_DE;
                shipmentDataRequest.DST_CTY_TE = shipmentData.DST_CTY_TE;
                shipmentDataRequest.DST_PSL_TE = shipmentData.DST_PSL_TE;
                shipmentDataRequest.EXP_SLC_CD = shipmentData.EXP_SLC_CD;
                shipmentDataRequest.EXP_TYP = shipmentData.EXP_TYP;
                shipmentDataRequest.FST_INV_LN_DES_TE = shipmentData.FST_INV_LN_DES_TE;
                shipmentDataRequest.IMP_NR = shipmentData.IMP_NR;
                shipmentDataRequest.IMP_SLC_TE = shipmentData.IMP_SLC_TE;
                shipmentDataRequest.IN_FLG_TE = shipmentData.IN_FLG_TE;
                shipmentDataRequest.ORG_CTY_TE = shipmentData.ORG_CTY_TE;
                shipmentDataRequest.ORG_PSL_CD = shipmentData.ORG_PSL_CD;
                shipmentDataRequest.OU_FLG_TE = shipmentData.OU_FLG_TE;
                shipmentDataRequest.PCS_QTY_NR = shipmentData.PCS_QTY_NR;
                shipmentDataRequest.PH_NR = shipmentData.PH_NR;
                shipmentDataRequest.PKG_NR_TE = shipmentData.PKG_NR_TE;
                shipmentDataRequest.PKG_WGT_DE = shipmentData.PKG_WGT_DE;
                shipmentDataRequest.PK_UP_TM = shipmentData.PK_UP_TM;
                shipmentDataRequest.PYM_MTD = shipmentData.PYM_MTD;
                shipmentDataRequest.PY_MT_TE = shipmentData.PY_MT_TE;
                shipmentDataRequest.QQS_TRA_LG_ID = shipmentData.QQS_TRA_LG_ID;
                shipmentDataRequest.RCV_ADR_TE = shipmentData.RCV_ADR_TE;
                shipmentDataRequest.RCV_CPY_TE = shipmentData.RCV_CPY_TE;
                shipmentDataRequest.SF_TRA_LG_ID = shipmentData.SF_TRA_LG_ID;
                shipmentDataRequest.SHP_ADR_TE = shipmentData.SHP_ADR_TE;
                shipmentDataRequest.SHP_ADR_TR_TE = shipmentData.SHP_ADR_TR_TE;
                shipmentDataRequest.SHP_CPY_NA = shipmentData.SHP_CPY_NA;
                shipmentDataRequest.SHP_CTC_TE = shipmentData.SHP_CTC_TE;
                shipmentDataRequest.SHP_DT = shipmentData.SHP_DT;
                shipmentDataRequest.SHP_NR = shipmentData.SHP_NR;
                shipmentDataRequest.SHP_PH_TE = shipmentData.SHP_PH_TE;
                shipmentDataRequest.SMT_NR_TE = shipmentData.SMT_NR_TE;
                shipmentDataRequest.SMT_STA_NR = shipmentData.SMT_STA_NR;
                shipmentDataRequest.SMT_VAL_DE = shipmentData.SMT_VAL_DE;
                shipmentDataRequest.SMT_WGT_DE = shipmentData.SMT_WGT_DE;
                shipmentDataRequest.SVL_NR = shipmentData.SVL_NR;
                shipmentDataRequest.WFL_ID = shipmentData.WFL_ID;
                shipmentDataRequest.WGT_UNT_TE = shipmentData.WGT_UNT_TE;
                shipmentDataRequest.ACY_TE = shipmentData.ACY_TE;
                shipmentDataRequest.CON_NR = shipmentData.CON_NR;
                shipmentDataRequest.SPC_SLIC_NR = shipmentData.SPC_SLIC_NR;
                shipmentDataRequest.POD_RTN_SVC = shipmentData.POD_RTN_SVC;
                context.shipmentDataRequests.Add(shipmentDataRequest);
                context.Entry(shipmentDataRequest).State = EntityState.Added;
                context.SaveChanges();
            }

            return 0;
        }

        public ShipmentDataResponse CreateShipments(List<ShipmentDataRequest> shipmentData)
        {
            ShipmentDataResponse shipmentDataResponse = new ShipmentDataResponse();
            try
            {
                optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    context.BulkInsert(shipmentData);
                    shipmentDataResponse.Shipments = shipmentData; //context.shipmentDataRequests.ToList();
                    shipmentDataResponse.Success = true;
                    return shipmentDataResponse;
                }
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }

        public ShipmentDataResponse UpdateShipmentStatusById(ShipmentDataRequest shipmentDataRequest)
        {
            ShipmentDataResponse shipmentDataResponse = new ShipmentDataResponse();
            try
            {
                optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.EnableSensitiveDataLogging(true);

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    ShipmentDataRequest data = context.shipmentDataRequests.Where(s => s.ID == shipmentDataRequest.ID).FirstOrDefault();
                    data.ID = shipmentDataRequest.ID;
                    data.WFL_ID = shipmentDataRequest.WFL_ID;
                    data.SMT_STA_NR = shipmentDataRequest.SMT_STA_NR;
                    context.shipmentDataRequests.Update(data);
                    context.Entry(shipmentDataRequest).State = EntityState.Detached;
                    context.SaveChanges();
                    shipmentDataResponse.ShipmentDataRequest = context.shipmentDataRequests.Where(s => s.ID == shipmentDataRequest.ID).FirstOrDefault();
                    shipmentDataResponse.Success = true;
                    return shipmentDataResponse;
                }
            }
            catch (Exception ex)
            {
                shipmentDataResponse.Success = false;
                shipmentDataResponse.OperationExceptionMsg = ex.Message;
            }
            return shipmentDataResponse;
        }

        public int? SelectShipmentTotalStatusByWorkflowId(int wid)
        {
            int? result = 0;
            decimal? i = 0;
            decimal? count = 1;
            decimal? avg = 0; 
            try
            {
                optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.EnableSensitiveDataLogging(true);

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    i = context.shipmentDataRequests.Where(ship => ship.WFL_ID == wid).Sum(s => s.SMT_STA_NR);
                    count = context.shipmentDataRequests.Where(ship => ship.WFL_ID == wid).Count();
                    i = i ?? 0;
                    count = count ?? 1;
                    avg = i / count;
                    avg = Math.Round(avg.Value);
                    result = Convert.ToInt32(avg);
                    return result ?? 0;
                }
            }
            catch
            {
                return result;
            }
        }

        public ShipmentDataResponse UpdateShipmentAddressById(ShipmentDataRequest shipmentDataRequest)
        {
            ShipmentDataResponse shipmentDataResponse = new ShipmentDataResponse();
            try
            {
                optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    ShipmentDataRequest data = context.shipmentDataRequests.Where(s => s.ID == shipmentDataRequest.ID).FirstOrDefault();
                    int? shipmentStaus =
                       data.SHP_ADR_TR_TE.ToLower() != shipmentDataRequest.SHP_ADR_TR_TE.ToLower()
                       ? ((int)Enums.ATStatus.Curated)
                       : (data.SMT_STA_NR);

                    data.SHP_ADR_TR_TE = shipmentDataRequest.SHP_ADR_TR_TE;
                    data.COD_TE = shipmentDataRequest.COD_TE;
                    data.POD_RTN_SVC = shipmentDataRequest.POD_RTN_SVC;
                    data.SMT_STA_NR = shipmentStaus;
                    context.shipmentDataRequests.Update(data);
                    context.Entry(shipmentDataRequest).State = EntityState.Detached;
                    context.SaveChanges();
                    shipmentDataResponse.ShipmentDataRequest = context.shipmentDataRequests.Where(s => s.ID == shipmentDataRequest.ID).FirstOrDefault();
                    shipmentDataResponse.Success = true;
                    return shipmentDataResponse;
                }
            }
            catch (Exception ex)
            {
                shipmentDataResponse.Success = false;
                shipmentDataResponse.OperationExceptionMsg = ex.Message;
            }
            return shipmentDataResponse;
        }
        public void UpdateShipmentAddressByIds(List<ShipmentDataRequest> shipmentDataRequest)
        {
            try
            { 
                context.BulkUpdate(shipmentDataRequest);
            }
            catch (Exception ex)
            {
                
            }
        }
        //public ShipmentDataResponse UpdateShipmentAddressByIds(List<ShipmentDataRequest> shipmentDataRequest)
        //{
        //    ShipmentDataResponse shipmentDataResponse = new ShipmentDataResponse();
        //    try
        //    {
        //        optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();


        //        optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        //        var context = new ApplicationDbContext(optionsBuilder.Options);

        //        List<ShipmentDataRequest> shipmentDetailsToUpdate = new List<ShipmentDataRequest>();
        //        foreach (ShipmentDataRequest request in shipmentDataRequest)
        //        {
        //            ShipmentDataRequest data = context.shipmentDataRequests.Where(s => s.ID == request.ID).FirstOrDefault();
        //            data.ID = request.ID;
        //            data.WFL_ID = request.WFL_ID;
        //            data.SHP_ADR_TR_TE = request.SHP_ADR_TR_TE;
        //            data.SMT_STA_NR = request.SMT_STA_NR;
        //            data.ACY_TE = request.ACY_TE;
        //            data.CON_NR = request.CON_NR;
        //            data.POD_RTN_SVC = request.POD_RTN_SVC;
        //            shipmentDetailsToUpdate.Add(data);
        //            //context.shipmentDataRequests.Update(data);
        //            //context.Entry(request).State = EntityState.Detached;
        //            //context.SaveChanges();
        //            shipmentDataResponse.Shipments = context.shipmentDataRequests;
        //        }
        //        context.BulkUpdate(shipmentDetailsToUpdate);
        //        //shipmentDataResponse.Shipments = context.shipmentDataRequests;
        //        shipmentDataResponse.Success = true;
        //        return shipmentDataResponse;
        //    }
        //    catch (Exception ex)
        //    {
        //        shipmentDataResponse.Success = false;
        //        shipmentDataResponse.OperationExceptionMsg = ex.Message;
        //    }
        //    return shipmentDataResponse;
        //}

        public ShipmentDataResponse DeleteShipments(List<ShipmentDataRequest> shipmentDataRequest)
        {
            ShipmentDataResponse shipmentDataResponse = new ShipmentDataResponse();
            try
            {
                optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();


                optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

                var context = new ApplicationDbContext(optionsBuilder.Options);

                foreach (ShipmentDataRequest request in shipmentDataRequest)
                {
                    ShipmentDataRequest data = context.shipmentDataRequests.Where(s => s.ID == request.ID).FirstOrDefault();
                    context.shipmentDataRequests.Remove(data);
                    context.Entry(request).State = EntityState.Detached;
                    context.SaveChanges();
                }
                shipmentDataResponse.Success = true;
                return shipmentDataResponse;
            }
            catch (Exception ex)
            {
                shipmentDataResponse.Success = false;
                shipmentDataResponse.OperationExceptionMsg = ex.Message;
            }
            return shipmentDataResponse;
        }

        public ShipmentDataResponse DeleteShipment(ShipmentDataRequest shipmentDataRequest)
        {
            ShipmentDataResponse shipmentDataResponse = new ShipmentDataResponse();
            try
            {
                optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();


                optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

                var context = new ApplicationDbContext(optionsBuilder.Options);

                ShipmentDataRequest data = context.shipmentDataRequests.Where(s => s.ID == shipmentDataRequest.ID).FirstOrDefault();
                context.shipmentDataRequests.Remove(data);
                context.Entry(shipmentDataRequest).State = EntityState.Deleted;
                context.SaveChanges();
                shipmentDataResponse.Shipments = context.shipmentDataRequests;
                shipmentDataResponse.Success = true;
                return shipmentDataResponse;
            }
            catch (Exception ex)
            {
                shipmentDataResponse.Success = false;
                shipmentDataResponse.OperationExceptionMsg = ex.Message;
            }
            return shipmentDataResponse;
        }
    }
}
