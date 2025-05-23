﻿using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.Users;
using Chamedoon.Domin.Enums;

namespace Chamedoon.Domin.Entity.Customers
{
    public class Customer : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Job { get; set; }
        public string? Description { get; set; }
        public string? ProfileImage { get; set; }
        public Gender Gender { get; set; }
        public User User { get; set; }
    }
}
