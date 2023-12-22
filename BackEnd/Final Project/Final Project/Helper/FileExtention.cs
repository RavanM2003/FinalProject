namespace Final_Project.Helper
{
    public static class FileExtention
    {
        public static bool IsImage(this IFormFile file)
        {
            return file.ContentType.Contains("image");
        }
        public static bool IsLenghSuit(this IFormFile file, int lenght)
        {
            return file.Length / 1024 < lenght;
        }
    }
}
