using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InstaKiller.Model;

namespace InstaKiller.Api.Controllers
{
    public class PhotoController : ApiController
    {
        private const string ConnectionSql = @"Data Source=TOSHA-PC\SQLEXPRESS;
            Initial Catalog=Insta_Killer;Integrated Security=True";
        private readonly IDataLayer _dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);

        //Actions with photo

        [HttpPost]
        [Route("api/v1/photo")]
        public Photo PostPhoto(Photo photo)
        {
            photo = _dataLayer.AddPhoto(photo);
            return _dataLayer.GetPhoto(photo.Id);
        }

        [HttpGet]
        [Route("api/v1/photo/{id}")]
        public Photo GetPhoto(Guid id)
        {
            return _dataLayer.GetPhoto(id);
        }

        [HttpPost]
        [Route("api/v1/photo/{id}")]
        public Photo UpdatePhoto(Guid id, Photo photo)
        {
            photo.Id = id;
            photo = _dataLayer.UpdatePhoto(photo);
            return _dataLayer.GetPhoto(photo.Id);
        }

        [HttpDelete]
        [Route("api/v1/photo/{id}")]
        public void DeletePhoto(Guid id)
        {
            _dataLayer.DeletePhoto(id);
        }

    }
}
