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
        private readonly InstaKiller.Services.IDataLayer _dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
        

        [HttpPost]
        [Route("api/v1/photo/{photoId}/like")]
        public Guid PostLike(Guid photoId, Person user)
        {
            var photo = _dataLayer.GetPhoto(photoId);
            return _dataLayer.AddLike(photo, user);
        }

        [HttpGet]
        [Route("api/v1/photo/{photoId}/like")]
        public Guid GetLike(Guid photoId, Person user)
        {
            var photo = _dataLayer.GetPhoto(photoId);
            return _dataLayer.GetLike(photo, user);
        }

        [HttpGet]
        [Route("api/v1/photo/{photoId}/likes")]
        public List<Person> GetLikes(Guid photoId)
        {
            var photo = _dataLayer.GetPhoto(photoId);
            return _dataLayer.GetLikes(photo);
        }

        [HttpDelete]
        [Route("api/v1/photo/{photoId}/like/{id}")]
        public bool DeleteLike(Guid photoId, Guid id)
        {
            var photo = _dataLayer.GetPhoto(photoId);
            return _dataLayer.DeleteLike(photo, id);
        }
    }
}
