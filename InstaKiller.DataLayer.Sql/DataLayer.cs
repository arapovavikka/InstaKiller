using System;
using System.Collections.Generic;
using InstaKiller.Model;
using System.Data.SqlClient;

namespace InstaKiller.DataLayer.Sql
{
    public class DataLayer:IDataLayer
    {
        private readonly string _connectionSql;
        
        public DataLayer(string connectionSql)
        {
            if (connectionSql == null) throw new ArgumentNullException(nameof(connectionSql));

            _connectionSql = connectionSql;
        }

        public Comment AddComment(Comment comment)
        {
            using (var connection = new SqlConnection(_connectionSql))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    comment.Id = Guid.NewGuid();
                    comment.Date = DateTime.Now;

                    command.CommandText = @"insert into comment(id, photo_id, user_id, text, date_time) 
                        values (@id, @photo_id, @user_id, @text, @date_time)";
                    command.Parameters.AddWithValue("@id", comment.Id);
                    command.Parameters.AddWithValue("@photo_id", comment.PhotoId);
                    command.Parameters.AddWithValue("@user_id", comment.UserId);
                    command.Parameters.AddWithValue("@text", comment.Text);
                    command.Parameters.AddWithValue("@date_time", comment.Date);

                    command.ExecuteNonQuery();
                    return comment;
                }
            }
        }

        public Photo AddPhoto(Photo photo)
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
                    return photo;
                }
            }
        }

        public Person UpdateUser(Person userUpdate)
        {
            using (var connection = new SqlConnection(_connectionSql))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"update person set last_name = @last_name, first_name = @first_name,
                        email = @email, user_name = @user_name where id = @id";
                    command.Parameters.AddWithValue(@"id", userUpdate.Id);
                    command.Parameters.AddWithValue(@"user_name", userUpdate.Name);
                    command.Parameters.AddWithValue(@"last_name", userUpdate.LastName);
                    command.Parameters.AddWithValue(@"first_name", userUpdate.FirstName);
                    command.Parameters.AddWithValue(@"email", userUpdate.Email);

                    command.ExecuteNonQuery();
                    return userUpdate;
                }
            }
        }

        public Photo UpdatePhoto(Photo photoUpdate)
        {
            using (var connection = new SqlConnection(_connectionSql))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"update photo set date_time = @date_time, user_id = @user_id,
                        image_url = @image_url where id = @id";
                    command.Parameters.AddWithValue(@"id", photoUpdate.Id);
                    command.Parameters.AddWithValue(@"date_time", photoUpdate.TimeDate);
                    command.Parameters.AddWithValue(@"user_id", photoUpdate.UserId);
                    command.Parameters.AddWithValue(@"image_url", photoUpdate.ImageUrl);

                    command.ExecuteNonQuery();
                    return photoUpdate;
                }
            }
        }

        public Person AddUser(Person user)
        {
            using (var connection = new SqlConnection(_connectionSql))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    user.Id = Guid.NewGuid();

                    command.CommandText = @"insert into person(id, user_name, last_name, first_name, email) 
                        values (@id, @user_name, @last_name, @first_name, @email)";
                    command.Parameters.AddWithValue("@id", user.Id);
                    command.Parameters.AddWithValue("@user_name", user.Name);
                    command.Parameters.AddWithValue("@first_name", user.FirstName);
                    command.Parameters.AddWithValue("@last_name", user.LastName);
                    command.Parameters.AddWithValue("@email", user.Email);

                    command.ExecuteNonQuery();
                    return user;
                }
            }
        }

        public Comment UpdateComment(Comment commentUpdate)
        {
            using (var connection = new SqlConnection(_connectionSql))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"update comment set date_time = @date_time, user_id = @user_id,
                        photo_id = @photo_id, text = @text where id = @id";
                    command.Parameters.AddWithValue(@"id", commentUpdate.Id);
                    command.Parameters.AddWithValue(@"date_time", commentUpdate.Date);
                    command.Parameters.AddWithValue(@"user_id", commentUpdate.UserId);
                    command.Parameters.AddWithValue(@"photo_id", commentUpdate.PhotoId);
                    command.Parameters.AddWithValue(@"text", commentUpdate.Text);

                    command.ExecuteNonQuery();
                    return commentUpdate;
                }
            }
        }

        public Comment GetComment(Guid commentId)
        {
            using (var connection = new SqlConnection(_connectionSql))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"select id, text, user_id, photo_id from comment where @id = id";
                    command.Parameters.AddWithValue("@id", commentId);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        if (reader.HasRows)
                            return new Comment
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("id")),
                                Text = reader.GetString(reader.GetOrdinal("text")),
                                UserId = reader.GetGuid(reader.GetOrdinal("user_id")),
                                PhotoId = reader.GetGuid(reader.GetOrdinal("photo_id")),
                            };

                        //if don't find comment in db
                        return new Comment();
                    }
                }
            }
        }

        public void DeleteUser(Guid userId)
        {
            using (var connection = new SqlConnection(_connectionSql))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"delete person where @id = id";
                    command.Parameters.AddWithValue(@"id", userId);

                    command.ExecuteNonQuery();
                }
            }
        }

        //TODO: change return type for all delete methods.
        public void DeleteComment(Guid commentId)
        {
            using (var connection = new SqlConnection(_connectionSql))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"delete comment where @id = id";
                    command.Parameters.AddWithValue(@"id", commentId);

                    command.ExecuteNonQuery();
                    //return command.ExecuteNonQuery();
                }
            }
        }

        public Photo GetPhoto(Guid photoId)
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
                            return new Photo
                            {
                                //param of Get - ordinal number of coloumn in table
                                Id = reader.GetGuid(reader.GetOrdinal(@"id")),
                                UserId = reader.GetGuid(reader.GetOrdinal(@"user_id")),
                                ImageUrl = reader.GetString(reader.GetOrdinal(@"image_url")),
                                TimeDate = reader.GetDateTime(reader.GetOrdinal(@"date_time"))
                            };

                        //if don't find photo in db
                        return new Photo();
                    }
                }
            }
        }

        public Person GetUser(Guid userId)
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
                            return new Person
                            {
                                //param of Get - ordinal number of coloumn in table
                                Id = reader.GetGuid(reader.GetOrdinal(@"id")),
                                LastName = reader.GetString(reader.GetOrdinal(@"last_name")),
                                FirstName = reader.GetString(reader.GetOrdinal(@"first_name")),
                                Name = reader.GetString(reader.GetOrdinal(@"user_name")),
                                Email = reader.GetString(reader.GetOrdinal(@"email"))
                            };

                        //if don't find user in db
                        return new Person(); 
                    }
                }
            }
        }

        public void DeletePhoto(Guid photoId)
        {
            using (var connection = new SqlConnection(_connectionSql))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"delete photo where @id = id";
                    command.Parameters.AddWithValue(@"id", photoId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddSubscription(Person user, Person userFriend)
        {
            using (var connection = new SqlConnection(_connectionSql))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var id = Guid.NewGuid();

                    command.CommandText = @"insert into relation(id, user_id, friend_user_id) values(@id, @user_id,
                        @friend_user_id)";
                    command.Parameters.AddWithValue(@"id", id);
                    command.Parameters.AddWithValue(@"user_id", user.Id);
                    command.Parameters.AddWithValue(@"friend_user_id", userFriend.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public bool HaveSubscription(Person user, Person userAnother)
        {
            using (var connection = new SqlConnection(_connectionSql))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        @"select id from relation where @friend_user_id = friend_user_id and @user_id = user_id";
                    command.Parameters.AddWithValue(@"friend_user_id", userAnother.Id);
                    command.Parameters.AddWithValue(@"user_id", user.Id);

                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        return reader.HasRows;
                    }
                }
            }
        }

        public List<Person> GetSubscription(Person user)
        {
            List<Person> persons = new List<Person>();
            using (var connection = new SqlConnection(_connectionSql))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"select * from relation where @user_id = user_id";
                    command.Parameters.AddWithValue(@"user_id", user.Id);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var userFriend = new Person
                            {
                                Id = reader.GetGuid(reader.GetOrdinal(@"friend_user_id"))
                            };
                            userFriend = GetUser(userFriend.Id);
                            persons.Add(userFriend);
                        }
                    }
                }
            }
            return persons;
        }

        public void DeleteSubscription(Person user, Person userFriend)
        {
            using (var connection = new SqlConnection(_connectionSql))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"delete relation where @user_id = user_id and @friend_user_id = friend_user_id";
                    command.Parameters.AddWithValue(@"user_id", user.Id);
                    command.Parameters.AddWithValue(@"friend_user_id", userFriend.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddLike(Photo photo, Person user)
        {
            using (var connection = new SqlConnection(_connectionSql))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var id = Guid.NewGuid();

                    command.CommandText = @"insert into [like](id, user_id, photo_id) values(@id, @user_id,
                        @photo_id)";
                    command.Parameters.AddWithValue(@"id", id);
                    command.Parameters.AddWithValue(@"user_id", user.Id);
                    command.Parameters.AddWithValue(@"photo_id", photo.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public bool HaveLike(Photo photo, Person user)
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

        public void DeleteLike(Photo photo, Person user)
        {
            using (var connection = new SqlConnection(_connectionSql))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"delete [like] where @user_id = user_id and @photo_id = photo_id";
                    command.Parameters.AddWithValue(@"user_id", user.Id);
                    command.Parameters.AddWithValue(@"photo_id", photo.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Person> GetLikes(Photo photo)
        {
            List<Person> persons = new List<Person>();
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

        public bool HaveHashtag(Comment comment, string hashtag)
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

        public Guid GetHashtag(string hashtag)
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

        public Comment AddHashtag(Comment comment, string hashtag)
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
                    command.CommandText = @"insert into [hashtagcomments](id, hashtag_id, comment_id) values(@id, @hashtag_id, @comment_id)";
                    command.Parameters.AddWithValue(@"id", hashtagInCommentId);
                    command.Parameters.AddWithValue(@"hashtag_id", hashtagId);
                    command.Parameters.AddWithValue(@"comment_id", comment.Id);

                    command.ExecuteNonQuery();
                    return comment;
                }
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

        public void DeleteHashtag(Comment comment, string hashtag)
        {
            using (var connection = new SqlConnection(_connectionSql))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var hashtagId = GetHashtag(hashtag);
                    command.CommandText = @"delete [hashtagcomments] where @comment_id = comment_id and @hashtag_id = hashtag_id";
                    command.Parameters.AddWithValue(@"comment_id", comment.Id);
                    command.Parameters.AddWithValue(@"hashtag_id", hashtagId);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
