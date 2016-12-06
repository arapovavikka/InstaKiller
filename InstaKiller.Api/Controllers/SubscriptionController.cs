using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InstaKiller.Model;

namespace InstaKiller.Api.Controllers
{
    public class SubscriptionController : ApiController
    {
        private const string ConnectionSql = @"Data Source=TOSHA-PC\SQLEXPRESS;
            Initial Catalog=Insta_Killer;Integrated Security=True";
        private readonly InstaKiller.Services.IDataLayer _dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);

        //Actions with user subscription

        [HttpPost]
        [Route("api/v1/user/{userId}/subscription")]
        public List<Person> PostSubscription(Guid userId, Person subscription)
        {
            var user = _dataLayer.GetUser(userId);
            _dataLayer.AddSubscription(user, subscription);

            return user.Subscriptions;
        }

        [HttpGet]
        [Route("api/v1/user/{userId}/subscriptions")]
        public List<Person> GetSubscriptions(Guid userId)
        {
            var user = _dataLayer.GetUser(userId);
            return _dataLayer.GetSubscription(user);
        }

        [HttpGet]
        [Route("api/v1/user/{userId}/subscription")]
        public Guid GetSubscription(Guid userId, Person subscription)
        {
            var user = _dataLayer.GetUser(userId);
            return _dataLayer.GetSubscription(user, subscription);
        }

        [HttpDelete]
        [Route("api/v1/user/{userId}/subscription/{id}")]
        public bool DeleteSubscription(Guid userId, Guid id)
        {
            var user = _dataLayer.GetUser(userId);
            return _dataLayer.DeleteSubscription(user, id);
        }
    }
}
