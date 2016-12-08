using System;
using System.Collections.Generic;
using InstaKiller.Model;

namespace InstaKiller.Services
{
    public interface IDataLayer
    {
        //User methods
        bool AddUser(Person user);
        Person GetUser(Guid userId);
        bool DeleteUser(Guid userId);
        bool UpdateUser(Guid userId, Person userUpdate);
        Person GetUserByEmail(string email);

        //Photo methods
        bool AddPhoto(Photo photo);
        Photo GetPhoto(Guid photoId);
        bool DeletePhoto(Guid photoId);
        bool UpdatePhoto(Guid photoId, Photo photoUpdate);
        List<Photo> GetLatestPhotos(DateTime timeFrom, DateTime timeTo);
        List<Comment> GetAllComments(Guid photoId);

        //Like methods
        Guid AddLike(Photo photo, Person user);
        Guid GetLike(Photo photo, Person user);
        List<Person> GetLikes(Photo photo);
        bool DeleteLike(Photo photo, Guid likeId);
        bool HaveLike(Photo photo, Person user);
        bool HaveLike(Guid photo, Guid likeId);

        //Comment methods
        bool AddComment(Comment comment);
        Comment GetComment(Guid commentId);
        Comment GetComment(Guid photoId, Guid commentId);
        bool DeleteComment(Guid commentId);
        bool DeleteComment(Guid photoId, Guid commentId);
        bool UpdateComment(Guid commentId, Comment commentUpdate);

        //Hashtag methods
        List<string> GetAllHashtags(Comment comment);
        //TODO: delete string hashtag in param
        Comment AddHashtag(Comment comment, string hashtag);
        Guid GetHashtag(string hashtag);
        bool HaveHashtag(Comment comment, string hashtag);
        bool DeleteHashtag(Comment comment, string hashtag);

        //User subscription methods
        void AddSubscription(Person user, Person userSubscription);
        bool HaveSubscription(Person user, Person userSubscription);
        bool DeleteSubscription(Person user, Person userSubscription);
        bool DeleteSubscription(Person user, Guid relationId);
        List<Person> GetSubscription(Person user);
        Guid GetSubscription(Person user, Person userSubscription);

        //User subscribers 
        bool HaveSubscriber(Person user, Person userSub);
        List<Person> GetSubscribers(Person user);

        //TODO: Session
        Session AddSession(Session session);
        Session GetSession(Guid sessionId);

        //TODO: PersonMention - (@username)
    }
}
