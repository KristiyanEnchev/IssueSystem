namespace IssueSystem.Data
{
    public class ModelConstants
    {
        public class Departmenet
        {
            public const int DepartmentNameMaxLenght = 50;
        }

        public class Project
        {
            public const int ProjectNameMaxLenght = 50;
        }

        public class Employee
        {
            public const int EmployeeFirstMaxLenght = 20;
            public const int EmployeeLastMaxLenght = 40;

            public const int passwordMinLenght = 3;

            public const int EmailAdressMinLenght = 3;
            public const int EmailAdressMaxLenght = 100;
            public const string UserEmailRegularExpression = 
                @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        }

        public class Ticket 
        {
            public const int TicketTitleMaxLenght = 100;
        }

        public class TicketPriority 
        {
            public const int PriorityTypeMaxLenght = 50;
        }
        public class TicketCategory 
        {
            public const int CategoryNameMaxLenght = 50;
        }

        public class Comment
        {
            public const int CommentMaxLenght = 1000;
        }
    }
}
