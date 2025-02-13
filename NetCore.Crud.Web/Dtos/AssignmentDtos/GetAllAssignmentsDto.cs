using Microsoft.AspNetCore.Mvc.Rendering;
using NetCore.Crud.Web.Models;

namespace NetCore.Crud.Web.Dtos.AssignmentDtos
{
    public class GetAllAssignmentsDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
