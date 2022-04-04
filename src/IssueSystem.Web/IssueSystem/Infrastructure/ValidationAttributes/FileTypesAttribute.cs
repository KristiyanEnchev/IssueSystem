namespace IssueSystem.Infrastructure.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    public class FileTypesAttribute : ValidationAttribute
    {
        private readonly List<string> _types;

        public FileTypesAttribute(string types)
        {
            _types = types.Split(", ").ToList();
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var fileExt = Path.GetExtension((value as IFormFile).FileName).Substring(1);
            return _types.Contains(fileExt, StringComparer.OrdinalIgnoreCase);
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("Invalid file type. Only the following types {0} are supported.", string.Join(", ", _types));
        }
    }

}
