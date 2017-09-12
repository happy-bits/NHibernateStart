using System;

namespace NHibernateStart.Domain
{
    class Customer
    {
        public virtual Guid Id { get; set; }
        public virtual string FirstName { get; set; }
    }
}
