using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TRWLASystemMaster.Models.DB;
using System.Web.Mvc;
using TRWLASystemMaster.Models.EntityManager;


namespace TRWLASystemMaster.Security
{
    public class AuthorizeRoleAttribute: AuthorizeAttribute
    {

        private readonly string[] userAssignedRoles;
        public AuthorizeRoleAttribute(params string[] roles)
        {
            this.userAssignedRoles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            using (TWRLADB_Staging_V2Entities17 db = new TWRLADB_Staging_V2Entities17())
            {
                UserManager UM = new UserManager();
                foreach (var roles in userAssignedRoles)
                {
                    authorize = UM.IsUserInRole(httpContext.User.Identity.Name, roles);
                    if (authorize)
                        return authorize;
                }
            }
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/TRWLASchedules/Index");
        }
    }
}