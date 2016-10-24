using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InstaKiller.Model;

namespace InstaKiller.Api.Controllers
{
    public class UserController : ApiController
    {
        private const string ConnectionSql = @"Data Source=TOSHA-PC\SQLEXPRESS;
            Initial Catalog=Insta_Killer;Integrated Security=True";
        private readonly IDataLayer _dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);

        //Actions with user

        [HttpPost]
        [Route("api/v1/user")]
        public Person PostUser(Person user)
        {
            user = _dataLayer.AddUser(user);
            return _dataLayer.GetUser(user.Id);
        }

        [HttpGet]
        [Route("api/v1/user/{id}")]
        public Person GetUser(Guid id)
        {
            return _dataLayer.GetUser(id);
        }

        [HttpPost]
        [Route("api/v1/user/{id}")]
        public Person UpdateUser(Guid id, Person user)
        {
            user.Id = id;
            user = _dataLayer.UpdateUser(user);
            return _dataLayer.GetUser(user.Id);
        }

        [HttpDelete]
        [Route("api/v1/user/{id}")]
        public void DeleteUser(Guid id)
        {
            _dataLayer.DeleteUser(id);
        }
    }
}
