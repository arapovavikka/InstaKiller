using System;
using System.Collections.Generic;
using InstaKiller.Model;
using System.Data.SqlClient;
using NLog;
using InstaKiller.Services;


namespace InstaKiller.DataLayer.Sql
{
    public class DataLayer : InstaKiller.Services.IDataLayer
    {
        private readonly string _connectionSql;
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public DataLayer(string connectionSql)
        {
            if (connectionSql == null) throw new ArgumentNullException(nameof(connectionSql));

            _connectionSql = connectionSql;
        }

        public bool AddComment(Comment comment)
        {
            _log.Info("Adding comment...");
            if (HaveUser(comment.UserId) && HavePhoto(comment.PhotoId))
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        comment.Id = Guid.NewGuid();
                        comment.DateTime = DateTime.Now;
                        _log.Info("Unique ID generated:{0}", comment.Id);

                        command.CommandText = @"insert into comment(id, photo_id, user_id, text, date_time) 
                        values (@id, @photo_id, @user_id, @text, @date_time)";
                        command.Parameters.AddWithValue("@id", comment.Id);
                        command.Parameters.AddWithValue("@photo_id", comment.PhotoId);
                        command.Parameters.AddWithValue("@user_id", comment.UserId);
                        command.Parameters.AddWithValue("@text", comment.Text);
                        command.Parameters.AddWithValue("@date_time", comment.DateTime);
                        command.ExecuteNonQuery();

                        _log.Info("Comment added.");
                        LogComment(comment.Id);

                        return true;
                    }
                }
            }
            //can't find person that want add comment
            // or can't find photo 
            _log.Debug("Comment wasn't added");
            _log.Error("User id: {0}\n Photo id: {1} \n Comment with id = {2} wasn't added", comment.UserId,
                comment.PhotoId, comment.Id);
            LogUser(comment.UserId);
            LogPhoto(comment.PhotoId);

            return false;
        }

        public bool AddPhoto(Photo photo)
        {
            _log.Info("Adding photo...");
            if (HaveUser(photo.UserId))
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        photo.Id = Guid.NewGuid();
                        photo.TimeDate = DateTime.Now;

                        command.CommandText = @"insert into photo(id, user_id, date_time, image_url) 
                        values (@id, @user_id, @date_time, @image_url)";
                        command.Parameters.AddWithValue("@id", photo.Id);
                        command.Parameters.AddWithValue("@user_id", photo.UserId);
                        command.Parameters.AddWithValue("@date_time", photo.TimeDate);
                        command.Parameters.AddWithValue("@image_url", photo.ImageUrl);
                        command.ExecuteNonQuery();

                        _log.Info("Photo added with id: {0}", photo.Id);
                        LogPhoto(photo.Id);
       

                        return true;
                    }
                }
            }
            //can't find user that want post photo
            _log.Debug("Photo wasn't added. \n User with id = {0} doesn't exist.", photo.UserId);
            _log.Error("User id = {0}\n Photo with id = {1} wasn't added.", photo.UserId, photo.Id);
            LogUser(photo.UserId);
            LogPhoto(photo.Id);

            return false;
        }

        public bool HaveUser(Guid userId)
        {
            _log.Info("Checking existence of user...");
            if (userId != Guid.Empty)
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText =
                            "select last_name, user_name, first_name, email from person where @id = id";
                        command.Parameters.AddWithValue(@"id", userId);
                        using (var reader = command.ExecuteReader())
                        {
                            reader.Read();
                            _log.Info(reader.HasRows ? "User exists." : "User doesn't exist.");

                            return reader.HasRows;
                        }
                    }
                }
            }
            //wrong user id
            _log.Warn("User doesn't exist.");

            return false;
        }

        public bool HaveComment(Guid commentId)
        {
            _log.Info("Checking existence of comment...");
            if (commentId != Guid.Empty)
            {
                using (var connecton = new SqlConnection(_connectionSql))
                {
                    connecton.Open();
                    using (var command = connecton.CreateCommand())
                    {
                        command.CommandText =
                            "select text, user_id, photo_id, date_time from comment where @id = id";
                        command.Parameters.AddWithValue(@"id", commentId);
                        using (var reader = command.ExecuteReader())
                        {
                            reader.Read();
                            _log.Info(reader.HasRows ? "Comment exists." : "Comment doesn't exist.");

                            return reader.HasRows;
                        }
                    }
                }
            }
            //wrong id of comment
            _log.Warn("Comment doesn't exist.");

            return false;
        }

        public bool HaveLike(Guid photoId, Guid likeId)
        {
            _log.Info("Checking existence of like...");
            if (likeId != Guid.Empty && photoId != Guid.Empty)
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText =
                            "select user_id, photo_id from [like] where @id = id";
                        command.Parameters.AddWithValue(@"id", likeId);
                        using (var reader = command.ExecuteReader())
                        {
                            reader.Read();
                            _log.Info(reader.HasRows ? "Comment exists." : "Comment doesn't exist.");

                            if (reader.HasRows)
                            {
                                if (reader.GetGuid(reader.GetOrdinal("photo_id")) == photoId)
                                {
                                    return true;
                                }
                                return false;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool UpdateUser(Guid userId, Person userUpdate)
        {
            _log.Info("Updating user with id: {0}", userId);
            if (userId != Guid.Empty)
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        if (HaveUser(userId))
                        {
                            LogUser(userId);

                            command.CommandText = @"update person set last_name = @last_name, first_name = @first_name,
                        email = @email, user_name = @user_name where id = @id";
                            command.Parameters.AddWithValue(@"id", userId);
                            command.Parameters.AddWithValue(@"user_name", userUpdate.Name);
                            command.Parameters.AddWithValue(@"last_name", userUpdate.LastName);
                            command.Parameters.AddWithValue(@"first_name", userUpdate.FirstName);
                            command.Parameters.AddWithValue(@"email", userUpdate.Email);
                            command.ExecuteNonQuery();

                            _log.Info("User updated.");
                            LogUser(userId);

                            return true;
                        }
                        _log.Debug("User wasn't updated.");
                        _log.Error("User wasn't updated at {0}. User id = {1}", DateTime.Now, userId);

                        return false;
                    }
                }
            }
            //empty current user id
            _log.Warn("User doesn't exist.");
            LogManager.Flush();

            return false;
        }

        public bool UpdatePhoto(Guid photoId, Photo photoUpdate)
        {
            _log.Info("Updating photo with id: {0}", photoId);
            if (photoId != Guid.Empty)
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        if (HavePhoto(photoId))
                        {
                            LogPhoto(photoId);

                            command.CommandText = @"update photo set
                        image_url = @image_url where id = @id";
                            command.Parameters.AddWithValue(@"id", photoId);
                            command.Parameters.AddWithValue(@"image_url", photoUpdate.ImageUrl);
                            command.ExecuteNonQuery();

                            _log.Info("Photo updated.");
                            LogPhoto(photoId);
                             

                            return true;
                        }
                        _log.Debug("Photo doesn't exist.");
                        _log.Error("Photo with id = {0} doesn't exist.", photoId);
                         

                        return false;
                    }
                }
            }
            //empty photo id
            _log.Debug("Photo doesn't exist.");
             

            return false;
        }

        public bool HavePhoto(Guid photoId)
        {
            _log.Info("Checking existence of photo...");
            if (photoId != Guid.Empty)
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "select user_id, image_url, date_time from photo where @id = id";
                        command.Parameters.AddWithValue(@"id", photoId);

                        using (var reader = command.ExecuteReader())
                        {
                            reader.Read();
                            _log.Info(reader.HasRows ? "Photo exists." : "Photo doesn't exist.");
                             
                            return reader.HasRows;
                        }
                    }

                }
            }
            //can't find photo
            _log.Debug("Photo doesn't exist.");
             

            return false;
        }

        public bool AddUser(Person user)
        {
            _log.Info("Adding user...");
            using (var connection = new SqlConnection(_connectionSql))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    user.Id = Guid.NewGuid();

                    _log.Info("Unique id generated: {0}", user.Id);

                    command.CommandText = @"insert into person(id, user_name, last_name, first_name, email) 
                        values (@id, @user_name, @last_name, @first_name, @email)";
                    command.Parameters.AddWithValue("@id", user.Id);
                    command.Parameters.AddWithValue("@user_name", user.Name);
                    command.Parameters.AddWithValue("@first_name", user.FirstName);
                    command.Parameters.AddWithValue("@last_name", user.LastName);
                    command.Parameters.AddWithValue("@email", user.Email);
                    command.ExecuteNonQuery();

                    _log.Info("User added.");
                    LogUser(user.Id);
                     

                    return true;
                }
            }
        }

        public bool UpdateComment(Guid commentId, Comment commentUpdate)
        {
            _log.Info("Updating comment...");
            if (commentId != Guid.Empty)
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        if (HaveComment(commentId))
                        {
                            LogComment(commentId);

                            command.CommandText = @"update comment set user_id = @user_id,
                        photo_id = @photo_id, text = @text where id = @id";
                            command.Parameters.AddWithValue(@"id", commentId);
                            //TODO: check that while updating user id can't be changed
                            command.Parameters.AddWithValue(@"user_id", commentUpdate.UserId);
                            command.Parameters.AddWithValue(@"photo_id", commentUpdate.PhotoId);
                            command.Parameters.AddWithValue(@"text", commentUpdate.Text);
                            command.ExecuteNonQuery();

                            _log.Info("Comment updated.");
                            LogComment(commentId);
                             

                            return true;
                        }
                        _log.Debug("Comment doesn't exist");
                        _log.Error("Comment with id = {0} doesn't exist", commentId);
                         

                        return false;
                    }
                }
            }
            //wrong comment id
            _log.Debug("Comment doesn't exist.");
             

            return false;
        }

        public List<string> GetAllHashtags(Comment comment)
        {
            throw new NotImplementedException();
        }

        public Comment GetComment(Guid commentId)
        {
            _log.Info("Getting comment...");
            if (commentId != Guid.Empty)
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText =
                            @"select id, text, user_id, photo_id, date_time from comment where @id = id";
                        command.Parameters.AddWithValue("@id", commentId);
                        using (var reader = command.ExecuteReader())
                        {
                            reader.Read();
                            if (reader.HasRows)
                            {
                                _log.Info("Comment with id = {0} was found.", commentId);
                                 

                                return new Comment
                                {
                                    Id = reader.GetGuid(reader.GetOrdinal("id")),
                                    Text = reader.GetString(reader.GetOrdinal("text")),
                                    UserId = reader.GetGuid(reader.GetOrdinal("user_id")),
                                    PhotoId = reader.GetGuid(reader.GetOrdinal("photo_id")),
                                    DateTime = reader.GetDateTime(reader.GetOrdinal("date_time"))
                                };
                            }

                            //if don't find comment in db
                            _log.Debug("Comment with id = {0} wasn't found.", commentId);
                            _log.Error("Comment with id = {0} wasn't found.", commentId);
                             

                            return new Comment();
                        }
                    }
                }
            }
            _log.Debug("Comment doesn't exist.");
             

            return new Comment();
        }

        public bool DeleteUser(Guid userId)
        {
            _log.Info("Deleting user...");
            if (userId != Guid.Empty)
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        if (HaveUser(userId))
                        {
                            LogUser(userId);

                            command.CommandText = @"delete person where @id = id";
                            command.Parameters.AddWithValue(@"id", userId);
                            command.ExecuteNonQuery();
                            _log.Info(!HaveUser(userId)
                                ? ("User with id = " + userId + " was deleted.")
                                : "User with id = " + userId + " wasn't deleted.");

                            return !HaveUser(userId);
                        }
                        else
                        {
                            _log.Info("User with id = {0} doesn't exist.", userId);
                            return false;
                        }
                    }
                }
            }
            else
            {
                _log.Info("User doesn't exist.");
                 

                return false;
            }
        }

        public bool DeleteComment(Guid commentId)
        {
            _log.Info("Deleting comment...");
            LogComment(commentId);

            if (commentId != Guid.Empty)
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        if (HaveComment(commentId))
                        {
                            LogComment(commentId);

                            command.CommandText = @"delete comment where @id = id";
                            command.Parameters.AddWithValue(@"id", commentId);
                            command.ExecuteNonQuery();

                            if (!HaveComment(commentId))
                            {
                                _log.Info("Comment with id = " + commentId + " was deleted.");
                                 

                                return true;
                            }
                            else
                            {
                                _log.Info("Comment with id = " + commentId + " wasn't deleted.");
                                 

                                return false;
                            }
                        }
                        else
                        {
                            _log.Info("Comment with id = {0} doesn't exist.", commentId);
                             

                            return false;
                        }
                    }
                }
            }
            else
            {
                //empty id
                _log.Info("Comment doesn't exist.");
                 

                return false;
            }
        }

        public Photo GetPhoto(Guid photoId)
        {
            _log.Info("Getting photo from datebase...");
            if (photoId != Guid.Empty)
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = @"select id, user_id, image_url, date_time from photo
                        where @id = id";
                        command.Parameters.AddWithValue(@"id", photoId);

                        using (var reader = command.ExecuteReader())
                        {
                            reader.Read();
                            if (reader.HasRows)
                            {
                                _log.Info("Photo with id = {0} was found.", photoId);
                                 

                                return new Photo
                                {
                                    //param of Get - ordinal number of coloumn in table
                                    Id = reader.GetGuid(reader.GetOrdinal(@"id")),
                                    UserId = reader.GetGuid(reader.GetOrdinal(@"user_id")),
                                    ImageUrl = reader.GetString(reader.GetOrdinal(@"image_url")),
                                    TimeDate = reader.GetDateTime(reader.GetOrdinal(@"date_time"))
                                };
                            }

                            //if don't find photo in db
                            _log.Debug("Photo with id = {0} wasn't found.", photoId);
                            _log.Error("Photo with id = {0} wasn't found.", photoId);
                             

                            return new Photo();
                        }
                    }
                }
            }
            //empty id
            _log.Debug("Photo doesn't exist.");
             

            return new Photo();
        }

        public Person GetUser(Guid userId)
        {
            _log.Info("Getting user from datebase...");
            if (userId != Guid.Empty)
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = @"select id, user_name, last_name, first_name, email from person
                        where @id = id";
                        command.Parameters.AddWithValue(@"id", userId);

                        using (var reader = command.ExecuteReader())
                        {
                            reader.Read();

                            if (reader.HasRows)
                            {
                                _log.Info("User with id = {0} was found.", userId);
                                 

                                return new Person
                                {
                                    //param of Get - ordinal number of coloumn in table
                                    Id = reader.GetGuid(reader.GetOrdinal(@"id")),
                                    LastName = reader.GetString(reader.GetOrdinal(@"last_name")),
                                    FirstName = reader.GetString(reader.GetOrdinal(@"first_name")),
                                    Name = reader.GetString(reader.GetOrdinal(@"user_name")),
                                    Email = reader.GetString(reader.GetOrdinal(@"email"))
                                };
                            }

                            //if don't find user in db
                            _log.Info("User with id = {0} wasn't found.", userId);
                            _log.Error("User with id = {0} wasn't found.", userId);
                             

                            return new Person();
                        }
                    }
                }
            }
            _log.Info("User doesn't exist.");
             

            return new Person();
        }

        public bool DeletePhoto(Guid photoId)
        {
            _log.Info("Deleting photo...");
            if (photoId != Guid.Empty)
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        if (HavePhoto(photoId))
                        {
                            LogPhoto(photoId);

                            command.CommandText = @"delete photo where @id = id";
                            command.Parameters.AddWithValue(@"id", photoId);
                            command.ExecuteNonQuery();

                            _log.Info(!HavePhoto(photoId)
                                ? "Photo with id = " + photoId + " was deleted."
                                : "Photo with id = " + photoId + " wasn't deleted.");
                             

                            return !HavePhoto(photoId);
                        }
                        else
                        {
                            _log.Info("Photo with id = {0} doesn't exist.", photoId);
                             

                            return false;
                        }

                    }
                }
            }
            else
            {
                //wrong id
                _log.Info("Photo doesn't exist.");
                 

                return false;
            }
        }


        //store subscription that have user (for ex he has subscription on userSubscription)
        public void AddSubscription(Person user, Person userSubscription)
        {
            _log.Info("Adding subscription...");

            if (HaveUser(user.Id))
            {
                if (HaveUser(userSubscription.Id))
                {
                    using (var connection = new SqlConnection(_connectionSql))
                    {
                        connection.Open();
                        using (var command = connection.CreateCommand())
                        {
                            if (!HaveSubscription(user, userSubscription))
                            {
                                var id = Guid.NewGuid();

                                _log.Info("User with id = {0}", user.Id);
                                HaveUser(user.Id);
                                _log.Info("User subscriber with id = {0}", userSubscription.Id);
                                HaveUser(userSubscription.Id);

                                command.CommandText =
                                    @"insert into subscription(id, user_id, user_subscription_id) values(@id, @user_id,
                        @user_subscription_id)";
                                command.Parameters.AddWithValue(@"id", id);
                                command.Parameters.AddWithValue(@"user_id", user.Id);
                                command.Parameters.AddWithValue(@"user_subscription_id", userSubscription.Id);
                                command.ExecuteNonQuery();
                                user.Subscriptions.Add(userSubscription);

                                _log.Info("User with id = {0} subscribed on user with id = {1}.", userSubscription.Id,
                                    user.Id);
                            }
                            else
                            {
                                //already have such subscriptions
                                _log.Info("User with id = {0} already have subscribed on user with id = {1}.",
                                    userSubscription.Id, user.Id);
                            }
                        }
                    }
                }
                else
                {
                    _log.Warn("User subscriber with id = {0} doesn't exist.", userSubscription.Id);
                }
            }
            else
            {
                _log.Warn("User with id = {0} doesn't exist.", user.Id);
            }
             
        }

        public bool HaveSubscription(Person user, Person userSubscription)
        {
            _log.Info("Checking subscription...");
            if (HaveUser(user.Id))
            {
                if (HaveUser(userSubscription.Id))
                {
                    using (var connection = new SqlConnection(_connectionSql))
                    {
                        connection.Open();
                        using (var command = connection.CreateCommand())
                        {
                            _log.Info("User with id = {0}", user.Id);
                            HaveUser(user.Id);
                            _log.Info("User subscriber with id = {0}", userSubscription.Id);
                            HaveUser(userSubscription.Id);

                            command.CommandText =
                                @"select id from subscription where @user_id = user_id and @user_subscription_id = user_subscription_id";
                            command.Parameters.AddWithValue(@"user_id", user.Id);
                            command.Parameters.AddWithValue(@"user_subscription_id", userSubscription.Id);

                            using (var reader = command.ExecuteReader())
                            {
                                reader.Read();
                                _log.Info(reader.HasRows ? "Subscription exists." : "Subscription doesn't exist.");
                                 

                                return reader.HasRows;
                            }
                        }
                    }
                }
                _log.Warn("User subscriber with id = {0} doesn't exist.", userSubscription.Id);
                 

                return false;
            }
            _log.Warn("User with id = {0} doesn't exist.", user.Id);
             

            return false;
        }

        public bool HaveSubscriber(Person user, Person userSubscription)
        {
            _log.Info("Checking subscriber...");
            if (HaveUser(user.Id))
            {
                if (HaveUser(userSubscription.Id))
                {
                    using (var connection = new SqlConnection(_connectionSql))
                    {
                        connection.Open();
                        using (var command = connection.CreateCommand())
                        {
                            _log.Info("User with id = {0}", user.Id);
                            HaveUser(user.Id);
                            _log.Info("User subscriber with id = {0}", userSubscription.Id);
                            HaveUser(userSubscription.Id);

                            command.CommandText =
                                "select id from realation where @user_subscribe_id = user_subscribe_id and @user_id = user_id";
                            command.Parameters.AddWithValue(@"user_id", user.Id);
                            command.Parameters.AddWithValue(@"user_subscribe_id", userSubscription.Id);

                            using (var reader = command.ExecuteReader())
                            {
                                reader.Read();
                                _log.Info(reader.HasRows ? "User has subscriber." : "User doesn't have subsciber.");
                                 

                                return reader.HasRows;
                            }
                        }
                    }
                }
                _log.Warn("User subscriber with id = {0} doesn't exist.", userSubscription.Id);
                 

                return false;
            }
            _log.Debug("User with id = {0} doesn't exist.", user.Id);
             

            return false;
        }

        public List<Person> GetSubscription(Person user)
        {
            _log.Info("Getting all user subscribers...");
            List<Person> personsSubscription = new List<Person>();
            if (HaveUser(user.Id))
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        LogUser(user.Id);

                        command.CommandText = @"select * from subscription where @user_id = user_id";
                        command.Parameters.AddWithValue(@"user_id", user.Id);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                _log.Info("Information about subscribers.");
                                var userSubscription = new Person
                                {
                                    Id = reader.GetGuid(reader.GetOrdinal(@"user_subscription_id"))

                                };
                                userSubscription = GetUser(userSubscription.Id);
                                personsSubscription.Add(userSubscription);

                                LogUser(userSubscription.Id);
                            }
                            _log.Info("Got all subscribers.");
                        }
                    }
                }
            }
            else
            {
                //don't have user in db
                _log.Warn("User with id = {0} doesn't exist.", user.Id);
            }
             
            return personsSubscription;
        }

        public bool DeleteSubscription(Person user, Person userSubscription)
        {
            _log.Info("Deleting subscriber...");
            if (HaveUser(user.Id))
            {
                if (HaveUser(userSubscription.Id))
                {
                    using (var connection = new SqlConnection(_connectionSql))
                    {
                        connection.Open();
                        using (var command = connection.CreateCommand())
                        {
                            _log.Info("User with id = {0}", user.Id);
                            HaveUser(user.Id);
                            _log.Info("User subscriber with id = {0}", userSubscription.Id);
                            HaveUser(userSubscription.Id);

                            if (HaveSubscription(user, userSubscription))
                            {
                                command.CommandText =
                                    @"delete subscription where @user_id = user_id and @user_subscription_id = user_subscription_id";
                                command.Parameters.AddWithValue(@"user_id", user.Id);
                                command.Parameters.AddWithValue(@"user_subscription_id", userSubscription.Id);

                                command.ExecuteNonQuery();
                                _log.Info(HaveSubscription(user, userSubscription)
                                    ? "Subscription wasn't removed"
                                    : "Subscription was removed.");
                                return !HaveSubscription(user, userSubscription);
                            }
                            else
                            {
                                _log.Info("Subscription doesn't exist.");
                                return false;
                            }
                        }
                    }
                }
                _log.Warn("User subscriber with id = {0} doesn't exist.", userSubscription.Id);
                return false;
            }
            _log.Warn("User with id = {0} doesn't exist.", user.Id);
             
            return false;
        }

        public bool DeleteSubscription(Person user, Guid relationId)
        {
            if (HaveSubscription(user, relationId))
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText =
                                    @"delete subscription where @user_id = user_id and @id = id";
                        command.Parameters.AddWithValue(@"user_id", user.Id);
                        command.Parameters.AddWithValue(@"id", relationId);

                        command.ExecuteNonQuery();
                        _log.Info(HaveSubscription(user, relationId)
                            ? "Subscription wasn't removed"
                            : "Subscription was removed.");
                        return !HaveSubscription(user, relationId);
                    }
                }
            }
            return false;
        }

        public Guid AddLike(Photo photo, Person user)
        {
            _log.Info("Adding like...");
            if (HaveUser(user.Id))
            {
                if (HavePhoto(photo.Id))
                {
                    using (var connection = new SqlConnection(_connectionSql))
                    {
                        connection.Open();
                        using (var command = connection.CreateCommand())
                        {
                            if (!HaveLike(photo, user))
                            {
                                var id = Guid.NewGuid();

                                _log.Info("User with id = {0}", user.Id);
                                HaveUser(user.Id);
                                _log.Info("Photo with id = {0}", photo.Id);
                                HavePhoto(photo.Id);

                                command.CommandText = @"insert into [like](id, user_id, photo_id) values(@id, @user_id,
                        @photo_id)";
                                command.Parameters.AddWithValue(@"id", id);
                                command.Parameters.AddWithValue(@"user_id", user.Id);
                                command.Parameters.AddWithValue(@"photo_id", photo.Id);
                                command.ExecuteNonQuery();
                                photo.UsersThatLike.Add(user);

                                _log.Info("Like added.");
                                 

                                return id;
                            }
                            //already has like on this photo
                            _log.Info("Like already exists.");
                             

                            return Guid.Empty;
                        }
                    }
                }
                _log.Warn("Photo with id = {0} doesn't exist.", photo.Id);
                 

                return Guid.Empty;
            }
            else
            {
                // can't find person 
                _log.Warn("User with id = {0} doesn't exist.", user.Id);
                 

                return Guid.Empty;
            }
        }

        public bool HaveLike(Photo photo, Person user)
        {
            if (HavePhoto(photo.Id) && HaveUser(user.Id))
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText =
                            @"select id from [like] where @user_id = user_id and @photo_id = photo_id";
                        command.Parameters.AddWithValue(@"user_id", user.Id);
                        command.Parameters.AddWithValue(@"photo_id", photo.Id);

                        using (var reader = command.ExecuteReader())
                        {
                            reader.Read();
                            return reader.HasRows;
                        }
                    }
                }
            }
            else
            {
                //can't find photo or user
                return false;
            }
        }

        public bool DeleteLike(Photo photo, Person user)
        {
            if (HaveUser(user.Id) && HavePhoto(photo.Id))
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        if (HaveLike(photo, user))
                        {
                            command.CommandText = @"delete [like] where @user_id = user_id and @photo_id = photo_id";
                            command.Parameters.AddWithValue(@"user_id", user.Id);
                            command.Parameters.AddWithValue(@"photo_id", photo.Id);

                            command.ExecuteNonQuery();
                            return !HaveLike(photo, user);
                        }
                        return false;
                    }
                }
            }
            else
            {
                //can't find user or photo
                return false;
            }
        }

        public bool DeleteLike(Photo photo, Guid likeId)
        {
            if (HavePhoto(photo.Id) && HaveLike(photo.Id, likeId))
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = @"delete [like] where @id = id and @photo_id = photo_id";
                        command.Parameters.AddWithValue(@"id", likeId);
                        command.Parameters.AddWithValue(@"photo_id", photo.Id);

                        command.ExecuteNonQuery();
                        return !HaveLike(photo.Id, likeId);
                    }
                }
            }
            return false;
        }

        public List<Person> GetLikes(Photo photo)
        {
            if (HavePhoto(photo.Id))
            {
                var persons = new List<Person>();
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = @"select * from [like] where @photo_id = photo_id";
                        command.Parameters.AddWithValue(@"photo_id", photo.Id);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var user = new Person
                                {
                                    Id = reader.GetGuid(reader.GetOrdinal(@"user_id"))
                                };
                                user = GetUser(user.Id);
                                persons.Add(user);
                            }
                        }
                    }
                }
                return persons;
            }
            else
            {
                //can't find photo
                throw new ArgumentException();
            }

        }

        public bool HaveHashtag(Comment comment, string hashtag)
        {
            if (HaveComment(comment.Id) && hashtag != string.Empty)
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText =
                            @"select id from [hashtag] where @text = text";
                        command.Parameters.AddWithValue(@"text", hashtag);
                        Guid hashtagId;

                        using (var reader = command.ExecuteReader())
                        {
                            reader.Read();
                            if (!reader.HasRows)
                            {
                                return false;
                            }
                            hashtagId = reader.GetGuid(reader.GetOrdinal("id"));
                        }

                        command.CommandText =
                            @"select id from [hashtagcomments] where @comment_id = comment_id and @hashtag_id = hashtag_id";
                        command.Parameters.AddWithValue(@"comment_id", comment.Id);
                        command.Parameters.AddWithValue(@"hashtag_id", hashtagId);

                        using (var reader = command.ExecuteReader())
                        {
                            reader.Read();
                            return reader.HasRows;
                        }
                    }
                }
            }
            else
            {
                //can't find comment or empty hashtag
                throw new ArgumentException();
            }

        }

        public Guid GetHashtag(string hashtag)
        {
            if (hashtag != string.Empty)
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText =
                            @"select id from [hashtag] where @text = text";
                        command.Parameters.AddWithValue(@"text", hashtag);

                        using (var reader = command.ExecuteReader())
                        {
                            reader.Read();
                            if (!reader.HasRows)
                            {
                                return Guid.Empty;
                            }
                            return reader.GetGuid(reader.GetOrdinal("id"));
                        }
                    }
                }
            }
            else
            {
                //empty hashtag
                throw new ArgumentException();
            }
        }

        public Comment AddHashtag(Comment comment, string hashtag)
        {
            if (HaveComment(comment.Id) && hashtag != string.Empty)
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    var hashtagId = Guid.NewGuid();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = @"insert into [hashtag](id, text) values(@id, @text)";
                        command.Parameters.AddWithValue(@"id", hashtagId);
                        command.Parameters.AddWithValue(@"text", hashtag);

                        command.ExecuteNonQuery();
                        command.Parameters.Clear();

                        var hashtagInCommentId = Guid.NewGuid();
                        command.CommandText =
                            @"insert into [hashtagcomments](id, hashtag_id, comment_id) values(@id, @hashtag_id, @comment_id)";
                        command.Parameters.AddWithValue(@"id", hashtagInCommentId);
                        command.Parameters.AddWithValue(@"hashtag_id", hashtagId);
                        command.Parameters.AddWithValue(@"comment_id", comment.Id);

                        command.ExecuteNonQuery();
                        return comment;
                    }
                }
            }
            else
            {
                //can't find comment or string is empty
                throw new ArgumentException();
            }

        }

        public List<Photo> GetLatestPhotos(DateTime timeFrom, DateTime timeTo)
        {
            var photos = new List<Photo>();
            using (var connection = new SqlConnection(_connectionSql))
            {

                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"select * from [photo] where date_time between @time_from and @time_to";
                    command.Parameters.AddWithValue(@"time_from", timeFrom);
                    command.Parameters.AddWithValue(@"time_to", timeTo);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var photo = new Photo
                            {
                                Id = reader.GetGuid(reader.GetOrdinal(@"id"))
                            };
                            photo = GetPhoto(photo.Id);
                            photos.Add(photo);
                        }
                    }
                }
            }
            return photos;
        }

        public bool DeleteHashtag(Comment comment, string hashtag)
        {
            if (HaveComment(comment.Id) && hashtag != string.Empty)
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        var hashtagId = GetHashtag(hashtag);
                        if (hashtagId != Guid.Empty)
                        {
                            command.CommandText =
                                @"delete [hashtagcomments] where @comment_id = comment_id and @hashtag_id = hashtag_id";
                            command.Parameters.AddWithValue(@"comment_id", comment.Id);
                            command.Parameters.AddWithValue(@"hashtag_id", hashtagId);

                            command.ExecuteNonQuery();
                            return !HaveHashtag(comment, hashtag);
                        }
                        else
                        {
                            //can't get id of hashtag 
                            return false;
                        }
                    }
                }
            }
            else
            {
                //can't find comment or string is empty
                return false;
            }
        }

        public bool HaveSubscription(Person user, Guid relationId)
        {
            if (HaveUser(user.Id))
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText =
                            "select id from subscription where @id = id and @user_id = user_id";
                        command.Parameters.AddWithValue(@"id", relationId);
                        command.Parameters.AddWithValue(@"user_id", user.Id);
                        using (var reader = command.ExecuteReader())
                        {
                            reader.Read();
                            return reader.HasRows;
                        }
                    }
                }
            }
            return false;
        }

        public bool HaveSubscribe(Person user, Person userSubscriber)
        {
            if (HaveUser(user.Id) && HaveUser(userSubscriber.Id))
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText =
                            "select id from subscription where @user_subscription_id = user_subscription_id and @user_id = user_id";
                        command.Parameters.AddWithValue(@"user_subscription_id", user.Id);
                        command.Parameters.AddWithValue(@"user_id", userSubscriber.Id);

                        using (var reader = command.ExecuteReader())
                        {
                            reader.Read();
                            return reader.HasRows;
                        }
                    }
                }
            }
            else
            {
                //can't find user or userSubscriber in db
                throw new ArgumentException();
            }
        }

        public List<Person> GetSubscribers(Person user)
        {
            if (HaveUser(user.Id))
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    List<Person> subscribers = new List<Person>();
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText =
                            "select * from subscription where @user_subscription_id = user_subscription_id";
                        command.Parameters.AddWithValue(@"user_subscription_id", user.Id);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var userSubscriber = new Person
                                {
                                    Id = reader.GetGuid(reader.GetOrdinal(@"user_id"))
                                };
                                userSubscriber = GetUser(userSubscriber.Id);
                                subscribers.Add(userSubscriber);
                            }
                        }
                    }
                    return subscribers;
                }
            }
            else
            {
                //can't find user
                throw new ArgumentException();
            }

        }

        public Session AddSession(Session session)
        {
            using (var connection = new SqlConnection(_connectionSql))
            {
                session.Id = Guid.NewGuid();
                // time?

                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "insert into session(id, user_id, user_ip, token, date_from, date_to) " +
                                          "values(@id, @user_id, @user_ip, @token, @date_from, @date_to)";
                    command.Parameters.AddWithValue(@"id", session.Id);
                    command.Parameters.AddWithValue(@"user_id", session.UserId);
                    command.Parameters.AddWithValue(@"token", session.Token);
                    command.Parameters.AddWithValue(@"user_ip", session.UserIp);
                    command.Parameters.AddWithValue(@"date_from", session.DateFrom);
                    command.Parameters.AddWithValue(@"date_to", session.DateTo);

                    command.ExecuteNonQuery();
                }
                return session;
            }
        }

        public Session GetSession(Guid sessionId)
        {
            using (var connection = new SqlConnection(_connectionSql))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "select id, user_id, user_ip, token, date_from, date_to from session where @id = id";
                    command.Parameters.AddWithValue(@"id", sessionId);

                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        if (!reader.HasRows)
                            return new Session();
                        else
                            return new Session()
                            {
                                Id = reader.GetGuid(reader.GetOrdinal(@"id")),
                                UserIp = reader.GetInt32(reader.GetOrdinal(@"user_ip")),
                                UserId = reader.GetGuid(reader.GetOrdinal(@"user_id")),
                                Token = reader.GetInt32(reader.GetOrdinal(@"token")),
                                DateFrom = reader.GetDateTime(reader.GetOrdinal(@"date_from")),
                                DateTo = reader.GetDateTime(reader.GetOrdinal(@"date_to"))
                            };
                    }
                }
            }
        }

        public void GetAllUsersSessions()
        {
            throw new NotImplementedException();
        }

        public void LogUser(Guid userId)
        {
            if (userId != Guid.Empty)
            {
                var user = GetUser(userId);
                _log.Debug("User id: {0} \n Name: {1} \n Lastname: {2} \n FirstName: {3} \n" +
                           "Email: {4} \n", user.Id, user.Name, user.LastName, user.FirstName, user.Email);
                //TODO: log subscribers    
            }
            else
            {
                _log.Debug("Don't have user in datebase!");
            }
        }

        public void LogPhoto(Guid photoId)
        {
            if (photoId != Guid.Empty)
            {
                var photo = GetPhoto(photoId);
                _log.Debug("Photo id: {0} \n User id: {1} \n Image URL: {2} \n" +
                           "TimeData: {3} \n", photo.Id, photo.UserId, photo.ImageUrl, photo.TimeDate);
                //TODO: all users that like and all comments
            }
            else
            {
                _log.Debug("Don't have photo in datebase!");
            }
        }

        public void LogComment(Guid commentId)
        {
            if (commentId != Guid.Empty)
            {
                var comment = GetComment(commentId);
                _log.Debug("Comment id: {0} \n User id: {1} \n Photo id: {2} \n" +
                           "Text: {3} \n DateTime: {4} \n", comment.Id, comment.UserId,
                    comment.PhotoId, comment.Text, comment.DateTime);
                //TODO: add all hashtags
            }
            else
            {
                _log.Debug("Don't have comment in datebase!");
            }
        }

        public List<Comment> GetAllComments(Guid photoId)
        {
            if (photoId != Guid.Empty)
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    List<Comment> AllComments = new List<Comment>();
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = @"select * from comment where @photo_id = photo_id";
                        command.Parameters.AddWithValue(@"photo_id", photoId);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var comment = new Comment()
                                {
                                    Id = reader.GetGuid(reader.GetOrdinal(@"id"))
                                };
                                comment = GetComment(comment.Id);
                                AllComments.Add(comment);
                            }
                        }

                        return AllComments;
                    }
                }
            }
            else
            {
                //wrong id
                throw new ArgumentException();
            }
        }

        public Comment GetComment(Guid photoId, Guid commentId)
        {
            if (photoId != Guid.Empty && HavePhoto(photoId))
            {
                if (commentId != Guid.Empty && HaveComment(commentId))
                {
                    List<Comment> allComments = GetAllComments(photoId);
                    foreach (var comment in allComments)
                    {
                        if (comment.Id == commentId)
                        {
                            return GetComment(comment.Id);
                        }
                    }
                }
                return new Comment();
            }
            return new Comment();
        }

        public bool DeleteComment(Guid photoId, Guid commentId)
        {
            if (HavePhoto(photoId))
            {
                if (HaveComment(commentId))
                {
                    if (DeleteComment(commentId))
                    {
                        var photo = GetPhoto(photoId);
                        photo.AllComments = GetAllComments(commentId);
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }

        public Person GetLike(Photo photo, Guid likeId)
        {
            if (HavePhoto(photo.Id) && HaveLike(photo.Id, likeId))
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = @"select * from [like] where @id = id";
                        command.Parameters.AddWithValue(@"id", likeId);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //TODO:add checking that photo_id = photo.Id
                                var user = new Person
                                {
                                    Id = reader.GetGuid(reader.GetOrdinal(@"user_id"))
                                };
                                return GetUser(user.Id);
                            }
                            return new Person();
                        }
                    }
                }
            }
            return new Person();
        }

        public Guid GetLike(Photo photo, Person user)
        {
            if (HavePhoto(photo.Id) && HaveUser(user.Id) && HaveLike(photo, user))
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = @"select * from [like] where @photo_id = photo_id and @user_id = user_id";
                        command.Parameters.AddWithValue(@"photo_id", photo.Id);
                        command.Parameters.AddWithValue(@"user_id", user.Id);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var likeId = reader.GetGuid(reader.GetOrdinal(@"id"));
                                return likeId;
                            }
                            return Guid.Empty;
                        }
                    }
                }
            }
            return Guid.Empty;
        }

        public Guid GetSubscription(Person user, Person userSubscription)
        {
            if (HaveUser(user.Id) && HaveUser(userSubscription.Id) && HaveSubscription(user, userSubscription))
            {
                using (var connection = new SqlConnection(_connectionSql))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText =
                            @"select * from subscription where @user_id = user_id and @user_subscription_id = user_subscription_id";
                        command.Parameters.AddWithValue(@"user_id", user.Id);
                        command.Parameters.AddWithValue(@"user_subscription_id", userSubscription.Id);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var relationId = reader.GetGuid(reader.GetOrdinal(@"id"));
                                return relationId;
                            }
                            return Guid.Empty;
                        }
                    }
                }
                
            }
            return Guid.Empty;
        }
    }
}
