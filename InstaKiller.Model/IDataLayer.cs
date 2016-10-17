using System;
using System.Collections.Generic;

namespace InstaKiller.Model
{
    public interface IDataLayer
    {
        //User methods
        Person AddUser(Person user);
        Person GetUser(Guid userId);
        void DeleteUser(Guid userId);
        Person UpdateUser(Person userUpdate);

        //Photo methods
        Photo AddPhoto(Photo photo);
        Photo GetPhoto(Guid photoId);
        void DeletePhoto(Guid photoId);
        Photo UpdatePhoto(Photo photoUpdate);
        List<Photo> GetLatestPhotos(DateTime timeFrom, DateTime timeTo);

        //Like methods
        void AddLike(Photo photo, Person user);
        List<Person> GetLikes(Photo photo);
        void DeleteLike(Photo photo, Person user);
        bool HaveLike(Photo photo, Person user);

        //Comment methods
        Comment AddComment(Comment comment);
        Comment GetComment(Guid commentId);
        void DeleteComment(Guid commentId);
        Comment UpdateComment(Comment commentUpdate);

        //Hashtag methods
        //TODO: GetAllHashtags(Comment comment)
        Comment AddHashtag(Comment comment, string hashtag);
        Guid GetHashtag(string hashtag);
        bool HaveHashtag(Comment comment, string hashtag);
        void DeleteHashtag(Comment comment, string hashtag);

        //User subscription methods
        void AddSubscription(Person user, Person friendUser);
        bool HaveSubscription(Person user, Person friendUser);
        void DeleteSubscription(Person user, Person friendUser);
        List<Person> GetSubscription(Person user);

        //TODO: User subscribers 

        //TODO: Session

        //TODO: PersonMention - (@username)
    }
}
