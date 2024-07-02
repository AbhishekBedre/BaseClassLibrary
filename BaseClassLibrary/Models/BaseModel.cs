namespace BaseClassLibrary.Models
{
    /// <summary>
    /// Base model for Audit Columns
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// CreatedDate
        /// </summary>
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// CreatedBy
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// UpdatedDate
        /// </summary>
        public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// UpdatedBy
        /// </summary>
        public string? UpdatedBy { get; set; }
    }
}
