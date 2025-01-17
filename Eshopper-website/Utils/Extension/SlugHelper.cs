using System.Text.RegularExpressions;

namespace Eshopper_website.Utils.Extension
{
    public class SlugHelper
    {
        public static string GenerateSlug(string slug, int id)
        {
            if (string.IsNullOrEmpty(slug)) return string.Empty;

            if (IsSlug(slug)) return slug;
            
            // Convert to lowercase
            slug = slug.ToLowerInvariant();

            // Replace spaces with hyphens
            slug = Regex.Replace(slug, @"\s+", "-");

            // Remove invalid characters
            slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");

            // Trim hyphens from start and end
            slug = slug.Trim('-');

            // Remove multiple consecutive hyphens
            slug = Regex.Replace(slug, @"-+", "-");

            return $"{slug}-{id}";
        }
        public static bool IsSlug(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;

            // Regex to match a valid slug with an optional numeric ID at the end
            string pattern = @"^[a-z0-9]+(-[a-z0-9]+)*(-\d+)?$";

            return Regex.IsMatch(input, pattern);
        }
    }
}
