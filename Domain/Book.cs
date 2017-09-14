using System;

namespace NHibernateStart.Domain
{
    class Book
    {
        public virtual Guid Id { get; set; }
        public virtual string Title { get; set; }
        public virtual Library Library { get; set; }
    }
}
