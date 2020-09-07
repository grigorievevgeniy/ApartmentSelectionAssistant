using AutoMapper;
using Parser.Interfaces;
using Parser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Parser.DtoModels
{
    public class AdvertisementRepository : IRepository
    {
        ParserContext parserContext;
        Mapper mapper;
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
            var ownerId = AddOrFindOwner(advertisement.Owner);
            var houseId = AddOrFindHouse(advertisement.House);

            advertisement.Owner = null;
            advertisement.House = null;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Advertisement, DtoAdvertisement>()
                //.ForMember(x => x.Owner, op => op.Ignore)
                //.ForMember("HouseId", opt => opt.MapFrom(c => c.Id == h))
                );
            mapper = new Mapper(config);
            DtoAdvertisement dtoAdvertisement = mapper.Map<DtoAdvertisement>(advertisement);

            dtoAdvertisement.OwnerId = ownerId;
            dtoAdvertisement.HouseId = houseId;

            parserContext.Advertisements.Add(dtoAdvertisement);
            parserContext.SaveChanges();
        }

        public Guid AddOrFindHouse(House house)
        {
            DtoHouse dtoHouse = parserContext.Houses.SingleOrDefault(x => x.FullAdress == house.FullAdress);

            if (dtoHouse == default)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<House, DtoHouse>());
                mapper = new Mapper(config);
                dtoHouse = mapper.Map<DtoHouse>(house);

                Guid id = parserContext.Houses.Add(dtoHouse).Entity.Id;
                parserContext.SaveChanges();
                return id;
            }

            return dtoHouse.Id;
        }

        public Guid AddOrFindOwner(Owner owner)
        {
            DtoOwner dtoOwner = parserContext.Owners.SingleOrDefault(x => x.Url == owner.Url);

            if (dtoOwner == default)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Owner, DtoOwner>());
                mapper = new Mapper(config);
                dtoOwner = mapper.Map<DtoOwner>(owner);

                Guid id = parserContext.Owners.Add(dtoOwner).Entity.Id;
                parserContext.SaveChanges();
                return id;
            }

            return dtoOwner.Id;
        }
    }
}
