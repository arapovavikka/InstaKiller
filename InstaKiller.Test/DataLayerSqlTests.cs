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

        private static Comment GenerateComment()
        {
            return new Comment
            {
                Text = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid(),
                PhotoId = Guid.NewGuid(),
                Date = DateTime.Now
            };
        }

        private static Photo GeneratePhoto()
        {
            return new Photo
            {
                UserId = Guid.NewGuid(),
                ImageUrl = Guid.NewGuid().ToString(),
                TimeDate = DateTime.Now
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
            user = dataLayer.AddUser(user);
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
            user = dataLayer.AddUser(user);
            userUpdate.Id = user.Id;
            dataLayer.UpdateUser(userUpdate);

            var resUser = dataLayer.GetUser(user.Id);

            //assert
            Assert.AreEqual(resUser.Id, user.Id);
        }

        [TestMethod]
        public void ShouldUpdatePhoto()
        {
            //arrange
            var photo = GeneratePhoto();
            var photoUpdate = GeneratePhoto();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            photo = dataLayer.AddPhoto(photo);
            photoUpdate.Id = photo.Id;
            photoUpdate = dataLayer.UpdatePhoto(photoUpdate);

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
            user = dataLayer.AddUser(user);

            var resultUser = dataLayer.GetUser(user.Id);

            //assert
            Assert.AreEqual(user.Id, resultUser.Id);
        }

        [TestMethod]
        public void ShouldUpdateComment()
        {
            //arrange
            var comment = GenerateComment();
            var commentUpdate = GenerateComment();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            comment = dataLayer.AddComment(comment);
            commentUpdate.Id = comment.Id;
            commentUpdate = dataLayer.UpdateComment(commentUpdate);

            var resComment = dataLayer.GetComment(comment.Id);

            //assert
            Assert.AreEqual(resComment.Id, commentUpdate.Id);
        }

        [TestMethod]
        public void ShouldAddPhoto()
        {
            //arrange
            var photo = GeneratePhoto();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            photo = dataLayer.AddPhoto(photo);

            var resultPhoto = dataLayer.GetPhoto(photo.Id);

            //assert
            Assert.AreEqual(photo.Id, resultPhoto.Id);
        }

        [TestMethod] 
        public void ShouldGetPhoto()
        {
            //arrange
            var photo = GeneratePhoto();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            photo = dataLayer.AddPhoto(photo);

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
            user = dataLayer.AddUser(user);
            dataLayer.DeleteUser(user.Id);

            var resUser = dataLayer.GetUser(user.Id);

            //assert
            Assert.AreNotEqual(resUser.Id, user.Id);
        }

        [TestMethod]
        public void ShouldDeletePhoto()
        {
            //arrange
            var photo = GeneratePhoto();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            photo = dataLayer.AddPhoto(photo);
            dataLayer.DeletePhoto(photo.Id);

            var resPhoto = dataLayer.GetPhoto(photo.Id);

            //assert
            Assert.AreNotEqual(resPhoto.Id, photo.Id);
        }

        [TestMethod]
        public void ShouldAddComment()
        {
            //arrange
            var comment = GenerateComment();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            comment = dataLayer.AddComment(comment);

            var resultComment = dataLayer.GetComment(comment.Id);

            //assert
            Assert.AreEqual(resultComment.Id, comment.Id);
        }

        [TestMethod]
        public void ShouldDeleteComment()
        {
            //arrange
            var comment = GenerateComment();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            comment = dataLayer.AddComment(comment);
            dataLayer.DeleteComment(comment.Id);

            var resComment = dataLayer.GetComment(comment.Id);

            //assert
            Assert.AreNotEqual(resComment.Id, comment.Id);
        }

        [TestMethod]
        public void ShouldGetComment()
        {
            //arrange
            var comment = GenerateComment();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            comment = dataLayer.AddComment(comment);

            var resComment = dataLayer.GetComment(comment.Id);

            //assert
            Assert.AreEqual(resComment.Id, comment.Id);
        }

        [TestMethod]
        public void ShouldAddLike()
        {
            //arrange
            var photo = GeneratePhoto();

            var user = new Person
            {
                Name = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                FirstName = Guid.NewGuid().ToString(),
                Email = Guid.NewGuid().ToString()
            };

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            photo = dataLayer.AddPhoto(photo);
            user = dataLayer.AddUser(user);

            dataLayer.AddLike(photo, user);
            bool haveLike = dataLayer.HaveLike(photo, user);

            //assert
            Assert.AreEqual(haveLike, true);
        }

        [TestMethod]
        public void ShouldDeleteLike()
        {
            //arrange
            var photo = GeneratePhoto();

            var user = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            photo = dataLayer.AddPhoto(photo);
            user = dataLayer.AddUser(user);

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
            var photo = GeneratePhoto();
            var user = GenerateUser();
            var userAnother = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            photo = dataLayer.AddPhoto(photo);
            user = dataLayer.AddUser(user);
            userAnother = dataLayer.AddUser(userAnother);

            dataLayer.AddLike(photo, user);
            dataLayer.AddLike(photo, userAnother);

            photo.UsersThatLike = dataLayer.GetLikes(photo);

            var resUser = photo.UsersThatLike[0];

            //assert
            Assert.AreEqual(resUser.Id, user.Id);
        }

        [TestMethod]
        public void ShouldAddHastag()
        {
            //arrange
            var comment = GenerateComment();
            var hashtag = Guid.NewGuid().ToString();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
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
            var comment = GenerateComment();
            var hashtag = Guid.NewGuid().ToString();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
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
        public void ShouldAddFriend()
        {
            //arrange
            var user = GenerateUser();
            var userFriend = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            user = dataLayer.AddUser(user);
            userFriend = dataLayer.AddUser(userFriend);

            dataLayer.AddSubscription(user, userFriend);

            bool areFriends = dataLayer.HaveSubscription(user, userFriend);

            //assert
            Assert.AreEqual(areFriends, true);
        }

        [TestMethod]
        public void ShouldGetSubscriptions()
        {
            //arrange
            var user = GenerateUser();
            var userFriend = GenerateUser();
            var userFriend2 = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            user = dataLayer.AddUser(user);
            userFriend = dataLayer.AddUser(userFriend);
            userFriend2 = dataLayer.AddUser(userFriend2);

            dataLayer.AddSubscription(user, userFriend);
            dataLayer.AddSubscription(user, userFriend2);

            user.Subscriptions = dataLayer.GetSubscription(user);
            var trueSubscription = true;

            foreach (var friend in user.Subscriptions)
            {
                if (!dataLayer.HaveSubscription(user, friend))
                {
                    trueSubscription = false;
                }
            }

            //assert
            Assert.AreEqual(trueSubscription, true);
        }

        [TestMethod]
        public void ShouldDeleteFriend()
        {
            //arrange
            var user = GenerateUser();
            var userFriend = GenerateUser();

            //act
            var dataLayer = new DataLayer.Sql.DataLayer(ConnectionSql);
            user = dataLayer.AddUser(user);
            userFriend = dataLayer.AddUser(userFriend);

            dataLayer.DeleteSubscription(user, userFriend);
            var areFriends = dataLayer.HaveSubscription(user, userFriend);

            //assert
            Assert.AreEqual(areFriends, false);
        }
    }
}
