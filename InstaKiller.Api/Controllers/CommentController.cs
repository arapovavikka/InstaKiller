using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InstaKiller.Model;
using InstaKiller.Services;

namespace InstaKiller.Api.Controllers
{
    public class CommentController : ApiController
    {
        private const string ConnectionSql = @"Data Source=TOSHA-PC\SQLEXPRESS;
            Initial Catalog=Insta_Killer;Integrated Security=True";
        private readonly InstaKiller.Services.IDataLayer _dataLayer = new InstaKiller.DataLayer.Sql.DataLayer(ConnectionSql);

        [HttpPost]
        [Route("api/v1/photo/{photoId}/comment")]
        public Comment PostComment(Guid photoId, Comment comment)
        {
            comment.PhotoId = photoId;
            _dataLayer.AddComment(comment);
            return _dataLayer.GetComment(comment.Id);
        }

        [HttpGet]
        [Route("api/v1/photo/{photoId}/comment/{id}")]
        public Comment GetComment(Guid photoId, Guid id)
        {
            return _dataLayer.GetComment(photoId, id);
        }

        [HttpPost]
        [Route("api/v1/photo/{photoId}/comment/{id}")]
        public Comment ChangeComment(Guid photoId, Guid id, Comment comment)
        {
            _dataLayer.UpdateComment(id, comment);
            return _dataLayer.GetComment(comment.Id);
        }

        [HttpDelete]
        [Route("api/v1/photo/{photoId}/comment/{id}")]
        public bool DeleteComment(Guid photoId, Guid id)
        {
            return _dataLayer.DeleteComment(photoId, id);
        }
    }
}
