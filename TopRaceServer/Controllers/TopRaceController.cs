using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//Add the below
using TopRaceServerBL.Models;
using System.IO;

namespace TopRaceServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopRaceController : ControllerBase
    {
        #region Add connection to the db context using dependency injection
        TopRaceDBContext context;
        public TopRaceController(TopRaceDBContext context)
        {
            this.context = context;
        }
        #endregion
    }
}
