using System.Text.RegularExpressions;

namespace PoeLogsParser.Models
{
    public class Item
    {
        public string Name { get; set; }

        public string EscapedName
        {
            get
            {
                if (string.IsNullOrEmpty(Name))
                {
                    return "";
                }

                return Escape(Name);
            }
        }

        public int Quantity { get; set; } = 1;

        private string Escape(string val)
        {
            return CleanBulkName(CleanMapName(val));
        }

        private string CleanMapName(string val)
        {
            return Regex.Replace(val, @" \(T[0-9]+\)", "");
        }

        private string CleanBulkName(string val)
        {
            return Regex.Replace(val, "[0-9]+ ", "");
        }
    }
}