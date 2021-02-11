using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using StudentAchievements.Areas.Authorization.Models;
using StudentAchievements.Models;

namespace StudentAchievements.Infrastructure
{
    public class InitDatabase
    {
        private IConfiguration configuration;
        private IUserRepository repository;
        private StudentAchievementsDbContext context;

        public InitDatabase(IConfiguration _configuration, IUserRepository _repository, StudentAchievementsDbContext _context)
        {
            configuration = _configuration;
            repository = _repository;
            context = _context;
        }

        public async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = configuration["Data:AdministratorAccount:Login"];
            string password = configuration["Data:AdministratorAccount:Password"];
            string adminName = configuration["Data:AdministratorAccount:Name"];
            string adminGender = configuration["Data:AdministratorAccount:Gender"];

            if (await roleManager.FindByNameAsync("Admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (await roleManager.FindByNameAsync("Employer") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Employer"));
            }
            if (await roleManager.FindByNameAsync("Teacher") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Teacher"));
            }
            if (await roleManager.FindByNameAsync("Student") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Student"));
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User() { Email = adminEmail, UserName = adminEmail, Name = adminName };

                var result = await repository.AddUser(admin, password, new Administrator()
                {
                    User = admin,
                    Gender = adminGender
                });

                if (result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(admin);
                    await userManager.ConfirmEmailAsync(admin, token);
                }
            }

