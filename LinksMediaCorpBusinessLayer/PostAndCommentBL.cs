namespace LinksMediaCorpBusinessLayer
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using LinksMediaCorpDataAccessLayer;
    using LinksMediaCorpEntity;
    using LinksMediaCorpUtility;
    using System.Web;
    using LinksMediaCorpUtility.Resources;
    public class PostAndCommentBL
    {
        /// <summary>
        /// Function to post Share message text on trainer profile UI
        /// </summary>
        /// <returns>ViewPostVM</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/18/2015
        /// </devdoc>
        public static ViewPostVM PostShare(PostVM<TextMessageStream> message)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {

                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: PostShare");
                    Credentials objCred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    tblMessageStream objMessageStream = new tblMessageStream();
                    objMessageStream.Content = message.Stream.TextMessage;
                    objMessageStream.MessageType = Message.TextMessageType;
                    objMessageStream.PostedDate = DateTime.Now;
                    objMessageStream.TargetType = Message.UserTargetType;
                    objMessageStream.SubjectId = objCred.Id;
                    //Modified by 08/13/2015
                    objMessageStream.IsImageVideo = message.IsImageVideo;
                    objMessageStream.TargetId = _dbContext.Credentials.FirstOrDefault(c => c.UserId == message.TargetId && c.UserType == Message.UserTypeTrainer).Id;
                    _dbContext.MessageStraems.Add(objMessageStream);
                    _dbContext.SaveChanges();
                    // User Notification when user post message on other user or trainer post data their team member
                    if (objCred.UserId == message.TargetId && objCred.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        NotificationApiBL.SendTrainerTeamsUserNotificationByTrainer(objCred.UserId, objMessageStream.MessageStraemId);
                    }
                    else
                    {
                        NotificationApiBL.SendNotificationToALLNType(message.TargetId, Message.UserTypeTrainer, NotificationType.ProfilesPostToUser.ToString(), objCred, objMessageStream.MessageStraemId);
                    }
                    ViewPostVM objViewPostVM = new ViewPostVM()
                    {
                        PostId = objMessageStream.MessageStraemId,
                        Message = objMessageStream.Content,
                        PostedDate = CommonWebApiBL.GetDateTimeFormat(objMessageStream.PostedDate),
                        BoomsCount = _dbContext.Booms.Count(b => b.MessageStraemId == objMessageStream.MessageStraemId),
                        CommentsCount = _dbContext.Comments.Count(cmnt => cmnt.MessageStraemId == objMessageStream.MessageStraemId)
                    };
                    if (objCred.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                    {
                        //tblUser user1 = _dbContext.User.FirstOrDefault(u => u.UserId == objMessageStream.SubjectId);
                        var user = (from usr in _dbContext.User
                                    join crd in _dbContext.Credentials on usr.UserId equals crd.UserId
                                    where crd.UserType == Message.UserTypeUser && crd.Id == objMessageStream.SubjectId
                                    select new
                                    {
                                        usr.FirstName,
                                        usr.LastName,
                                        usr.UserImageUrl,
                                        crd.UserId,
                                        crd.UserType
                                    }).FirstOrDefault();
                        if (user != null)
                        {
                            objViewPostVM.PostedByImageUrl = (!string.IsNullOrEmpty(user.UserImageUrl)) ? CommonUtility.VirtualPath + Message.ProfilePicDirectory + user.UserImageUrl : string.Empty;
                            objViewPostVM.UserName = user.FirstName + " " + user.LastName;
                            objViewPostVM.UserId = user.UserId;
                            objViewPostVM.UserType = user.UserType;
                        }
                    }
                    else if (objCred.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        // tblTrainer trainer = _dbContext.Trainer.FirstOrDefault(t => t.TrainerId == objMessageStream.SubjectId);
                        var trainer = (from t in _dbContext.Trainer
                                       join crd in _dbContext.Credentials on t.TrainerId equals crd.UserId
                                       where crd.UserType == Message.UserTypeTrainer && crd.Id == objMessageStream.SubjectId
                                       select new
                                       {
                                           t.FirstName,
                                           t.LastName,
                                           t.TrainerImageUrl,
                                           crd.UserId,
                                           crd.UserType
                                       }).FirstOrDefault();
                        if (trainer != null)
                        {
                            objViewPostVM.PostedByImageUrl = (!string.IsNullOrEmpty(trainer.TrainerImageUrl)) ? CommonUtility.VirtualPath + Message.ProfilePicDirectory + trainer.TrainerImageUrl : string.Empty;
                            objViewPostVM.UserName = trainer.FirstName + " " + trainer.LastName;
                            objViewPostVM.UserId = trainer.UserId;
                            objViewPostVM.UserType = trainer.UserType;
                        }
                    }
                    return objViewPostVM;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End PostShare: --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get post list on on trainer profile UI
        /// </summary>
        /// <returns>ViewPostVM</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/18/2015
        /// </devdoc>
        public static Total<List<ViewPostVM>> GetPostList(int trainerId, int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetPostList");
                    int streamTrainerId = _dbContext.Credentials.Where(c => c.UserId == trainerId && c.UserType == Message.UserTypeTrainer).Select(crd => crd.Id).FirstOrDefault();
                    Credentials userCredential = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    // Add  trainer profile post message
                    List<ViewPostVM> objList = (from m in _dbContext.MessageStraems
                                                join c in _dbContext.Credentials on m.SubjectId equals c.Id
                                                where m.IsImageVideo && (!(string.IsNullOrEmpty(m.Content)) || m.IsImageVideo) &&
                                                !(m.TargetType == Message.UserTargetType && m.SubjectId == streamTrainerId && m.SubjectId != m.TargetId)
                                                &&
                                                (m.SubjectId == streamTrainerId || (m.TargetType == Message.UserTargetType && m.TargetId == streamTrainerId))
                                                orderby m.PostedDate descending
                                                select new ViewPostVM
                                                {
                                                    PostId = m.MessageStraemId,
                                                    DbPostedDate = m.PostedDate,
                                                    Message = m.Content,
                                                    BoomsCount = _dbContext.Booms.Count(b => b.MessageStraemId == m.MessageStraemId),
                                                    CommentsCount = _dbContext.Comments.Count(cmnt => cmnt.MessageStraemId == m.MessageStraemId),
                                                    PostedBy = m.SubjectId,
                                                    UserId = _dbContext.Credentials.FirstOrDefault(crede => crede.Id == m.SubjectId).UserId,
                                                    UserType = _dbContext.Credentials.FirstOrDefault(crede => crede.Id == m.SubjectId).UserType,
                                                    IsLoginUserBoom = _dbContext.Booms.Where(bm => bm.MessageStraemId == m.MessageStraemId).Any(b => b.BoomedBy == userCredential.Id),
                                                    IsLoginUserComment = _dbContext.Comments.Where(cm => cm.MessageStraemId == m.MessageStraemId).Any(b => b.CommentedBy == userCredential.Id),
                                                    VideoList = (from vl in _dbContext.MessageStreamVideo
                                                                 where vl.MessageStraemId == m.MessageStraemId && !string.IsNullOrEmpty(vl.VideoUrl)
                                                                 select new VideoInfo
                                                                 {
                                                                     RecordId = vl.RecordId,
                                                                     VideoUrl = vl.VideoUrl
                                                                 }).ToList(),
                                                    PicList = (from pl in _dbContext.MessageStreamPic
                                                               where pl.MessageStraemId == m.MessageStraemId && !string.IsNullOrEmpty(pl.PicUrl)
                                                               select new PicsInfo
                                                               {
                                                                   PicId = pl.RecordId,
                                                                   PicsUrl = pl.PicUrl,
                                                                   ImageMode = pl.ImageMode,
                                                                   Width = pl.Width,
                                                                   Height = pl.Height
                                                               }).ToList()
                                                }).ToList();

                    foreach (ViewPostVM item in objList)
                    {
                        tblCredentials objCred = _dbContext.Credentials.FirstOrDefault(cr => cr.Id == item.PostedBy);
                        string imageUrl = string.Empty;
                        if (objCred != null && objCred.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                        {
                            tblUser userTemp = _dbContext.User.FirstOrDefault(usr => usr.UserId == objCred.UserId);
                            if (userTemp != null)
                                imageUrl = string.IsNullOrEmpty(userTemp.UserImageUrl) ? string.Empty : CommonUtility.VirtualPath +
                                    Message.ProfilePicDirectory + userTemp.UserImageUrl;
                        }
                        else if (objCred != null && objCred.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                        {
                            tblTrainer trainerTemp = _dbContext.Trainer.FirstOrDefault(usr => usr.TrainerId == objCred.UserId);
                            if (trainerTemp != null)
                                imageUrl = string.IsNullOrEmpty(trainerTemp.TrainerImageUrl) ? string.Empty :
                                    CommonUtility.VirtualPath + Message.ProfilePicDirectory + trainerTemp.TrainerImageUrl;
                        }
                        item.PostedByImageUrl = imageUrl;
                        item.UserName = objCred != null ? (objCred.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase)
                                         ? _dbContext.User.FirstOrDefault(usr => usr.UserId == objCred.UserId).FirstName + " "
                                         + _dbContext.User.FirstOrDefault(usr => usr.UserId == objCred.UserId).LastName
                                         : _dbContext.Trainer.FirstOrDefault(tnr => tnr.TrainerId == objCred.UserId).FirstName + " "
                                         + _dbContext.Trainer.FirstOrDefault(tnr => tnr.TrainerId == objCred.UserId).LastName) : string.Empty;
                        item.PostedDate = CommonWebApiBL.GetDateTimeFormat(item.DbPostedDate);
                        //Code For Getting Posted Pics 
                        item.PicList.ForEach(pic =>
                        {
                            // pic.PicsUrl = CommonUtility.VirtualPath + Message.ResultPicDirectory + pic.PicsUrl;
                            pic.PicsUrl = (!string.IsNullOrEmpty(pic.PicsUrl) &&
                                System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ResultPicDirectory + pic.PicsUrl))) ?
                                CommonUtility.VirtualPath + Message.ResultPicDirectory + pic.PicsUrl : string.Empty;
                        }
                        );
                        string thumnailHeight, thumnailWidth;
                        //Code For Getting Posted Videos
                        item.VideoList.ForEach(vid =>
                        {
                            string thumnailFileName = vid.VideoUrl.Split('.')[0] + Message.JpgImageExtension;
                            vid.ThumbNailUrl = string.IsNullOrEmpty(vid.VideoUrl) ? string.Empty : CommonUtility.VirtualPath +
                                Message.ResultVideoDirectory + vid.VideoUrl.Split('.')[0] + Message.JpgImageExtension;
                            // vid.VideoUrl = CommonUtility.VirtualPath + Message.ResultVideoDirectory + vid.VideoUrl;
                            vid.VideoUrl = !string.IsNullOrEmpty(vid.VideoUrl) ? CommonUtility.VirtualPath +
                                Message.ResultVideoDirectory + vid.VideoUrl : string.Empty;
                            thumnailHeight = string.Empty;
                            thumnailWidth = string.Empty;
                            CommonWebApiBL.GetThumNailDemension(thumnailFileName, out thumnailHeight, out thumnailWidth);
                            vid.ThumbNailHeight = thumnailHeight;
                            vid.ThumbNailWidth = thumnailWidth;
                        });

                    }
                    Total<List<ViewPostVM>> objresult = new Total<List<ViewPostVM>>();
                    objresult.TotalList = (from l in objList
                                           orderby l.DbPostedDate descending
                                           select l).Skip(startIndex).Take(endIndex - startIndex).ToList();
                    objresult.TotalCount = objList.Count();
                    if ((objresult.TotalCount) > endIndex)
                    {
                        objresult.IsMoreAvailable = true;
                    }
                    return objresult;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End : GetPostList  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to post boom on trainer profile UI
        /// </summary>
        /// <returns>int</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/16/2015
        /// </devdoc>
        public static int PostBoom(ViewPostVM model)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: PostBoom");
                    Credentials objCred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    if (!_dbContext.Booms.Any(b => b.MessageStraemId == model.PostId && b.BoomedBy == objCred.Id))
                    {
                        tblBoom objBoom = new tblBoom();
                        objBoom.BoomedBy = objCred.Id;
                        objBoom.BoomedDate = DateTime.Now;
                        objBoom.MessageStraemId = model.PostId;
                        _dbContext.Booms.Add(objBoom);
                        _dbContext.SaveChanges();
                        // Send the User Notification to user on result boomed
                        if (model.UserId > 0 && !string.IsNullOrEmpty(model.UserType) && !(objCred.UserType == model.UserType && objCred.UserId == model.UserId))
                        {
                            NotificationApiBL.SendNotificationToALLNType(model.UserId, model.UserType, NotificationType.NewsFeedBoomed.ToString(), objCred, model.PostId);
                        }
                    }
                    return _dbContext.Booms.Count(b => b.MessageStraemId == model.PostId);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End : PostBoom  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Fucntion to post comment on trainer profile UI
        /// </summary>
        /// <returns>CommentVM</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/16/2015
        /// </devdoc>
        public static CommentVM PostComment(CommentVM model)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: PostComment");
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    tblComment objComment = new tblComment();
                    objComment.CommentedBy = cred.Id;
                    objComment.CommentedDate = DateTime.Now;
                    objComment.Message = model.Message;
                    objComment.MessageStraemId = model.PostId;
                    _dbContext.Comments.Add(objComment);
                    _dbContext.SaveChanges();
                    // Send the User Notification to user on result boomed                     
                    if (!string.IsNullOrEmpty(model.UserType) && model.UserId > 0 && cred.UserId == model.UserId && cred.UserType == model.UserType)
                    {
                        NotificationApiBL.SendALLSenderNotificationByUser(model.PostId, NotificationType.NewsFeedCommented.ToString(), NotificationType.PostCommentedReplyMsg.ToString(), cred);
                    }
                    else if (model.UserId > 0 && !string.IsNullOrEmpty(model.UserType))
                    {
                        NotificationApiBL.SendNotificationToALLNType(model.UserId, model.UserType, NotificationType.NewsFeedCommented.ToString(), cred, model.PostId);
                        NotificationApiBL.SendALLSenderNotificationByUser(model.PostId, NotificationType.NewsFeedCommented.ToString(), NotificationType.PostCommentedReplyMsg.ToString(), cred);
                    }
                    CommentVM objCommentVM = new CommentVM()
                    {
                        CommentId = objComment.CommentId,
                        PostId = model.PostId,
                        CommentBy = objComment.CommentedBy,
                        Message = model.Message,
                        CommentDate = CommonWebApiBL.GetDateTimeFormat(objComment.CommentedDate),
                        PostedCommentDate = objComment.CommentedDate.ToUniversalTime()
                    };
                    if (!string.IsNullOrEmpty(cred.UserType) && cred.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                    {
                        var user = _dbContext.User.FirstOrDefault(u => u.UserId == cred.UserId);
                        if (user != null)
                        {
                            objCommentVM.ImageUrl = string.IsNullOrEmpty(user.UserImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + user.UserImageUrl;
                            objCommentVM.UserName = user.FirstName + " " + user.LastName;
                            objCommentVM.UserId = user.UserId;
                            objCommentVM.UserType = Message.UserTypeUser;
                        }
                    }
                    else if (!string.IsNullOrEmpty(cred.UserType) && cred.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        var trainer = _dbContext.Trainer.FirstOrDefault(t => t.TrainerId == cred.UserId);
                        if (trainer != null)
                        {
                            objCommentVM.ImageUrl = string.IsNullOrEmpty(trainer.TrainerImageUrl) ? string.Empty :
                                CommonUtility.VirtualPath + Message.ProfilePicDirectory + trainer.TrainerImageUrl;
                            objCommentVM.UserName = trainer.FirstName + " " + trainer.LastName;
                            objCommentVM.UserId = trainer.TrainerId;
                            objCommentVM.UserType = Message.UserTypeTrainer;
                        }
                    }
                    return objCommentVM;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End : PostComment  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get boom list of a comment on trainer profile UI
        /// </summary>
        /// <returns>Total<List<BoomVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/18/2015
        /// </devdoc>
        public static Total<List<BoomVM>> GetBoomList(int messageStreamId, int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetBoomList");
                    var result = (from b in _dbContext.Booms
                                  join c in _dbContext.Credentials on b.BoomedBy equals c.Id
                                  where b.MessageStraemId == messageStreamId
                                  select new
                                  {
                                      _Credential = c,
                                      _Boom = b
                                  });
                    List<BoomVM> objBoomList = new List<BoomVM>();
                    foreach (var item in result)
                    {
                        BoomVM objBoom = new BoomVM();
                        objBoom.BoomedtDate = CommonWebApiBL.GetDateTimeFormat(item._Boom.BoomedDate);
                        objBoom.BoomId = item._Boom.BoomId;
                        if (item._Credential.UserType.Equals(Message.UserTypeUser))
                        {
                            var user = _dbContext.User.FirstOrDefault(u => u.UserId == item._Credential.UserId);
                            if (user != null)
                            {
                                objBoom.UserName = user.FirstName + " " + user.LastName;
                                objBoom.ImageUrl = string.IsNullOrEmpty(user.UserImageUrl) ? string.Empty : CommonUtility.VirtualPath +
                                    Message.ProfilePicDirectory + user.UserImageUrl;
                            }
                        }
                        else
                        {
                            var trainer = _dbContext.Trainer.FirstOrDefault(t => t.TrainerId == item._Credential.UserId);
                            if (trainer != null)
                            {
                                objBoom.UserName = trainer.FirstName + " " + trainer.LastName;
                                objBoom.ImageUrl = string.IsNullOrEmpty(trainer.TrainerImageUrl) ? string.Empty : CommonUtility.VirtualPath +
                                    Message.ProfilePicDirectory + trainer.TrainerImageUrl;
                            }
                        }
                        objBoomList.Add(objBoom);
                    }
                    Total<List<BoomVM>> objresult = new Total<List<BoomVM>>();
                    objresult.TotalList = (from l in objBoomList
                                           orderby l.BoomedtDate descending
                                           select l).Skip(startIndex).Take(endIndex - startIndex).ToList();
                    objresult.TotalCount = objBoomList.Count();
                    if ((objresult.TotalCount) > endIndex)
                    {
                        objresult.IsMoreAvailable = true;
                    }
                    return objresult;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End : GetBoomList  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get comment list of a shared message on trainer profile UI
        /// </summary>
        /// <returns>Total<List<CommentVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/19/2015
        public static Total<List<CommentVM>> GetCommentList(int messageStreamId, int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetCommentList");

                    var result = (from cmnt in _dbContext.Comments
                                  join c in _dbContext.Credentials on cmnt.CommentedBy equals c.Id
                                  where cmnt.MessageStraemId == messageStreamId
                                  orderby cmnt.CommentedDate descending
                                  select new
                                  {
                                      _Credential = c,
                                      _Comment = cmnt
                                  }).ToList();
                    List<CommentVM> objCommentList = new List<CommentVM>();
                    foreach (var item in result)
                    {
                        CommentVM objComment = new CommentVM();
                        objComment.CommentDate = CommonWebApiBL.GetDateTimeFormat(item._Comment.CommentedDate);
                        objComment.PostedCommentDate = item._Comment.CommentedDate.ToUniversalTime();
                        objComment.CommentId = item._Comment.CommentId;
                        objComment.Message = item._Comment.Message;
                        if (item._Credential.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                        {
                            var user = _dbContext.User.FirstOrDefault(u => u.UserId == item._Credential.UserId);
                            if (user != null)
                            {
                                objComment.UserName = user.FirstName + " " + user.LastName;
                                objComment.ImageUrl = string.IsNullOrEmpty(user.UserImageUrl) ? string.Empty : CommonUtility.VirtualPath +
                                    Message.ProfilePicDirectory + user.UserImageUrl;
                            }
                        }
                        else if (item._Credential.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                        {
                            var trainer = _dbContext.Trainer.FirstOrDefault(t => t.TrainerId == item._Credential.UserId);
                            if (trainer != null)
                            {
                                objComment.UserName = trainer.FirstName + " " + trainer.LastName;
                                objComment.ImageUrl = string.IsNullOrEmpty(trainer.TrainerImageUrl) ? string.Empty : CommonUtility.VirtualPath +
                                    Message.ProfilePicDirectory + trainer.TrainerImageUrl;
                            }
                        }
                        objCommentList.Add(objComment);
                    }
                    Total<List<CommentVM>> objresult = new Total<List<CommentVM>>();
                    objresult.TotalList = (from l in objCommentList
                                               // orderby l.CommentId ascending
                                           select l).Skip(startIndex).Take(endIndex - startIndex).ToList().OrderBy(c => c.CommentId).ToList();
                    objresult.TotalCount = objCommentList.Count();
                    if ((objresult.TotalCount) > endIndex)
                    {
                        objresult.IsMoreAvailable = true;
                    }
                    return objresult;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End : GetCommentList  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Delete  message stream post based on MessageStraemId
        /// </summary>
        /// <param name="MessageStraemId"></param>
        /// <returns></returns>
        public static bool DeleteMessagePost(int MessageStraemId)
        {
            StringBuilder traceLog = null;
            bool isDeleted = false;
            // string root = string.Empty;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start PostAndCommentBL: DeleteMessagePost()");
                    // Delete the comments on post message based on specficed post message
                    var messagePostComment = _dbContext.Comments.Where(cm => cm.MessageStraemId == MessageStraemId).ToList();
                    if (messagePostComment != null && messagePostComment.Count > 0)
                    {
                        _dbContext.Comments.RemoveRange(messagePostComment);
                    }
                    var messageStreamMessage = _dbContext.MessageStraems.Where(ms => ms.MessageStraemId == MessageStraemId).FirstOrDefault();
                    if (messageStreamMessage != null)
                    {
                        _dbContext.MessageStraems.Remove(messageStreamMessage);
                        int DeletedStatsus = _dbContext.SaveChanges();
                        if (DeletedStatsus > 0)
                        {
                            // Delete the message Boom post based on specficed post message
                            var messagePostBoom = _dbContext.Booms.Where(bm => bm.MessageStraemId == MessageStraemId).ToList();
                            if (messagePostBoom != null && messagePostBoom.Count > 0)
                            {
                                _dbContext.Booms.RemoveRange(messagePostBoom);
                            }
                            _dbContext.SaveChanges();
                            // Delete the all associated  MessageStreamPics based on MessageStraemId
                            var messageStreamPicList = _dbContext.MessageStreamPic.Where(ms => ms.MessageStraemId == MessageStraemId).FirstOrDefault();
                            if (messageStreamPicList != null)
                            {
                                _dbContext.MessageStreamPic.Remove(messageStreamPicList);
                                _dbContext.SaveChanges();
                            }
                            // Delete the all associated  MessageStreamVideo based on MessageStraemId
                            var messageStreamVideoList = _dbContext.MessageStreamVideo.Where(ms => ms.MessageStraemId == MessageStraemId).FirstOrDefault();
                            if (messageStreamVideoList != null)
                            {
                                _dbContext.MessageStreamVideo.Remove(messageStreamVideoList);
                                _dbContext.SaveChanges();

                            }
                            isDeleted = true;
                        }

                    }
                    return isDeleted;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End PostAndCommentBL: DeleteMessagePost()  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Delete the comments based on comment ID
        /// </summary>
        /// <param name="commentID"></param>
        /// <returns></returns>
        public static bool DeleteComment(int commentID)
        {
            StringBuilder traceLog = null;
            bool isDeleted = false;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start PostAndCommentBL: DeleteComment()");
                    var commentMessage = _dbContext.Comments.Where(ms => ms.CommentId == commentID).FirstOrDefault();
                    if (commentMessage != null)
                    {
                        _dbContext.Comments.Remove(commentMessage);
                        int DeletedStatsus = _dbContext.SaveChanges();
                        if (DeletedStatsus > 0)
                        {
                            isDeleted = true;
                        }
                    }
                    return isDeleted;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End PostAndCommentBL: DeleteMessagePost()  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }
        /// <summary>
        /// Function to post  resut boom on result feed
        /// </summary>
        /// <returns>int</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/16/2015
        /// </devdoc>
        public static int PostResultBoom(ViewPostVM model)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: PostResultBoom");
                    Credentials objCred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    if (!_dbContext.ResultBooms.Any(b => b.ResultId == model.PostId && b.BoomedBy == objCred.Id))
                    {
                        tblResultBoom objBoom = new tblResultBoom();
                        objBoom.BoomedBy = objCred.Id;
                        objBoom.BoomedDate = DateTime.Now;
                        objBoom.ResultId = model.PostId;
                        _dbContext.ResultBooms.Add(objBoom);
                        _dbContext.SaveChanges();
                        // Send the User Notification to user on result boomed
                        if (model.UserId > 0 && !string.IsNullOrEmpty(model.UserType) && !(objCred.UserType == model.UserType && objCred.UserId == model.UserId))
                        {
                            NotificationApiBL.SendNotificationToALLNType(model.UserId, model.UserType, NotificationType.ResultFeedBoomed.ToString(), objCred, model.PostId);
                        }
                    }
                    return _dbContext.ResultBooms.Count(b => b.ResultId == model.PostId);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End : PostResultBoom  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Fucntion to post  result comment on MY team Result Feed UI
        /// </summary>
        /// <returns>CommentVM</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/16/2015
        /// </devdoc>
        public static CommentVM PostResultCommentData(CommentVM model)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: PostResultComment");
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    tblResultComment objComment = new tblResultComment();
                    objComment.CommentedBy = cred.Id;
                    objComment.CommentedDate = DateTime.Now;
                    objComment.Message = model.Message;
                    objComment.Id = model.PostId;
                    _dbContext.ResultComments.Add(objComment);
                    _dbContext.SaveChanges();
                    // User Notification when user post message on other user or trainer post data their team member
                    if (!string.IsNullOrEmpty(model.UserType) && model.UserId == cred.UserId && model.UserType == cred.UserType)
                    {
                        NotificationApiBL.SendALLSenderNotificationByUser(model.PostId, NotificationType.ResultCommented.ToString(), NotificationType.PostResultReplyMsg.ToString(), cred);
                    }
                    else if (model.UserId > 0 && !string.IsNullOrEmpty(model.UserType))
                    {
                        NotificationApiBL.SendNotificationToALLNType(model.UserId, model.UserType, NotificationType.ResultCommented.ToString(), cred, model.PostId);
                        NotificationApiBL.SendALLSenderNotificationByUser(model.PostId, NotificationType.ResultCommented.ToString(), NotificationType.PostResultReplyMsg.ToString(), cred);
                    }
                    CommentVM objCommentVM = new CommentVM()
                    {
                        CommentId = objComment.ResultCommentId,
                        PostId = model.PostId,
                        CommentBy = objComment.CommentedBy,
                        Message = model.Message,
                        CommentDate = CommonWebApiBL.GetDateTimeFormat(objComment.CommentedDate)
                    };
                    if (cred.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                    {
                        var user = _dbContext.User.FirstOrDefault(u => u.UserId == cred.UserId);
                        if (user != null)
                        {
                            objCommentVM.ImageUrl = string.IsNullOrEmpty(user.UserImageUrl) ? string.Empty : CommonUtility.VirtualPath +
                                Message.ProfilePicDirectory + user.UserImageUrl;
                            objCommentVM.UserName = user.FirstName + " " + user.LastName;
                            objCommentVM.UserId = user.UserId;
                            objCommentVM.UserType = Message.UserTypeUser;
                        }
                    }
                    else if (cred.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        var trainer = _dbContext.Trainer.FirstOrDefault(t => t.TrainerId == cred.UserId);
                        if (trainer != null)
                        {
                            objCommentVM.ImageUrl = string.IsNullOrEmpty(trainer.TrainerImageUrl) ? string.Empty : CommonUtility.VirtualPath +
                                Message.ProfilePicDirectory + trainer.TrainerImageUrl;
                            objCommentVM.UserName = trainer.FirstName + " " + trainer.LastName;
                            objCommentVM.UserId = trainer.TrainerId;
                            objCommentVM.UserType = Message.UserTypeTrainer;
                        }
                    }
                    return objCommentVM;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End : PostResultComment  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get result boom list of on myteam feed UI
        /// </summary>
        /// <returns>Total<List<BoomVM>></returns>
        /// <devdoc>
        /// Developer Name - Irshad
        /// Date - 02/10/2016
        /// </devdoc>
        public static Total<List<BoomVM>> GetResultBoomList(int resultId, int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetBoomList");
                    var result = (from b in _dbContext.ResultBooms
                                  join c in _dbContext.Credentials on b.BoomedBy equals c.Id
                                  where b.ResultId == resultId
                                  select new
                                  {
                                      _Credential = c,
                                      _Boom = b
                                  }).ToList();
                    List<BoomVM> objBoomList = new List<BoomVM>();
                    foreach (var item in result)
                    {
                        BoomVM objBoom = new BoomVM();
                        objBoom.BoomedtDate = CommonWebApiBL.GetDateTimeFormat(item._Boom.BoomedDate);
                        objBoom.BoomId = item._Boom.ResultBoomId;
                        if (item._Credential.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                        {
                            var user = _dbContext.User.FirstOrDefault(u => u.UserId == item._Credential.UserId);
                            if (user != null)
                            {
                                objBoom.UserName = user.FirstName + " " + user.LastName;
                                objBoom.ImageUrl = string.IsNullOrEmpty(user.UserImageUrl) ? string.Empty : CommonUtility.VirtualPath +
                                    Message.ProfilePicDirectory + user.UserImageUrl;
                                objBoom.UserId = user.UserId;
                                objBoom.UserType = Message.UserTypeUser;
                            }
                        }
                        else if (item._Credential.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                        {
                            var trainer = _dbContext.Trainer.FirstOrDefault(t => t.TrainerId == item._Credential.UserId);
                            if (trainer != null)
                            {
                                objBoom.UserName = trainer.FirstName + " " + trainer.LastName;
                                objBoom.ImageUrl = string.IsNullOrEmpty(trainer.TrainerImageUrl) ? string.Empty : CommonUtility.VirtualPath +
                                    Message.ProfilePicDirectory + trainer.TrainerImageUrl;
                                objBoom.UserId = trainer.TrainerId;
                                objBoom.UserType = Message.UserTypeTrainer;
                            }
                        }
                        objBoomList.Add(objBoom);
                    }
                    Total<List<BoomVM>> objresult = new Total<List<BoomVM>>();
                    objresult.TotalList = (from l in objBoomList
                                           orderby l.BoomedtDate descending
                                           select l).Skip(startIndex).Take(endIndex - startIndex).ToList();
                    objresult.TotalCount = objBoomList.Count();
                    if ((objresult.TotalCount) > endIndex)
                    {
                        objresult.IsMoreAvailable = true;
                    }
                    return objresult;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End : GetBoomList  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to getresult comment list of a shared message on myteam feed UI
        /// </summary>
        /// <returns>Total<List<CommentVM>></returns>
        /// <devdoc>
        /// Developer Name - Irshad
        /// Date - 02/10/2016
        /// </devdoc>
        public static Total<List<CommentVM>> GetResultCommentList(int resultFeedId, int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetCommentList");
                    var result = (from cmnt in _dbContext.ResultComments
                                  join c in _dbContext.Credentials on cmnt.CommentedBy equals c.Id
                                  where cmnt.Id == resultFeedId
                                  orderby cmnt.CommentedDate descending
                                  select new
                                  {
                                      _Credential = c,
                                      _Comment = cmnt
                                  }).ToList();
                    List<CommentVM> objCommentList = new List<CommentVM>();
                    foreach (var item in result)
                    {
                        CommentVM objComment = new CommentVM();
                        objComment.CommentDate = CommonWebApiBL.GetDateTimeFormat(item._Comment.CommentedDate);
                        objComment.PostedCommentDate = item._Comment.CommentedDate.ToUniversalTime();
                        objComment.CommentId = item._Comment.ResultCommentId;
                        objComment.Message = item._Comment.Message;
                        if (item._Credential.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                        {
                            var user = _dbContext.User.FirstOrDefault(u => u.UserId == item._Credential.UserId);
                            if (user != null)
                            {
                                objComment.UserName = user.FirstName + " " + user.LastName;
                                objComment.ImageUrl = string.IsNullOrEmpty(user.UserImageUrl) ? string.Empty : CommonUtility.VirtualPath +
                                    Message.ProfilePicDirectory + user.UserImageUrl;
                                objComment.UserId = user.UserId;
                                objComment.UserType = Message.UserTypeUser;
                            }
                        }
                        else if (item._Credential.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                        {
                            var trainer = _dbContext.Trainer.FirstOrDefault(t => t.TrainerId == item._Credential.UserId);
                            if (trainer != null)
                            {
                                objComment.UserName = trainer.FirstName + " " + trainer.LastName;
                                objComment.ImageUrl = string.IsNullOrEmpty(trainer.TrainerImageUrl) ? string.Empty : CommonUtility.VirtualPath +
                                    Message.ProfilePicDirectory + trainer.TrainerImageUrl;
                                objComment.UserId = trainer.TrainerId;
                                objComment.UserType = Message.UserTypeTrainer;
                            }
                        }
                        objCommentList.Add(objComment);
                    }
                    Total<List<CommentVM>> objresult = new Total<List<CommentVM>>();
                    objresult.TotalList = (from l in objCommentList
                                               // orderby l.CommentId ascending
                                           select l).Skip(startIndex).Take(endIndex - startIndex).ToList().OrderBy(c => c.CommentId).ToList(); ;
                    objresult.TotalCount = objCommentList.Count();
                    if ((objresult.TotalCount) > endIndex)
                    {
                        objresult.IsMoreAvailable = true;
                    }
                    return objresult;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End : GetCommentList  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

    }
}