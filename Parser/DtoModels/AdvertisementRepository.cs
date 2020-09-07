using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Parser.Interfaces;
using Parser.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Parser.DtoModels
{
    public class AdvertisementRepository : IRepository
    {
        ParserContext parserContext;
        public AdvertisementRepository()
        {
            parserContext = new ParserContext();
        }

        public void AddListAdvertisement(ListAdvertisements listAdvertisements)
        {
            foreach (var item in listAdvertisements.Advertisements)
            {
                AddAdvertisement(item);
            }
        }

        public void AddAdvertisement(Advertisement advertisement)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Advertisement, DtoAdvertisement>());
            var mapper = new Mapper(config);

            

            DtoAdvertisement dtoAdvertisement = parserContext.Advertisements.SingleOrDefault(x => x.Url == advertisement.Url);

            if (dtoAdvertisement != default)
            {
                advertisement.Id = dtoAdvertisement.Id;

                var ownerId = AddOrFindOwner(advertisement.Owner);
                var houseId = AddOrFindHouse(advertisement.House);

                advertisement.Owner = null;
                advertisement.House = null;

                mapper.Map(advertisement, dtoAdvertisement);

                dtoAdvertisement.OwnerId = ownerId;
                dtoAdvertisement.HouseId = houseId;

                parserContext.Advertisements.Update(dtoAdvertisement);
            }
            else
            {
                var ownerId = AddOrFindOwner(advertisement.Owner);
                var houseId = AddOrFindHouse(advertisement.House);

                advertisement.Owner = null;
                advertisement.House = null;

                dtoAdvertisement = mapper.Map<DtoAdvertisement>(advertisement);

                dtoAdvertisement.OwnerId = ownerId;
                dtoAdvertisement.HouseId = houseId;

                parserContext.Advertisements.Add(dtoAdvertisement);
            }

            parserContext.SaveChanges();
        }

        internal ICollection<string> GetUnfinishedAdvertisementUrls()
        {
            return parserContext.Advertisements.Where(x => !x.FullParse).Select(x => x.Url).ToArray();
        }

        public Guid AddOrFindHouse(House house)
        {
            DtoHouse dtoHouse = parserContext.Houses.SingleOrDefault(x => x.FullAdress == house.FullAdress);

            if (dtoHouse == default)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<House, DtoHouse>());
                var mapper = new Mapper(config);
                dtoHouse = mapper.Map<DtoHouse>(house);

                Guid id = parserContext.Houses.Add(dtoHouse).Entity.Id;
                parserContext.SaveChanges();
                return id;
            }

            return dtoHouse.Id;
        }

        public Guid AddOrFindOwner(Owner owner)
        {
            DtoOwner dtoOwner = parserContext.Owners.SingleOrDefault(x => x.Url == owner.Url && x.Name == owner.Name);

            if (dtoOwner == default)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Owner, DtoOwner>());
                 var mapper = new Mapper(config);
                dtoOwner = mapper.Map<DtoOwner>(owner);

                Guid id = parserContext.Owners.Add(dtoOwner).Entity.Id;
                parserContext.SaveChanges();
                return id;
            }

            return dtoOwner.Id;
        }
    }
}
