using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InstaKiller.Model;

namespace InstaKiller.Api.Controllers
{
    public class HashtagController : ApiController
    {
        private const string ConnectionSql = @"Data Source=TOSHA-PC\SQLEXPRESS;
            Initial Catalog=Insta_Killer;Integrated Security=True";
        private readonly IDataLayer _dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
    }

    //[HttpPost] 
    //[Route("api/v1/hashgtag/")]
    //public Comment

}
