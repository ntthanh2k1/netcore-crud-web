namespace NetCore.Crud.Web.Models
{
    public class UserAssignment
    {
        public int UserId { get; set; }
        public int AssignmentId { get; set; }
        public User? User { get; set; }
        public Assignment? Assignment { get; set; }
    }
}
