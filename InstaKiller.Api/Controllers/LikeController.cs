using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InstaKiller.Model;

namespace InstaKiller.Api.Controllers
{
    public class LikeController : ApiController
    {
        private const string ConnectionSql = @"Data Source=TOSHA-PC\SQLEXPRESS;
            Initial Catalog=Insta_Killer;Integrated Security=True";
        private readonly IDataLayer _dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);

        //Actions with likes of current photo

        [HttpPost]
        [Route("api/v1/like/{id}")]
        public List<Person> PostLike(Guid id, Person user)
        {
            var photo = _dataLayer.GetPhoto(id);
            user = _dataLayer.GetUser(user.Id);
            _dataLayer.AddLike(photo, user);
            return _dataLayer.GetLikes(photo);
        }

        [HttpGet]
        [Route("api/v1/like/{id}")]
        public List<Person> GetLikes(Guid id)
        {
            var photo = _dataLayer.GetPhoto(id);
            return _dataLayer.GetLikes(photo);
        }

        [HttpDelete]
        [Route("api/v1/like/{id}")]
        public List<Person> DeleteLike(Guid id, Person user)
        {
            user = _dataLayer.GetUser(user.Id);
            var photo = _dataLayer.GetPhoto(id);

            _dataLayer.DeleteLike(photo, user);
            return _dataLayer.GetLikes(photo);
        }
    }
}