            SetAllData();
        }

        private bool SetFormEducation()
        {
            if (!context.FormEducations.Any())
            {
                var formEducation = new List<FormEducation>()
                {
                    new FormEducation()
                    {
                        Name = "Очная"
                    },
                    new FormEducation()
                    {
                        Name = "Заочная"
                    }
                };

                context.FormEducations.AddRange(formEducation);
                context.SaveChanges();

                return true;
            }

            return false;
        }

        private bool SetGroupNames()
        {
            if (!context.GroupNames.Any())
            {
                var groupNames = new List<GroupNames>()
                {
                    new GroupNames()
                    {
                        Name = "ЭИС"
                    },
                    new GroupNames()
                    {
                        Name = "ЭПИ"
                    }
                };

                context.GroupNames.AddRange(groupNames);
                context.SaveChanges();

                return true;
            }

            return false;
        }

        private bool SetGroups()
        {
            if (!context.Groups.Any())
            {
                var groups = new List<Group>()
                {
                    new Group()
                    {
                        Name = context.GroupNames.FirstOrDefault(n => n.Name == "ЭИС"),
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        Number = 14
                    },
                    new Group()
                    {
                        Name = context.GroupNames.FirstOrDefault(n => n.Name == "ЭИС"),
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        Number = 15
                    },
                    new Group()
                    {
                        Name = context.GroupNames.FirstOrDefault(n => n.Name == "ЭИС"),
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        Number = 16
                    }
                };

                context.Groups.AddRange(groups);
                context.SaveChanges();

                return true;
            }

            return false;
        }

        private bool SetGroupsType()
        {
            if (!context.ProgramType.Any())
            {
                var groupsType = new List<ProgramType>()
                {
                    new ProgramType()
                    {
                        Name = "Бакалавриат"
                    },
                    new ProgramType()
                    {
                        Name = "Магистратура"
                    }
                };

                context.ProgramType.AddRange(groupsType);
                context.SaveChanges();

                return true;
            }

            return false;
        }

        private bool SetDepartments()
        {
            if (!context.Departments.Any())
            {
                var departments = new List<Department>()
                {
                    new Department()
                    {
                        Name = "Архитектуры и дизайна"
                    },
                    new Department()
                    {
                        Name = "Цифровых систем"
                    },
                    new Department()
                    {
                        Name = "Экономики и менеджмента"
                    },
                    new Department()
                    {
                        Name = "Химии и химической технологии"
                    },
                    new Department()
                    {
                        Name = "Инженерии и машиностроения"
                    },
                    new Department()
                    {
                        Name = "Инженеров строительства и транспорта"
                    },
                };

                context.Departments.AddRange(departments);
                context.SaveChanges();

                return true;
            }

            return false;
        }

        private bool SetDirection()
        {
            if (!context.Directions.Any())
            {
                var directions = new List<Direction>()
                {
                    #region Институт Архитектуры и дизайна

                    new Direction()
                    {
                        Name = "Архитектурное проектирование",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Архитектуры и дизайна"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Дизайн городской среды",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Архитектуры и дизайна"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Архитектурное проектирование: архитектурная реставрация и реконструкция",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Архитектуры и дизайна"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Магистратура")
                    },

                    #endregion

                    #region Институт Цифровых систем

                    new Direction()
                    {
                        Name = "Автоматизация управления в технических системах (технологических процессов и производств)",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Цифровых систем"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Информационные системы и технологии",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Цифровых систем"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Системы управления информационной безопасностью предприятия",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Цифровых систем"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Управление проектами разработки программного обеспечения",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Цифровых систем"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Информационные системы управления предприятием",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Цифровых систем"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Магистратура")
                    },
                    new Direction()
                    {
                        Name = "Управление в технических системах",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Цифровых систем"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Магистратура")
                    },

                    #endregion

                    #region Институт Экономики и менеджмента

                    new Direction()
                    {
                        Name = "Управление качеством",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Экономики и менеджмента"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Логистика",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Экономики и менеджмента"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Производственный менеджмент",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Экономики и менеджмента"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Учет, анализ, финансы, кредит",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Экономики и менеджмента"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Цифровые технологии в производстве и образовании - Направление \"ПРОФЕССИОНАЛЬНОЕ ОБУЧЕНИЕ\"",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Экономики и менеджмента"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Бизнес аналитика",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Экономики и менеджмента"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Производственный менеджмент",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Экономики и менеджмента"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Магистратура")
                    },
                    new Direction()
                    {
                        Name = "Проектирование и управление образовательной средой",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Экономики и менеджмента"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Магистратура")
                    },
                    new Direction()
                    {
                        Name = "Экономика фирмы",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Экономики и менеджмента"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Магистратура")
                    },
                    new Direction()
                    {
                        Name = "Управление качеством в социально-экономических системах",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Экономики и менеджмента"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Магистратура")
                    },
                    new Direction()
                    {
                        Name = "Корпоративные финансы",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Экономики и менеджмента"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Магистратура")
                    },

                    #endregion

                    #region Институт Химии и химической технологии

                    new Direction()
                    {
                        Name = "Фармацевтическая химия",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Химии и химической технологии"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Биоорганическая химия",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Химии и химической технологии"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Нефтехимия",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Химии и химической технологии"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Охрана окружающей среды и рациональное использование природных ресурсов",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Химии и химической технологии"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Фармацевтическая химия (направление \"Химия\")",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Химии и химической технологии"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Производство химико-фармацевтических препаратов",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Химии и химической технологии"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Технология переработки нефти и органический синтез",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Химии и химической технологии"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Химическая технология",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Химии и химической технологии"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Химическая технология органических веществ",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Химии и химической технологии"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Химико-технологические процессы получения веществ, материалов и изделий",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Химии и химической технологии"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Магистратура")
                    },
                    new Direction()
                    {
                        Name = "Утилизация и переработка отходов производства и потребления",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Химии и химической технологии"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Магистратура")
                    },
                    new Direction()
                    {
                        Name = "Химическая технология переработки нефти и углеводородных материалов",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Химии и химической технологии"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Магистратура")
                    },
                    new Direction()
                    {
                        Name = "Синтез и технология полифункциональных соединений многоцелевого назначения",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Химии и химической технологии"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Магистратура")
                    },
                    #endregion

                    #region Институт Инженерии и машиностроения

                    new Direction()
                    {
                        Name = "Материаловедение и технологии материалов",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Инженерии и машиностроения"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Технологическое оборудование химических и нефтехимических производств",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Инженерии и машиностроения"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Компьютерно-интегрированное машиностроение",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Инженерии и машиностроения"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Подъемно-транспортные, строительные, дорожные средства и оборудование (специалитет)",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Инженерии и машиностроения"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Двигатели внутреннего сгорания",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Инженерии и машиностроения"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Подъемно-транспортные, строительные, дорожные машины и оборудование",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Инженерии и машиностроения"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Стандартизация и сертификация",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Инженерии и машиностроения"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Оборудование и технология сварочного производства",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Инженерии и машиностроения"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Подъемно-транспортные, строительные, дорожные машины и оборудование",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Инженерии и машиностроения"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Магистратура")
                    },
                    new Direction()
                    {
                        Name = "Стандартизация и сертификация",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Инженерии и машиностроения"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Магистратура")
                    },
                    new Direction()
                    {
                        Name = "Двигатели внутреннего сгорания",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Инженерии и машиностроения"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Магистратура")
                    },
                    new Direction()
                    {
                        Name = "Инновационные производственные технологические процессы",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Инженерии и машиностроения"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Магистратура")
                    },
                    new Direction()
                    {
                        Name = "Материаловедение и прикладная механика",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Инженерии и машиностроения"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Магистратура")
                    },
                    #endregion

                    #region Институт Инженеров строительства и транспорта

                    new Direction()
                    {
                        Name = "Aвтомобили и автомобильное хозяйство",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Инженеров строительства и транспорта"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Промышленное и гражданское строительство",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Инженеров строительства и транспорта"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Автомобильные дороги",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Инженеров строительства и транспорта"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Цифровые технологии в строительстве",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Инженеров строительства и транспорта"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Пространственное проектирование, градостроительное зонирование и планировка территории",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Инженеров строительства и транспорта"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Комплексное использование и охрана водных ресурсов",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Инженеров строительства и транспорта"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Бакалавриат")
                    },
                    new Direction()
                    {
                        Name = "Проектирование, строительство и эксплуатация зданий и сооружений",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Инженеров строительства и транспорта"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Магистратура")
                    },
                    new Direction()
                    {
                        Name = "Комплексное природообустройство и водопользование на устойчивой основе",
                        Department = context.Departments.FirstOrDefault(d => d.Name == "Инженеров строительства и транспорта"),
                        ProgramType = context.ProgramType.FirstOrDefault(p => p.Name == "Магистратура")
                    }
                    #endregion
                };

                context.Directions.AddRange(directions);
                context.SaveChanges();

                return true;
            }

            return false;
        }

        private bool SetAllData()
        {
            if (SetGroupsType() && SetFormEducation() && SetDepartments() && SetDirection() && SetGroupNames() && SetGroups())
            {
                return true;
            }

            return false;
        }
    }
}