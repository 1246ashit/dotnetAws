using System.ComponentModel.DataAnnotations;

namespace awsapi.Entities
{
    public class JwtSetEntity
    {
        public const string SectionName = "Jwt";
        [Required] public string IssUser { get; set; } = null!;
        [Required] public string SignKey { get; set; } = null!;
        public int ExpireMinutes { get; set; } = 60 * 24; // 過期時間(分鐘)，這裡範例的是一天(24 小時)
    }
}