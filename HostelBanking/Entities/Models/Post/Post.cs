using HostelBanking.Entities.DataTransferObjects.Post;
using System.ComponentModel.DataAnnotations;

namespace HostelBanking.Entities.Models.Post
{
    public class Post : Base
    {
        public string Title { get; set; }
        public float Price { get; set; }
        public int Acreage { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string DescriptionPost { get; set; }
        public string Images { get; set; }
        public DateTime CreateDate { get; set; }
        public string PhoneNumber { get; set; }
        public string OwnerHouse { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int PaymentType { get; set; }
        public int HostelTypeId { get; set; }
        public int AccountId { get; set; }

        public static implicit operator Post(PostDto v)
        {
            throw new NotImplementedException();
        }
    }
}
