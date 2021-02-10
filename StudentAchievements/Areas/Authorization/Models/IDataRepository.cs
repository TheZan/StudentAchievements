using StudentAchievements.Areas.Admin.Models.ViewModels;
using System.Threading.Tasks;

namespace StudentAchievements.Areas.Authorization.Models
{
    public interface IDataRepository
    {
        Task<bool> AddDepartment(AddDepartmentsViewModel model);
        Task<bool> EditDepartment(Department department);
        Task<bool> DeleteDepartment(Department department);

        Task<bool> AddDirection(AddDirectionsViewModel model);
        Task<bool> EditDirection(Direction direction);
        Task<bool> DeleteDirection(Direction direction);

        Task<bool> AddGroupName(AddGroupNamesViewModel model);
        Task<bool> EditGroupName(GroupNames groupNames);
        Task<bool> DeleteGroupName(GroupNames groupNames);

        Task<bool> AddGroup(AddGroupsViewModel model);
        Task<bool> EditGroup(Group group);
        Task<bool> DeleteGroup(Group group);
    }
}
