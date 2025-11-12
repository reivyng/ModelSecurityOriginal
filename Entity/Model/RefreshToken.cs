namespace Entity.Model
{
    public class RefreshToken : BaseModel
    {
        public int user_id { get; set; }
        public string token_hash { get; set; }
        public System.DateTime created_at { get; set; } = System.DateTime.UtcNow;
        public System.DateTime expires_at { get; set; }
        public bool is_revoked { get; set; } = false;
        public string? replaced_by_token_hash { get; set; }
    }
}
