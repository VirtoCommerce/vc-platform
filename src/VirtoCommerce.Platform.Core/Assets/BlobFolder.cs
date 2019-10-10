namespace VirtoCommerce.Platform.Core.Assets
{
    public class BlobFolder : BlobEntry
    {
        public BlobFolder()
        {
            Type = "folder";
        }
        public string ParentUrl { get; set; }
    }
}
