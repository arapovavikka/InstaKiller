using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InstaKiller.Model;
using InstaKiller.DataLayer.Sql;

namespace InstaKiller.Api.Controllers
{
    public class CommentController : ApiController
    {
        private const string ConnectionSql = @"Data Source=TOSHA-PC\SQLEXPRESS;
            Initial Catalog=Insta_Killer;Integrated Security=True";
        private readonly IDataLayer _dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);

        [HttpPost]
        [Route("api/v1/comment")]
        public Comment PostComment(Comment comment)
        {
            _dataLayer.AddComment(comment);
            return _dataLayer.GetComment(comment.Id);
        }

        [HttpGet]
        [Route("api/v1/comment/{id}")]
        public Comment GetComment(Guid id)
        {
            return _dataLayer.GetComment(id);
        }

        [HttpPost]
        [Route("api/v1/comment/{id}")]
        public Comment ChangeComment(Guid id, Comment comment)
        {
            _dataLayer.UpdateComment(id, comment);
            return _dataLayer.GetComment(comment.Id);
        }

        [HttpDelete]
        [Route("api/v1/comment/{id}")]
        public void DeleteComment(Guid id)
        {
            _dataLayer.DeleteComment(id);
        }
    }
}
