using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Infrastructure.Helper
{
    public class FileSettings
    {
        public static string SaveImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            // Define the directory for storing files
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            Directory.CreateDirectory(uploadsFolder); // Ensure the directory exists

            // Create a unique filename
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            // Save the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
                fileStream.Close();
            }

            return fileName;
        }

        public static void DeleteImage(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            // Construct the file path
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", fileName);

            // Delete the file if it exists
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
