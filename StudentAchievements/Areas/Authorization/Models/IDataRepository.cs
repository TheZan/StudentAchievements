using StudentAchievements.Areas.Admin.Models.ViewModels;
using System.Threading.Tasks;

namespace StudentAchievements.Areas.Authorization.Models
{
    public interface IDataRepository
    {
        Task<bool> AddDepartment(Department department);
        Task<bool> EditDepartment(Department department);
        Task<bool> DeleteDepartment(Department department);

        Task<bool> AddDirection(Direction direction);
        Task<bool> EditDirection(Direction direction);
        Task<bool> DeleteDirection(Direction direction);

        Task<bool> AddGroupName(GroupNames groupNames);
        Task<bool> EditGroupName(GroupNames groupNames);
        Task<bool> DeleteGroupName(GroupNames groupNames);

        Task<bool> AddGroup(Group group);
        Task<bool> EditGroup(Group group);
        Task<bool> DeleteGroup(Group group);
    }
}
