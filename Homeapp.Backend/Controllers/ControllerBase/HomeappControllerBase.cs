namespace Homeapp.Backend.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The base controller class for the Home App.
    /// </summary>
    public class HomeappControllerBase : ControllerBase
    {
        protected Guid GetUserId()
        {
            return Guid.Parse(this.User.Claims.First(i => i.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
        }
    }
}
