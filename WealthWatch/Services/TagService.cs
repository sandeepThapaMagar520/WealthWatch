
using System.Text.Json;
using WealthWatch.Models;

namespace WealthWatch.Services
{
    public class TagService
    {
        private readonly string _dataFilePath;

        public TagService()
        {
            // Set the file path to the desired directory
            string appDataPath = Path.Combine("D:\\final year\\application development");
            _dataFilePath = Path.Combine(appDataPath, "tags.json");

            // Ensure the directory exists
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }
        }

        private async Task SaveTagsAsync(List<Tags> tags)
        {
            string json = JsonSerializer.Serialize(tags, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_dataFilePath, json);
        }

        public async Task<List<Tags>> GetAllTagsAsync()
        {
            try
            {
                if (!File.Exists(_dataFilePath))
                {
                    return new List<Tags>();
                }

                string jsonTags = await File.ReadAllTextAsync(_dataFilePath);
                return JsonSerializer.Deserialize<List<Tags>>(jsonTags) ?? new List<Tags>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing tags.json: {ex.Message}");
                return new List<Tags>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return new List<Tags>();
            }
        }
        public async Task<bool> CheckTag(string tagName)
        {
            // Fetch all tags asynchronously
            List<Tags> tags = await GetAllTagsAsync();

            // Check if any tag matches the provided tag name (case-insensitive)
            return tags.Any(t => t.TagName.Equals(tagName, StringComparison.OrdinalIgnoreCase));
        }



        public async Task<int> CreateTagAsync(Tags tag)
        {
            // Check if the tag name already exists using CheckTag method
            bool tagExists = await CheckTag(tag.TagName);

            if (tagExists)
            {
                return 2; // Tag already exists
            }

            // Fetch existing tags and add the new tag
            var tags = await GetAllTagsAsync();
            tags.Add(tag);

            // Save updated tag list
            await SaveTagsAsync(tags);

            return 1; // Tag created successfully
        }


        public async Task<bool> DeleteTagAsync(Guid id)
        {
            var tags = await GetAllTagsAsync();
            var tagToDelete = tags.FirstOrDefault(t => t.TagId == id);

            if (tagToDelete == null)
            {
                return false; // Tag not found
            }

            tags.Remove(tagToDelete);
            await SaveTagsAsync(tags);
            return true;
        }
    }
}
