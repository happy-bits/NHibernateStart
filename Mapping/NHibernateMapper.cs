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
            MapBook();
            MapLibrary();
            return _modelMapper.CompileMappingForAllExplicitlyAddedEntities();
        }

        private void MapBook()
        {
            _modelMapper.Class<Book>(e =>
            {
                e.Id(p => p.Id, p=>p.Generator(Generators.GuidComb));
                e.Property(p => p.Title);

                e.ManyToOne(p => p.Library, mapper =>
                {
                    // Ange namn på kolumnen som pekar på pappan
                    mapper.Column("LibraryId");

                    // Valfritt om du sätter "NotNullable(true)" eller "NotNullable(false)". (false är default). 
                    // false = BlogPostId kan vara null
                    // true =  BlogPostId får inte vara null

                    //mapper.NotNullable(true);

                    // Sätt Cascade.None här vid ManyToOne. (annars tas bloggposten bort när vi tar bort en kommentar = inte bra)
                    // (Cascade.None är default)

                    mapper.Cascade(Cascade.None);
                });
            });

        }


        private void MapLibrary()
        {
            _modelMapper.Class<Library>(e =>
            {
                e.Id(p => p.Id, p => p.Generator(Generators.GuidComb));
                e.Property(p => p.Name);

                e.Set(p => p.Books, p =>
                {
                    // "Set" används på "en-sidan" av en "en-till-många-relation". Alltså *en* bloggpost kan har flera kommentarer
                    // Sätt alltid "Inverse(true)" på "OneToMany"-sidan (ej på andra sidan)
                    // Detta påverkar vilken sqlkod som genereras
                    p.Inverse(true);

                    // Cascade påverkar vad som händer med barnen om pappan tas bort. 
                    // Cascade.All = om en bloggpost tas bort, så tas även kommentarerna bort (vilket är bra)
                    p.Cascade(Cascade.All);

                    // Ange namnet på kolumnen här som referar till pappan
                    p.Key(k => k.Column(col => col.Name("LibraryId")));
                }, p => p.OneToMany());

            });



        }
    }
}
