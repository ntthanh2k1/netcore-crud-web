namespace NetCore.Crud.Web.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<UserAssignment>? UsersAssignments { get; set; }
    }
}
