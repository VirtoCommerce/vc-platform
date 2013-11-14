using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.ManagementClient.Customers.Infrastructure.Communications;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Helpers;
using VirtoCommerce.ManagementClient.Customers.ViewModel;
using VirtoCommerce.ManagementClient.Customers.Infrastructure.Communications.Model;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Helpers;


namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Adaptors
{
	public class CommunicationAdaptor
	{

		#region NotesConvertionMethods

		public Note NoteCommunicationViewModel2Note(CommunicationItemNoteViewModel viewModel)
		{
			Note retVal = new Note
			{
				AuthorName = viewModel.AuthorName,
				Body = viewModel.Body,
				Title = viewModel.Title,
			};

			if (viewModel.Id != null)
				retVal.NoteId = viewModel.Id;

			return retVal;

		}

		public CommunicationItemNoteViewModel Note2NoteCommunicationViewModel(Note note)
		{
			CommunicationItemNoteViewModel retVal = new CommunicationItemNoteViewModel
			{
				AuthorName = note.AuthorName,
				Body = note.Body,
				Title = note.Title,
				Id = note.NoteId,
				ModifierName = note.ModifierName,
                LastModified = note.LastModified
                
			};

			return retVal;
		}


		#endregion

		#region KnowledgeBaseArticleConvertionMethods

		public KnowledgeBaseArticle KnowledgeBaseArticleCommunicationViewModel2KnowledgeBaseArticle(CommunicationItemKnowledgeBaseArticleViewModel viewModel)
		{
			KnowledgeBaseArticle retVal = new KnowledgeBaseArticle
			{
				AuthorName = viewModel.AuthorName,
				Body = viewModel.Body,
				Title = viewModel.Title,

			};
			if (viewModel.Id != null)
				retVal.KnowledgeBaseArticleId = viewModel.Id;

			return retVal;
		}


		public CommunicationItemKnowledgeBaseArticleViewModel KnowledgeBaseArticle2KnowledgeBaseArticleCommunicationViewModel(KnowledgeBaseArticle item)
		{

			CommunicationItemKnowledgeBaseArticleViewModel retVal = new CommunicationItemKnowledgeBaseArticleViewModel
			{
				//LastModified = item.LastModified,
				//Created = item.Created,
				AuthorId = item.AuthorId,
				AuthorName = item.AuthorName,
				Body = item.Body,
				Title = item.Title,
				Id = item.KnowledgeBaseArticleId
			};
			if (item.Attachments != null)
			{
				foreach (var attachment in item.Attachments)
				{
					retVal.Attachments.Add(Attachment2CommunicationAttachment(attachment));
				}
			}

			return retVal;

		}

		#endregion

		#region AttachmentConvertionMethods

		public Attachment CommunicationAttachment2Attachment(CommunicationAttachment commAttach)
		{
			Attachment retVal = new Attachment();
			if (commAttach.AttachmentId != null)
			{
				retVal.AttachmentId = commAttach.AttachmentId;
			}
			retVal.CreatorName = commAttach.CreatorName;
			retVal.DisplayName = commAttach.DisplayName;
			retVal.FileType = commAttach.FileType;
			retVal.FileUrl = commAttach.FileUrl;
			retVal.LastModified = commAttach.LastModified;

			return retVal;
		}


		public CommunicationAttachment Attachment2CommunicationAttachment(Attachment attach)
		{

			CommunicationAttachment retVal = new CommunicationAttachment
			{
				AttachmentId = attach.AttachmentId,
				CreatorName = attach.CreatorName,
				DisplayName = attach.DisplayName,
				FileType = attach.FileType,
				FileUrl = attach.FileUrl,
				LastModified = attach.LastModified
			};

			return retVal;
		}


		#endregion

        #region PublicReplyConvertionMethods

        public PublicReplyItem PublicReplyViewModel2PublicReplyItem(CommunicationItemPublicReplyViewModel viewModel)
        {
            PublicReplyItem retVal=new PublicReplyItem();

            retVal.AuthorName = viewModel.AuthorName;
            retVal.Body = viewModel.Body;
            retVal.Title = viewModel.Title;

            if (viewModel.Id != null)
                retVal.CommunicationItemId = viewModel.Id;

            return retVal;
        }

        public CommunicationItemPublicReplyViewModel PublicReply2PublicReplyViewModel(PublicReplyItem item)
        {
            CommunicationItemPublicReplyViewModel retVal=new CommunicationItemPublicReplyViewModel();

            retVal.Id = item.CommunicationItemId;
            retVal.AuthorName = item.AuthorName;
            retVal.Body = item.Body;
            retVal.LastModified = item.LastModified;

            return retVal;
        }


        #endregion

    }
}
