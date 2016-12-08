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
        private readonly InstaKiller.Services.IDataLayer _dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);

        //Actions with user

        [HttpPost]
        [Route("api/v1/user")]
        public Person PostUser(Person user)
        {
            _dataLayer.AddUser(user);
            return _dataLayer.GetUser(user.Id);
        }

        [HttpGet]
        [Route("api/v1/user/{id}")]
        public Person GetUser(Guid id)
        {
            return _dataLayer.GetUser(id);
        }

        [HttpGet]
        [Route("api/v1/user/email/{email}")]
        public Person GetUserByEmail(string email)
        {
            return _dataLayer.GetUserByEmail(email);
        }

        [HttpPost]
        [Route("api/v1/user/{id}")]
        public Person UpdateUser(Guid id, Person user)
        {
            _dataLayer.UpdateUser(id, user);
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
