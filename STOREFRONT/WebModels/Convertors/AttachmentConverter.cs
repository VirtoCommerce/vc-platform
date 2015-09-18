using VirtoCommerce.Web.Models;
using DataContracts = VirtoCommerce.ApiClient.DataContracts;

namespace VirtoCommerce.Web.Convertors
{
    public static class AttachmentConverter
    {
        public static Attachment ToViewModel(this DataContracts.Attachment attachment)
        {
            var attachmentModel = new Attachment();

            attachmentModel.MimeType = attachment.MimeType;
            attachmentModel.Name = attachment.Name;
            attachmentModel.Size = attachment.Size;
            attachmentModel.Url = attachment.Url;

            return attachmentModel;
        }

        public static DataContracts.Attachment ToServiceModel(this Attachment attachmentModel)
        {
            var attachment = new DataContracts.Attachment();

            attachment.MimeType = attachmentModel.MimeType;
            attachment.Name = attachmentModel.Name;
            attachment.Size = attachmentModel.Size;
            attachment.Url = attachmentModel.Url;

            return attachment;
        }
    }
}