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

        private void SetScores()
        {
            if (!context.Scores.Any())
            {
                var scores = new List<Score>()
                {
                    new Score()
                    {
                        Name = "Удовлетворительно"
                    },
                    new Score()
                    {
                        Name = "Хорошо"
                    },
                    new Score()
                    {
                        Name = "Отлично"
                    },
                    new Score()
                    {
                        Name = "Зачет"
                    },
                };

                context.Scores.AddRange(scores);
                context.SaveChanges();
            }
        }

        private void SetControlType()
        {
            if (!context.ControlTypes.Any())
            {
                var controlTypes = new List<ControlType>()
                {
                    new ControlType()
                    {
                        Name = "Зачет"
                    },
                    new ControlType()
                    {
                        Name = "Дифференцированный зачет"
                    },
                    new ControlType()
                    {
                        Name = "Экзамен"
                    }
                };

                context.ControlTypes.AddRange(controlTypes);
                context.SaveChanges();
            }
        }

        private void SetFormEducation()
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
            }
        }

        private void SetGroups()
        {
            if (!context.Groups.Any())
            {
                var groups = new List<Group>()
                {
                    new Group()
                    {
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        Number = 4,
                        Grade = 1
                    },
                    new Group()
                    {
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        Number = 5,
                        Grade = 1
                    },
                    new Group()
                    {
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        Number = 6,
                        Grade = 1
                    }
                };

                context.Groups.AddRange(groups);
                context.SaveChanges();
            }
        }

        private void SetGroupsType()
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
            }
        }

        private void SetDepartments()
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
            }
        }

        private void SetDirection()
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
                        GroupName = "ЭИС",
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
                        GroupName = "ЭПИ",
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
            }
        }

        private void SetSubjects()
        {
            if (!context.Subjects.Any())
            {
                var subjects = new List<Subject>()
                {
                    new Subject()
                    {
                        Name = "Иностранный язык",
                        Grade = 1,
                        Semester = 1,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Зачет")
                    },
                    new Subject()
                    {
                        Name = "Информатика",
                        Grade = 1,
                        Semester = 1,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Дифференцированный зачет")
                    },
                    new Subject()
                    {
                        Name = "История развития отрасли",
                        Grade = 1,
                        Semester = 1,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Экзамен")
                    },
                    new Subject()
                    {
                        Name = "Линейная алгебра",
                        Grade = 1,
                        Semester = 1,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Экзамен")
                    },
                    new Subject()
                    {
                        Name = "Социология",
                        Grade = 1,
                        Semester = 1,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Зачет")
                    },
                    new Subject()
                    {
                        Name = "Физическая культура и спорт",
                        Grade = 1,
                        Semester = 1,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Зачет")
                    },
                    new Subject()
                    {
                        Name = "Философия",
                        Grade = 1,
                        Semester = 1,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Экзамен")
                    },
                    new Subject()
                    {
                        Name = "Химия",
                        Grade = 1,
                        Semester = 1,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Зачет")
                    },
                    new Subject()
                    {
                        Name = "Экология",
                        Grade = 1,
                        Semester = 1,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Зачет")
                    },
                    new Subject()
                    {
                        Name = "Экономика",
                        Grade = 1,
                        Semester = 1,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Дифференцированный зачет")
                    },
                    new Subject()
                    {
                        Name = "Архитектура информационных систем",
                        Grade = 1,
                        Semester = 2,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Экзамен")
                    },
                    new Subject()
                    {
                        Name = "Иностранный язык",
                        Grade = 1,
                        Semester = 2,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Зачет")
                    },
                    new Subject()
                    {
                        Name = "Информатика",
                        Grade = 1,
                        Semester = 2,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Экзамен")
                    },
                    new Subject()
                    {
                        Name = "История",
                        Grade = 1,
                        Semester = 2,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Экзамен")
                    },
                    new Subject()
                    {
                        Name = "Математика",
                        Grade = 1,
                        Semester = 2,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Зачет")
                    },
                    new Subject()
                    {
                        Name = "Операционные системы",
                        Grade = 1,
                        Semester = 2,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Дифференцированный зачет")
                    },
                    new Subject()
                    {
                        Name = "Физика",
                        Grade = 1,
                        Semester = 2,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Зачет")
                    },
                    new Subject()
                    {
                        Name = "Физическая культура и спорт",
                        Grade = 1,
                        Semester = 2,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Зачет")
                    },
                    new Subject()
                    {
                        Name = "Химия",
                        Grade = 1,
                        Semester = 2,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Дифференцированный зачет")
                    },
                    new Subject()
                    {
                        Name = "Web - технологии",
                        Grade = 2,
                        Semester = 3,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Экзамен")
                    },
                    new Subject()
                    {
                        Name = "Иностранный язык",
                        Grade = 2,
                        Semester = 3,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Зачет")
                    },
                    new Subject()
                    {
                        Name = "Математика",
                        Grade = 2,
                        Semester = 3,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Экзамен")
                    },
                    new Subject()
                    {
                        Name = "Математика (доп.главы)",
                        Grade = 2,
                        Semester = 3,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Дифференцированный зачет")
                    },
                    new Subject()
                    {
                        Name = "Психология управления коллективом",
                        Grade = 2,
                        Semester = 3,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Зачет")
                    },
                    new Subject()
                    {
                        Name = "Теория информационных процессов и систем",
                        Grade = 2,
                        Semester = 3,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Дифференцированный зачет")
                    },
                    new Subject()
                    {
                        Name = "Технологии программирования",
                        Grade = 2,
                        Semester = 3,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Зачет")
                    },
                    new Subject()
                    {
                        Name = "Физика",
                        Grade = 2,
                        Semester = 3,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Экзамен")
                    },
                    new Subject()
                    {
                        Name = "Физическая культура и спорт",
                        Grade = 2,
                        Semester = 3,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Зачет")
                    },
                    new Subject()
                    {
                        Name = "Иностранный язык",
                        Grade = 2,
                        Semester = 4,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Дифференцированный зачет")
                    },
                    new Subject()
                    {
                        Name = "Иностранный язык (специализация)",
                        Grade = 2,
                        Semester = 4,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Зачет")
                    },
                    new Subject()
                    {
                        Name = "Инфокоммуникационные системы и сети",
                        Grade = 2,
                        Semester = 4,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Дифференцированный зачет")
                    },
                    new Subject()
                    {
                        Name = "Информационные технологии",
                        Grade = 2,
                        Semester = 4,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Экзамен")
                    },
                    new Subject()
                    {
                        Name = "Правоведение",
                        Grade = 2,
                        Semester = 4,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Зачет")
                    },
                    new Subject()
                    {
                        Name = "Технологии программирования",
                        Grade = 2,
                        Semester = 4,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Экзамен")
                    },
                    new Subject()
                    {
                        Name = "Управление данными",
                        Grade = 2,
                        Semester = 4,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Экзамен")
                    },
                    new Subject()
                    {
                        Name = "Физическая культура и спорт",
                        Grade = 2,
                        Semester = 4,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Зачет")
                    },
                    new Subject()
                    {
                        Name = "Иностранный язык (специализация)",
                        Grade = 3,
                        Semester = 5,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Дифференцированный зачет")
                    },
                    new Subject()
                    {
                        Name = "Информационная безопасность и защита информации",
                        Grade = 3,
                        Semester = 5,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Дифференцированный зачет")
                    },
                    new Subject()
                    {
                        Name = "Корпоративные информационные системы управления предприятием",
                        Grade = 3,
                        Semester = 5,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Экзамен")
                    },
                    new Subject()
                    {
                        Name = "Методы и средства проектирования информационных систем и технологий",
                        Grade = 3,
                        Semester = 5,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Дифференцированный зачет")
                    },
                    new Subject()
                    {
                        Name = "Системы управления базами данных",
                        Grade = 3,
                        Semester = 5,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Экзамен")
                    },
                    new Subject()
                    {
                        Name = "Технологии обработки информации",
                        Grade = 3,
                        Semester = 5,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Экзамен")
                    },
                    new Subject()
                    {
                        Name = "Физическая культура и спорт",
                        Grade = 3,
                        Semester = 5,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Зачет")
                    },
                    new Subject()
                    {
                        Name = "Инвестиционный менеджмент",
                        Grade = 3,
                        Semester = 6,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Экзамен")
                    },
                    new Subject()
                    {
                        Name = "Инструментальные средства информационных систем",
                        Grade = 3,
                        Semester = 6,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Экзамен")
                    },
                    new Subject()
                    {
                        Name = "Культурология и этика общения",
                        Grade = 3,
                        Semester = 6,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Дифференцированный зачет")
                    },
                    new Subject()
                    {
                        Name = "Менеджмент ИТ проектов",
                        Grade = 3,
                        Semester = 6,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Дифференцированный зачет")
                    },
                    new Subject()
                    {
                        Name = "Моделирование производственных процессов. Моделирование основных производственных процессов",
                        Grade = 3,
                        Semester = 6,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Экзамен")
                    },
                    new Subject()
                    {
                        Name = "Физическая культура и спорт",
                        Grade = 3,
                        Semester = 6,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Зачет")
                    },
                    new Subject()
                    {
                        Name = "Информационные технологии управления предприятием",
                        Grade = 4,
                        Semester = 7,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Экзамен")
                    },
                    new Subject()
                    {
                        Name = "Логистика",
                        Grade = 4,
                        Semester = 7,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Дифференцированный зачет")
                    },
                    new Subject()
                    {
                        Name = "Маркетинг",
                        Grade = 4,
                        Semester = 7,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Дифференцированный зачет")
                    },
                    new Subject()
                    {
                        Name = "Телекоммуникационные системы",
                        Grade = 4,
                        Semester = 7,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Экзамен")
                    },
                    new Subject()
                    {
                        Name = "Управление качеством",
                        Grade = 4,
                        Semester = 7,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Дифференцированный зачет")
                    },
                    new Subject()
                    {
                        Name = "Электронная коммерция",
                        Grade = 4,
                        Semester = 7,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Экзамен")
                    },
                    new Subject()
                    {
                        Name = "Безопасность жизнедеятельности",
                        Grade = 4,
                        Semester = 8,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Дифференцированный зачет")
                    },
                    new Subject()
                    {
                        Name = "Защита интеллектуальной собственности и патентоведение",
                        Grade = 4,
                        Semester = 8,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Зачет")
                    },
                    new Subject()
                    {
                        Name = "Интеллектуальные информационные системы и технологии",
                        Grade = 4,
                        Semester = 8,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Дифференцированный зачет")
                    },
                    new Subject()
                    {
                        Name = "Практика производственная (научно-исследовательская работа)",
                        Grade = 4,
                        Semester = 8,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Зачет")
                    },
                    new Subject()
                    {
                        Name = "Практика производственная (практика по получению профессиональных умений и опыта профессиональной деятельности)",
                        Grade = 4,
                        Semester = 8,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Дифференцированный зачет")
                    },
                    new Subject()
                    {
                        Name = "Практика производственная преддипломная",
                        Grade = 4,
                        Semester = 8,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Дифференцированный зачет")
                    },
                    new Subject()
                    {
                        Name = "Практика учебная (практика по получению первичных профессиональных умений и навыков, в т.ч. первичных умений и навыков научно-исследовательской деятельности)",
                        Grade = 4,
                        Semester = 8,
                        Direction = context.Directions.FirstOrDefault(d => d.Name == "Информационные системы и технологии"),
                        ControlType = context.ControlTypes.FirstOrDefault(c => c.Name == "Дифференцированный зачет")
                    },
                };
            }
        }

        private void SetAllData()
        {
            SetControlType();
            SetScores();
            SetGroupsType();
            SetFormEducation();
            SetDepartments();
            SetDirection();
            SetGroups();
            SetSubjects();
        }
    }
}