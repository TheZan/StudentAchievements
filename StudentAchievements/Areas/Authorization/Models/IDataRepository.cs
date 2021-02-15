﻿using StudentAchievements.Areas.Admin.Models.ViewModels;
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

        Task<bool> AddGroup(Group group);
        Task<bool> EditGroup(Group group);
        Task<bool> DeleteGroup(Group group);

        Task<bool> AddSubject(Subject subject);
        Task<bool> EditSubject(Subject subject);
        Task<bool> DeleteSubject(Subject subject);
    }
}
