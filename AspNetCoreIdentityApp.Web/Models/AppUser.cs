﻿namespace AspNetCoreIdentityApp.Web.Models
{
  public class AppUser : IdentityUser
  {
        public string City { get; set; }
        public string Picture { get; set; }
        public DateOnly BirthDate { get; set; }
        public Gender Gender { get; set; }
    }
}
