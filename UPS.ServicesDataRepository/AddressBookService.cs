using EFCore.BulkExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using UPS.DataObjects.AddressBook;
using UPS.DataObjects.Common;
using UPS.ServicesAsyncActions;
using UPS.ServicesDataRepository.DataContext;
using UPS.ServicesDataRepository.Common;

namespace UPS.ServicesDataRepository
{
    public class AddressBookService : IAddressBookService
    {
        private ApplicationDbContext context;
        private IEntityValidationService entityValidationService;
        public AddressBookService(ApplicationDbContext applicationDbContext, IEntityValidationService entityValidationService)
        {
            this.context = applicationDbContext;
            this.entityValidationService = entityValidationService;
        }

        public List<AddressBook> GetAddressBooks()
        {
            return this.context.AddressBooks
                    .OrderBy(( AddressBook ab)=> ab.Id)
                    .ToList();
        }

        public AddressBookResponse UpdateAddressBookById(AddressBook addressBookData)
        {
            AddressBookResponse addressBookResponse = new AddressBookResponse();
            try
            {
                AddressBook data = this.context.AddressBooks.Where(s => s.Id == addressBookData.Id).FirstOrDefault();

                string beforeAddress = data.ConsigneeTranslatedAddress;

                data.ConsigneeTranslatedAddress = addressBookData.ConsigneeTranslatedAddress;
                data.ModifiedDate = DateTime.Now;

                this.context.AddressBooks.Update(data);
                this.context.Entry(addressBookData).State = EntityState.Detached;
                this.context.SaveChanges();
                addressBookResponse.AddressBookData = this.context.AddressBooks.Where(s => s.Id == addressBookData.Id).FirstOrDefault();
                addressBookResponse.Success = true;
                addressBookResponse.BeforeAddress = string.Empty;
                if (!string.Equals(beforeAddress, data.ConsigneeTranslatedAddress))
                {
                    addressBookResponse.BeforeAddress = beforeAddress;

                    var matchedShipments = this.context.shipmentDataRequests.Where(s => s.RCV_ADR_TE == addressBookData.ConsigneeAddress).ToList();
                    if (matchedShipments.Any())
                    {
                        matchedShipments.ForEach(shipment =>
                        {
                            shipment.SHP_ADR_TR_TE = data.ConsigneeTranslatedAddress;
                            shipment.SMT_STA_NR = (int)Enums.ATStatus.Curated;
                        });

                        this.context.BulkUpdate(matchedShipments);
                    }
                }
                return addressBookResponse;
            }
            catch (Exception ex)
            {
                addressBookResponse.Success = false;
                addressBookResponse.OperatonExceptionMessage = ex.Message;
            }
            return addressBookResponse;
        }

        public void InsertAddress(List<QuincusReponseData> quincusReponseDataList)
        {
            try
            {
                List<AddressBook> addressBooksList = new List<AddressBook>();
                foreach (var quincusReponseData in quincusReponseDataList)
                {
                    if
                    (
                        quincusReponseData.geocode != null
                        && quincusReponseData.addresses != null
                    )
                    {
                        List<Address> newAddress = FilterNewAddress(quincusReponseData.addresses);

                        List<AddressBook> addressBooks =
                            (from address in newAddress
                             join geocode in quincusReponseData.geocode?.ToList()
                             on address.id equals geocode.id
                             select
                             new AddressBook
                             {
                                 Accuracy = geocode?.accuracy,
                                 AddressTypeFlag = address?.address_type_flag != null ? Convert.ToBoolean(address?.address_type_flag) : false,
                                 Address_One = address?.addressline1,
                                 Address_Two = address?.addressline2,
                                 Address_Three = address?.addressline3,
                                 Address_Four = address?.addressline4,
                                 Area = geocode?.area,
                                 BatchId = quincusReponseData?.batch_id,
                                 Bat_Id = string.Empty,
                                 BuildingName = geocode?.building_name,
                                 BuldingNumber = geocode?.building_number,
                                 City = geocode?.city,
                                 Confidence = geocode?.confidence,
                                 ConsigneeAddress = address?.address,
                                 ConsigneeAddressId = address?.id != null ? address?.id : "-1",//
                                 ConsigneeTranslatedAddress = geocode?.translated_adddress,
                                 Country = geocode?.country,
                                 CreatedDate = DateTime.Now,
                                 GeoCode = geocode?.postcode,
                                 GeoCodeError = Convert.ToString(quincusReponseData?.geocode_errors ?? string.Empty),
                                 Latitude = geocode?.latitude?.Trim(),
                                 Longitude = geocode?.longitude?.Trim(),
                                 PostalCode = geocode?.postcode,
                                 Organization = quincusReponseData?.organisation != null ? Convert.ToString(quincusReponseData?.organisation) : string.Empty,
                                 Region = geocode?.region,
                                 Road = geocode?.road,
                                 SemanticCheck = geocode?.semantic_check,
                                 ShipmentId = null, //address?.id != null ? Convert.ToInt32(address?.id) : -1,
                                 StatusCode = quincusReponseData?.status_code,
                                 Unit = geocode?.unit,
                                 VerifyMatch = geocode?.verify_match
                             }).ToList();

                        List<AddressBook> validEntity = this.entityValidationService.FilterValidEntity<AddressBook>(addressBooks);
                        if (validEntity != null && validEntity.Any())
                        {
                            addressBooksList.AddRange(validEntity);
                        }
                    }
                }
                if (addressBooksList.Any())
                {
                    this.context.BulkInsert(addressBooksList);
                }
            }
            catch (Exception exception)
            {
               // Need to log exception if Any
            }
        }

        private List<Address> FilterNewAddress(IList<Address> addresses)
        {
            List<Address> newAddresses = addresses
                   .Where(
                       (Address ad) =>
                           !this.context.AddressBooks
                               .Select(
                               (AddressBook x) =>
                                   x.ConsigneeAddress.ToLower()).ToList()
                               .Contains(ad.address.ToLower()))
                    .GroupBy(_=>_.address)
                    .Select(x=>x.First())
                    .ToList();

            return newAddresses;
        }
    }
}
