using StudentAchievements.Areas.Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAchievements.Areas.Authorization.Models
{
    public class DataRepository : IDataRepository
    {
        public Task<bool> AddDepartment(AddDepartmentsViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddDirection(AddDirectionsViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddGroup(AddGroupsViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddGroupName(AddGroupNamesViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteDepartment(Department department)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteDirection(Direction direction)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteGroup(Group group)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteGroupName(GroupNames groupNames)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditDepartment(Department department)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditDirection(Direction direction)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditGroup(Group group)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditGroupName(GroupNames groupNames)
        {
            throw new NotImplementedException();
        }
    }
}
