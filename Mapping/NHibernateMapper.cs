using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NHibernateStart.Domain;

namespace NHibernateStart.Mapping
{
    public class NHibernateMapper
    {
        private readonly ModelMapper _modelMapper;

        public NHibernateMapper()
        {
            _modelMapper = new ModelMapper();
        }

        public HbmMapping Map()
        {
            MapCustomer();
            return _modelMapper.CompileMappingForAllExplicitlyAddedEntities();
        }

        private void MapCustomer()
        {
            _modelMapper.Class<Customer>(e =>
            {
                e.Id(p => p.Id, p=>p.Generator(Generators.GuidComb));
                e.Property(p => p.FirstName);
            });

        }
    }
}

