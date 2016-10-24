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
        private readonly IDataLayer _dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);

        //Actions with user subscription

        [HttpPost]
        [Route("api/v1/subscription/{id}")]
        public List<Person> PostSubscription(Guid id, Person subscription)
        {
            var user = _dataLayer.GetUser(id);
            subscription = _dataLayer.GetUser(subscription.Id);
            _dataLayer.AddSubscription(user, subscription);

            return user.Subscriptions;
        }

        [HttpGet]
        [Route("api/v1/subscription/{id}")]
        public List<Person> GetSubscriptions(Guid id)
        {
            var user = _dataLayer.GetUser(id);
            return _dataLayer.GetSubscription(user);
        }

        [HttpDelete]
        [Route("api/v1/subscription/{id}")]
        public void DeleteSubscription(Guid id, Person subscription)
        {
            var user = _dataLayer.GetUser(id);
            subscription = _dataLayer.GetUser(subscription.Id);
            _dataLayer.DeleteSubscription(user, subscription);
        }
    }
}
