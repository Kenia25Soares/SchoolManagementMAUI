using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class ProfileUpdateData
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public Stream? ProfilePictureStream { get; set; }
        public string? ProfilePictureFileName { get; set; }
        public string? ProfilePictureContentType { get; set; }
        public Stream? OfficialPhotoStream { get; set; }
        public string? OfficialPhotoFileName { get; set; }
        public string? OfficialPhotoContentType { get; set; }
    }
}
