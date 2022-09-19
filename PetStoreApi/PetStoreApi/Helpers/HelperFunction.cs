using System.Text;

namespace PetStoreApi.Helpers
{
    public class HelperFunction
    {
        public static string stripAccents(string s)
        {
            s = s.Normalize(NormalizationForm.FormD);
            s = s.Replace("[\\p{InCombiningDiacriticalMarks}]", "");
            return s;
        }

        public static string normalizeUri(string input)
        {
            input = input.Replace("đ", "d").Replace("Đ", "D");

            input = stripAccents(input);

            return input.Trim().Replace("[^a-zA-Z0-9]+", "-").ToLower();
        }
    }
}
