using WealthWatch.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WealthWatch.Services
{
    public interface ITagService
    {
        Task<List<Tags>> GetAllTagsAsync();
        Task<bool> CheckTag(string tagName);
        Task<int> CreateTagAsync(Tags tag);
        Task<bool> DeleteTagAsync(Guid id);
    }
}
