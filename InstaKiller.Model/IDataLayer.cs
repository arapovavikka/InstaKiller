using System;
using System.Collections.Generic;

namespace InstaKiller.Model
{
    public interface IDataLayer
    {
        //User methods
        bool AddUser(Person user);
        Person GetUser(Guid userId);
        void DeleteUser(Guid userId);
        bool UpdateUser(Guid userId, Person userUpdate);

        //Photo methods
        bool AddPhoto(Photo photo);
        Photo GetPhoto(Guid photoId);
        void DeletePhoto(Guid photoId);
        bool UpdatePhoto(Guid photoId, Photo photoUpdate);
        List<Photo> GetLatestPhotos(DateTime timeFrom, DateTime timeTo);
        List<Comment> GetAllComments(Guid photoId);

        //Like methods
        bool AddLike(Photo photo, Person user);
        List<Person> GetLikes(Photo photo);
        void DeleteLike(Photo photo, Person user);
        bool HaveLike(Photo photo, Person user);

        //Comment methods
        bool AddComment(Comment comment);
        Comment GetComment(Guid commentId);
        void DeleteComment(Guid commentId);
        bool UpdateComment(Guid commentId, Comment commentUpdate);

        //Hashtag methods
        List<string> GetAllHashtags(Comment comment);
        //TODO: delete string hashtag in param
        Comment AddHashtag(Comment comment, string hashtag);
        Guid GetHashtag(string hashtag);
        bool HaveHashtag(Comment comment, string hashtag);
        void DeleteHashtag(Comment comment, string hashtag);

        //User subscription methods
        void AddSubscription(Person user, Person friendUser);
        bool HaveSubscription(Person user, Person friendUser);
        void DeleteSubscription(Person user, Person friendUser);
        List<Person> GetSubscription(Person user);

        //User subscribers 
        bool HaveSubscriber(Person user, Person userSub);
        List<Person> GetSubscribers(Person user);

        //TODO: Session
        Session AddSession(Session session);
        Session GetSession(Guid sessionId);

        //TODO: PersonMention - (@username)
    }
}
