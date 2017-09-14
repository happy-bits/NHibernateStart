using System;
using System.Collections.Generic;

namespace NHibernateStart.Domain
{
    class Library
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
