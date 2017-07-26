using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS_Project.Helpers
{
    public static class RoleHelpers
    {
        public static IEnumerable<SelectListItem> GetRoles(this Type type, string selectedValue)
        {
            if (!typeof(Role).IsAssignableFrom(type))
            {
                throw new ArgumentException("Type must be a Role");
            }

            var items = new RolesRepository().Roles().Select(r =>
                    new SelectListItem
                    {
                        Text = r.Name,
                        Value = r.Id,
                        Selected = string.Compare(r.Id, selectedValue, false) == 0
                    }
                );
            return items;
        }
    }
}