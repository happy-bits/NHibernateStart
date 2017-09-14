/*
 
 Exempelfil som visar sätt att mappa relationer med "mapping by code":
 - one-to-many
 - many-to-many

 */

using BlogData.Domain;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;

namespace BlogData.Mapping
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
            MapBlogPost();
            MapComment();
            MapTag();

            return _modelMapper.CompileMappingForAllExplicitlyAddedEntities();
        }

        private void MapBlogPost()
        {
            _modelMapper.Class<BlogPost>(e =>
            {
                // Låt varje tabell ha en "uniqueidentifier" med namn "Id" och är "primary key". Förutom kopplingstabeller.
                e.Id(p => p.Id, p => p.Generator(Generators.GuidComb));

                // Koppla ihop dina klassers fält med kolumnerna i databastabellen
                e.Property(p => p.Title);
                e.Property(p => p.Description);
                e.Property(p => p.Created);
                e.Property(p => p.Author);
                e.Property(p => p.Updated, m => m.Column("Uppdaterad")); // Exempel när en kolumn i en tabell heter "Uppdaterad" (och kopplas till "Updated")

                // En bloggpost kan ha flera kommentarer
                e.Set(p => p.Comments, p =>
                {
                    // "Set" används på "en-sidan" av en "en-till-många-relation". Alltså *en* bloggpost kan har flera kommentarer
                    // Sätt alltid "Inverse(true)" på "OneToMany"-sidan (ej på andra sidan)
                    // Detta påverkar vilken sqlkod som genereras
                    p.Inverse(true);

                    // Cascade påverkar vad som händer med barnen om pappan tas bort. 
                    // Cascade.All = om en bloggpost tas bort, så tas även kommentarerna bort (vilket är bra)
                    p.Cascade(Cascade.All);

                    // Ange namnet på kolumnen här som referar till pappan
                    p.Key(k => k.Column(col => col.Name("BlogPostId")));
                }, p => p.OneToMany());

                // Många-till-många-relation mellan BlogPost och Tag. (Det behövs en ManyToMany på andra sidan också)
                e.Set(x => x.Tags, collectionMapping =>
                {
                    // Ange namn på den mellanliggande tabellen
                    collectionMapping.Table("BlogPostsTags");

                    // Sätt alltid "Cascade.None" vid en många-till-många-relation
                    collectionMapping.Cascade(Cascade.None);

                    // Här anger du tabellens kolumn-namn
                    collectionMapping.Key(keyMap => keyMap.Column("BlogPostId"));
                }, map => map.ManyToMany(p => p.Column("TagId")));
            });

        }

        private void MapComment()
        {

            _modelMapper.Class<Comment>(e =>
            {
                e.Id(p => p.Id, p => p.Generator(Generators.GuidComb));

                e.Property(p => p.DateTime);
                e.Property(p => p.Author);
                e.Property(p => p.CommentText);

                // Många kommentarer hör till en blogpost
                e.ManyToOne(p => p.BlogPost, mapper =>
                {
                    // Ange namn på kolumnen som pekar på pappan
                    mapper.Column("BlogPostId");

                    // Valfritt om du sätter "NotNullable(true)" eller "NotNullable(false)". (false är default). 
                    // false = BlogPostId kan vara null
                    // true =  BlogPostId får inte vara null

                    mapper.NotNullable(true);

                    // Sätt Cascade.None här vid ManyToOne. (annars tas bloggposten bort när vi tar bort en kommentar = inte bra)
                    // (Cascade.None är default)

                    mapper.Cascade(Cascade.None);
                });
            });

        }

        private void MapTag()
        {
            _modelMapper.Class<Tag>(e =>
            {
                e.Id(p => p.Id, p => p.Generator(Generators.GuidComb));

                e.Property(p => p.Name, p => p.Unique(true));

                // Många-till-många-relation mellan BlogPost och Tag. (Det behövs en ManyToMany på andra sidan också)

                e.Set(x => x.BlogPosts, collectionMapping =>
                {
                    // Inverse true måste vara på ena sidan (men inte den andra). Det spelar ingen roll vilken sida du väljer.
                    collectionMapping.Inverse(true);

                    // Ange namn på den mellanliggande tabellen
                    collectionMapping.Table("BlogPostsTags");

                    // Sätt alltid "Cascade.None" vid en många-till-många-relation
                    collectionMapping.Cascade(Cascade.None);

                    // Här anger du tabellens kolumn-namn
                    collectionMapping.Key(keyMap => keyMap.Column("TagId"));

                }, map => map.ManyToMany(p => p.Column("BlogPostId")));

            });


        }

    }
}

