using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using InstaKiller.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace InstaKiller.Test
{
    [TestClass]
    public class DataLayerSqlTests
    {
        private const string ConnectionSql = @"Data Source=TOSHA-PC\SQLEXPRESS;
            Initial Catalog=Insta_Killer;Integrated Security=True";

        // Generate data for tests (user, comment, id)
        private static Person GenerateUser()
        {
            return new Person
            {
                Name = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                FirstName = Guid.NewGuid().ToString(),
                Email = Guid.NewGuid().ToString()
            };
        }

        private static Comment GenerateComment(Guid userId, Guid photoId)
        {
            return new Comment
            {
                Text = Guid.NewGuid().ToString(),
                UserId = userId,
                PhotoId = photoId,
                DateTime = DateTime.Now
            };
        }

        private static Photo GeneratePhoto(Guid userId)
        {
            return new Photo
            {
                UserId = userId,
                ImageUrl = Guid.NewGuid().ToString(),
                TimeDate = DateTime.Now
            };
        }

        private static Session GenerateSession(Guid userId)
        {
            return new Session()
            {
                UserId = Guid.NewGuid(),
                UserIp = new Random().Next(100000),
                DateFrom = DateTime.Now,
                DateTo = DateTime.Now.Add(new TimeSpan(3, 0, 0, 0)),
                Token = new Random().Next(100000)
            };
        }

        [TestMethod]
        public void ShouldConnectToDateBase()
        {
            //arrange
            System.Data.ConnectionState res;

            //act
            using (SqlConnection connection = new SqlConnection(ConnectionSql))
            {
                connection.Open();
                res = connection.State;
            }

            //assert
            Assert.AreEqual(res, System.Data.ConnectionState.Open);
        }

        [TestMethod] 
        public void ShouldAddUser()
        {
            //arrange
            var user = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            
            dataLayer.AddUser(user);
            var resUser = dataLayer.GetUser(user.Id);

            //assert
            Assert.AreEqual(resUser.Id, user.Id);

        }

        [TestMethod]
        public void ShouldUpdateUser()
        {
            //arrange
            var user = GenerateUser();
            var userUpdate = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            dataLayer.AddUser(user);

            dataLayer.UpdateUser(user.Id, userUpdate);

            var resUser = dataLayer.GetUser(user.Id);

            //assert
            Assert.AreEqual(resUser.Id, user.Id);
        }

        [TestMethod]
        public void ShouldUpdatePhoto()
        {
            //arrange
            var user = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            dataLayer.AddUser(user);

            var photo = GeneratePhoto(user.Id);
            dataLayer.AddPhoto(photo);
            
            var photoUpdate = GeneratePhoto(user.Id);
            dataLayer.UpdatePhoto(photo.Id, photoUpdate);
            photoUpdate = dataLayer.GetPhoto(photo.Id);

            var resUser = dataLayer.GetPhoto(photo.Id);

            //assert
            Assert.AreEqual(resUser.Id, photoUpdate.Id);
        }

        [TestMethod]
        public void ShouldGetUser()
        {
            //arrange
            var user = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            dataLayer.AddUser(user);

            var resultUser = dataLayer.GetUser(user.Id);

            //assert
            Assert.AreEqual(user.Id, resultUser.Id);
        }

        [TestMethod]
        public void ShouldNotGetUser()
        {
            //arrange 
            var user = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            user = dataLayer.GetUser(user.Id);

            //assert
            Assert.AreEqual(user.Id, Guid.Empty);
        }

        [TestMethod]
        public void ShouldUpdateComment()
        {
            //arrange
            var user = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            dataLayer.AddUser(user);

            var photo = GeneratePhoto(user.Id);
            dataLayer.AddPhoto(photo);

            var comment = GenerateComment(user.Id, photo.Id);
            dataLayer.AddComment(comment);

            var commentUpdate = GenerateComment(user.Id, photo.Id);
            dataLayer.UpdateComment(comment.Id, commentUpdate);
            commentUpdate = dataLayer.GetComment(comment.Id);

            var resComment = dataLayer.GetComment(comment.Id);

            //assert
            Assert.AreEqual(resComment.Id, commentUpdate.Id);
        }

        [TestMethod]
        public void ShouldAddPhoto()
        {
            //arrange
            var user = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            dataLayer.AddUser(user);

            var photo = GeneratePhoto(user.Id);
            dataLayer.AddPhoto(photo);
            var resultPhoto = dataLayer.GetPhoto(photo.Id);

            //assert
            Assert.AreEqual(photo.Id, resultPhoto.Id);
        }

        [TestMethod] 
        public void ShouldGetPhoto()
        {
            //arrange
            var user = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            dataLayer.AddUser(user);
            var photo = GeneratePhoto(user.Id);
            dataLayer.AddPhoto(photo);

            var resPhoto = dataLayer.GetPhoto(photo.Id);

            //assert
            Assert.AreEqual(photo.Id, resPhoto.Id);
        }

        [TestMethod]
        public void ShouldDeleteUser()
        {
            //arrange
            var user = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            dataLayer.AddUser(user);
            dataLayer.DeleteUser(user.Id);

            var resUser = dataLayer.GetUser(user.Id);

            //assert
            Assert.AreNotEqual(resUser.Id, user.Id);
        }

        [TestMethod]
        public void ShouldDeletePhoto()
        {
            //arrange
            var user = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            dataLayer.AddUser(user);
            var photo = GeneratePhoto(user.Id);
            dataLayer.AddPhoto(photo);
            dataLayer.DeletePhoto(photo.Id);

            var resPhoto = dataLayer.GetPhoto(photo.Id);

            //assert
            Assert.AreNotEqual(resPhoto.Id, photo.Id);
        }

        [TestMethod]
        public void ShouldAddComment()
        {
            //arrange
            var user = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);

            dataLayer.AddUser(user);

            var photo = GeneratePhoto(user.Id);
            dataLayer.AddPhoto(photo);

            var comment = GenerateComment(user.Id, Guid.NewGuid());
            dataLayer.AddComment(comment);

            var resultComment = dataLayer.GetComment(comment.Id);

            //assert
            Assert.AreNotEqual(resultComment.Id, Guid.Empty);
        }

        [TestMethod]
        public void ShouldDeleteComment()
        {
            //arrange
            var user = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            dataLayer.AddUser(user);

            var photo = GeneratePhoto(user.Id);
            dataLayer.AddPhoto(photo);

            var comment = GenerateComment(user.Id, photo.Id);
            dataLayer.AddComment(comment);
            dataLayer.DeleteComment(comment.Id);

            var resComment = dataLayer.GetComment(comment.Id);

            //assert
            Assert.AreNotEqual(resComment.Id, comment.Id);
        }

        [TestMethod]
        public void ShouldGetComment()
        {
            //arrange
            var user = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            dataLayer.AddUser(user);

            var photo = GeneratePhoto(user.Id);
            dataLayer.AddPhoto(photo);

            var comment = GenerateComment(user.Id, photo.Id);
            dataLayer.AddComment(comment);
            var resComment = dataLayer.GetComment(comment.Id);

            //assert
            Assert.AreEqual(resComment.Id, comment.Id);
        }

        [TestMethod]
        public void ShouldAddLike()
        {
            //arrange
            var user = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            dataLayer.AddUser(user);

            var photo = GeneratePhoto(user.Id);
            dataLayer.AddPhoto(photo);

            dataLayer.AddLike(photo, user);
            var haveLike = dataLayer.HaveLike(photo, user);

            //assert
            Assert.AreEqual(haveLike, true);
        }

        [TestMethod]
        public void ShouldDeleteLike()
        {
            //arrange
            var user = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            dataLayer.AddUser(user);

            var photo = GeneratePhoto(user.Id);
            dataLayer.AddPhoto(photo);
            
            dataLayer.AddLike(photo, user);
            dataLayer.DeleteLike(photo, user);

            var haveLike = dataLayer.HaveLike(photo, user);

            //assert
            Assert.AreEqual(haveLike, false);
        }

        [TestMethod]
        public void ShouldGetLikes()
        {
            //arrange
            var user = GenerateUser();
            var userAnother = GenerateUser();
            
            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            dataLayer.AddUser(user);
            dataLayer.AddUser(userAnother);

            var photo = GeneratePhoto(user.Id);
            dataLayer.AddPhoto(photo);

            dataLayer.AddLike(photo, user);
            dataLayer.AddLike(photo, userAnother);

            photo.UsersThatLike = dataLayer.GetLikes(photo);

            var resAllLikes = true;
            foreach (var usersLikes in photo.UsersThatLike)
            {
                if (usersLikes.Id != user.Id && usersLikes.Id != userAnother.Id)
                {
                    resAllLikes = false;
                }
            }

            //assert
            Assert.AreEqual(resAllLikes, true);
        }

        [TestMethod]
        public void ShouldAddHastag()
        {
            //arrange
            var user = GenerateUser();
            var hashtag = Guid.NewGuid().ToString();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            dataLayer.AddUser(user);

            var photo = GeneratePhoto(user.Id);
            dataLayer.AddPhoto(photo);

            var comment = GenerateComment(user.Id, photo.Id);
            dataLayer.AddComment(comment);
            comment = dataLayer.AddHashtag(comment, hashtag);

            var haveHashtag = dataLayer.HaveHashtag(comment, hashtag);

            //assert
            Assert.AreEqual(haveHashtag, true);

        }


        //[TestMethod]
        //public void ShouldChangeHashtag()
        //{

        //}

        [TestMethod]
        public void ShouldDeleteHashtag()
        {
            //arrange
            var user = GenerateUser();
            var hashtag = Guid.NewGuid().ToString();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            dataLayer.AddUser(user);

            var photo = GeneratePhoto(user.Id);
            dataLayer.AddPhoto(photo);

            var comment = GenerateComment(user.Id, photo.Id);
            dataLayer.AddComment(comment);
            comment = dataLayer.AddHashtag(comment, hashtag);

            dataLayer.DeleteHashtag(comment, hashtag);

            var haveHashtag = dataLayer.HaveHashtag(comment, hashtag);

            //assert
            Assert.AreEqual(haveHashtag, false);
        }

        //TODO: 
        //[TestMethod]
        //public void ShouldAddReferenceOnUser()
        //{

        //}

        //TODO:
        //[TestMethod]
        //public void ShouldChangeReferenceOnUser()
        //{

        //}

        //TODO
        //[TestMethod]
        //public void ShouldDeleteReferenceOnUser()
        //{

        //}

        [TestMethod]
        public void ShouldGetLatestPhoto() 
        {
            //arrange
            var numOfDaysBefore = 1;
            var dateTo = DateTime.Now;
            var dateFrom = DateTime.Now.Subtract(new TimeSpan(numOfDaysBefore, 0, 0, 0));

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);

            List<Photo> latestPhotos = dataLayer.GetLatestPhotos(dateFrom, dateTo);
            var wrongDate = false;

            foreach (var latestPhoto in latestPhotos)
            {
                if ((latestPhoto.TimeDate < dateFrom) || (latestPhoto.TimeDate > dateTo))
                {
                    wrongDate = true;
                }
            }

            //assert
            Assert.AreEqual(wrongDate, false);
        }

        [TestMethod]
        public void ShouldGetSubscriber()
        {
            //arrange
            var user = GenerateUser();
            var userSubscriber = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            dataLayer.AddUser(user);
            dataLayer.AddUser(userSubscriber);

            dataLayer.AddSubscription(userSubscriber, user);

            var resSubscribe = dataLayer.HaveSubscribe(user, userSubscriber);

            //assert
            Assert.AreEqual(resSubscribe, true);
        }

        [TestMethod]
        public void ShouldGetSubscribers()
        {
            //arrange
            var user = GenerateUser();
            var userSubscriber = GenerateUser();
            var userSubscriber2 = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            dataLayer.AddUser(user);
            dataLayer.AddUser(userSubscriber);
            dataLayer.AddUser(userSubscriber2);

            dataLayer.AddSubscription(userSubscriber, user);
            dataLayer.AddSubscription(userSubscriber2, user);

            user.Subscribers = dataLayer.GetSubscribers(user);

            var allSubscribers = true;
            foreach (var subscriber in user.Subscribers)
            {
                if (subscriber.Id != userSubscriber.Id && subscriber.Id != userSubscriber2.Id)
                {
                    allSubscribers = false;
                }
            }

            //assert
            Assert.AreEqual(allSubscribers, true);
        } 

        [TestMethod]
        public void ShouldAddSubscription()
        {
            //arrange
            var user = GenerateUser();
            var userSubscriber = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            dataLayer.AddUser(user);
            dataLayer.AddUser(userSubscriber);

            dataLayer.AddSubscription(user, userSubscriber);

            bool areFriends = dataLayer.HaveSubscription(user, userSubscriber);

            //assert
            Assert.AreEqual(areFriends, true);
        }

        [TestMethod]
        public void ShouldGetSubscription()
        {
            //arrange
            var user = GenerateUser();
            var userSubscription = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            dataLayer.AddUser(user);
            dataLayer.AddUser(userSubscription);
            dataLayer.AddSubscription(user, userSubscription);

            var haveSubscription = dataLayer.HaveSubscription(user, userSubscription);

            //assert
            Assert.AreEqual(haveSubscription, true);
        }

        [TestMethod]
        public void ShouldGetSubscriptions()
        {
            //arrange
            var user = GenerateUser();
            var userSubscription = GenerateUser();
            var userSubscription2 = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);

            dataLayer.AddUser(user);
            dataLayer.AddUser(userSubscription);
            dataLayer.AddUser(userSubscription2);

            dataLayer.AddSubscription(user, userSubscription);
            dataLayer.AddSubscription(user, userSubscription2);

            user.Subscriptions = dataLayer.GetSubscription(user);
            var trueSubscription = true;

            foreach (var subscription in user.Subscriptions)
            {
                if (!dataLayer.HaveSubscription(user, subscription))
                {
                    trueSubscription = false;
                }
            }

            //assert
            Assert.AreEqual(trueSubscription, true);
        }

        [TestMethod]
        public void ShouldDeleteSubscription()
        {
            //arrange
            var user = GenerateUser();
            var userSubscription = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            dataLayer.AddUser(user);
            dataLayer.AddUser(userSubscription);

            dataLayer.AddSubscription(user, userSubscription);

            dataLayer.DeleteSubscription(user, userSubscription);
            var areFriends = dataLayer.HaveSubscription(user, userSubscription);

            //assert
            Assert.AreEqual(areFriends, false);
        }

        [TestMethod]
        public void ShouldAddSession()
        {
            //arrange
            var user = GenerateUser();
            var session = GenerateSession(user.Id);

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            dataLayer.AddUser(user);
            session.UserId = user.Id;

            session = dataLayer.AddSession(session);

            var resSession = dataLayer.GetSession(session.Id);

            //assert
            Assert.AreEqual(resSession.Id, session.Id);
        }

        [TestMethod]
        public void ShouldGetSession()
        {
            //arrange
            var user = GenerateUser();
            var session = GenerateSession(user.Id);

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            dataLayer.AddUser(user);
            session.UserId = user.Id;

            session = dataLayer.AddSession(session);

            var resSession = dataLayer.GetSession(session.Id);
    
            //assert
            Assert.AreEqual(resSession.Id, session.Id);
        }

        [TestMethod]
        public void ShouldGetAllCommentsOfPhoto()
        {
            //arrange
            var user = GenerateUser();
           
            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            dataLayer.AddUser(user);

            var photo = GeneratePhoto(user.Id);
            dataLayer.AddPhoto(photo);

            var comment = GenerateComment(user.Id, photo.Id);
            dataLayer.AddComment(comment);
            var anotherComment = GenerateComment(user.Id, photo.Id);
            dataLayer.AddComment(anotherComment);

            photo.AllComments = dataLayer.GetAllComments(photo.Id);
            foreach (var currentComment in photo.AllComments)
            {
                Assert.AreEqual(currentComment.PhotoId, photo.Id);
            }
            //assert
        }
    }
}
