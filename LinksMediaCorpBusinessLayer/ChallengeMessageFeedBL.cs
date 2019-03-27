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

    public class ChallengeMessageFeedBL
    {
        /// <summary>
        /// Function to post Share message text on challenge completed(Comment Section)
        /// </summary>
        /// <returns>ViewPostVM</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 05/06/2015
        /// </devdoc>
        public static ViewPostVM PostShare(PostVM<TextMessageStream> message)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {

                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: ChallengeMessageFeedBL PostShare");
                    Credentials objCred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    tblMessageStream objMessageStream = new tblMessageStream();
                    objMessageStream.Content = message.Stream.TextMessage;
                    objMessageStream.MessageType = Message.TextMessageType;
                    objMessageStream.PostedDate = DateTime.Now;
                    objMessageStream.TargetType = Message.ChallengeTargetType;
                    objMessageStream.SubjectId = objCred.Id;
                    objMessageStream.TargetId = message.TargetId;  //ChallengeId is assigned.
                    objMessageStream.IsTextImageVideo = true;
                    objMessageStream.IsImageVideo = message.IsImageVideo;
                    _dbContext.MessageStraems.Add(objMessageStream);
                    _dbContext.SaveChanges();
                    if (objCred.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        // User Notification when user post message on other user or trainer post data their team member                       
                        NotificationApiBL.SendALLTeamNotificationByTrainer(objCred.UserId, objMessageStream.MessageStraemId, objCred.UserId, objCred.UserType);
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
                            objViewPostVM.PostedByImageUrl = string.IsNullOrEmpty(user.UserImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + user.UserImageUrl;
                            objViewPostVM.UserName = user.FirstName + " " + user.LastName;
                            objViewPostVM.UserId = user.UserId;
                            objViewPostVM.UserType = user.UserType;
                        }
                    }
                    else if (objCred.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
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
                            objViewPostVM.PostedByImageUrl = string.IsNullOrEmpty(trainer.TrainerImageUrl) ? string.Empty :
                                CommonUtility.VirtualPath + Message.ProfilePicDirectory + trainer.TrainerImageUrl;
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
                    traceLog.AppendLine("End:ChallengeMessageFeedBL PostShare --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get post list on challenge completed(Comment Section)
        /// </summary>
        /// <returns>Total<List<ChallengeViewPostVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 05/13/2015
        /// </devdoc>
        public static Total<List<ChallengeViewPostVM>> GetPostListForChallenge(int challengeId, int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: ChallengeMessageFeedBL GetPostList");

                    Credentials userCredential = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    List<ChallengeViewPostVM> objList = (from m in _dbContext.MessageStraems
                                                         join c in _dbContext.Credentials on m.SubjectId equals c.Id
                                                         where m.TargetId == challengeId && m.IsImageVideo &&
                                                         (!(string.IsNullOrEmpty(m.Content)) || m.IsImageVideo)
                                                         && m.TargetType == Message.ChallengeTargetType
                                                         && m.IsTextImageVideo == true
                                                         orderby m.PostedDate descending
                                                         select new ChallengeViewPostVM
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
                    foreach (var item in objList)
                    {
                        tblCredentials objCred = _dbContext.Credentials.FirstOrDefault(cr => cr.Id == item.PostedBy);
                        string imageUrl = string.Empty;
                        if (objCred != null && objCred.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                        {
                            tblUser userTemp = _dbContext.User.FirstOrDefault(usr => usr.UserId == objCred.UserId);
                            if (userTemp != null)
                                imageUrl = string.IsNullOrEmpty(userTemp.UserImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + userTemp.UserImageUrl;
                        }
                        else if (objCred != null && objCred.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                        {
                            tblTrainer trainerTemp = _dbContext.Trainer.FirstOrDefault(usr => usr.TrainerId == objCred.UserId);
                            if (trainerTemp != null)
                                imageUrl = string.IsNullOrEmpty(trainerTemp.TrainerImageUrl) ? string.Empty : CommonUtility.VirtualPath +
                                    Message.ProfilePicDirectory + trainerTemp.TrainerImageUrl;
                        }
                        item.PostedByImageUrl = imageUrl;
                        item.UserName = (objCred.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase)
                                         ? _dbContext.User.FirstOrDefault(usr => usr.UserId == objCred.UserId).FirstName + " "
                                         + _dbContext.User.FirstOrDefault(usr => usr.UserId == objCred.UserId).LastName
                                         : _dbContext.Trainer.FirstOrDefault(tnr => tnr.TrainerId == objCred.UserId).FirstName + " "
                                         + _dbContext.Trainer.FirstOrDefault(tnr => tnr.TrainerId == objCred.UserId).LastName);
                        item.PostedDate = CommonWebApiBL.GetDateTimeFormat(item.DbPostedDate);
                        //Code For Getting Posted Pics 
                        item.PicList.ForEach(pic =>
                            {
                                pic.PicsUrl = (!string.IsNullOrEmpty(pic.PicsUrl) && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ResultPicDirectory + pic.PicsUrl))) ?
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
                                vid.VideoUrl = !string.IsNullOrEmpty(vid.VideoUrl) ? CommonUtility.VirtualPath + Message.ResultVideoDirectory + vid.VideoUrl : string.Empty;
                                thumnailHeight = string.Empty;
                                thumnailWidth = string.Empty;
                                CommonWebApiBL.GetThumNailDemension(thumnailFileName, out thumnailHeight, out thumnailWidth);
                                vid.ThumbNailHeight = thumnailHeight;
                                vid.ThumbNailWidth = thumnailWidth;
                            }
                         );
                    }
                    Total<List<ChallengeViewPostVM>> objresult = new Total<List<ChallengeViewPostVM>>();
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
                    traceLog.AppendLine("End :ChallengeMessageFeedBL GetPostList  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to post boom on challenge completed(Comment Section)
        /// </summary>
        /// <returns>int</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 05/06/2015
        /// </devdoc>
        public static int PostBoom(ViewPostVM model)
        {
            StringBuilder traceLog = null;

            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: ChallengeMessageFeedBL PostBoom");
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
                    traceLog.AppendLine("End : ChallengeMessageFeedBL PostBoom  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to post comment on challenge completed(Comment Section)
        /// </summary>
        /// <returns>CommentVM</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 05/06/2015
        /// </devdoc>
        public static CommentVM PostComment(CommentVM model)
        {
            StringBuilder traceLog = null;

            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: ChallengeMessageFeedBL PostComment");
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    tblComment objComment = new tblComment();
                    objComment.CommentedBy = cred.Id;
                    objComment.CommentedDate = DateTime.Now;
                    objComment.Message = model.Message;
                    objComment.MessageStraemId = model.PostId;
                    _dbContext.Comments.Add(objComment);
                    _dbContext.SaveChanges();
                    // User Notification when user post message on other user or trainer post data their team member
                    if (!string.IsNullOrEmpty(model.UserType) && model.UserId == cred.UserId && cred.UserType == model.UserType)
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

                    if (cred.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
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
                    else if (cred.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        var trainer = _dbContext.Trainer.FirstOrDefault(t => t.TrainerId == cred.UserId);
                        if (trainer != null)
                        {
                            objCommentVM.ImageUrl = string.IsNullOrEmpty(trainer.TrainerImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + trainer.TrainerImageUrl;
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
                    traceLog.AppendLine("End : ChallengeMessageFeedBL PostComment  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get boom list of a challenge completed(Comment Section)
        /// </summary>
        /// <returns>Total<List<BoomVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 05/06/2015
        /// </devdoc>
        public static Total<List<BoomVM>> GetBoomList(int messageStreamId, int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: ChallengeMessageFeedBL GetBoomList");
                    var result = (from b in _dbContext.Booms
                                  join c in _dbContext.Credentials on b.BoomedBy equals c.Id
                                  where b.MessageStraemId == messageStreamId
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
                        objBoom.BoomId = item._Boom.BoomId;
                        if (item._Credential.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                        {
                            var user = _dbContext.User.FirstOrDefault(u => u.UserId == item._Credential.UserId);
                            if (user != null)
                            {
                                objBoom.UserName = user.FirstName + " " + user.LastName;
                                objBoom.ImageUrl = string.IsNullOrEmpty(user.UserImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + user.UserImageUrl;
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
                                objBoom.ImageUrl = string.IsNullOrEmpty(trainer.TrainerImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + trainer.TrainerImageUrl;
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
                    traceLog.AppendLine("End : ChallengeMessageFeedBL GetBoomList  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get comment list of a challenge completed(Comment Section)
        /// </summary>
        /// <returns>Total<List<CommentVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 05/06/2015
        /// </devdoc>
        public static Total<List<CommentVM>> GetCommentList(int messageStreamId, int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: ChallengeMessageFeedBL GetCommentList");

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
                                objComment.ImageUrl = string.IsNullOrEmpty(user.UserImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + user.UserImageUrl;
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
                                objComment.ImageUrl = string.IsNullOrEmpty(trainer.TrainerImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + trainer.TrainerImageUrl;
                                objComment.UserId = trainer.TrainerId;
                                objComment.UserType = Message.UserTypeTrainer;
                            }
                        }
                        objCommentList.Add(objComment);
                    }
                    Total<List<CommentVM>> objresult = new Total<List<CommentVM>>();
                    objresult.TotalList = (from l in objCommentList
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
                    traceLog.AppendLine("End : ChallengeMessageFeedBL GetCommentList  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
    }
}