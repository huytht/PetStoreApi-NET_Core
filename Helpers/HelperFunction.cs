using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace PetStoreApi.Helpers
{
    public class HelperFunction
    {
        public static string normalizeUri(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').Replace(" ", "-").ToLower();
        }
    }
}
